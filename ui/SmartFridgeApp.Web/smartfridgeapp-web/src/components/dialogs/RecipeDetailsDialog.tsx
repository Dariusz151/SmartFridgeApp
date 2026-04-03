import { useMemo, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Typography,
  Divider,
  Chip,
  CircularProgress,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import CancelIcon from "@mui/icons-material/Cancel";
import HelpOutlineIcon from "@mui/icons-material/HelpOutline";
import PlaylistAddIcon from "@mui/icons-material/PlaylistAdd";
import type { Recipe, RecipeFoodProduct, MissingProduct } from "@/types";
import { api } from "@/services/api";
import { toast } from "react-toastify";

interface Props {
  open: boolean;
  onClose: () => void;
  recipe: Recipe;
  kitchenId?: string;
}

function parseFoodProducts(raw: string | RecipeFoodProduct[]): RecipeFoodProduct[] {
  if (Array.isArray(raw)) return raw;
  try {
    const parsed = JSON.parse(raw);
    const list =
      parsed?.ArrayOfFoodProductDetails?.FoodProductDetails ?? parsed;
    if (!Array.isArray(list)) return [list];
    return list.map((item: Record<string, unknown>) => ({
      foodProductId: item.FoodProductId ?? item.foodProductId,
      foodProductName: (item.FoodProductName ?? item.foodProductName) as string,
      amountValue: {
        value: (item.AmountValue as Record<string, unknown>)?.Value ??
               (item.amountValue as Record<string, unknown>)?.value ?? 0,
        unit: (item.AmountValue as Record<string, unknown>)?.Unit ??
              (item.amountValue as Record<string, unknown>)?.unit ?? "NotAssigned",
      },
    })) as RecipeFoodProduct[];
  } catch {
    return [];
  }
}

export default function RecipeDetailsDialog({ open, onClose, recipe, kitchenId }: Props) {
  const foodProducts = useMemo(
    () => parseFoodProducts(recipe.foodProducts),
    [recipe.foodProducts],
  );

  const [missingProducts, setMissingProducts] = useState<MissingProduct[]>([]);
  const [missingOpen, setMissingOpen] = useState(false);
  const [missingLoading, setMissingLoading] = useState(false);
  const [addingToList, setAddingToList] = useState(false);

  const handleShowMissing = async () => {
    if (!kitchenId || !recipe.recipeId) return;
    setMissingLoading(true);
    try {
      const missing = await api.get<MissingProduct[]>(
        `/api/recipes/kitchens/${kitchenId}/${recipe.recipeId}/missing-products`,
        true,
      );
      setMissingProducts(missing);
      setMissingOpen(true);
    } catch {
      toast.error("Failed to check missing products", { position: "bottom-center", autoClose: 1500 });
    } finally {
      setMissingLoading(false);
    }
  };

  const handleAddMissingToShoppingList = async () => {
    if (!kitchenId || !recipe.recipeId) return;
    setAddingToList(true);
    try {
      await api.post(
        `/api/recipes/kitchens/${kitchenId}/${recipe.recipeId}/add-missing-to-shopping-list`,
        null,
        true,
      );
      toast.success("Missing products added to shopping list!", { position: "bottom-center", autoClose: 1500 });
      setMissingOpen(false);
    } catch {
      toast.error("Failed to add to shopping list", { position: "bottom-center", autoClose: 1500 });
    } finally {
      setAddingToList(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle sx={{ fontWeight: 600 }}>
        {recipe.recipeName ?? recipe.name}
      </DialogTitle>
      <DialogContent dividers>
        <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
          Ingredients
        </Typography>
        <List dense>
          {foodProducts.map((fp) => (
            <ListItem key={fp.foodProductId} disablePadding sx={{ py: 0.5 }}>
              <ListItemIcon sx={{ minWidth: 32 }}>
                <CheckCircleOutlineIcon fontSize="small" color="primary" />
              </ListItemIcon>
              <ListItemText
                primary={fp.foodProductName}
                secondary={
                  <Chip
                    label={`${fp.amountValue.value} ${fp.amountValue.unit}`}
                    size="small"
                    variant="outlined"
                    sx={{ mt: 0.25 }}
                  />
                }
              />
            </ListItem>
          ))}
        </List>
        {recipe.description && (
          <>
            <Divider sx={{ my: 2 }} />
            <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 0.5 }}>
              Description
            </Typography>
            <Typography variant="body2" sx={{ whiteSpace: "pre-wrap" }}>
              {recipe.description}
            </Typography>
          </>
        )}

        {missingOpen && (
          <>
            <Divider sx={{ my: 2 }} />
            <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
              Missing Products
            </Typography>
            {missingProducts.length === 0 ? (
              <Typography variant="body2" color="success.main">
                You have all the ingredients!
              </Typography>
            ) : (
              <>
                <List dense>
                  {missingProducts.map((mp) => (
                    <ListItem key={mp.foodProductId} disablePadding sx={{ py: 0.25 }}>
                      <ListItemIcon sx={{ minWidth: 32 }}>
                        <CancelIcon fontSize="small" color="error" />
                      </ListItemIcon>
                      <ListItemText primary={mp.foodProductName} />
                    </ListItem>
                  ))}
                </List>
                <Button
                  variant="outlined"
                  color="secondary"
                  size="small"
                  startIcon={addingToList ? <CircularProgress size={16} /> : <PlaylistAddIcon />}
                  disabled={addingToList}
                  onClick={handleAddMissingToShoppingList}
                  sx={{ mt: 1 }}
                >
                  Add all to Shopping List
                </Button>
              </>
            )}
          </>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} startIcon={<CloseIcon />}>
          Close
        </Button>
        {kitchenId && recipe.recipeId && (
          <Button
            variant="outlined"
            startIcon={missingLoading ? <CircularProgress size={16} /> : <HelpOutlineIcon />}
            disabled={missingLoading}
            onClick={handleShowMissing}
          >
            What's Missing?
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
