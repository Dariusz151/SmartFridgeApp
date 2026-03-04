import { useState, useMemo, useCallback, useEffect } from "react";
import {
  Container,
  Button,
  Typography,
  Stack,
  Paper,
  Box,
  CircularProgress,
  ToggleButton,
  ToggleButtonGroup,
  TextField,
  Chip,
  Avatar,
  Tooltip,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
} from "@mui/material";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import RefreshIcon from "@mui/icons-material/Refresh";
import AddIcon from "@mui/icons-material/Add";
import FastfoodIcon from "@mui/icons-material/Fastfood";
import SearchIcon from "@mui/icons-material/Search";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import StarIcon from "@mui/icons-material/Star";
import GroupIcon from "@mui/icons-material/Group";
import SendIcon from "@mui/icons-material/Send";
import { useParams } from "react-router-dom";
import { useFetch, useSubmit } from "@/hooks/useApi";
import NewUserDialog from "@/components/dialogs/NewUserDialog";
import NewFridgeItemDialog from "@/components/dialogs/NewFridgeItemDialog";
import RecipeCarouselDialog from "@/components/dialogs/RecipeCarouselDialog";
import type { FridgeItem, FridgeUser, FridgeMember, Recipe } from "@/types";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import { useAuth } from "@/context/AuthContext";

