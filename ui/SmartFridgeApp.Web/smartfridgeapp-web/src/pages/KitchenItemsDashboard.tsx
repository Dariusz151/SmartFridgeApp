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
  const [filterLocation, setFilterLocation] = useState<StorageLocation | "">("");
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

  const handleAddToShoppingList = useCallback(async (productName: string) => {
    try {
      await api.post(`/api/kitchens/${kitchenId}/shopping-list`, { name: productName }, true);
      setShoppingListRefreshKey((k) => k + 1);
      toast.success(`"${productName}" added to shopping list`, { position: "bottom-center", autoClose: 1500 });
    } catch {
      toast.error("Failed to add to shopping list", { position: "bottom-center", autoClose: 1500 });
    }
  }, [kitchenId]);

  const columns: GridColDef[] = useMemo(
    () => {
      const cols: GridColDef[] = [
        {
          field: "productName",
          headerName: "Product",
          flex: 1,
          minWidth: 180,
          renderCell: (params) => {
            const loc = STORAGE_LOCATIONS.find((l) => l.value === params.row.location);
            const itemTags: string[] = params.row.tags ?? [];
            return (
              <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ height: "100%", width: "100%" }}>
                <Stack justifyContent="center" sx={{ minWidth: 0, flex: 1 }}>
                  <Stack direction="row" alignItems="center" spacing={0.75}>
                    {loc && <Box component="span" sx={{ fontSize: 14, lineHeight: 1 }}>{loc.icon}</Box>}
                    <Typography variant="body2" fontWeight={500} noWrap>
                      {params.value}
                      {params.row.variantName && (
                        <Box component="span" sx={{ ml: 0.75, fontSize: "0.75rem", fontWeight: 400, opacity: 0.55 }}>
                          {params.row.variantName}
                        </Box>
                      )}
                    </Typography>
                  </Stack>
                </Stack>
                {itemTags.length > 0 && (
                  <Stack direction="row" spacing={0.25} alignItems="center" sx={{ flexShrink: 0, ml: 1, opacity: 0.45 }}>
                    {itemTags.map((t) => {
                      const tagDef = ITEM_TAGS.find((it) => it.value === t);
                      return (
                        <Chip
                          key={t}
                          label={tagDef?.label ?? t}
                          size="small"
                          variant="outlined"
                          sx={{ height: 18, fontSize: 10, borderRadius: 1 }}
                        />
                      );
                    })}
                  </Stack>
                )}
              </Stack>
            );
          },
        },
      ];

      if (showAll) {
        cols.push({
          field: "userName",
          headerName: "User",
          width: 130,
          renderCell: (params) => {
            const color = params.row.userColor ?? "#000";
            const name = params.row.userName ?? params.row.userEmail ?? "—";
            return (
              <Stack direction="row" spacing={0.75} alignItems="center" sx={{ height: "100%" }}>
                <Box sx={{ width: 8, height: 8, borderRadius: "50%", bgcolor: color, flexShrink: 0 }} />
                <Typography variant="body2" noWrap>{name}</Typography>
              </Stack>
            );
          },
        });
      }

      cols.push(
        {
          field: "amount",
          headerName: "Qty",
          width: 80,
          renderCell: (params) => (
            <Typography variant="body2" fontWeight={500}>
              {formatAmount(params.row.amount, params.row.unit)}
            </Typography>
          ),
        },
        {
          field: "expirationDate",
          headerName: "Expires",
          width: 70,
          renderCell: (params) => {
            const days = daysUntil(params.value);
            const { count, color } = expiryDots(days);
            return (
              <Tooltip title={`${expiryLabel(days)} — ${new Date(params.value).toLocaleDateString()}`}>
                <Stack direction="row" spacing={0.4} alignItems="center" sx={{ height: "100%" }}>
                  {Array.from({ length: count }, (_, i) => (
                    <Box
                      key={i}
                      sx={{
                        width: 8, height: 8,
                        borderRadius: "50%",
                        bgcolor: color,
                        boxShadow: `0 0 4px ${color}80`,
                      }}
                    />
                  ))}
                </Stack>
              </Tooltip>
            );
          },
        },
        {
          field: "consume",
          headerName: "",
          width: 260,
          sortable: false,
          filterable: false,
          renderCell: (params) => (
            <Stack direction="row" spacing={0.5} alignItems="center" sx={{ height: "100%" }}>
              <TextField
                type="number"
                size="small"
                disabled={isReadOnly || params.row._optimistic}
                value={consumeAmounts[params.row.stockItemId] ?? ""}
                onChange={(e) =>
                  setConsumeAmounts((prev) => ({
                    ...prev,
                    [params.row.stockItemId]: Math.max(0, parseInt(e.target.value) || 0),
                  }))
                }
                slotProps={{ htmlInput: { min: 0, max: 10000, style: { width: 56 } } }}
              />
              <Tooltip title="Consume">
                <span>
                  <Button
                    size="small"
                    variant="contained"
                    disabled={isReadOnly || params.row._optimistic}
                    onClick={() => handleConsume(params.row.stockItemId, params.row.unit)}
                    sx={{ minWidth: 36, px: 1 }}
                  >
                    <FastfoodIcon fontSize="small" />
                  </Button>
                </span>
              </Tooltip>
              <Tooltip title={params.row._optimistic ? "Syncing..." : "Waste (−25 pts)"}>
                <span>
                  <Button
                    size="small"
                    variant="outlined"
                    color="error"
                    disabled={isReadOnly || params.row._optimistic}
                    onClick={() => { setWasteItemId(params.row.stockItemId); setWasteDialogOpen(true); }}
                    sx={{ minWidth: 36, px: 0.5 }}
                  >
                    <DeleteIcon fontSize="small" />
                  </Button>
                </span>
              </Tooltip>
              <Tooltip title="Add to shopping list">
                <span>
                  <Button
                    size="small"
                    variant="outlined"
                    color="secondary"
                    onClick={() => handleAddToShoppingList(params.row.productName)}
                    sx={{ minWidth: 36, px: 0.5 }}
                  >
                    <PlaylistAddIcon fontSize="small" />
                  </Button>
                </span>
              </Tooltip>
            </Stack>
          ),
        },
      );

      return cols;
    },
    [isReadOnly, showAll, consumeAmounts, handleConsume, handleAddToShoppingList],
  );

  const rows = useMemo(
    () => [...(items ?? [])]
      .filter((item) => {
        if (filterLocation && item.location !== filterLocation) return false;
        if (filterTags.length > 0 && !filterTags.every((t) => item.tags?.includes(t))) return false;
        return true;
      })
      .sort((a, b) => a.stockItemId.localeCompare(b.stockItemId))
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
    [items, foodProductMap, memberMap, variantMap, filterLocation, filterTags],
  );

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
              {kitchen?.name ?? "Kitchen Items"}
            </Typography>
            {kitchen?.address && (
              <Typography variant="body2" color="text.secondary" sx={{ opacity: 0.7 }}>
                📍 {kitchen.address}
              </Typography>
            )}
            {kitchen?.desc && (
              <Typography variant="caption" color="text.secondary" sx={{ opacity: 0.55, display: "block" }}>
                {kitchen.desc}
              </Typography>
            )}
            <Typography variant="body2" color="text.secondary" sx={{ mt: 0.25 }}>
              {items?.length
                ? `${items.length} item${items.length !== 1 ? "s" : ""} stored`
                : "No items yet — add something!"}
            </Typography>
          </Box>
        </Stack>

        {/* Kitchen score badge */}
        {KitchenScore && (
          <Tooltip title={`Kitchen eco score: ${KitchenScore.rank}`}>
            <Paper
              elevation={2}
              sx={{
                px: 2, py: 1, borderRadius: 3,
                display: "flex", alignItems: "center", gap: 0.5,
              }}
            >
              <Typography variant="h5" fontWeight={700} lineHeight={1} sx={{ color: scoreColor(KitchenScore.wasteScore) }}>
                {KitchenScore.wasteScore}
              </Typography>
              <Typography variant="caption" color="text.secondary">{KitchenScore.rank}</Typography>
            </Paper>
          </Tooltip>
        )}
      </Paper>

      {/* Expiring items warning + shopping low banner */}
      {(expiringItems && expiringItems.length > 0 || shoppingStatus?.isShoppingNeeded) && (
        <Stack spacing={1} sx={{ mb: 2 }}>
          {expiringItems && expiringItems.length > 0 && (
            <Alert severity="warning" icon={<WarningAmberIcon />} sx={{ borderRadius: 3 }}>
              <AlertTitle>Items expiring soon!</AlertTitle>
              {expiringItems.slice(0, 5).map((ei) => (
                <Typography key={ei.stockItemId} variant="body2">
                  <strong>{foodProductMap.get(ei.foodProductId)?.foodProductName ?? `Product #${ei.foodProductId}`}</strong> — {ei.daysUntilExpiry <= 0 ? "EXPIRED" : `${ei.daysUntilExpiry} day(s) left`}
                  {memberMap.get(ei.memberId)?.name ? ` (${memberMap.get(ei.memberId)?.name})` : ""}
                </Typography>
              ))}
              {expiringItems.length > 5 && (
                <Typography variant="body2" color="text.secondary">
                  ...and {expiringItems.length - 5} more
                </Typography>
              )}
            </Alert>
          )}
          {shoppingStatus?.isShoppingNeeded && (
            <Alert severity="error" sx={{ borderRadius: 3 }}>
              <AlertTitle>🛒 Kitchen is running low!</AlertTitle>
              <Typography variant="body2">
                Only <strong>{shoppingStatus.activeItemCount}</strong> item{shoppingStatus.activeItemCount !== 1 ? "s" : ""} left
                {" "}(average is <strong>{shoppingStatus.averageItemCount.toFixed(1)}</strong>). Time to go shopping!
              </Typography>
            </Alert>
          )}
        </Stack>
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
            No members yet — invite someone to your Kitchen!
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

      {/* Filters */}
      <Paper sx={{ p: 2, mb: 2, borderRadius: 3 }}>
        <Stack direction="row" spacing={2} alignItems="center" flexWrap="wrap" useFlexGap>
          <Typography variant="subtitle2" color="text.secondary" sx={{ mr: 1 }}>
            Filters
          </Typography>
          <TextField
            label="Location"
            select
            size="small"
            value={filterLocation}
            onChange={(e) => setFilterLocation(e.target.value as StorageLocation | "")}
            sx={{ minWidth: 150 }}
          >
            <MenuItem value="">All locations</MenuItem>
            {STORAGE_LOCATIONS.map((loc) => (
              <MenuItem key={loc.value} value={loc.value}>
                <Box component="span" sx={{ mr: 1 }}>{loc.icon}</Box>
                {loc.label}
              </MenuItem>
            ))}
          </TextField>
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
          {(filterLocation || filterTags.length > 0) && (
            <Button
              size="small"
              onClick={() => { setFilterLocation(""); setFilterTags([]); }}
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

      {/* Data grid */}
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
        <Paper sx={{ borderRadius: 3, overflow: "hidden", position: "relative" }}>
          {itemsRefreshing && (
            <Box
              sx={{
                position: "absolute",
                top: 0,
                left: 0,
                right: 0,
                height: 2,
                zIndex: 1,
                overflow: "hidden",
                "&::after": {
                  content: '""',
                  position: "absolute",
                  top: 0,
                  left: 0,
                  right: 0,
                  height: "100%",
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
          <DataGrid
            rows={rows}
            columns={columns}
            autoHeight
            pageSizeOptions={[10, 20, 50]}
            initialState={{ pagination: { paginationModel: { pageSize: 20 } }, sorting: { sortModel: [{ field: "expirationDate", sort: "asc" }] } }}
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
              transition: "opacity 0.15s ease",
              opacity: itemsRefreshing ? 0.7 : 1,
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
