import { useState, useEffect } from "react";
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
import { useFetch, useSubmit } from "@/hooks/useApi";
import UnitSelector from "@/components/UnitSelector";
import type { FoodProduct, Unit } from "@/types";

interface Props {
  fridgeId: string;
  selectedUserId: string;
  open: boolean;
  onClose: () => void;
}

export default function NewFridgeItemDialog({ fridgeId, selectedUserId, open, onClose }: Props) {
  const { data: foodProducts } = useFetch<FoodProduct[]>("/api/foodProducts");
  const { submit, loading } = useSubmit();
  const [foodProductId, setFoodProductId] = useState(0);
  const [value, setValue] = useState("");
  const [unit, setUnit] = useState<Unit>("NotAssigned");

  const handleAdd = async () => {
    const result = await submit(
      `/api/fridgeItems/${fridgeId}/add`,
      {
        userId: selectedUserId,
        fridgeItem: {
          foodProductId,
          value: parseInt(value),
          note: "",
          unit,
        },
      },
      {
        auth: true,
        successMessage: "Fridge item added!",
        errorMessage: "Can't add fridge item!",
      },
    );
    if (result !== null) {
      setFoodProductId(0);
      setValue("");
      setUnit("NotAssigned");
      onClose();
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add Fridge Item</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <Autocomplete
            options={foodProducts ?? []}
            getOptionLabel={(option) => option.foodProductName}
            onChange={(_, val) => val && setFoodProductId(val.foodProductId)}
            renderInput={(params) => (
              <TextField {...params} label="Select food product" />
            )}
          />
          <TextField
            label="Amount"
            type="number"
            fullWidth
            value={value}
            onChange={(e) => {
              const v = e.target.value;
              if (/^\d*$/.test(v)) setValue(v);
            }}
          />
          <UnitSelector value={unit} onChange={setUnit} size="medium" />
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
          disabled={loading || !foodProductId || !value}
        >
          Add
        </Button>
      </DialogActions>
    </Dialog>
  );
}
