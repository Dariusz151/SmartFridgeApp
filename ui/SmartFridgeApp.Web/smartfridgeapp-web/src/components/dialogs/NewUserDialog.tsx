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
  kitchenId: string;
  open: boolean;
  onClose: () => void;
}

export default function NewUserDialog({ kitchenId, open, onClose }: Props) {
  const { submit, loading } = useSubmit();
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");

  const handleAdd = async () => {
    const result = await submit(
      `/api/fridgeUsers/${kitchenId}`,
      { user: { name, email } },
      {
        auth: true,
        successMessage: "User added!",
        errorMessage: "Can't add user!",
      },
    );
    if (result !== null) {
      setName("");
      setEmail("");
      onClose();
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add User</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Name"
            fullWidth
            value={name}
            onChange={(e) => setName(e.target.value)}
            autoFocus
          />
          <TextField
            label="Email"
            type="email"
            fullWidth
            value={email}
            onChange={(e) => setEmail(e.target.value)}
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
