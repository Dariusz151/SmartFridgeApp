import { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Stack,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import { useSubmit } from "@/hooks/useApi";

interface Props {
  open: boolean;
  onClose: () => void;
}

export default function NewFridgeDialog({ open, onClose }: Props) {
  const { submit, loading } = useSubmit();
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [desc, setDesc] = useState("");

  const handleAdd = async () => {
    const result = await submit("/api/fridges", { name, address, desc }, {
      auth: true,
      successMessage: "Fridge added!",
      errorMessage: "Can't add fridge!",
    });
    if (result !== null) {
      setName("");
      setAddress("");
      setDesc("");
      onClose();
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add New Fridge</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Fridge name"
            fullWidth
            value={name}
            onChange={(e) => setName(e.target.value)}
            autoFocus
          />
          <TextField
            label="Address"
            fullWidth
            value={address}
            onChange={(e) => setAddress(e.target.value)}
          />
          <TextField
            label="Description"
            fullWidth
            value={desc}
            onChange={(e) => setDesc(e.target.value)}
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
