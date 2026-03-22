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

async function request<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
  const { method = "GET", body, auth = false } = options;

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
  };

  if (auth) {
    const token = sessionStorage.getItem("token");
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
  }

  const response = await fetch(`${config.SERVER_URL}${endpoint}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (!response.ok) {
    if (response.status === 401) {
      toast.error("Unauthorized operation!", { position: "bottom-center", autoClose: 2000 });
    }
    throw new ApiError(response.statusText, response.status);
  }

  // Some endpoints return 204 No Content
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

  logout: () => request<void>("/api/auth/logout", { method: "POST" }),

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
