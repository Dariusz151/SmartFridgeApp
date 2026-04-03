import { useState, useMemo, useEffect } from "react";
import {
  Container,
  Button,
  Typography,
  Paper,
  Box,
  CircularProgress,
  Stack,
  IconButton,
  Collapse,
  TextField,
  List,
  ListItem,
  ListItemText,
  MenuItem,
  Chip,
} from "@mui/material";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ExpandLessIcon from "@mui/icons-material/ExpandLess";
import EditIcon from "@mui/icons-material/Edit";
import { useAuth } from "@/context/AuthContext";
import { useFetch, useSubmit } from "@/hooks/useApi";
import { api } from "@/services/api";
import NewFoodProductDialog from "@/components/dialogs/NewFoodProductDialog";
import type { FoodProduct, FoodProductCategory, ProductVariant, StorageLocation, Unit } from "@/types";
import { STORAGE_LOCATIONS } from "@/types";
import { toast } from "react-toastify";

export default function FoodProducts() {
  const { state } = useAuth();
  const { data: products, loading, refetch } = useFetch<FoodProduct[]>("/api/foodProducts");
  const { data: categories } = useFetch<FoodProductCategory[]>("/api/foodProducts/categories");
  const { submit } = useSubmit();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [filterCategory, setFilterCategory] = useState<string>("");

  // Variant management
  const [expandedId, setExpandedId] = useState<number | null>(null);
  const [variants, setVariants] = useState<ProductVariant[]>([]);
  const [variantsLoading, setVariantsLoading] = useState(false);
  const [newVariantName, setNewVariantName] = useState("");
  const [newVariantBarcode, setNewVariantBarcode] = useState("");

  // Defaults editing
  const [defaultsEditId, setDefaultsEditId] = useState<number | null>(null);
  const [editDefaultLocation, setEditDefaultLocation] = useState<StorageLocation | "">("");
  const [editDefaultUnit, setEditDefaultUnit] = useState<Unit | "">("");

  const toggleDefaults = (p: FoodProduct) => {
    if (defaultsEditId === p.foodProductId) {
      setDefaultsEditId(null);
      return;
    }
    setDefaultsEditId(p.foodProductId);
    setEditDefaultLocation(p.defaultStorageLocation ?? "");
    setEditDefaultUnit(p.defaultUnit ?? "");
  };

  const handleSaveDefaults = async () => {
    if (!defaultsEditId) return;
    const product = products?.find((p) => p.foodProductId === defaultsEditId);
    if (!product) return;
    try {
      await api.put(
        "/api/foodProducts",
        {
          foodProductId: defaultsEditId,
          foodProductName: product.foodProductName,
          defaultStorageLocation: editDefaultLocation || null,
          defaultUnit: editDefaultUnit || null,
        },
        true,
      );
      toast.success("Defaults saved!", { position: "bottom-center", autoClose: 1500 });
      setDefaultsEditId(null);
      refetch();
    } catch {
      toast.error("Can't save defaults", { position: "bottom-center", autoClose: 2000 });
    }
  };

  const toggleVariants = async (foodProductId: number) => {
    if (expandedId === foodProductId) {
      setExpandedId(null);
      return;
    }
    setExpandedId(foodProductId);
    setVariantsLoading(true);
    try {
      const data = await api.get<ProductVariant[]>(`/api/foodProducts/${foodProductId}/variants`);
      setVariants(data);
    } catch {
      setVariants([]);
    } finally {
      setVariantsLoading(false);
    }
  };

  const handleAddVariant = async () => {
    if (!expandedId || !newVariantName.trim()) return;
    await submit(
      `/api/foodProducts/${expandedId}/variants`,
      { name: newVariantName.trim(), barcode: newVariantBarcode.trim() || null },
      { auth: true, successMessage: "Variant added!", errorMessage: "Can't add variant" },
    );
    setNewVariantName("");
    setNewVariantBarcode("");
    // Refresh variants
    const data = await api.get<ProductVariant[]>(`/api/foodProducts/${expandedId}/variants`);
    setVariants(data);
  };

  const uniqueCategories = useMemo(
    () => [...new Set((products ?? []).map((p) => p.foodProductCategory).filter(Boolean))] as string[],
    [products],
  );

  const columns: GridColDef[] = [
    { field: "idx", headerName: "#", width: 60 },
    {
      field: "foodProductName",
      headerName: "Product",
      flex: 1,
      minWidth: 220,
      renderCell: (params) => (
        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ height: "100%", width: "100%" }}>
          <Typography variant="body2" fontWeight={500} noWrap>
            {params.value}
            {params.row.foodProductCategory && (
              <Box component="span" sx={{ ml: 0.75, fontSize: "0.75rem", fontWeight: 400, opacity: 0.5 }}>
                {params.row.foodProductCategory}
              </Box>
            )}
          </Typography>
          {params.row.variantCount > 0 && (
            <Typography variant="caption" sx={{ opacity: 0.4, fontSize: "0.7rem", whiteSpace: "nowrap", ml: 1 }}>
              {params.row.variantCount} variant{params.row.variantCount !== 1 ? "s" : ""}
            </Typography>
          )}
        </Stack>
      ),
    },
    {
      field: "defaults",
      headerName: "Defaults",
      width: 200,
      sortable: false,
      filterable: false,
      renderCell: (params) => {
        const loc = STORAGE_LOCATIONS.find((l) => l.value === params.row.defaultStorageLocation);
        return (
          <Stack direction="row" alignItems="center" spacing={0.5} sx={{ height: "100%" }}>
            {loc ? (
              <Chip
                label={<>{loc.icon} {loc.label}</>}
                size="small"
                variant="outlined"
                sx={{ fontSize: "0.72rem" }}
              />
            ) : (
              <Typography variant="caption" color="text.disabled">—</Typography>
            )}
            {params.row.defaultUnit && params.row.defaultUnit !== "NotAssigned" && (
              <Chip label={params.row.defaultUnit} size="small" sx={{ fontSize: "0.72rem" }} />
            )}
          </Stack>
        );
      },
    },
    {
      field: "editDefaults",
      headerName: "",
      width: 50,
      sortable: false,
      filterable: false,
      renderCell: (params) => (
        <IconButton
          size="small"
          disabled={!state.isAdmin}
          onClick={() => {
            const p = products?.find((pr) => pr.foodProductId === params.row.id);
            if (p) toggleDefaults(p);
          }}
        >
          <EditIcon fontSize="small" />
        </IconButton>
      ),
    },
    {
      field: "variants",
      headerName: "Variants",
      width: 80,
      sortable: false,
      filterable: false,
      renderCell: (params) => (
        <IconButton size="small" onClick={() => toggleVariants(params.row.id)}>
          {expandedId === params.row.id ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </IconButton>
      ),
    },
  ];

  const rows = useMemo(
    () =>
      (products ?? [])
        .filter((p) => !filterCategory || p.foodProductCategory === filterCategory)
        .map((p, i) => ({
          id: p.foodProductId,
          idx: i + 1,
          foodProductName: p.foodProductName,
          foodProductCategory: p.foodProductCategory ?? "",
          variantCount: p.variantCount ?? 0,
          defaultStorageLocation: p.defaultStorageLocation,
          defaultUnit: p.defaultUnit,
        })),
    [products, filterCategory],
  );

  return (
    <Container maxWidth="lg">
      {/* Hero header */}
      <Paper
        elevation={0}
        sx={{
          p: 3,
          mb: 3,
          borderRadius: 4,
          background: "linear-gradient(135deg, #e8f5e9 0%, #e0f7fa 50%, #fce4ec 100%)",
          display: "flex",
          alignItems: "center",
          gap: 2,
        }}
      >
        <Box sx={{ fontSize: 48, lineHeight: 1 }}>🥚</Box>
        <Box>
          <Typography variant="h4" fontWeight={700} color="primary.dark">
            Food Products
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {rows.length
              ? `${rows.length} product${rows.length !== 1 ? "s" : ""} in your catalog`
              : "No products yet — add some!"}
          </Typography>
        </Box>
      </Paper>

      <Stack direction="row" spacing={1} sx={{ mb: 2 }} alignItems="center" justifyContent="space-between" flexWrap="wrap" useFlexGap>
        <TextField
          label="Category"
          select
          size="small"
          value={filterCategory}
          onChange={(e) => setFilterCategory(e.target.value)}
          sx={{ minWidth: 180 }}
        >
          <MenuItem value="">All categories</MenuItem>
          {uniqueCategories.map((cat) => (
            <MenuItem key={cat} value={cat}>{cat}</MenuItem>
          ))}
        </TextField>
        <Stack direction="row" spacing={1}>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            disabled={!state.isAdmin}
            onClick={() => setDialogOpen(true)}
          >
            Add New
          </Button>
          <Button variant="outlined" startIcon={<RefreshIcon />} onClick={refetch}>
            Refresh
          </Button>
        </Stack>
      </Stack>

      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
          <CircularProgress />
        </Box>
      ) : rows.length === 0 ? (
        <Paper sx={{ textAlign: "center", py: 8, borderRadius: 3 }}>
          <Box sx={{ fontSize: 72, mb: 1 }}>🥚</Box>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            No food products yet
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Add your first food product to the catalog!
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            disabled={!state.isAdmin}
            onClick={() => setDialogOpen(true)}
          >
            Add First Product
          </Button>
        </Paper>
      ) : (
        <Paper sx={{ borderRadius: 3, overflow: "hidden" }}>
          <DataGrid
            rows={rows}
            columns={columns}
            autoHeight
            pageSizeOptions={[10, 25, 50, 100]}
            initialState={{ pagination: { paginationModel: { pageSize: 25 } } }}
            disableRowSelectionOnClick
            sx={{ border: "none" }}
          />
        </Paper>
      )}

      {/* Variant panel */}
      <Collapse in={expandedId !== null} unmountOnExit>
        <Paper sx={{ mt: 2, p: 2, borderRadius: 3 }}>
          <Typography variant="subtitle1" fontWeight={600} sx={{ mb: 1 }}>
            Variants for: {products?.find((p) => p.foodProductId === expandedId)?.foodProductName ?? "..."}
          </Typography>
          {variantsLoading ? (
            <CircularProgress size={24} />
          ) : variants.length === 0 ? (
            <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
              No variants yet
            </Typography>
          ) : (
            <List dense disablePadding>
              {variants.map((v) => (
                <ListItem key={v.variantId} sx={{ pl: 0 }}>
                  <ListItemText
                    primary={v.name}
                    secondary={v.barcode ? `Barcode: ${v.barcode}` : undefined}
                  />
                </ListItem>
              ))}
            </List>
          )}
          <Stack direction="row" spacing={1} sx={{ mt: 1 }} alignItems="center">
            <TextField
              size="small"
              label="Variant name"
              value={newVariantName}
              onChange={(e) => setNewVariantName(e.target.value)}
              sx={{ flex: 1 }}
            />
            <TextField
              size="small"
              label="Barcode (optional)"
              value={newVariantBarcode}
              onChange={(e) => setNewVariantBarcode(e.target.value)}
              sx={{ width: 200 }}
            />
            <Button
              variant="contained"
              size="small"
              startIcon={<AddIcon />}
              disabled={!newVariantName.trim()}
              onClick={handleAddVariant}
            >
              Add
            </Button>
          </Stack>
        </Paper>
      </Collapse>

      <NewFoodProductDialog
        categories={categories ?? []}
        open={dialogOpen}
        onClose={() => {
          setDialogOpen(false);
          refetch();
        }}
      />

      {/* Defaults edit panel */}
      <Collapse in={defaultsEditId !== null} unmountOnExit>
        <Paper sx={{ mt: 2, p: 2, borderRadius: 3 }}>
          <Typography variant="subtitle1" fontWeight={600} sx={{ mb: 2 }}>
            Edit defaults for: {products?.find((p) => p.foodProductId === defaultsEditId)?.foodProductName ?? "..."}
          </Typography>
          <Stack direction="row" spacing={2} alignItems="center" flexWrap="wrap" useFlexGap>
            <TextField
              label="Default Storage Location"
              select
              size="small"
              value={editDefaultLocation}
              onChange={(e) => setEditDefaultLocation(e.target.value as StorageLocation | "")}
              sx={{ minWidth: 200 }}
            >
              <MenuItem value="">— None —</MenuItem>
              {STORAGE_LOCATIONS.map((loc) => (
                <MenuItem key={loc.value} value={loc.value}>
                  <Box component="span" sx={{ mr: 1 }}>{loc.icon}</Box>{loc.label}
                </MenuItem>
              ))}
            </TextField>
            <TextField
              label="Default Unit"
              select
              size="small"
              value={editDefaultUnit}
              onChange={(e) => setEditDefaultUnit(e.target.value as Unit | "")}
              sx={{ minWidth: 150 }}
            >
              <MenuItem value="">— None —</MenuItem>
              <MenuItem value="Pieces">Pieces</MenuItem>
              <MenuItem value="Grams">Grams</MenuItem>
              <MenuItem value="Mililiter">Millilitres</MenuItem>
            </TextField>
            <Button variant="contained" size="small" onClick={handleSaveDefaults}>Save</Button>
            <Button size="small" onClick={() => setDefaultsEditId(null)}>Cancel</Button>
          </Stack>
        </Paper>
      </Collapse>
    </Container>
  );
}
