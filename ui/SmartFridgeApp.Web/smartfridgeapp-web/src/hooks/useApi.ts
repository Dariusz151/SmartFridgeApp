import { useState, useEffect, useCallback, useRef } from "react";
import { api, ApiError } from "@/services/api";
import { toast } from "react-toastify";

interface UseFetchResult<T> {
  data: T | null;
  loading: boolean;
  refreshing: boolean;
  error: string | null;
  refetch: () => void;
  setData: React.Dispatch<React.SetStateAction<T | null>>;
}

export function useFetch<T>(endpoint: string, auth = false): UseFetchResult<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [trigger, setTrigger] = useState(0);
  const initialLoadDone = useRef(false);

  const refetch = useCallback(() => setTrigger((n) => n + 1), []);

  useEffect(() => {
    let cancelled = false;

    if (initialLoadDone.current) {
      setRefreshing(true);
    } else {
      setLoading(true);
    }

    api
      .get<T>(endpoint, auth)
      .then((result) => {
        if (!cancelled) {
          setData(result);
          setError(null);
        }
      })
      .catch((err) => {
        if (!cancelled) {
          const message = err instanceof ApiError ? err.message : "Network error";
          setError(message);
          console.error(`Failed to fetch ${endpoint}:`, err);
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
          setRefreshing(false);
          initialLoadDone.current = true;
        }
      });

    return () => {
      cancelled = true;
    };
  }, [endpoint, trigger]);

  return { data, loading, refreshing, error, refetch, setData };
}

export function useSubmit() {
  const [loading, setLoading] = useState(false);

  const submit = useCallback(
    async <T>(
      endpoint: string,
      body: unknown,
      options: {
        method?: "POST" | "DELETE";
        auth?: boolean;
        successMessage?: string;
        errorMessage?: string;
      } = {},
    ): Promise<T | null> => {
      const {
        method = "POST",
        auth = false,
        successMessage = "Success!",
        errorMessage = "Operation failed!",
      } = options;

      setLoading(true);
      try {
        const result =
          method === "DELETE"
            ? await api.delete<T>(endpoint, body, auth)
            : await api.post<T>(endpoint, body, auth);

        toast.success(successMessage, { position: "bottom-center", autoClose: 1500 });
        return result;
      } catch (err) {
        const message =
          err instanceof ApiError && err.status === 401
            ? "Unauthorized!"
            : errorMessage;
        toast.error(message, { position: "bottom-center", autoClose: 2000 });
        console.error(err);
        return null;
      } finally {
        setLoading(false);
      }
    },
    [],
  );

  return { submit, loading };
}
