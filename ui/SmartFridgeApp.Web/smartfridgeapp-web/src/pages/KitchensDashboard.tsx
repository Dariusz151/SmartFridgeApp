import { useState, useMemo, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import {
  Container,
  Button,
  Typography,
  Stack,
  Paper,
  Box,
  CircularProgress,
  Chip,
  alpha,
} from "@mui/material";
import { DataGrid, type GridColDef, type GridRowParams } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";
import DeleteIcon from "@mui/icons-material/Delete";
import OpenInNewIcon from "@mui/icons-material/OpenInNew";
import KitchenIcon from "@mui/icons-material/Kitchen";
import { useAuth } from "@/context/AuthContext";
import { useFetch, useSubmit } from "@/hooks/useApi";
import { useMainKitchen, setMainKitchen } from "@/hooks/useMainKitchen";
import NewKitchenDialog from "@/components/dialogs/NewKitchenDialog";
import type { Kitchen } from "@/types";

export default function KitchensDashboard() {
  const { state } = useAuth();  const navigate = useNavigate();  const fridgesEndpoint = state.isAdmin ? "/api/Kitchens/all" : "/api/Kitchens";
  const { data, loading, refetch } = useFetch<Kitchen[]>(fridgesEndpoint, true);
  const { submit } = useSubmit();
  const [dialogOpen, setDialogOpen] = useState(false);
  const autoNavigated = useRef(false);
  const mainKitchen = useMainKitchen();

  // Default main kitchen to first one when no main kitchen is set
  useEffect(() => {
    if (!loading && data && data.length > 0 && !mainKitchen) {
      setMainKitchen(data[0]!.id, data[0]!.name);
    }
  }, [loading, data, mainKitchen]);

  // Auto-navigate to Kitchen details when user belongs to exactly one Kitchen (first load only)
  useEffect(() => {
    if (!loading && data && data.length === 1 && !autoNavigated.current && !sessionStorage.getItem("fridges_visited")) {
      autoNavigated.current = true;
      sessionStorage.setItem("fridges_visited", "1");
      setMainKitchen(data[0]!.id, data[0]!.name);
      navigate(`/KitchenItems/${data[0]!.id}`, { replace: true });
    }
  }, [loading, data, navigate]);

  const handleDelete = async (kitchenId: string) => {
    await submit("/api/Kitchens", { kitchenId }, {
      method: "DELETE",
      auth: true,
      successMessage: "Kitchen deleted!",
      errorMessage: "Can't delete Kitchen!",
    });
    refetch();
  };

  const columns: GridColDef[] = useMemo(
    () => [
      {
        field: "name",
        headerName: "Name",
        flex: 1,
        minWidth: 200,
        renderCell: (params) => (
          <Stack direction="row" spacing={1.5} alignItems="center" sx={{ height: "100%" }}>
              <KitchenIcon sx={{ fontSize: 22, color: "primary.main" }} />
            <Typography fontWeight={600}>{params.value}</Typography>
            {mainKitchen?.id === params.row.id && (
              <Chip label="⭐ Main" size="small" color="warning" variant="outlined" sx={{ borderRadius: 2, ml: 0.5 }} />
            )}
          </Stack>
        ),
      },
      {
        field: "address",
        headerName: "Address",
        flex: 1,
        minWidth: 160,
        renderCell: (params) => (
          <Stack direction="row" spacing={1} alignItems="center" sx={{ height: "100%" }}>
            <Box sx={{ fontSize: 16, lineHeight: 1 }}>📍</Box>
            <Typography variant="body2" color="text.secondary">{params.value || "—"}</Typography>
          </Stack>
        ),
      },
      {
        field: "actions",
        headerName: "",
        width: 260,
        sortable: false,
        filterable: false,
        renderCell: (params) => (
          <Stack direction="row" spacing={1} alignItems="center" sx={{ height: "100%" }}>
            <Button
              size="small"
              variant="contained"
              startIcon={<OpenInNewIcon />}
              onClick={(e) => {
                e.stopPropagation();
                setMainKitchen(params.row.id, params.row.name);
                navigate(`/KitchenItems/${params.row.id}`);
              }}
            >
              Details
            </Button>
            <Button
              size="small"
              variant="outlined"
              color="error"
              startIcon={<DeleteIcon />}
              disabled={!state.isAdmin}
              onClick={() => handleDelete(params.row.id)}
            >
              Remove
            </Button>
          </Stack>
        ),
      },
    ],
    [state.isAdmin, mainKitchen],
  );

  const rows = useMemo(
    () => (data ?? []).map((f, i) => ({ ...f, _idx: i + 1 })),
    [data],
  );

  return (
    <Container maxWidth="lg">
      {/* Hero header */}
      <Paper
        sx={{
          mb: 3, p: 3, borderRadius: 4,
          background: "linear-gradient(135deg, #e8f5f3 0%, #e0f7fa 50%, #fff3e0 100%)",
          display: "flex", alignItems: "center", justifyContent: "space-between",
          flexWrap: "wrap", gap: 2, position: "relative", overflow: "hidden",
        }}
        elevation={0}
      >
        <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
          <KitchenIcon sx={{ fontSize: 52, color: "primary.dark", opacity: 0.85 }} />
          <Box>
            <Typography variant="h4" sx={{ fontWeight: 700, color: "primary.dark" }}>
              My Kitchens
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {rows.length === 0 ? "Get started by adding your first Kitchen!" : `You have ${rows.length} Kitchen${rows.length !== 1 ? "s" : ""}`}
            </Typography>
          </Box>
        </Box>
        <Stack direction="row" spacing={1}>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => setDialogOpen(true)}
          >
            Add New
          </Button>
          <Button
            variant="outlined"
            startIcon={<RefreshIcon />}
            onClick={refetch}
          >
            Refresh
          </Button>
        </Stack>
      </Paper>

      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
          <CircularProgress />
        </Box>
      ) : rows.length === 0 ? (
        <Paper sx={{ borderRadius: 4, py: 8, textAlign: "center" }}>
          <KitchenIcon sx={{ fontSize: 80, color: "primary.light", opacity: 0.6, mb: 2 }} />
          <Typography variant="h6" color="text.secondary" gutterBottom>
            No Kitchens yet
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            Add your first Kitchen and start tracking your food!
          </Typography>
          <Button variant="contained" startIcon={<AddIcon />} onClick={() => setDialogOpen(true)}>
            Add Your First Kitchen
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
            onRowClick={(params: GridRowParams) => {
              setMainKitchen(params.row.id, params.row.name);
              navigate(`/KitchenItems/${params.row.id}`);
            }}
            sx={{ border: "none", cursor: "pointer" }}
          />
        </Paper>
      )}

      <NewKitchenDialog
        open={dialogOpen}
        onClose={() => {
          setDialogOpen(false);
          refetch();
        }}
      />
    </Container>
  );
}
