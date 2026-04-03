import { useState } from "react";
import {
  Dialog,
  DialogActions,
  DialogContent,
  Button,
  Typography,
  IconButton,
  Box,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Chip,
  Stack,
  Divider,
  CircularProgress,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import RestaurantIcon from "@mui/icons-material/Restaurant";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import HelpOutlineIcon from "@mui/icons-material/HelpOutline";
import PlaylistAddIcon from "@mui/icons-material/PlaylistAdd";
import CancelIcon from "@mui/icons-material/Cancel";
import { useSubmit } from "@/hooks/useApi";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import type { Recipe, RecipeFoodProduct, MissingProduct } from "@/types";

interface Props {
  open: boolean;
  onClose: () => void;
  recipes: Recipe[];
  memberId: number;
  kitchenId: string;
}

export default function RecipeCarouselDialog({ open, onClose, recipes, memberId, kitchenId }: Props) {
  const [index, setIndex] = useState(0);
  const { submit } = useSubmit();
  const [missingProducts, setMissingProducts] = useState<MissingProduct[]>([]);
  const [missingOpen, setMissingOpen] = useState(false);
  const [missingLoading, setMissingLoading] = useState(false);
  const [addingToList, setAddingToList] = useState(false);

  const recipe = recipes[index]!;
  const numSlides = recipes.length;

  const prev = () => { setIndex((i) => (i - 1 + numSlides) % numSlides); setMissingOpen(false); };
  const next = () => { setIndex((i) => (i + 1) % numSlides); setMissingOpen(false); };

  const foodProducts: RecipeFoodProduct[] = Array.isArray(recipe.foodProducts)
    ? recipe.foodProducts
    : [];

  const handleUseRecipe = async () => {
    const result = await submit(
      "/api/KitchenItems/ConsumeRecipe",
      { memberId, kitchenId, foodProducts: recipe.foodProducts },
      {
        auth: true,
        successMessage: "Recipe consumed!",
        errorMessage: "Can't consume this recipe!",
      },
    );
    if (result !== null) {
      onClose();
    }
  };

  const handleShowMissing = async () => {
    if (!recipe.recipeId) return;
    setMissingLoading(true);
    try {
      const missing = await api.get<MissingProduct[]>(
        `/api/recipes/kitchens/${kitchenId}/${recipe.recipeId}/missing-products?memberId=${memberId}`,
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
    if (!recipe.recipeId) return;
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
      {/* Header with arrows */}
      <Stack direction="row" alignItems="center" sx={{ px: 2, pt: 2 }}>
        <IconButton onClick={prev} disabled={numSlides <= 1}>
          <ChevronLeftIcon />
        </IconButton>
        <Box sx={{ flex: 1, textAlign: "center" }}>
          <Typography variant="h5" fontWeight={600}>
            {recipe.name ?? recipe.recipeName}
          </Typography>
          {numSlides > 1 && (
            <Typography variant="caption" color="text.secondary">
              {index + 1} / {numSlides}
            </Typography>
          )}
        </Box>
        <IconButton onClick={next} disabled={numSlides <= 1}>
          <ChevronRightIcon />
        </IconButton>
      </Stack>

      <DialogContent>
        <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
          Ingredients
        </Typography>
        <List dense>
          {foodProducts.map((fp) => (
            <ListItem key={fp.foodProductId} disablePadding sx={{ py: 0.25 }}>
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
                  />
                }
              />
            </ListItem>
          ))}
        </List>

        {recipe.description && (
          <>
            <Divider sx={{ my: 2 }} />
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
          Cancel
        </Button>
        {recipe.recipeId && (
          <Button
            variant="outlined"
            startIcon={missingLoading ? <CircularProgress size={16} /> : <HelpOutlineIcon />}
            disabled={missingLoading}
            onClick={handleShowMissing}
          >
            What's Missing?
          </Button>
        )}
        <Button
          variant="contained"
          startIcon={<RestaurantIcon />}
          onClick={handleUseRecipe}
        >
          Use this recipe
        </Button>
      </DialogActions>
    </Dialog>
  );
}
