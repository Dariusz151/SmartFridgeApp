import { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Autocomplete,
  Stack,
  MenuItem,
  Box,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import { useSubmit } from "@/hooks/useApi";
import type { FoodProductCategory, StorageLocation, Unit } from "@/types";
import { STORAGE_LOCATIONS } from "@/types";

interface Props {
  categories: FoodProductCategory[];
  open: boolean;
  onClose: () => void;
}

export default function NewFoodProductDialog({ categories, open, onClose }: Props) {
  const { submit, loading } = useSubmit();
  const [name, setName] = useState("");
  const [categoryId, setCategoryId] = useState<number>(0);
  const [defaultLocation, setDefaultLocation] = useState<StorageLocation | "">("");
  const [defaultUnit, setDefaultUnit] = useState<Unit | "">("");

  const handleAdd = async () => {
    const result = await submit(
      "/api/foodProducts",
      {
        name,
        category: categoryId,
        defaultStorageLocation: defaultLocation || null,
        defaultUnit: defaultUnit || null,
      },
      {
        auth: true,
        successMessage: "Food product added!",
        errorMessage: "Can't add food product!",
      },
    );
    if (result !== null) {
      setName("");
      setCategoryId(0);
      setDefaultLocation("");
      setDefaultUnit("");
      onClose();
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add Food Product</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Name"
            fullWidth
            value={name}
            onChange={(e) => setName(e.target.value)}
            autoFocus
          />
          <Autocomplete
            options={categories}
            getOptionLabel={(option) => option.name}
            onChange={(_, val) => val && setCategoryId(val.categoryId)}
            renderInput={(params) => (
              <TextField {...params} label="Select category" />
            )}
          />
          <TextField
            label="Default Storage Location"
            select
            fullWidth
            value={defaultLocation}
            onChange={(e) => setDefaultLocation(e.target.value as StorageLocation | "")}
          >
            <MenuItem value="">— None —</MenuItem>
            {STORAGE_LOCATIONS.map((loc) => (
              <MenuItem key={loc.value} value={loc.value}>
                <Box component="span" sx={{ mr: 1 }}>{loc.icon}</Box>
                {loc.label}
              </MenuItem>
            ))}
          </TextField>
          <TextField
            label="Default Unit"
            select
            fullWidth
            value={defaultUnit}
            onChange={(e) => setDefaultUnit(e.target.value as Unit | "")}
          >
            <MenuItem value="">— None —</MenuItem>
            <MenuItem value="Pieces">Pieces</MenuItem>
            <MenuItem value="Grams">Grams</MenuItem>
            <MenuItem value="Mililiter">Millilitres</MenuItem>
          </TextField>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} startIcon={<CloseIcon />}>
          Cancel
        </Button>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          disabled={loading || !name}
        >
          Add
        </Button>
      </DialogActions>
    </Dialog>
  );
}
