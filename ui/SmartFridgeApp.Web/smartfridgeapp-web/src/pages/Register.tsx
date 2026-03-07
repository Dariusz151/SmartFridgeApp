import { useState } from "react";
import { useNavigate, Link as RouterLink } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Paper,
  Stack,
  Divider,
  InputAdornment,
  IconButton,
  Box,
} from "@mui/material";
import GoogleIcon from "@mui/icons-material/Google";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import config from "@/config";

export default function RegisterPage() {
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPw, setShowPw] = useState(false);
  const [loading, setLoading] = useState(false);

  const passwordsMatch = password === confirmPassword;
  const canSubmit = name && email && password.length >= 6 && passwordsMatch && !loading;

  const handleRegister = async () => {
    if (!canSubmit) return;
    setLoading(true);
    try {
      await api.register(email, name, password);
      toast.success("Account created! You can now sign in.", { position: "bottom-center", autoClose: 2500 });
      navigate("/login");
    } catch (err: any) {
      const msg = err?.status === 409
        ? "An account with this email already exists."
        : "Registration failed. Please try again.";
      toast.error(msg, { position: "bottom-center", autoClose: 3000 });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="xs" sx={{ mt: 6 }}>
      <Paper sx={{ p: 4, borderRadius: 4, position: "relative", overflow: "hidden" }} elevation={3}>
        {/* Decorative gradient strip */}
        <Box sx={{
          position: "absolute", top: 0, left: 0, right: 0, height: 6,
          background: "linear-gradient(90deg, #14a3a8 0%, #0d7377 50%, #ff6f3c 100%)",
        }} />

        <Box sx={{ textAlign: "center", mb: 2, mt: 1 }}>
          <Box sx={{ fontSize: 56, lineHeight: 1, mb: 1 }}>🍽️</Box>
          <Typography variant="h5" sx={{ fontWeight: 700 }}>
            Create account
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
            Join SmartFridgeApp
          </Typography>
        </Box>

        {/* ── Google ── */}
        <Button
          fullWidth
          variant="outlined"
          size="large"
          startIcon={<GoogleIcon />}
          onClick={() => { window.location.href = `${config.SERVER_URL}/api/auth/google-login`; }}
          sx={{ borderRadius: 2, textTransform: "none", fontWeight: 600, mb: 2 }}
        >
          Continue with Google
        </Button>

        <Divider sx={{ my: 2, fontSize: 13 }}>or register with email</Divider>

        {/* ── Form ── */}
        <Stack spacing={2}>
          <TextField
            label="Name"
            fullWidth
            value={name}
            onChange={(e) => setName(e.target.value)}
            autoFocus
            size="small"
          />
          <TextField
            label="Email"
            type="email"
            fullWidth
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            size="small"
          />
          <TextField
            label="Password"
            type={showPw ? "text" : "password"}
            fullWidth
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            size="small"
            helperText={password && password.length < 6 ? "Min. 6 characters" : ""}
            error={!!password && password.length < 6}
            slotProps={{
              input: {
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton size="small" onClick={() => setShowPw(!showPw)} edge="end">
                      {showPw ? <VisibilityOff fontSize="small" /> : <Visibility fontSize="small" />}
                    </IconButton>
                  </InputAdornment>
                ),
              },
            }}
          />
          <TextField
            label="Confirm password"
            type={showPw ? "text" : "password"}
            fullWidth
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && handleRegister()}
            size="small"
            helperText={confirmPassword && !passwordsMatch ? "Passwords don't match" : ""}
            error={!!confirmPassword && !passwordsMatch}
          />

          <Button
            fullWidth
            variant="contained"
            size="large"
            startIcon={<PersonAddIcon />}
            onClick={handleRegister}
            disabled={!canSubmit}
            sx={{ borderRadius: 2, textTransform: "none", fontWeight: 600 }}
          >
            {loading ? "Creating account..." : "Create account"}
          </Button>
        </Stack>

        <Typography variant="body2" sx={{ mt: 3, textAlign: "center" }}>
          Already have an account?{" "}
          <Typography
            component={RouterLink}
            to="/login"
            variant="body2"
            color="primary"
            sx={{ fontWeight: 600, textDecoration: "none" }}
          >
            Sign in
          </Typography>
        </Typography>
      </Paper>
    </Container>
  );
}
