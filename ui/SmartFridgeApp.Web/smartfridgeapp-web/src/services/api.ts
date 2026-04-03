import config from "@/config";
import { toast } from "react-toastify";

type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";

interface RequestOptions {
  method?: HttpMethod;
  body?: unknown;
  auth?: boolean;
}

class ApiError extends Error {
  status: number;
  constructor(message: string, status: number) {
    super(message);
    this.status = status;
    this.name = "ApiError";
  }
}

// ── Refresh-token machinery ──
let refreshPromise: Promise<boolean> | null = null;
let logoutCallback: (() => void) | null = null;

/** Call once from App.tsx to wire up the auto-logout on refresh failure. */
export function setApiLogoutCallback(cb: () => void) {
  logoutCallback = cb;
}

async function tryRefreshToken(): Promise<boolean> {
  try {
    const res = await fetch(`${config.SERVER_URL}/api/auth/refresh`, {
      method: "POST",
      credentials: "include",           // sends httpOnly cookie
    });
    if (!res.ok) return false;

    const data = await res.json();
    sessionStorage.setItem("token", data.token);
    if (data.role)  sessionStorage.setItem("role", data.role);
    if (data.name)  sessionStorage.setItem("name", data.name);
    if (data.email) sessionStorage.setItem("email", data.email);
    return true;
  } catch {
    return false;
  }
}

async function request<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
  const { method = "GET", body, auth = false } = options;

  const buildHeaders = (): Record<string, string> => {
    const h: Record<string, string> = { "Content-Type": "application/json" };
    if (auth) {
      const token = sessionStorage.getItem("token");
      if (token) h["Authorization"] = `Bearer ${token}`;
    }
    return h;
  };

  let response = await fetch(`${config.SERVER_URL}${endpoint}`, {
    method,
    headers: buildHeaders(),
    credentials: "include",
    body: body ? JSON.stringify(body) : undefined,
  });

  // On 401 with auth, attempt a silent refresh then retry once
  if (response.status === 401 && auth) {
    // Coalesce concurrent refreshes into a single call
    if (!refreshPromise) {
      refreshPromise = tryRefreshToken().finally(() => { refreshPromise = null; });
    }
    const refreshed = await refreshPromise;

    if (refreshed) {
      response = await fetch(`${config.SERVER_URL}${endpoint}`, {
        method,
        headers: buildHeaders(),
        credentials: "include",
        body: body ? JSON.stringify(body) : undefined,
      });
    } else {
      logoutCallback?.();
      toast.error("Session expired. Please sign in again.", { position: "bottom-center", autoClose: 2500 });
      throw new ApiError("Session expired", 401);
    }
  }

  if (!response.ok) {
    if (response.status === 401) {
      toast.error("Unauthorized operation!", { position: "bottom-center", autoClose: 2000 });
    }
    throw new ApiError(response.statusText, response.status);
  }

  const text = await response.text();
  return text ? (JSON.parse(text) as T) : (undefined as unknown as T);
}

/* ── Convenience wrappers ── */

export const api = {
  get: <T>(endpoint: string, auth = false) => request<T>(endpoint, { auth }),

  post: <T>(endpoint: string, body: unknown, auth = false) =>
    request<T>(endpoint, { method: "POST", body, auth }),

  put: <T>(endpoint: string, body: unknown, auth = false) =>
    request<T>(endpoint, { method: "PUT", body, auth }),

  delete: <T>(endpoint: string, body?: unknown, auth = false) =>
    request<T>(endpoint, { method: "DELETE", body, auth }),

  logout: () => request<void>("/api/auth/logout", { method: "POST", auth: true }),

  login: (email: string, password: string) =>
    request<{ token: string; email: string; name: string; role: string }>("/api/auth/login", {
      method: "POST",
      body: { email, password },
    }),

  register: (email: string, name: string, password: string) =>
    request<void>("/api/auth/register", {
      method: "POST",
      body: { email, name, password },
    }),

  // Invite endpoints
  getPendingInvites: () =>
    request<{ id: number; kitchenId: string; kitchenName: string; inviterEmail: string; inviterName: string; invitedAt: string }[]>(
      "/api/invites/pending", { auth: true },
    ),

  acceptInvite: (inviteId: number) =>
    request<void>(`/api/invites/${inviteId}/accept`, { method: "POST", auth: true }),

  declineInvite: (inviteId: number) =>
    request<void>(`/api/invites/${inviteId}/decline`, { method: "POST", auth: true }),

  inviteUser: (kitchenId: string, email: string) =>
    request<void>(`/api/Kitchens/${kitchenId}/invite`, { method: "POST", body: { email }, auth: true }),

  getKitchenMembers: (kitchenId: string) =>
    request<{ id: number; kitchenId: string; email: string; name: string; memberRole: string; status: string; color: string }[]>(
      `/api/Kitchens/${kitchenId}/members`, { auth: true },
    ),
};

export { ApiError };
