import { useMemo } from "react";
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
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import type { Recipe, RecipeFoodProduct } from "@/types";

interface Props {
  open: boolean;
  onClose: () => void;
  recipe: Recipe;
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

export default function RecipeDetailsDialog({ open, onClose, recipe }: Props) {
  const foodProducts = useMemo(
    () => parseFoodProducts(recipe.foodProducts),
    [recipe.foodProducts],
  );

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
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} startIcon={<CloseIcon />}>
          Close
        </Button>
      </DialogActions>
    </Dialog>
  );
}
