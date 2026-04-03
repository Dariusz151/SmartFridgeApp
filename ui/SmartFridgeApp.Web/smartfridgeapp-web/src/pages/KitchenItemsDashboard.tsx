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
  MenuItem,
  Autocomplete,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import RefreshIcon from "@mui/icons-material/Refresh";
import AddIcon from "@mui/icons-material/Add";
import FastfoodIcon from "@mui/icons-material/Fastfood";
import SearchIcon from "@mui/icons-material/Search";
import StarIcon from "@mui/icons-material/Star";
import GroupIcon from "@mui/icons-material/Group";
import SendIcon from "@mui/icons-material/Send";
import DeleteIcon from "@mui/icons-material/Delete";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import PlaylistAddIcon from "@mui/icons-material/PlaylistAdd";
import { useParams } from "react-router-dom";
import { useFetch, useSubmit } from "@/hooks/useApi";
import NewStockItemDialog from "@/components/dialogs/NewStockItemDialog";
import RecipeCarouselDialog from "@/components/dialogs/RecipeCarouselDialog";
import type { FridgeItem, KitchenMember, Kitchen, Recipe, ExpiringItem, KitchenScore, ShoppingStatus, FoodProduct, ProductVariant, StorageLocation, ItemTag } from "@/types";
import { STORAGE_LOCATIONS, ITEM_TAGS } from "@/types";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import { useAuth } from "@/context/AuthContext";
import { setMainKitchen } from "@/hooks/useMainKitchen";
import ShoppingListPanel from "@/components/ShoppingListPanel";

