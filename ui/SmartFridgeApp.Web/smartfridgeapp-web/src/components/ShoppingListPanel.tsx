import { useState, useCallback, useEffect, useRef } from "react";
import {
  Paper,
  Typography,
  Stack,
  IconButton,
  TextField,
  Box,
  Tooltip,
  Chip,
  Collapse,
  Fade,
  InputAdornment,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import ShoppingCartCheckoutIcon from "@mui/icons-material/ShoppingCartCheckout";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import CloseIcon from "@mui/icons-material/Close";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import type { ShoppingListItem } from "@/types";
import { api } from "@/services/api";
import { toast } from "react-toastify";

interface ShoppingListPanelProps {
  kitchenId: string;
  open: boolean;
  onClose: () => void;
  refreshKey?: number;
}

export default function ShoppingListPanel({ kitchenId, open, onClose, refreshKey }: ShoppingListPanelProps) {
  const [items, setItems] = useState<ShoppingListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [newItemName, setNewItemName] = useState("");
  const [adding, setAdding] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);

  const fetchItems = useCallback(async () => {
    try {
      const data = await api.get<ShoppingListItem[]>(
        `/api/kitchens/${kitchenId}/shopping-list`,
        true,
      );
      setItems(data);
    } catch {
      toast.error("Failed to load shopping list", { position: "bottom-center", autoClose: 1500 });
    } finally {
      setLoading(false);
    }
  }, [kitchenId]);

  useEffect(() => {
    if (open) fetchItems();
  }, [open, fetchItems, refreshKey]);

  const handleAdd = async () => {
    const name = newItemName.trim();
    if (!name) return;

    setAdding(true);
    try {
      const item = await api.post<ShoppingListItem>(
        `/api/kitchens/${kitchenId}/shopping-list`,
        { name },
        true,
      );
      setItems((prev) => [item, ...prev]);
      setNewItemName("");
      inputRef.current?.focus();
    } catch {
      toast.error("Failed to add item", { position: "bottom-center", autoClose: 1500 });
    } finally {
      setAdding(false);
    }
  };

  const handleBuy = async (id: string) => {
    const prev = items;
    setItems((cur) => cur.filter((item) => item.id !== id));
    try {
      await api.post(`/api/kitchens/${kitchenId}/shopping-list/${id}/buy`, {}, true);
      toast.success("Item bought!", { position: "bottom-center", autoClose: 1000 });
    } catch {
      setItems(prev);
      toast.error("Failed to mark item as bought", { position: "bottom-center", autoClose: 1500 });
    }
  };

  const handleRemove = async (id: string) => {
    const prev = items;
    setItems((cur) => cur.filter((item) => item.id !== id));
    try {
      await api.delete(`/api/kitchens/${kitchenId}/shopping-list/${id}`, undefined, true);
    } catch {
      setItems(prev);
      toast.error("Failed to remove item", { position: "bottom-center", autoClose: 1500 });
    }
  };

  if (!open) return null;

  return (
    <Fade in={open}>
      <Paper
        elevation={3}
        sx={{
          width: 340,
          maxHeight: "calc(100vh - 140px)",
          display: "flex",
          flexDirection: "column",
          borderRadius: 3,
          overflow: "hidden",
          flexShrink: 0,
        }}
      >
        {/* Header */}
        <Stack
          direction="row"
          alignItems="center"
          justifyContent="space-between"
          sx={{
            px: 2,
            py: 1.5,
            background: "linear-gradient(135deg, #e0f7fa 0%, #fff3e0 100%)",
            borderBottom: "1px solid",
            borderColor: "divider",
          }}
        >
          <Stack direction="row" alignItems="center" spacing={1}>
            <ShoppingCartIcon color="primary" />
            <Typography variant="subtitle1" fontWeight={700}>
              Shopping List
            </Typography>
            {items.length > 0 && (
              <Chip
                label={items.length}
                size="small"
                color="primary"
                variant="outlined"
                sx={{ height: 22, fontSize: 11 }}
              />
            )}
          </Stack>
          <IconButton size="small" onClick={onClose}>
            <CloseIcon fontSize="small" />
          </IconButton>
        </Stack>

        {/* Add item input */}
        <Box sx={{ px: 2, pt: 1.5, pb: 1 }}>
          <TextField
            inputRef={inputRef}
            fullWidth
            size="small"
            placeholder="Add item..."
            value={newItemName}
            onChange={(e) => setNewItemName(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && handleAdd()}
            disabled={adding}
            slotProps={{
              input: {
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      size="small"
                      onClick={handleAdd}
                      disabled={!newItemName.trim() || adding}
                      color="primary"
                    >
                      <AddIcon fontSize="small" />
                    </IconButton>
                  </InputAdornment>
                ),
              },
            }}
            sx={{
              "& .MuiOutlinedInput-root": { borderRadius: 2 },
            }}
          />
        </Box>

        {/* Items list */}
        <Box
          sx={{
            flex: 1,
            overflowY: "auto",
            px: 1,
            py: 0.5,
            "&::-webkit-scrollbar": { width: 6 },
            "&::-webkit-scrollbar-thumb": {
              borderRadius: 3,
              bgcolor: "action.disabled",
            },
          }}
        >
          {loading ? (
            <Typography variant="body2" color="text.secondary" sx={{ textAlign: "center", py: 4 }}>
              Loading...
            </Typography>
          ) : items.length === 0 ? (
            <Stack alignItems="center" sx={{ py: 4, opacity: 0.5 }}>
              <Box sx={{ fontSize: 48, mb: 1 }}>🛒</Box>
              <Typography variant="body2" color="text.secondary">
                No items yet
              </Typography>
            </Stack>
          ) : (
            items.map((item) => (
              <ShoppingItem
                key={item.id}
                item={item}
                onBuy={handleBuy}
                onRemove={handleRemove}
              />
            ))
          )}
        </Box>
      </Paper>
    </Fade>
  );
}

function ShoppingItem({
  item,
  onBuy,
  onRemove,
}: {
  item: ShoppingListItem;
  onBuy: (id: string) => void;
  onRemove: (id: string) => void;
}) {
  return (
    <Collapse in appear>
      <Stack
        direction="row"
        alignItems="center"
        sx={{
          px: 1,
          py: 0.25,
          mx: 0.5,
          my: 0.25,
          borderRadius: 2,
          transition: "all 0.2s ease",
          "&:hover": {
            bgcolor: "action.hover",
            "& .action-btn": { opacity: 1 },
          },
        }}
      >
        <Typography
          variant="body2"
          sx={{
            flex: 1,
            ml: 0.5,
            fontWeight: 500,
            overflow: "hidden",
            textOverflow: "ellipsis",
            whiteSpace: "nowrap",
          }}
        >
          {item.name}
        </Typography>
        <Tooltip title="Bought">
          <IconButton
            size="small"
            onClick={() => onBuy(item.id)}
            color="success"
            className="action-btn"
            sx={{ opacity: 0, transition: "opacity 0.15s ease", p: 0.5 }}
          >
            <ShoppingCartCheckoutIcon sx={{ fontSize: 16 }} />
          </IconButton>
        </Tooltip>
        <Tooltip title="Remove">
          <IconButton
            size="small"
            onClick={() => onRemove(item.id)}
            className="action-btn"
            sx={{ opacity: 0, transition: "opacity 0.15s ease", p: 0.5 }}
          >
            <DeleteOutlineIcon sx={{ fontSize: 16 }} />
          </IconButton>
        </Tooltip>
      </Stack>
    </Collapse>
  );
}
