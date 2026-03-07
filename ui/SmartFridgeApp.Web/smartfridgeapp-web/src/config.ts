// API configuration - uses Vite environment variables
const config = {
  SERVER_URL: import.meta.env.VITE_SERVER_URL || "http://localhost:8080",
} as const;

export default config;
