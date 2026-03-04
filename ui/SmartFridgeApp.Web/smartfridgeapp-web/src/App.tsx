import { useReducer, useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import { AuthContext, authReducer, initialAuthState } from "@/context/AuthContext";
import Header from "@/components/Header";
import Footer from "@/components/Footer";
import FridgesDashboard from "@/pages/FridgesDashboard";
import FridgeItemsDashboard from "@/pages/FridgeItemsDashboard";
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

  return (
    <AuthContext.Provider value={{ state, dispatch }}>
      <BrowserRouter>
        <Box sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
          <Header />

          <Box component="main" sx={{ flex: 1, py: 3, px: 2 }}>
            <Routes>
              <Route path="/" element={<Navigate to="/fridges" replace />} />
              <Route path="/fridges" element={<ProtectedRoute token={state.token}><FridgesDashboard /></ProtectedRoute>} />
              <Route path="/fridgeitems/:fridgeId" element={<ProtectedRoute token={state.token}><FridgeItemsDashboard /></ProtectedRoute>} />
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
