import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Paper,
  Box,
  Stack,
  ToggleButton,
  ToggleButtonGroup,
  Autocomplete,
  Checkbox,
  IconButton,
  Divider,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import RemoveIcon from "@mui/icons-material/Remove";
import CloseIcon from "@mui/icons-material/Close";
import SaveIcon from "@mui/icons-material/Save";
import { useFetch, useSubmit } from "@/hooks/useApi";
import UnitSelector from "@/components/UnitSelector";
import type { FoodProduct, Unit, RecipeCreatePayload } from "@/types";

interface FoodProductInput {
  foodProductId: number;
  foodProductName: string;
  amount: string;
  unit: Unit | "";
  optional: boolean;
}

const emptyInput: FoodProductInput = {
  foodProductId: 0,
  foodProductName: "",
  amount: "",
  unit: "",
  optional: false,
};

export default function AddNewRecipe() {
  const navigate = useNavigate();
  const { submit } = useSubmit();
  const { data: foodProducts } = useFetch<FoodProduct[]>("/api/foodProducts");

  const [recipeName, setRecipeName] = useState("");
  const [recipeDesc, setRecipeDesc] = useState("");
  const [recipeCategory, setRecipeCategory] = useState<string>("");
  const [levelOfDifficulty, setDifficulty] = useState<string>("");
  const [requiredTime, setRequiredTime] = useState("");
  const [inputList, setInputList] = useState<FoodProductInput[]>([{ ...emptyInput }]);

  const updateInput = (index: number, updates: Partial<FoodProductInput>) => {
    setInputList((prev) =>
      prev.map((item, i) => (i === index ? { ...item, ...updates } : item)),
    );
  };

  const addInput = () => setInputList((prev) => [...prev, { ...emptyInput }]);

  const removeInput = (index: number) =>
    setInputList((prev) => prev.filter((_, i) => i !== index));

  const handleCreate = async () => {
    const payload: RecipeCreatePayload = {
      name: recipeName,
      description: recipeDesc,
      recipeCategory: parseInt(recipeCategory),
      requiredTime: parseInt(requiredTime),
      levelOfDifficulty: parseInt(levelOfDifficulty),
      products: inputList.map((item) => ({
        foodProductId: item.foodProductId,
        amountValue: { value: parseInt(item.amount), unit: item.unit || "NotAssigned" },
        optional: item.optional,
      })),
    };

    const result = await submit("/api/recipes/", payload, {
      auth: true,
      successMessage: "Recipe created!",
      errorMessage: "Can't add recipe!",
    });

    if (result !== null) {
      navigate("/recipes");
    }
  };

  return (
    <Container maxWidth="md">
      <Paper sx={{ p: 4, borderRadius: 3, position: "relative", overflow: "hidden" }}>
        {/* Gradient strip */}
        <Box
          sx={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            height: 5,
            background: "linear-gradient(90deg, #ff6f3c 0%, #14a3a8 40%, #0d7377 100%)",
          }}
        />

        <Stack direction="row" spacing={1.5} alignItems="center" sx={{ mb: 4 }}>
          <Box sx={{ fontSize: 40, lineHeight: 1 }}>👨‍🍳</Box>
          <Typography variant="h4" fontWeight={700}>
            Add Recipe
          </Typography>
        </Stack>

        {/* Food products list */}
        <Typography variant="h6" sx={{ mb: 2 }}>
          🧂 Ingredients
        </Typography>
        {inputList.map((fp, i) => (
          <Stack
            key={i}
            direction={{ xs: "column", sm: "row" }}
            spacing={1}
            alignItems="center"
            sx={{ mb: 1.5 }}
          >
            <Checkbox
              checked={fp.optional}
              onChange={() => updateInput(i, { optional: !fp.optional })}
              title="Optional"
              size="small"
            />
            <Autocomplete
              options={foodProducts ?? []}
              getOptionLabel={(o) => o.foodProductName}
              onChange={(_, val) =>
                val && updateInput(i, { foodProductId: val.foodProductId, foodProductName: val.foodProductName })
              }
              renderInput={(params) => (
                <TextField {...params} label="Food product" size="small" />
              )}
              sx={{ minWidth: 200, flex: 1 }}
            />
            <TextField
              label="Amount"
              type="number"
              size="small"
              value={fp.amount}
              onChange={(e) => updateInput(i, { amount: e.target.value })}
              sx={{ width: 100 }}
            />
            <UnitSelector value={fp.unit} onChange={(unit) => updateInput(i, { unit })} />
            <IconButton
              size="small"
              color="error"
              onClick={() => removeInput(i)}
              disabled={inputList.length === 1}
            >
              <RemoveIcon />
            </IconButton>
          </Stack>
        ))}
        <Button size="small" startIcon={<AddIcon />} onClick={addInput} sx={{ mb: 3 }}>
          Add Ingredient
        </Button>

        <Divider sx={{ my: 3 }} />

        {/* Recipe details */}
        <Stack spacing={2.5}>
          <TextField
            label="Recipe name"
            fullWidth
            value={recipeName}
            onChange={(e) => setRecipeName(e.target.value)}
          />
          <TextField
            label="Required time (minutes)"
            fullWidth
            type="number"
            value={requiredTime}
            onChange={(e) => setRequiredTime(e.target.value)}
          />

          <Box>
            <Typography variant="subtitle2" sx={{ mb: 1, color: "text.secondary" }}>
              Difficulty
            </Typography>
            <ToggleButtonGroup
              value={levelOfDifficulty}
              exclusive
              onChange={(_, val) => val && setDifficulty(val)}
              size="small"
            >
              <ToggleButton value="1">🟢 Easy</ToggleButton>
              <ToggleButton value="2">🟡 Medium</ToggleButton>
              <ToggleButton value="3">🔴 Hard</ToggleButton>
            </ToggleButtonGroup>
          </Box>

          <Box>
            <Typography variant="subtitle2" sx={{ mb: 1, color: "text.secondary" }}>
              Category
            </Typography>
            <ToggleButtonGroup
              value={recipeCategory}
              exclusive
              onChange={(_, val) => val && setRecipeCategory(val)}
              size="small"
            >
              <ToggleButton value="1">🍳 Breakfast</ToggleButton>
              <ToggleButton value="2">🍽️ Dinner</ToggleButton>
              <ToggleButton value="3">🥘 Supper</ToggleButton>
            </ToggleButtonGroup>
          </Box>

          <TextField
            label="Description"
            multiline
            minRows={5}
            fullWidth
            value={recipeDesc}
            onChange={(e) => setRecipeDesc(e.target.value)}
          />
        </Stack>

        <Stack direction="row" spacing={2} justifyContent="flex-end" sx={{ mt: 4 }}>
          <Button
            variant="outlined"
            color="inherit"
            size="large"
            startIcon={<CloseIcon />}
            onClick={() => navigate("/recipes")}
          >
            Cancel
          </Button>
          <Button
            variant="contained"
            size="large"
            startIcon={<SaveIcon />}
            onClick={handleCreate}
            disabled={!recipeName || !recipeDesc}
          >
            Create Recipe
          </Button>
        </Stack>
      </Paper>
    </Container>
  );
}