/* ── Helpers ── */
function daysUntil(dateStr: string): number {
  const exp = new Date(dateStr);
  const now = new Date();
  return Math.ceil((exp.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
}

function expiryDots(days: number): { count: number; color: string } {
  if (days <= 0) return { count: 1, color: "#d32f2f" };       // expired — 1 red
  if (days === 1) return { count: 1, color: "#ff9800" };      // tomorrow — 1 orange
  if (days <= 3) return { count: 2, color: "#ff9800" };       // 2-3 days — 2 orange
  if (days <= 7) return { count: 3, color: "#4caf50" };       // 4-7 days — 3 green
  return { count: 4, color: "#2e7d32" };                      // 8+ days — 4 dark green
}

function formatAmount(amount: number, unit: string): string {
  if (unit === "NotAssigned") return String(amount);
  if (unit === "Grams") return amount >= 1000 ? `${(amount / 1000).toFixed(1)} kg` : `${amount} g`;
  if (unit === "Mililiter") return amount >= 1000 ? `${(amount / 1000).toFixed(1)} L` : `${amount} ml`;
  if (unit === "Pieces") return `${amount} pcs`;
  return `${amount} ${unit}`;
}

function scoreColor(score: number): string {
  if (score >= 1500) return "#f9a825";
  if (score >= 1000) return "#4caf50";
  if (score >= 500) return "#ff9800";
  return "#d32f2f";
}

function expiryLabel(days: number): string {
  if (days < 0) return `Expired ${Math.abs(days)}d ago`;
  if (days === 0) return "Expires today!";
  if (days === 1) return "Expires tomorrow";
  return `${days}d left`;
}



export default function KitchenItemsDashboard() {
  const { kitchenId } = useParams<{ kitchenId: string }>();
  const { state: authState } = useAuth();
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const [showAll, setShowAll] = useState(false);
  const [selectedItems, setSelectedItems] = useState<number[]>([]);
  const [consumeAmounts, setConsumeAmounts] = useState<Record<string, number>>({});
  const [filterTags, setFilterTags] = useState<ItemTag[]>([]);

  const [itemDialogOpen, setItemDialogOpen] = useState(false);
  const [shoppingListOpen, setShoppingListOpen] = useState(false);
  const [shoppingListRefreshKey, setShoppingListRefreshKey] = useState(0);
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

  // Fetch kitchens to resolve kitchen name for "main kitchen" feature
  const kitchensEndpoint = authState.isAdmin ? "/api/Kitchens/all" : "/api/Kitchens";
  const { data: kitchensData } = useFetch<Kitchen[]>(kitchensEndpoint, true);

  // Set this kitchen as the main kitchen when entering
  useEffect(() => {
    if (!kitchenId || !kitchensData) return;
    const kitchen = kitchensData.find((k) => k.id === kitchenId);
    if (kitchen) setMainKitchen(kitchen.id, kitchen.name);
  }, [kitchenId, kitchensData]);

  // Fetch members from the correct endpoint
  const { data: membersData, loading: membersLoading, refetch: refetchMembers } =
    useFetch<KitchenMember[]>(`/api/Kitchens/${kitchenId}/members`, true);

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
    ? `/api/Kitchens/${kitchenId}/inventory`
    : selectedUserId
      ? `/api/Kitchens/${kitchenId}/inventory/member/${selectedUserId}`
      : `/api/Kitchens/${kitchenId}/inventory`;

  const { data: items, loading: itemsLoading, refreshing: itemsRefreshing, refetch: refetchItems, setData: setItems } =
    useFetch<FridgeItem[]>(itemsEndpoint, true);

  const { data: foodProducts } = useFetch<FoodProduct[]>("/api/foodProducts");

  const foodProductMap = useMemo(
    () => new Map((foodProducts ?? []).map((fp) => [fp.foodProductId, fp])),
    [foodProducts],
  );

  // Variant name lookup — fetch for product IDs that appear in items with variantId
  const [variantMap, setVariantMap] = useState<Map<number, string>>(new Map());
  useEffect(() => {
    if (!items) return;
    const productIdsWithVariants = [...new Set(items.filter((i) => i.variantId).map((i) => i.foodProductId))];
    if (productIdsWithVariants.length === 0) return;
    Promise.all(
      productIdsWithVariants.map((pid) => api.get<ProductVariant[]>(`/api/foodProducts/${pid}/variants`, true)),
    ).then((results) => {
      const map = new Map<number, string>();
      results.flat().forEach((v) => map.set(v.variantId, v.name));
      setVariantMap(map);
    }).catch(() => {});
  }, [items]);

  // Kitchen details — fetch manually so we can retry once auth token is available
  const [kitchen, setKitchen] = useState<Kitchen | undefined>();
  const refetchKitchens = useCallback(() => {
    api.get<Kitchen[]>("/api/Kitchens", true)
      .then((list) => setKitchen(list.find((k) => k.id === kitchenId)))
      .catch(() => {});
  }, [kitchenId]);
  useEffect(() => { refetchKitchens(); }, [refetchKitchens]);

  const memberMap = useMemo(
    () => new Map(members.map((m) => [m.id, m])),
    [members],
  );

  // Expiring items & Kitchen score
  const { data: expiringItems, refetch: refetchExpiring } =
    useFetch<ExpiringItem[]>(`/api/Kitchens/${kitchenId}/inventory/expiring?days=3`, true);

  const { data: KitchenScore, refetch: refetchScore } =
    useFetch<KitchenScore>(`/api/Kitchens/${kitchenId}/inventory/score`, true);

  const { data: shoppingStatus, refetch: refetchShopping } =
    useFetch<ShoppingStatus>(`/api/Kitchens/${kitchenId}/inventory/shopping-status`, true);

  const isReadOnly = showAll || !selectedUserId;

  const isCreator = members.some(
    (m) => m.email === authState.email && m.memberRole === "Creator",
  );

  const handleInvite = async () => {
    if (!kitchenId || !inviteEmail) return;
    setInviting(true);
    try {
      await api.inviteUser(kitchenId, inviteEmail);
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
    async (stockItemId: string, unit: string) => {
      const consumeAmount = consumeAmounts[stockItemId] ?? 0;
      if (consumeAmount < 1) {
        toast.error("Invalid amount!", { position: "bottom-center", autoClose: 1500 });
        return;
      }
      const result = await submit(
        `/api/Kitchens/${kitchenId}/inventory/consume`,
        {
          stockItemId,
          memberId: Number(selectedUserId),
          amount: consumeAmount,
          unit,
        },
        { auth: true, successMessage: "Consumed!", errorMessage: "Can't consume Kitchen item!" },
      );
      setConsumeAmounts((prev) => ({ ...prev, [stockItemId]: 0 }));
      if (result !== null) {
        setItems((prev) =>
          (prev ?? []).map((item) =>
            item.stockItemId === stockItemId
              ? { ...item, amount: Math.max(0, item.amount - consumeAmount) }
              : item,
          ).filter((item) => item.amount > 0),
        );
      }
      refetchItems();
      refetchExpiring();
      refetchScore();
      refetchShopping();
    },
    [consumeAmounts, kitchenId, selectedUserId, submit, setItems, refetchItems, refetchExpiring, refetchScore, refetchShopping],
  );

  const handleWaste = useCallback(async () => {
    if (!wasteItemId) return;
    await submit(
      `/api/Kitchens/${kitchenId}/inventory/${wasteItemId}/waste`,
      {
        stockItemId: wasteItemId,
        memberId: Number(selectedUserId),
        reason: wasteReason || "No reason given",
      },
      { auth: true, successMessage: "Item marked as wasted", errorMessage: "Can't waste item!" },
    );
    setWasteDialogOpen(false);
    setWasteItemId("");
    setWasteReason("");
    refetchItems();
    refetchExpiring();
    refetchScore();
    refetchShopping();
  }, [wasteItemId, wasteReason, kitchenId, selectedUserId, submit, refetchItems, refetchExpiring, refetchScore, refetchShopping]);

  const handleFindRecipes = async () => {
    try {
      const found = await api.post<Recipe[]>(`/api/recipes/kitchens/${kitchenId}/find`, {
        selectedFoodProductIds: selectedItems,
        memberId: selectedUserId ? Number(selectedUserId) : undefined,
      }, true);
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

  const handleAddToShoppingList = useCallback(async (productName: string) => {
    try {
      await api.post(`/api/kitchens/${kitchenId}/shopping-list`, { name: productName }, true);
      setShoppingListRefreshKey((k) => k + 1);
      toast.success(`"${productName}" added to shopping list`, { position: "bottom-center", autoClose: 1500 });
    } catch {
      toast.error("Failed to add to shopping list", { position: "bottom-center", autoClose: 1500 });
    }
  }, [kitchenId]);

  const rows = useMemo(
    () => [...(items ?? [])]
      .filter((item) => {
        if (filterTags.length > 0 && !filterTags.every((t) => item.tags?.includes(t))) return false;
        return true;
      })
      .sort((a, b) => new Date(a.expirationDate).getTime() - new Date(b.expirationDate).getTime())
      .map((item, i) => {
        const fp = foodProductMap.get(item.foodProductId);
        const member = memberMap.get(item.memberId);
        return {
          ...item,
          id: item.stockItemId ?? i,
          _optimistic: (item as any)._optimistic ?? false,
          productName: fp?.foodProductName ?? `Product #${item.foodProductId}`,
          variantName: (item.variantId && variantMap.get(item.variantId)) ?? "",
          categoryName: fp?.foodProductCategory ?? "",
          userName: member?.name || member?.email,
          userColor: member?.color,
          userEmail: member?.email,
        };
      }),
    [items, foodProductMap, memberMap, variantMap, filterTags],
  );

  const groupedRows = useMemo(() => {
    const locationOrder = STORAGE_LOCATIONS.map((l) => l.value);
    const map = new Map<string, typeof rows>();
    for (const row of rows) {
      const loc = row.location ?? "Fridge";
      if (!map.has(loc)) map.set(loc, []);
      map.get(loc)!.push(row);
    }
    return [...map.entries()].sort(
      ([a], [b]) => locationOrder.indexOf(a as StorageLocation) - locationOrder.indexOf(b as StorageLocation),
    );
  }, [rows]);

  const refetchAll = () => {
    refetchKitchens();
    refetchMembers();
    refetchItems();
    refetchExpiring();
    refetchScore();
    refetchShopping();
  };

  return (
    <Stack direction="row" spacing={2} sx={{ width: "100%" }}>
    <Container maxWidth={shoppingListOpen ? "lg" : false} sx={{ flex: 1, minWidth: 0, transition: "max-width 0.3s ease" }}>
      {/* Hero header */}
      <Paper
        elevation={0}
        sx={{
          px: 2.5, py: 1.5,
          mb: 2,
          borderRadius: 3,
          background: "linear-gradient(135deg, #e0f7fa 0%, #e8f5f3 50%, #fff3e0 100%)",
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          gap: 1.5,
        }}
      >
        <Stack direction="row" alignItems="center" gap={1.5}>
          <Box sx={{ fontSize: 32, lineHeight: 1 }}>🍱</Box>
          <Box>
            <Typography variant="h6" fontWeight={700} color="primary.dark" lineHeight={1.3}>
              {kitchen?.name ?? "Kitchen Items"}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              {[
                kitchen?.address && `📍 ${kitchen.address}`,
                items?.length
                  ? `${items.length} item${items.length !== 1 ? "s" : ""}`
                  : "No items yet",
              ].filter(Boolean).join(" · ")}
            </Typography>
          </Box>
        </Stack>

        {/* Kitchen score badge */}
        {KitchenScore && (
          <Tooltip title={`Kitchen eco score: ${KitchenScore.rank}`}>
            <Paper
              elevation={2}
              sx={{
                px: 1.5, py: 0.5, borderRadius: 2,
                display: "flex", alignItems: "center", gap: 0.5,
              }}
            >
              <Typography variant="h6" fontWeight={700} lineHeight={1} sx={{ color: scoreColor(KitchenScore.wasteScore) }}>
                {KitchenScore.wasteScore}
              </Typography>
              <Typography variant="caption" color="text.secondary">{KitchenScore.rank}</Typography>
            </Paper>
          </Tooltip>
        )}
      </Paper>

      {/* Expiring items warning + shopping low banner */}
      {shoppingStatus?.isShoppingNeeded && (
        <Alert severity="error" sx={{ borderRadius: 3, mb: 2 }}>
          <AlertTitle>🛒 Kitchen is running low!</AlertTitle>
          <Typography variant="body2">
            Only <strong>{shoppingStatus.activeItemCount}</strong> item{shoppingStatus.activeItemCount !== 1 ? "s" : ""} left
            {" "}(average is <strong>{shoppingStatus.averageItemCount.toFixed(1)}</strong>). Time to go shopping!
          </Typography>
        </Alert>
      )}

      {/* User selector */}
      <Stack direction="row" alignItems="center" spacing={1} sx={{ mb: 2, flexWrap: "wrap" }}>
        {membersLoading ? (
          <CircularProgress size={18} />
        ) : acceptedMembers.length > 0 && (
          <ToggleButtonGroup
            value={selectedUserId}
            exclusive
            onChange={(_, val) => val && setSelectedUserId(val)}
            size="small"
            sx={{ gap: 0.5 }}
          >
            {acceptedMembers.map((m) => (
              <ToggleButton key={m.id} value={String(m.id)} sx={{ borderRadius: 2, py: 0.25, px: 1.5, textTransform: "none", fontSize: "0.8rem" }}>
                <Box sx={{ width: 8, height: 8, borderRadius: "50%", bgcolor: m.color, mr: 0.75 }} />
                {m.name || m.email}
              </ToggleButton>
            ))}
          </ToggleButtonGroup>
        )}
        <FormControlLabel
          control={
            <Checkbox
              checked={showAll}
              onChange={(e) => setShowAll(e.target.checked)}
              size="small"
            />
          }
          label={<Typography variant="body2" color="text.secondary">Show all</Typography>}
          sx={{ ml: "auto" }}
        />
      </Stack>

      {/* Filters */}
      <Paper sx={{ p: 2, mb: 2, borderRadius: 3 }}>
        <Stack direction="row" spacing={2} alignItems="center" flexWrap="wrap" useFlexGap>
          <Typography variant="subtitle2" color="text.secondary" sx={{ mr: 1 }}>
            Filters
          </Typography>
          <Autocomplete
            multiple
            size="small"
            options={ITEM_TAGS}
            getOptionLabel={(option) => option.label}
            value={ITEM_TAGS.filter((t) => filterTags.includes(t.value))}
            onChange={(_, val) => setFilterTags(val.map((v) => v.value))}
            renderTags={(value, getTagProps) =>
              value.map((option, index) => {
                const { key, ...rest } = getTagProps({ index });
                return <Chip key={key} label={option.label} size="small" {...rest} />;
              })
            }
            renderInput={(params) => (
              <TextField {...params} label="Tags" placeholder="Filter by tags..." />
            )}
            sx={{ minWidth: 250 }}
          />
          {filterTags.length > 0 && (
            <Button
              size="small"
              onClick={() => setFilterTags([])}
            >
              Clear filters
            </Button>
          )}
        </Stack>
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
        <Button
          variant={shoppingListOpen ? "contained" : "outlined"}
          color="secondary"
          startIcon={<ShoppingCartIcon />}
          onClick={() => setShoppingListOpen((v) => !v)}
        >
          Shopping List
        </Button>
      </Stack>

      {/* Grouped inventory */}
      {itemsLoading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
          <CircularProgress />
        </Box>
      ) : rows.length === 0 ? (
        <Paper sx={{ textAlign: "center", py: 8, borderRadius: 3 }}>
          <Box sx={{ fontSize: 72, mb: 1 }}>🍽️</Box>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            This Kitchen is empty
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
        <Box sx={{ position: "relative" }}>
          {itemsRefreshing && (
            <Box
              sx={{
                position: "absolute",
                top: 0, left: 0, right: 0, height: 2, zIndex: 10, overflow: "hidden",
                "&::after": {
                  content: '""', position: "absolute",
                  top: 0, left: 0, right: 0, height: "100%",
                  bgcolor: "primary.main",
                  animation: "indeterminate 1.5s infinite ease-in-out",
                  "@keyframes indeterminate": {
                    "0%": { transform: "translateX(-100%)" },
                    "100%": { transform: "translateX(100%)" },
                  },
                },
              }}
            />
          )}
          {groupedRows.map(([location, locRows]) => {
            const locDef = STORAGE_LOCATIONS.find((l) => l.value === location);
            return (
              <Accordion key={location} defaultExpanded sx={{ mb: 1, borderRadius: 2, "&:before": { display: "none" }, "&.Mui-expanded": { mt: 0 } }}>
                <AccordionSummary expandIcon={<ExpandMoreIcon />} sx={{ borderRadius: 2 }}>
                  <Stack direction="row" alignItems="center" spacing={1}>
                    <Box component="span" sx={{ fontSize: 20, lineHeight: 1 }}>{locDef?.icon}</Box>
                    <Typography variant="subtitle1" fontWeight={600}>{locDef?.label ?? location}</Typography>
                    <Chip label={locRows.length} size="small" sx={{ ml: 0.5, height: 20, fontSize: "0.72rem" }} />
                  </Stack>
                </AccordionSummary>
                <AccordionDetails sx={{ p: 0 }}>
                  <TableContainer>
                    <Table size="small">
                      <TableHead>
                        <TableRow sx={{ "& th": { fontWeight: 600, fontSize: "0.78rem", color: "text.secondary" } }}>
                          <TableCell>Product</TableCell>
                          {showAll && <TableCell width={120}>User</TableCell>}
                          <TableCell width={90}>Qty</TableCell>
                          <TableCell width={70}>Expires</TableCell>
                          <TableCell width={270} align="right" />
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {locRows.map((row) => {
                          const itemTags: string[] = row.tags ?? [];
                          const days = daysUntil(row.expirationDate);
                          const { count, color: dotColor } = expiryDots(days);
                          return (
                            <TableRow
                              key={row.stockItemId}
                              sx={{
                                opacity: row._optimistic ? 0.6 : 1,
                                bgcolor: row.userColor ? `${row.userColor}14` : undefined,
                              }}
                            >
                              {/* Product */}
                              <TableCell>
                                <Stack direction="row" alignItems="center" spacing={0.75}>
                                  <Typography variant="body2" fontWeight={500}>
                                    {row.productName}
                                    {row.variantName && (
                                      <Box component="span" sx={{ ml: 0.75, fontSize: "0.75rem", fontWeight: 400, opacity: 0.55 }}>
                                        {row.variantName}
                                      </Box>
                                    )}
                                  </Typography>
                                  {itemTags.length > 0 && (
                                    <Stack direction="row" spacing={0.25} alignItems="center" sx={{ opacity: 0.5 }}>
                                      {itemTags.map((t) => {
                                        const tagDef = ITEM_TAGS.find((it) => it.value === t);
                                        return (
                                          <Chip key={t} label={tagDef?.label ?? t} size="small" variant="outlined"
                                            sx={{ height: 16, fontSize: 9, borderRadius: 1 }} />
                                        );
                                      })}
                                    </Stack>
                                  )}
                                </Stack>
                              </TableCell>
                              {/* User (show all mode) */}
                              {showAll && (
                                <TableCell>
                                  <Stack direction="row" spacing={0.75} alignItems="center">
                                    <Box sx={{ width: 8, height: 8, borderRadius: "50%", bgcolor: row.userColor ?? "#000", flexShrink: 0 }} />
                                    <Typography variant="body2" noWrap>{row.userName ?? row.userEmail ?? "—"}</Typography>
                                  </Stack>
                                </TableCell>
                              )}
                              {/* Qty */}
                              <TableCell>
                                <Typography variant="body2" fontWeight={500}>
                                  {formatAmount(row.amount, row.unit)}
                                </Typography>
                              </TableCell>
                              {/* Expires */}
                              <TableCell>
                                <Tooltip title={`${expiryLabel(days)} — ${new Date(row.expirationDate).toLocaleDateString()}`}>
                                  <Stack direction="row" spacing={0.4} alignItems="center">
                                    {Array.from({ length: count }, (_, i) => (
                                      <Box key={i} sx={{ width: 8, height: 8, borderRadius: "50%", bgcolor: dotColor, boxShadow: `0 0 4px ${dotColor}80` }} />
                                    ))}
                                  </Stack>
                                </Tooltip>
                              </TableCell>
                              {/* Actions */}
                              <TableCell align="right">
                                <Stack direction="row" spacing={0.5} alignItems="center" justifyContent="flex-end">
                                  <TextField
                                    type="number"
                                    size="small"
                                    disabled={isReadOnly || row._optimistic}
                                    value={consumeAmounts[row.stockItemId] ?? ""}
                                    onChange={(e) =>
                                      setConsumeAmounts((prev) => ({
                                        ...prev,
                                        [row.stockItemId]: Math.max(0, parseInt(e.target.value) || 0),
                                      }))
                                    }
                                    slotProps={{ htmlInput: { min: 0, max: 10000, style: { width: 52 } } }}
                                  />
                                  <Tooltip title="Consume">
                                    <span>
                                      <Button size="small" variant="contained"
                                        disabled={isReadOnly || row._optimistic}
                                        onClick={() => handleConsume(row.stockItemId, row.unit)}
                                        sx={{ minWidth: 34, px: 0.75 }}
                                      >
                                        <FastfoodIcon fontSize="small" />
                                      </Button>
                                    </span>
                                  </Tooltip>
                                  <Tooltip title={row._optimistic ? "Syncing..." : "Waste (−25 pts)"}>
                                    <span>
                                      <Button size="small" variant="outlined" color="error"
                                        disabled={isReadOnly || row._optimistic}
                                        onClick={() => { setWasteItemId(row.stockItemId); setWasteDialogOpen(true); }}
                                        sx={{ minWidth: 34, px: 0.5 }}
                                      >
                                        <DeleteIcon fontSize="small" />
                                      </Button>
                                    </span>
                                  </Tooltip>
                                  <Tooltip title="Add to shopping list">
                                    <span>
                                      <Button size="small" variant="outlined" color="secondary"
                                        onClick={() => handleAddToShoppingList(row.productName)}
                                        sx={{ minWidth: 34, px: 0.5 }}
                                      >
                                        <PlaylistAddIcon fontSize="small" />
                                      </Button>
                                    </span>
                                  </Tooltip>
                                </Stack>
                              </TableCell>
                            </TableRow>
                          );
                        })}
                      </TableBody>
                    </Table>
                  </TableContainer>
                </AccordionDetails>
              </Accordion>
            );
          })}
        </Box>
      )}

      <NewStockItemDialog
        kitchenId={kitchenId!}
        memberId={Number(selectedUserId)}
        open={itemDialogOpen}
        onClose={() => {
          setItemDialogOpen(false);
        }}
        onItemAdded={(newItem) => {
          setItems((prev) => [...(prev ?? []), { ...newItem, _optimistic: true }]);
          refetchItems();
          refetchExpiring();
          refetchShopping();
        }}
      />
      {carouselOpen && recipes.length > 0 && (
        <RecipeCarouselDialog
          open={carouselOpen}
          onClose={() => {
            setCarouselOpen(false);
            refetchItems();
            refetchExpiring();
          }}
          recipes={recipes}
          memberId={Number(selectedUserId)}
          kitchenId={kitchenId!}
        />
      )}

      {/* Members Dialog */}
      <Dialog open={membersDialogOpen} onClose={() => setMembersDialogOpen(false)} maxWidth="xs" fullWidth>
        <DialogTitle>
          <Stack direction="row" spacing={1} alignItems="center">
            <Box sx={{ fontSize: 24, lineHeight: 1 }}>👥</Box>
            <span>Kitchen Members</span>
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
            <span>Invite User to Kitchen</span>
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
            Wasting food decreases the Kitchen's eco score by 25 points. Please provide a reason.
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

    <ShoppingListPanel
      kitchenId={kitchenId!}
      open={shoppingListOpen}
      onClose={() => setShoppingListOpen(false)}
      refreshKey={shoppingListRefreshKey}
    />
    </Stack>
  );
}
