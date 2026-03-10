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
  FormControlLabel,
  Checkbox,
  Alert,
  AlertTitle,
} from "@mui/material";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import RefreshIcon from "@mui/icons-material/Refresh";
import AddIcon from "@mui/icons-material/Add";
import FastfoodIcon from "@mui/icons-material/Fastfood";
import SearchIcon from "@mui/icons-material/Search";
import StarIcon from "@mui/icons-material/Star";
import GroupIcon from "@mui/icons-material/Group";
import SendIcon from "@mui/icons-material/Send";
import DeleteIcon from "@mui/icons-material/Delete";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";
import { useParams } from "react-router-dom";
import { useFetch, useSubmit } from "@/hooks/useApi";
import NewFridgeItemDialog from "@/components/dialogs/NewFridgeItemDialog";
import RecipeCarouselDialog from "@/components/dialogs/RecipeCarouselDialog";
import type { FridgeItem, FridgeMember, Recipe, ExpiringItem, FridgeScore } from "@/types";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import { useAuth } from "@/context/AuthContext";

/* ── Helpers ── */
function daysUntil(dateStr: string): number {
  const exp = new Date(dateStr);
  const now = new Date();
  return Math.ceil((exp.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
}

function expiryColor(days: number): "error" | "warning" | "success" | "default" {
  if (days <= 0) return "error";
  if (days <= 3) return "warning";
  return "success";
}

function expiryLabel(days: number): string {
  if (days < 0) return `Expired ${Math.abs(days)}d ago`;
  if (days === 0) return "Expires today!";
  if (days === 1) return "Expires tomorrow";
  return `${days}d left`;
}

function scoreEmoji(score: number): string {
  if (score >= 2000) return "\u{1F3C6}"; // trophy
  if (score >= 1500) return "\u{2B50}";  // star
  if (score >= 1000) return "\u{1F44D}"; // thumbs up
  if (score >= 500) return "\u{26A0}\u{FE0F}";  // warning
  return "\u{1F4A9}"; // poop
}

export default function FridgeItemsDashboard() {
  const { fridgeId } = useParams<{ fridgeId: string }>();
  const { state: authState } = useAuth();
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const [showAll, setShowAll] = useState(false);
  const [selectedItems, setSelectedItems] = useState<number[]>([]);
  const [consumeAmounts, setConsumeAmounts] = useState<Record<string, number>>({});

  const [itemDialogOpen, setItemDialogOpen] = useState(false);
  const [carouselOpen, setCarouselOpen] = useState(false);
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [wasteDialogOpen, setWasteDialogOpen] = useState(false);
  const [wasteItemId, setWasteItemId] = useState<string>("");
  const [wasteReason, setWasteReason] = useState("");

  // Members & invite state
  const [membersDialogOpen, setMembersDialogOpen] = useState(false);
  const [inviteDialogOpen, setInviteDialogOpen] = useState(false);
  const [inviteEmail, setInviteEmail] = useState("");
  const [inviting, setInviting] = useState(false);

  const { submit } = useSubmit();

  // Fetch members from the correct endpoint
  const { data: membersData, loading: membersLoading, refetch: refetchMembers } =
    useFetch<FridgeMember[]>(`/api/fridges/${fridgeId}/members`, true);

  const members = useMemo(() => membersData ?? [], [membersData]);

  // Only accepted members can be selected in the toggle group
  const acceptedMembers = useMemo(
    () => members.filter((m) => m.status === "Accepted"),
    [members],
  );

  // Auto-select the current user's member ID once members are loaded
  useEffect(() => {
    if (selectedUserId || acceptedMembers.length === 0) return;
    const myMember = acceptedMembers.find((m) => m.email === authState.email);
    if (myMember) {
      setSelectedUserId(String(myMember.id));
    } else if (acceptedMembers.length > 0) {
      setSelectedUserId(String(acceptedMembers[0]!.id));
    }
  }, [acceptedMembers, authState.email, selectedUserId]);

  const itemsEndpoint = showAll
    ? `/api/fridgeItems/${fridgeId}`
    : selectedUserId
      ? `/api/fridgeItems/${fridgeId}/${selectedUserId}`
      : `/api/fridgeItems/${fridgeId}`;

  const { data: items, loading: itemsLoading, refetch: refetchItems } =
    useFetch<FridgeItem[]>(itemsEndpoint, true);

  // Expiring items & fridge score
  const { data: expiringItems, refetch: refetchExpiring } =
    useFetch<ExpiringItem[]>(`/api/fridgeItems/${fridgeId}/expiring?days=3`, true);

  const { data: fridgeScore, refetch: refetchScore } =
    useFetch<FridgeScore>(`/api/fridgeItems/${fridgeId}/score`, true);

  const isReadOnly = showAll || !selectedUserId;

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
      refetchMembers();
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
          memberId: Number(selectedUserId),
          amountValue: { value: amount, unit },
        },
        { auth: true, successMessage: "Consumed!", errorMessage: "Can't consume fridge item!" },
      );
      setConsumeAmounts((prev) => ({ ...prev, [fridgeItemId]: 0 }));
      refetchItems();
      refetchScore();
    },
    [consumeAmounts, fridgeId, selectedUserId, submit, refetchItems, refetchScore],
  );

  const handleWaste = useCallback(async () => {
    if (!wasteItemId) return;
    await submit(
      `/api/fridgeItems/${fridgeId}/waste`,
      {
        fridgeItemId: wasteItemId,
        memberId: Number(selectedUserId),
        reason: wasteReason || "No reason given",
      },
      { auth: true, successMessage: "Item marked as wasted", errorMessage: "Can't waste item!" },
    );
    setWasteDialogOpen(false);
    setWasteItemId("");
    setWasteReason("");
    refetchItems();
    refetchScore();
  }, [wasteItemId, wasteReason, fridgeId, selectedUserId, submit, refetchItems, refetchScore]);

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
        field: "expirationDate",
        headerName: "Expires",
        width: 140,
        renderCell: (params) => {
          const days = daysUntil(params.value);
          return (
            <Tooltip title={new Date(params.value).toLocaleDateString()}>
              <Chip
                label={expiryLabel(days)}
                size="small"
                color={expiryColor(days)}
                variant={days <= 0 ? "filled" : "outlined"}
                icon={days <= 1 ? <WarningAmberIcon /> : undefined}
              />
            </Tooltip>
          );
        },
      },
      {
        field: "consume",
        headerName: "",
        width: 350,
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
            <Tooltip title="Mark as wasted (-25 pts)">
              <span>
                <Button
                  size="small"
                  variant="outlined"
                  color="error"
                  disabled={isReadOnly}
                  onClick={() => { setWasteItemId(params.row.fridgeItemId); setWasteDialogOpen(true); }}
                  sx={{ minWidth: 36, px: 0.5 }}
                >
                  <DeleteIcon fontSize="small" />
                </Button>
              </span>
            </Tooltip>
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
    refetchMembers();
    refetchItems();
    refetchExpiring();
    refetchScore();
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
          justifyContent: "space-between",
          gap: 2,
        }}
      >
        <Stack direction="row" alignItems="center" gap={2}>
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
        </Stack>

        {/* Fridge score badge */}
        {fridgeScore && (
          <Tooltip title={`Fridge eco score: ${fridgeScore.rank}`}>
            <Paper
              elevation={2}
              sx={{
                px: 2, py: 1, borderRadius: 3,
                background: fridgeScore.wasteScore >= 1500 ? "linear-gradient(135deg, #fff9c4, #ffe082)" :
                            fridgeScore.wasteScore >= 1000 ? "linear-gradient(135deg, #e8f5e9, #c8e6c9)" :
                            "linear-gradient(135deg, #ffebee, #ffcdd2)",
                display: "flex", alignItems: "center", gap: 1,
              }}
            >
              <Box sx={{ fontSize: 28 }}>{scoreEmoji(fridgeScore.wasteScore)}</Box>
              <Box>
                <Typography variant="h6" fontWeight={700} lineHeight={1}>{fridgeScore.wasteScore}</Typography>
                <Typography variant="caption" color="text.secondary">{fridgeScore.rank}</Typography>
              </Box>
            </Paper>
          </Tooltip>
        )}
      </Paper>

      {/* Expiring items warning */}
      {expiringItems && expiringItems.length > 0 && (
        <Alert
          severity="warning"
          icon={<WarningAmberIcon />}
          sx={{ mb: 2, borderRadius: 3 }}
        >
          <AlertTitle>Items expiring soon!</AlertTitle>
          {expiringItems.slice(0, 5).map((ei) => (
            <Typography key={ei.fridgeItemId} variant="body2">
              <strong>{ei.productName}</strong> — {ei.daysUntilExpiry <= 0 ? "EXPIRED" : `${ei.daysUntilExpiry} day(s) left`}
              {ei.userName ? ` (${ei.userName})` : ""}
            </Typography>
          ))}
          {expiringItems.length > 5 && (
            <Typography variant="body2" color="text.secondary">
              ...and {expiringItems.length - 5} more
            </Typography>
          )}
        </Alert>
      )}

      {/* User selector + Show all checkbox */}
      <Paper sx={{ p: 2, mb: 2, borderRadius: 3 }}>
        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 1 }}>
          <Typography variant="subtitle2" color="text.secondary">
            Select User
          </Typography>
          <FormControlLabel
            control={
              <Checkbox
                checked={showAll}
                onChange={(e) => setShowAll(e.target.checked)}
                size="small"
              />
            }
            label={<Typography variant="body2">Show all products</Typography>}
          />
        </Stack>
        {membersLoading ? (
          <CircularProgress size={24} />
        ) : acceptedMembers.length === 0 ? (
          <Typography variant="body2" color="text.secondary">
            No members yet — invite someone to your fridge!
          </Typography>
        ) : (
          <ToggleButtonGroup
            value={selectedUserId}
            exclusive
            onChange={(_, val) => val && setSelectedUserId(val)}
            size="small"
            sx={{ flexWrap: "wrap", gap: 0.5 }}
          >
            {acceptedMembers.map((m) => (
              <ToggleButton key={m.id} value={String(m.id)} sx={{ borderRadius: 2 }}>
                <Box sx={{ width: 8, height: 8, borderRadius: "50%", bgcolor: m.color, mr: 1 }} />
                {m.name || m.email}
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

      <NewFridgeItemDialog
        fridgeId={fridgeId!}
        memberId={Number(selectedUserId)}
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
          memberId={Number(selectedUserId)}
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
                  label={m.status === "Pending" ? "Pending" : m.memberRole}
                  size="small"
                  color={
                    m.status === "Pending"
                      ? "info"
                      : m.memberRole === "Creator"
                        ? "warning"
                        : "default"
                  }
                  variant="outlined"
                  sx={m.status === "Pending" ? { fontStyle: "italic", opacity: 0.75 } : undefined}
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

      {/* Waste Dialog */}
      <Dialog
        open={wasteDialogOpen}
        onClose={() => { setWasteDialogOpen(false); setWasteReason(""); }}
        maxWidth="xs"
        fullWidth
      >
        <DialogTitle>
          <Stack direction="row" spacing={1} alignItems="center">
            <DeleteIcon color="error" />
            <span>Mark Item as Wasted</span>
          </Stack>
        </DialogTitle>
        <DialogContent>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Wasting food decreases the fridge's eco score by 25 points. Please provide a reason.
          </Typography>
          <TextField
            label="Reason (optional)"
            fullWidth
            multiline
            minRows={2}
            value={wasteReason}
            onChange={(e) => setWasteReason(e.target.value)}
            autoFocus
            placeholder="e.g. expired, mouldy, forgot about it..."
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => { setWasteDialogOpen(false); setWasteReason(""); }}>Cancel</Button>
          <Button variant="contained" color="error" startIcon={<DeleteIcon />} onClick={handleWaste}>
            Confirm Waste
          </Button>
        </DialogActions>
      </Dialog>

    </Container>
  );
}
