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
  MenuItem,
  Chip,
  Box,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import { useFetch, useSubmit } from "@/hooks/useApi";
import UnitSelector from "@/components/UnitSelector";
import type { FoodProduct, Unit, FridgeItem, StorageLocation, ItemTag } from "@/types";
import { STORAGE_LOCATIONS, ITEM_TAGS } from "@/types";

interface Props {
  kitchenId: string;
  memberId: number;
  open: boolean;
  onClose: () => void;
  onItemAdded?: (item: FridgeItem) => void;
}

export default function NewStockItemDialog({ kitchenId, memberId, open, onClose, onItemAdded }: Props) {
  const { data: foodProducts } = useFetch<FoodProduct[]>("/api/foodProducts");
  const { submit, loading } = useSubmit();
  const [foodProductId, setFoodProductId] = useState(0);
  const [value, setValue] = useState("");
  const [unit, setUnit] = useState<Unit>("NotAssigned");
  const [location, setLocation] = useState<StorageLocation>("Fridge");
  const [tags, setTags] = useState<ItemTag[]>([]);

  // Default expiration: 7 days from now
  const defaultExpiration = () => {
    const d = new Date();
    d.setDate(d.getDate() + 7);
    return d.toISOString().slice(0, 10);
  };
  const [expirationDate, setExpirationDate] = useState(defaultExpiration());

  const handleAdd = async () => {
    const result = await submit(
      `/api/Kitchens/${kitchenId}/inventory`,
      {
        memberId,
        item: {
          foodProductId,
          amount: parseInt(value),
          note: "",
          unit,
          location,
          tags,
          expirationDate: new Date(expirationDate).toISOString(),
        },
      },
      {
        auth: true,
        successMessage: "Kitchen item added!",
        errorMessage: "Can't add Kitchen item!",
      },
    );
    if (result !== null) {
      onItemAdded?.({
        stockItemId: crypto.randomUUID(),
        foodProductId,
        memberId,
        amount: parseInt(value),
        unit,
        location,
        tags,
        expirationDate: new Date(expirationDate).toISOString(),
        stockedAt: new Date().toISOString(),
      });
      setFoodProductId(0);
      setValue("");
      setUnit("NotAssigned");
      setLocation("Fridge");
      setTags([]);
      setExpirationDate(defaultExpiration());
      onClose();
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add Kitchen Item</DialogTitle>
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
          <TextField
            label="Storage Location"
            select
            fullWidth
            value={location}
            onChange={(e) => setLocation(e.target.value as StorageLocation)}
          >
            {STORAGE_LOCATIONS.map((loc) => (
              <MenuItem key={loc.value} value={loc.value}>
                <Box component="span" sx={{ mr: 1 }}>{loc.icon}</Box>
                {loc.label}
              </MenuItem>
            ))}
          </TextField>
          <Autocomplete
            multiple
            options={ITEM_TAGS}
            getOptionLabel={(option) => option.label}
            value={ITEM_TAGS.filter((t) => tags.includes(t.value))}
            onChange={(_, val) => setTags(val.map((v) => v.value))}
            renderTags={(value, getTagProps) =>
              value.map((option, index) => {
                const { key, ...rest } = getTagProps({ index });
                return <Chip key={key} label={option.label} size="small" {...rest} />;
              })
            }
            renderInput={(params) => (
              <TextField {...params} label="Tags (optional)" placeholder="Add tags..." />
            )}
          />
          <TextField
            label="Expiration Date"
            type="date"
            fullWidth
            value={expirationDate}
            onChange={(e) => setExpirationDate(e.target.value)}
            slotProps={{ inputLabel: { shrink: true } }}
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
          disabled={loading || !foodProductId || !value}
        >
          Add
        </Button>
      </DialogActions>
    </Dialog>
  );
}
