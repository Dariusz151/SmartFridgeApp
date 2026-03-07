import { useState, useMemo } from "react";
import {
  Container,
  Button,
  Typography,
  Paper,
  Box,
  CircularProgress,
  Stack,
  Chip,
} from "@mui/material";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";
import { useAuth } from "@/context/AuthContext";
import { useFetch } from "@/hooks/useApi";
import NewFoodProductDialog from "@/components/dialogs/NewFoodProductDialog";
import type { FoodProduct, FoodProductCategory } from "@/types";

const categoryEmoji: Record<string, string> = {
  // Polish category names
  "Nabiał": "🧀",
  "Mięso": "🥩",
  "Warzywa": "🥦",
  "Owoce": "🍎",
  "Napoje": "🥤",
  "Pieczywo": "🍞",
  "Ryby": "🐟",
  "Mróżonki": "❄️",
  "Przekąski": "🍿",
  "Przyprawy": "🧂",
  "Słodycze": "🍫",
  "Jajka": "🥚",
  "Dania gotowe": "🍱",
  // English fallbacks
  "Dairy": "🧀",
  "Meat": "🥩",
  "Vegetables": "🥦",
  "Fruits": "🍎",
  "Beverages": "🥤",
  "Bakery": "🍞",
  "Seafood": "🐟",
  "Frozen": "❄️",
  "Snacks": "🍿",
  "Condiments": "🧂",
};

export default function FoodProducts() {
  const { state } = useAuth();
  const { data: products, loading, refetch } = useFetch<FoodProduct[]>("/api/foodProducts");
  const { data: categories } = useFetch<FoodProductCategory[]>("/api/foodProducts/categories");
  const [dialogOpen, setDialogOpen] = useState(false);

  const columns: GridColDef[] = [
    { field: "idx", headerName: "#", width: 70 },
    {
      field: "foodProductName",
      headerName: "Name",
      flex: 1,
      minWidth: 180,
    },
    {
      field: "foodProductCategory",
      headerName: "Category",
      flex: 1,
      minWidth: 140,
      renderCell: (params) => (
        <Chip
          label={`${categoryEmoji[params.value] ?? "📦"} ${params.value || "—"}`}
          size="small"
          variant="outlined"
          sx={{ borderRadius: 2, fontWeight: 500 }}
        />
      ),
    },
  ];

  const rows = useMemo(
    () =>
      (products ?? []).map((p, i) => ({
        id: p.foodProductId,
        idx: i + 1,
        foodProductName: p.foodProductName,
        foodProductCategory: p.foodProductCategory ?? "",
      })),
    [products],
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

      <Box sx={{ mb: 3, display: "flex", alignItems: "center", justifyContent: "flex-end", flexWrap: "wrap", gap: 1 }}>
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
      </Box>

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

      <NewFoodProductDialog
        categories={categories ?? []}
        open={dialogOpen}
        onClose={() => {
          setDialogOpen(false);
          refetch();
        }}
      />
    </Container>
  );
}
