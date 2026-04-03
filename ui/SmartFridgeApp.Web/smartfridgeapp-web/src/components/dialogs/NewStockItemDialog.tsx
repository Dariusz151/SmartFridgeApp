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
  Chip,
  Box,
  Typography,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import { useFetch, useSubmit } from "@/hooks/useApi";
import { api } from "@/services/api";
import type { FoodProduct, ProductVariant, Unit, FridgeItem, StorageLocation, ItemTag } from "@/types";
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
  const [selectedProduct, setSelectedProduct] = useState<FoodProduct | null>(null);
  const [foodProductId, setFoodProductId] = useState(0);
  const [variantId, setVariantId] = useState<number | null>(null);
  const [variants, setVariants] = useState<ProductVariant[]>([]);
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

  // When a product is selected, apply its defaults for location and unit
  useEffect(() => {
    if (!selectedProduct) {
      setVariants([]);
      setVariantId(null);
      return;
    }
    setLocation(selectedProduct.defaultStorageLocation ?? "Fridge");
    setUnit(selectedProduct.defaultUnit ?? "NotAssigned");
    api.get<ProductVariant[]>(`/api/foodProducts/${selectedProduct.foodProductId}/variants`)
      .then(setVariants)
      .catch(() => setVariants([]));
  }, [selectedProduct]);

  const locationDef = STORAGE_LOCATIONS.find((l) => l.value === location);

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
          variantId: variantId ?? undefined,
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
        variantId: variantId ?? undefined,
        variantName: variants.find((v) => v.variantId === variantId)?.name,
        expirationDate: new Date(expirationDate).toISOString(),
        stockedAt: new Date().toISOString(),
      });
      setSelectedProduct(null);
      setFoodProductId(0);
      setVariantId(null);
      setVariants([]);
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
            value={selectedProduct}
            onChange={(_, val) => {
              setSelectedProduct(val);
              setFoodProductId(val?.foodProductId ?? 0);
            }}
            renderInput={(params) => (
              <TextField {...params} label="Select food product" />
            )}
          />
          {variants.length > 0 && (
            <Autocomplete
              options={variants}
              getOptionLabel={(option) => option.name + (option.barcode ? ` (${option.barcode})` : "")}
              onChange={(_, val) => setVariantId(val?.variantId ?? null)}
              renderInput={(params) => (
                <TextField {...params} label="Variant (optional)" />
              )}
            />
          )}
          <TextField
            label="Amount"
            type="number"
            fullWidth
            value={value}
            onChange={(e) => {
              const v = e.target.value;
              if (/^\d*$/.test(v)) setValue(v);
            }}
            slotProps={{
              input: {
                endAdornment: unit !== "NotAssigned" ? (
                  <Typography variant="body2" color="text.secondary" sx={{ whiteSpace: "nowrap", pl: 1 }}>
                    {unit === "Grams" ? "g" : unit === "Mililiter" ? "ml" : "pcs"}
                  </Typography>
                ) : undefined,
              },
            }}
          />
          {/* Storage info derived from product defaults */}
          {selectedProduct && (
            <Box sx={{ display: "flex", alignItems: "center", gap: 1, px: 1 }}>
              <Box component="span" sx={{ fontSize: 18 }}>{locationDef?.icon}</Box>
              <Typography variant="body2" color="text.secondary">
                <strong>{locationDef?.label ?? location}</strong>
                {unit !== "NotAssigned" && <> · {unit}</>}
                <Box component="span" sx={{ ml: 1, opacity: 0.6, fontSize: "0.75rem" }}>
                  (from product defaults)
                </Box>
              </Typography>
            </Box>
          )}
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
