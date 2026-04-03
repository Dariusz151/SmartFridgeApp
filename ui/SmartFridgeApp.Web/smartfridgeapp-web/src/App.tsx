import { useReducer, useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import { AuthContext, authReducer, initialAuthState } from "@/context/AuthContext";
import { setApiLogoutCallback } from "@/services/api";
import Header from "@/components/Header";
import Footer from "@/components/Footer";
import KitchensDashboard from "@/pages/KitchensDashboard";
import KitchenItemsDashboard from "@/pages/KitchenItemsDashboard";
import FoodProducts from "@/pages/FoodProducts";
import Recipes from "@/pages/Recipes";
import AddNewRecipe from "@/pages/AddNewRecipe";
import LoginPage from "@/pages/AdminLogin";
import RegisterPage from "@/pages/Register";
import GoogleAuthCallback from "@/pages/GoogleAuthCallback";
import Box from "@mui/material/Box";

function ProtectedRoute({ children, token }: { children: React.ReactNode; token: string | null }) {
  if (!token) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

export default function App() {
  const [state, dispatch] = useReducer(authReducer, initialAuthState);

  useEffect(() => {
    const token = sessionStorage.getItem("token");
    const role = sessionStorage.getItem("role") ?? "User";
    const name = sessionStorage.getItem("name") ?? undefined;
    const email = sessionStorage.getItem("email") ?? undefined;
    if (token) {
      dispatch({ type: "LOGIN_ADMIN", payload: { token, role, name, email } });
    }
  }, []);

  // Wire up automatic logout when refresh token fails
  useEffect(() => {
    setApiLogoutCallback(() => dispatch({ type: "LOGOUT_ADMIN" }));
  }, []);

  return (
    <AuthContext.Provider value={{ state, dispatch }}>
      <BrowserRouter>
        <Box sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
          <Header />

          <Box component="main" sx={{ flex: 1, py: 3, px: 2 }}>
            <Routes>
              <Route path="/" element={<Navigate to="/Kitchens" replace />} />
              <Route path="/Kitchens" element={<ProtectedRoute token={state.token}><KitchensDashboard /></ProtectedRoute>} />
              <Route path="/KitchenItems/:kitchenId" element={<ProtectedRoute token={state.token}><KitchenItemsDashboard /></ProtectedRoute>} />
              <Route path="/foodProducts" element={<ProtectedRoute token={state.token}><FoodProducts /></ProtectedRoute>} />
              <Route path="/recipes" element={<ProtectedRoute token={state.token}><Recipes /></ProtectedRoute>} />
              <Route path="/recipes/add" element={<ProtectedRoute token={state.token}><AddNewRecipe /></ProtectedRoute>} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/admin" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
              <Route path="/auth/google-callback" element={<GoogleAuthCallback />} />
            </Routes>
          </Box>

          <Footer />
        </Box>
      </BrowserRouter>
    </AuthContext.Provider>
  );
}
