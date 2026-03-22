import { useState } from "react";
import { useNavigate, Link as RouterLink } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Paper,
  Box,
  Stack,
  Divider,
  InputAdornment,
  IconButton,
} from "@mui/material";
import GoogleIcon from "@mui/icons-material/Google";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import LoginIcon from "@mui/icons-material/Login";
import { useAuth } from "@/context/AuthContext";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import config from "@/config";

export default function LoginPage() {
  const { dispatch } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPw, setShowPw] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    if (!email || !password) return;
    setLoading(true);
    try {
      const data = await api.login(email, password);
      dispatch({ type: "LOGIN_ADMIN", payload: { token: data.token, role: data.role, name: data.name, email: data.email } });
      toast.success(`Welcome${data.name ? `, ${data.name}` : ""}!`, { position: "bottom-center", autoClose: 1500 });
      navigate("/Kitchens");
    } catch {
      toast.error("Invalid email or password.", { position: "bottom-center", autoClose: 2500 });
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
          background: "linear-gradient(90deg, #0d7377 0%, #14a3a8 40%, #ff6f3c 100%)",
        }} />

        <Box sx={{ textAlign: "center", mb: 2, mt: 1 }}>
          <Box sx={{ fontSize: 56, lineHeight: 1, mb: 1 }}>🧊</Box>
          <Typography variant="h5" sx={{ fontWeight: 700 }}>
            Sign in
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
            Welcome back to SmartFridgeApp
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

        <Divider sx={{ my: 2, fontSize: 13 }}>or sign in with email</Divider>

        {/* ── Email + Password ── */}
        <Stack spacing={2}>
          <TextField
            label="Email"
            type="email"
            fullWidth
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            autoFocus
            size="small"
          />
          <TextField
            label="Password"
            type={showPw ? "text" : "password"}
            fullWidth
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && handleLogin()}
            size="small"
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

          <Button
            fullWidth
            variant="contained"
            size="large"
            startIcon={<LoginIcon />}
            onClick={handleLogin}
            disabled={loading || !email || !password}
            sx={{ borderRadius: 2, textTransform: "none", fontWeight: 600 }}
          >
            {loading ? "Signing in..." : "Sign in"}
          </Button>
        </Stack>

        <Typography variant="body2" sx={{ mt: 3, textAlign: "center" }}>
          Don't have an account?{" "}
          <Typography
            component={RouterLink}
            to="/register"
            variant="body2"
            color="primary"
            sx={{ fontWeight: 600, textDecoration: "none" }}
          >
            Create one
          </Typography>
        </Typography>
      </Paper>
    </Container>
  );
}
