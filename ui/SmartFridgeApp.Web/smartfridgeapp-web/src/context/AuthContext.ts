import { createContext, useContext } from "react";
import type { AuthState, AuthAction } from "@/types";

interface AuthContextValue {
  state: AuthState;
  dispatch: React.Dispatch<AuthAction>;
}

export const AuthContext = createContext<AuthContextValue>({
  state: { isAdmin: false, token: null, role: null, name: null, email: null },
  dispatch: () => undefined,
});

export function useAuth() {
  return useContext(AuthContext);
}

export const initialAuthState: AuthState = {
  isAdmin: false,
  token: null,
  role: null,
  name: null,
  email: null,
};

export function authReducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case "LOGIN_ADMIN":
      sessionStorage.setItem("token", action.payload.token);
      sessionStorage.setItem("role", action.payload.role);
      if (action.payload.name) sessionStorage.setItem("name", action.payload.name);
      if (action.payload.email) sessionStorage.setItem("email", action.payload.email);
      return {
        ...state,
        isAdmin: action.payload.role === "Admin",
        token: action.payload.token,
        role: action.payload.role,
        name: action.payload.name ?? state.name,
        email: action.payload.email ?? state.email,
      };
    case "LOGOUT_ADMIN":
      sessionStorage.clear();
      return { ...state, isAdmin: false, token: null, role: null, name: null, email: null };
    default:
      return state;
  }
}
