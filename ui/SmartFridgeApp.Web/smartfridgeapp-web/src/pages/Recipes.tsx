import { useState, useMemo } from "react";
import { useNavigate } from "react-router-dom";
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
import VisibilityIcon from "@mui/icons-material/Visibility";
import { useAuth } from "@/context/AuthContext";
import { useFetch } from "@/hooks/useApi";
import RecipeDetailsDialog from "@/components/dialogs/RecipeDetailsDialog";
import type { Recipe } from "@/types";

const difficultyLabels: Record<string, { label: string; color: "success" | "warning" | "error" }> = {
  "1": { label: "Easy", color: "success" },
  "2": { label: "Medium", color: "warning" },
  "3": { label: "Hard", color: "error" },
};

const categoryLabels: Record<string, { label: string; emoji: string; color: "info" | "warning" | "secondary" }> = {
  "1": { label: "Breakfast", emoji: "🍳", color: "info" },
  "2": { label: "Dinner", emoji: "🍽️", color: "warning" },
  "3": { label: "Supper", emoji: "🥘", color: "secondary" },
};

export default function Recipes() {
  const { state } = useAuth();
  const navigate = useNavigate();
  const { data: rawRecipes, loading, refetch } = useFetch<Recipe[]>("/api/recipes");
  const [selectedRecipe, setSelectedRecipe] = useState<Recipe | null>(null);

  const columns: GridColDef[] = useMemo(
    () => [
      {
        field: "recipeName",
        headerName: "Name",
        flex: 1,
        minWidth: 180,
        renderCell: (params) => (
          <Typography variant="body2" fontWeight={600}>
            {params.value}
          </Typography>
        ),
      },
      {
        field: "recipeCategory",
        headerName: "Category",
        width: 140,
        renderCell: (params) => {
          const c = categoryLabels[String(params.value)];
          return c ? (
            <Chip label={`${c.emoji} ${c.label}`} size="small" color={c.color} sx={{ borderRadius: 2 }} />
          ) : params.value;
        },
      },
      {
        field: "requiredTime",
        headerName: "Time",
        width: 110,
        renderCell: (params) => (
          <Chip label={`⏱️ ${params.value} min`} size="small" variant="outlined" />
        ),
      },
      {
        field: "levelOfDifficulty",
        headerName: "Difficulty",
        width: 120,
        renderCell: (params) => {
          const d = difficultyLabels[String(params.value)];
          return d ? <Chip label={d.label} size="small" color={d.color} /> : params.value;
        },
      },
      {
        field: "actions",
        headerName: "",
        width: 130,
        sortable: false,
        filterable: false,
        renderCell: (params) => (
          <Button
            size="small"
            variant="contained"
            startIcon={<VisibilityIcon />}
            onClick={() => setSelectedRecipe(params.row as Recipe)}
          >
            Details
          </Button>
        ),
      },
    ],
    [],
  );

  const rows = useMemo(
    () => (rawRecipes ?? []).map((r, i) => ({ ...r, id: r.recipeId ?? i })),
    [rawRecipes],
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
          background: "linear-gradient(135deg, #fff3e0 0%, #ffe0b2 50%, #e0f7fa 100%)",
          display: "flex",
          alignItems: "center",
          gap: 2,
        }}
      >
        <Box sx={{ fontSize: 48, lineHeight: 1 }}>📖</Box>
        <Box>
          <Typography variant="h4" fontWeight={700} color="primary.dark">
            Recipes
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {rows.length
              ? `${rows.length} recipe${rows.length !== 1 ? "s" : ""} in your collection`
              : "No recipes yet — create one!"}
          </Typography>
        </Box>
      </Paper>

      <Box sx={{ mb: 3, display: "flex", alignItems: "center", justifyContent: "flex-end", flexWrap: "wrap", gap: 1 }}>
        <Stack direction="row" spacing={1}>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            disabled={!state.isAdmin}
            onClick={() => navigate("/recipes/add")}
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
          <Box sx={{ fontSize: 72, mb: 1 }}>📖</Box>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            No recipes yet
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Start building your recipe collection!
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            disabled={!state.isAdmin}
            onClick={() => navigate("/recipes/add")}
          >
            Create First Recipe
          </Button>
        </Paper>
      ) : (
        <Paper sx={{ borderRadius: 3, overflow: "hidden" }}>
          <DataGrid
            rows={rows}
            columns={columns}
            autoHeight
            pageSizeOptions={[5, 10, 25]}
            initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
            disableRowSelectionOnClick
            sx={{ border: "none" }}
          />
        </Paper>
      )}

      {selectedRecipe && (
        <RecipeDetailsDialog
          open={Boolean(selectedRecipe)}
          onClose={() => setSelectedRecipe(null)}
          recipe={selectedRecipe}
        />
      )}
    </Container>
  );
}
