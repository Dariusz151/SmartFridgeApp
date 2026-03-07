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
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import RestaurantIcon from "@mui/icons-material/Restaurant";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import { useSubmit } from "@/hooks/useApi";
import type { Recipe, RecipeFoodProduct } from "@/types";

interface Props {
  open: boolean;
  onClose: () => void;
  recipes: Recipe[];
  userId: string;
  fridgeId: string;
}

export default function RecipeCarouselDialog({ open, onClose, recipes, userId, fridgeId }: Props) {
  const [index, setIndex] = useState(0);
  const { submit } = useSubmit();

  const recipe = recipes[index]!;
  const numSlides = recipes.length;

  const prev = () => setIndex((i) => (i - 1 + numSlides) % numSlides);
  const next = () => setIndex((i) => (i + 1) % numSlides);

  const foodProducts: RecipeFoodProduct[] = Array.isArray(recipe.foodProducts)
    ? recipe.foodProducts
    : [];

  const handleUseRecipe = async () => {
    const result = await submit(
      "/api/fridgeItems/ConsumeRecipe",
      { userId, fridgeId, foodProducts: recipe.foodProducts },
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
      </DialogContent>

      <DialogActions>
        <Button onClick={onClose} startIcon={<CloseIcon />}>
          Cancel
        </Button>
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
