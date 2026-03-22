import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { CircularProgress, Box, Typography } from "@mui/material";
import { useAuth } from "@/context/AuthContext";
import { toast } from "react-toastify";

export default function GoogleAuthCallback() {
  const { dispatch } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const token = searchParams.get("token");
    const name = searchParams.get("name");
    const email = searchParams.get("email");
    const role = searchParams.get("role") ?? "User";

    if (token) {
      dispatch({ type: "LOGIN_ADMIN", payload: { token, role, name: name ?? undefined, email: email ?? undefined } });
      toast.success(`Welcome${name ? `, ${name}` : ""} (${role})!`, {
        position: "bottom-center",
        autoClose: 2000,
      });
      navigate("/Kitchens", { replace: true });
    } else {
      toast.error("Google login failed. Please try again.", {
        position: "bottom-center",
        autoClose: 3000,
      });
      navigate("/admin", { replace: true });
    }
  }, []);

  return (
    <Box sx={{ display: "flex", flexDirection: "column", alignItems: "center", mt: 10, gap: 2 }}>
      <CircularProgress />
      <Typography color="text.secondary">Signing you in...</Typography>
    </Box>
  );
}
