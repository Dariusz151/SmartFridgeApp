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
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import { useSubmit } from "@/hooks/useApi";
import type { FoodProductCategory } from "@/types";

interface Props {
  categories: FoodProductCategory[];
  open: boolean;
  onClose: () => void;
}

export default function NewFoodProductDialog({ categories, open, onClose }: Props) {
  const { submit, loading } = useSubmit();
  const [name, setName] = useState("");
  const [categoryId, setCategoryId] = useState<number>(0);

  const handleAdd = async () => {
    const result = await submit(
      "/api/foodProducts",
      { name, category: categoryId },
      {
        auth: true,
        successMessage: "Food product added!",
        errorMessage: "Can't add food product!",
      },
    );
    if (result !== null) {
      setName("");
      setCategoryId(0);
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