export default function FridgeItemsDashboard() {
  const { fridgeId } = useParams<{ fridgeId: string }>();
  const { state: authState } = useAuth();
  const [selectedUserId, setSelectedUserId] = useState<string>("All");
  const [selectedItems, setSelectedItems] = useState<number[]>([]);
  const [consumeAmounts, setConsumeAmounts] = useState<Record<string, number>>({});

  const [userDialogOpen, setUserDialogOpen] = useState(false);
  const [itemDialogOpen, setItemDialogOpen] = useState(false);
  const [carouselOpen, setCarouselOpen] = useState(false);
  const [recipes, setRecipes] = useState<Recipe[]>([]);

  // Members & invite state
  const [members, setMembers] = useState<FridgeMember[]>([]);
  const [membersDialogOpen, setMembersDialogOpen] = useState(false);
  const [inviteDialogOpen, setInviteDialogOpen] = useState(false);
  const [inviteEmail, setInviteEmail] = useState("");
  const [inviting, setInviting] = useState(false);

  const { submit } = useSubmit();

  const usersEndpoint = `/api/fridgeUsers/${fridgeId}`;
  const { data: rawUsers, loading: usersLoading, refetch: refetchUsers } =
    useFetch<FridgeUser[]>(usersEndpoint, true);

  const users: FridgeUser[] = useMemo(
    () => [{ id: "All", name: "All (read-only)" }, ...(rawUsers ?? [])],
    [rawUsers],
  );

  const itemsEndpoint =
    selectedUserId === "All"
      ? `/api/fridgeItems/${fridgeId}`
      : `/api/fridgeItems/${fridgeId}/${selectedUserId}`;

  const { data: items, loading: itemsLoading, refetch: refetchItems } =
    useFetch<FridgeItem[]>(itemsEndpoint, true);

  const isReadOnly = selectedUserId === "All";

  // Fetch fridge members
  const fetchMembers = useCallback(async () => {
    if (!fridgeId) return;
    try {
      const data = await api.getFridgeMembers(fridgeId);
      setMembers(data ?? []);
    } catch {
      // silent
    }
  }, [fridgeId]);

  useEffect(() => {
    fetchMembers();
  }, [fetchMembers]);

  const isCreator = members.some(
    (m) => m.email === authState.email && m.memberRole === "Creator",
  );

  const handleInvite = async () => {
    if (!fridgeId || !inviteEmail) return;
    setInviting(true);
    try {
      await api.inviteUser(fridgeId, inviteEmail);
      toast.success("Invite sent!", { position: "bottom-center", autoClose: 1500 });
      setInviteEmail("");
      setInviteDialogOpen(false);
      fetchMembers();
    } catch {
      toast.error("Failed to send invite. Check that the email is registered.", { position: "bottom-center", autoClose: 2500 });
    } finally {
      setInviting(false);
    }
  };

  const handleConsume = useCallback(
    async (fridgeItemId: string, unit: string) => {
      const amount = consumeAmounts[fridgeItemId] ?? 0;
      if (amount < 1) {
        toast.error("Invalid amount!", { position: "bottom-center", autoClose: 1500 });
        return;
      }
      await submit(
        `/api/fridgeItems/${fridgeId}/consume`,
        {
          fridgeItemId,
          userId: selectedUserId,
          amountValue: { value: amount, unit },
        },
        { auth: true, successMessage: "Consumed!", errorMessage: "Can't consume fridge item!" },
      );
      setConsumeAmounts((prev) => ({ ...prev, [fridgeItemId]: 0 }));
      refetchItems();
    },
    [consumeAmounts, fridgeId, selectedUserId, submit, refetchItems],
  );

  const handleFindRecipes = async () => {
    if (selectedItems.length === 0) {
      toast.error("Select items first!", { position: "bottom-center", autoClose: 1500 });
      return;
    }
    try {
      const found = await api.post<Recipe[]>("/api/recipes/find", {
        foodProducts: selectedItems,
      });
      if (found.length > 0) {
        setRecipes(found);
        setCarouselOpen(true);
      } else {
        toast.info("No matching recipes found", { position: "bottom-center", autoClose: 1500 });
      }
    } catch {
      toast.error("Can't find recipe!", { position: "bottom-center", autoClose: 1500 });
    }
  };

  const columns: GridColDef[] = useMemo(
    () => [
      {
        field: "productName",
        headerName: "Product",
        flex: 1,
        minWidth: 160,
      },
      {
        field: "categoryName",
        headerName: "Category",
        flex: 1,
        minWidth: 120,
        renderCell: (params) => (
          <Chip
            label={params.value || "—"}
            size="small"
            variant="outlined"
            sx={{ borderRadius: 2, fontWeight: 500 }}
          />
        ),
      },
      {
        field: "userName",
        headerName: "User",
        width: 160,
        renderCell: (params) => {
          const color = params.row.userColor ?? "#000";
          const name = params.row.userName ?? params.row.userEmail ?? "—";
          return (
            <Stack direction="row" spacing={1} alignItems="center" sx={{ height: "100%" }}>
              <Box sx={{ width: 10, height: 10, borderRadius: "50%", bgcolor: color, flexShrink: 0 }} />
              <Typography variant="body2" noWrap>{name}</Typography>
            </Stack>
          );
        },
      },
      {
        field: "value",
        headerName: "Amount",
        width: 100,
        renderCell: (params) => (
          <Chip
            label={`${params.row.value} ${params.row.unit}`}
            size="small"
            color="primary"
            variant="outlined"
          />
        ),
      },
      {
        field: "consume",
        headerName: "",
        width: 260,
        sortable: false,
        filterable: false,
        renderCell: (params) => (
          <Stack direction="row" spacing={1} alignItems="center" sx={{ height: "100%" }}>
            <TextField
              type="number"
              size="small"
              disabled={isReadOnly}
              value={consumeAmounts[params.row.fridgeItemId] ?? ""}
              onChange={(e) =>
                setConsumeAmounts((prev) => ({
                  ...prev,
                  [params.row.fridgeItemId]: Math.max(0, parseInt(e.target.value) || 0),
                }))
              }
              slotProps={{ htmlInput: { min: 0, max: 10000, style: { width: 70 } } }}
            />
            <Button
              size="small"
              variant="contained"
              disabled={isReadOnly}
              startIcon={<FastfoodIcon />}
              onClick={() => handleConsume(params.row.fridgeItemId, params.row.unit)}
            >
              Consume
            </Button>
          </Stack>
        ),
      },
    ],
    [isReadOnly, consumeAmounts, handleConsume],
  );

  const rows = useMemo(
    () => (items ?? []).map((item, i) => ({ ...item, id: item.fridgeItemId ?? i })),
    [items],
  );

  const refetchAll = () => {
    refetchUsers();
    refetchItems();
  };

  return (
    <Container maxWidth="lg">
      {/* Hero header */}
      <Paper
        elevation={0}
        sx={{
          p: 3,
          mb: 3,
          borderRadius: 4,
          background: "linear-gradient(135deg, #e0f7fa 0%, #e8f5f3 50%, #fff3e0 100%)",
          display: "flex",
          alignItems: "center",
          gap: 2,
        }}
      >
        <Box sx={{ fontSize: 48, lineHeight: 1 }}>🍱</Box>
        <Box>
          <Typography variant="h4" fontWeight={700} color="primary.dark">
            Fridge Items
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {items?.length
              ? `${items.length} item${items.length !== 1 ? "s" : ""} stored`
              : "No items yet — add something!"}
          </Typography>
        </Box>
      </Paper>

      {/* User selector */}
      <Paper sx={{ p: 2, mb: 2, borderRadius: 3 }}>
        <Typography variant="subtitle2" sx={{ mb: 1, color: "text.secondary" }}>
          Select User
        </Typography>
        {usersLoading ? (
          <CircularProgress size={24} />
        ) : (
          <ToggleButtonGroup
            value={selectedUserId}
            exclusive
            onChange={(_, val) => val && setSelectedUserId(val)}
            size="small"
            sx={{ flexWrap: "wrap", gap: 0.5 }}
          >
            {users.map((u) => (
              <ToggleButton key={u.id} value={u.id} sx={{ borderRadius: 2 }}>
                {u.name}
              </ToggleButton>
            ))}
          </ToggleButtonGroup>
        )}
      </Paper>

      {/* Actions */}
      <Stack direction="row" spacing={1} sx={{ mb: 2 }} flexWrap="wrap" useFlexGap>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          disabled={isReadOnly}
          onClick={() => setItemDialogOpen(true)}
        >
          Add Item
        </Button>
        <Button variant="outlined" startIcon={<PersonAddIcon />} onClick={() => setUserDialogOpen(true)}>
          Add User
        </Button>
        <Button variant="outlined" startIcon={<SearchIcon />} onClick={handleFindRecipes}>
          Find Recipes
        </Button>
        <Button variant="outlined" startIcon={<GroupIcon />} onClick={() => setMembersDialogOpen(true)}>
          Members ({members.length})
        </Button>
        {isCreator && (
          <Button variant="outlined" color="secondary" startIcon={<SendIcon />} onClick={() => setInviteDialogOpen(true)}>
            Invite User
          </Button>
        )}
        <Button variant="outlined" startIcon={<RefreshIcon />} onClick={refetchAll}>
          Refresh
        </Button>
      </Stack>

      {/* Data grid */}
      {itemsLoading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
          <CircularProgress />
        </Box>
      ) : rows.length === 0 ? (
        <Paper sx={{ textAlign: "center", py: 8, borderRadius: 3 }}>
          <Box sx={{ fontSize: 72, mb: 1 }}>🍽️</Box>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            This fridge is empty
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Select a user and add your first item!
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            disabled={isReadOnly}
            onClick={() => setItemDialogOpen(true)}
          >
            Add First Item
          </Button>
        </Paper>
      ) : (
        <Paper sx={{ borderRadius: 3, overflow: "hidden" }}>
          <DataGrid
            rows={rows}
            columns={columns}
            autoHeight
            pageSizeOptions={[10, 20, 50]}
            initialState={{ pagination: { paginationModel: { pageSize: 20 } } }}
            checkboxSelection={!isReadOnly}
            onRowSelectionModelChange={(ids) => {
              const foodProductIds = rows
                .filter((r) => ids.includes(r.id))
                .map((r) => r.foodProductId);
              setSelectedItems(foodProductIds);
            }}
            disableRowSelectionOnClick
            getRowClassName={(params) => (params.row.userColor ? `row-color-${params.row.id}` : "")}
            sx={{
              border: "none",
              ...Object.fromEntries(
                rows.filter((r) => r.userColor).map((r) => [
                  `& .row-color-${r.id}`,
                  { backgroundColor: `${r.userColor}14` },
                ]),
              ),
            }}
          />
        </Paper>
      )}

      <NewUserDialog
        fridgeId={fridgeId!}
        open={userDialogOpen}
        onClose={() => {
          setUserDialogOpen(false);
          refetchAll();
        }}
      />
      <NewFridgeItemDialog
        fridgeId={fridgeId!}
        selectedUserId={selectedUserId}
        open={itemDialogOpen}
        onClose={() => {
          setItemDialogOpen(false);
          refetchItems();
        }}
      />
      {carouselOpen && recipes.length > 0 && (
        <RecipeCarouselDialog
          open={carouselOpen}
          onClose={() => {
            setCarouselOpen(false);
            refetchItems();
          }}
          recipes={recipes}
          userId={selectedUserId}
          fridgeId={fridgeId!}
        />
      )}

      {/* Members Dialog */}
      <Dialog open={membersDialogOpen} onClose={() => setMembersDialogOpen(false)} maxWidth="xs" fullWidth>
        <DialogTitle>
          <Stack direction="row" spacing={1} alignItems="center">
            <Box sx={{ fontSize: 24, lineHeight: 1 }}>👥</Box>
            <span>Fridge Members</span>
          </Stack>
        </DialogTitle>
        <DialogContent dividers>
          <List dense>
            {members.map((m) => (
              <ListItem key={m.id}>
                <ListItemAvatar>
                  <Avatar sx={{ bgcolor: m.color, width: 32, height: 32, fontSize: 14 }}>
                    {((m.name ?? m.email)?.[0] ?? "?").toUpperCase()}
                  </Avatar>
                </ListItemAvatar>
                <ListItemText
                  primary={
                    <Stack direction="row" spacing={0.5} alignItems="center">
                      <Typography variant="body2" fontWeight={600}>
                        {m.name || m.email}
                      </Typography>
                      {m.memberRole === "Creator" && (
                        <Tooltip title="Creator">
                          <StarIcon sx={{ fontSize: 16, color: "warning.main" }} />
                        </Tooltip>
                      )}
                    </Stack>
                  }
                  secondary={m.email}
                />
                <Chip
                  label={m.memberRole}
                  size="small"
                  color={m.memberRole === "Creator" ? "warning" : "default"}
                  variant="outlined"
                />
              </ListItem>
            ))}
          </List>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setMembersDialogOpen(false)}>Close</Button>
        </DialogActions>
      </Dialog>

      {/* Invite Dialog */}
      <Dialog open={inviteDialogOpen} onClose={() => setInviteDialogOpen(false)} maxWidth="xs" fullWidth>
        <DialogTitle>
          <Stack direction="row" spacing={1} alignItems="center">
            <Box sx={{ fontSize: 24, lineHeight: 1 }}>✉️</Box>
            <span>Invite User to Fridge</span>
          </Stack>
        </DialogTitle>
        <DialogContent>
          <TextField
            label="User email"
            type="email"
            fullWidth
            value={inviteEmail}
            onChange={(e) => setInviteEmail(e.target.value)}
            sx={{ mt: 1 }}
            autoFocus
            onKeyDown={(e) => e.key === "Enter" && handleInvite()}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setInviteDialogOpen(false)}>Cancel</Button>
          <Button variant="contained" startIcon={<SendIcon />} onClick={handleInvite} disabled={inviting || !inviteEmail}>
            {inviting ? "Sending..." : "Send Invite"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}
