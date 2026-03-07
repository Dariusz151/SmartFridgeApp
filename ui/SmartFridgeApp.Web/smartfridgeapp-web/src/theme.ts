import { createTheme, alpha } from "@mui/material/styles";
import type {} from "@mui/x-data-grid/themeAugmentation";

const theme = createTheme({
  palette: {
    primary: {
      main: "#0d7377",
      light: "#14a3a8",
      dark: "#095456",
      contrastText: "#ffffff",
    },
    secondary: {
      main: "#ff6f3c",
      light: "#ff9a6c",
      dark: "#cc4f1e",
      contrastText: "#ffffff",
    },
    success: { main: "#2ec4b6" },
    warning: { main: "#ff9f1c" },
    error: { main: "#e63946" },
    info: { main: "#457b9d" },
    background: {
      default: "#f0f5f4",
      paper: "#ffffff",
    },
    text: {
      primary: "#14213d",
      secondary: "#4a5568",
    },
  },
  typography: {
    fontFamily: "'Inter', 'Roboto', 'Helvetica', 'Arial', sans-serif",
    h4: { fontWeight: 700, letterSpacing: "-0.02em" },
    h5: { fontWeight: 700 },
    h6: { fontWeight: 600 },
    button: { textTransform: "none", fontWeight: 600 },
  },
  shape: {
    borderRadius: 12,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 10,
          padding: "8px 22px",
          transition: "all 0.2s ease",
        },
        containedPrimary: {
          background: "linear-gradient(135deg, #0d7377 0%, #14a3a8 100%)",
          boxShadow: "0 3px 12px rgba(13,115,119,0.3)",
          "&:hover": {
            background: "linear-gradient(135deg, #095456 0%, #0d7377 100%)",
            boxShadow: "0 6px 20px rgba(13,115,119,0.4)",
            transform: "translateY(-1px)",
          },
        },
        containedSecondary: {
          background: "linear-gradient(135deg, #ff6f3c 0%, #ff9a6c 100%)",
          boxShadow: "0 3px 12px rgba(255,111,60,0.3)",
          "&:hover": {
            background: "linear-gradient(135deg, #cc4f1e 0%, #ff6f3c 100%)",
            boxShadow: "0 6px 20px rgba(255,111,60,0.4)",
            transform: "translateY(-1px)",
          },
        },
        outlined: {
          "&:hover": {
            transform: "translateY(-1px)",
          },
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          boxShadow: "0 2px 12px rgba(0,0,0,0.06)",
          transition: "box-shadow 0.25s ease, transform 0.25s ease",
        },
      },
    },
    MuiDialog: {
      styleOverrides: {
        paper: {
          borderRadius: 20,
          padding: 8,
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          background: "linear-gradient(135deg, #095456 0%, #0d7377 40%, #14a3a8 100%)",
          boxShadow: "0 2px 16px rgba(13,115,119,0.25)",
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          fontWeight: 600,
          transition: "all 0.2s ease",
        },
      },
    },
    MuiDataGrid: {
      styleOverrides: {
        root: {
          border: "none",
          borderRadius: 14,
          "& .MuiDataGrid-columnHeaders": {
            background: "linear-gradient(135deg, #e8f5f3 0%, #f0f5f4 100%)",
            borderRadius: "14px 14px 0 0",
            fontWeight: 700,
          },
          "& .MuiDataGrid-cell:focus": {
            outline: "none",
          },
          "& .MuiDataGrid-row:hover": {
            backgroundColor: alpha("#0d7377", 0.04),
          },
        },
      },
    },
    MuiToggleButton: {
      styleOverrides: {
        root: {
          borderRadius: "10px !important",
          transition: "all 0.2s ease",
          "&.Mui-selected": {
            background: "linear-gradient(135deg, #0d7377 0%, #14a3a8 100%)",
            color: "#fff",
            "&:hover": {
              background: "linear-gradient(135deg, #095456 0%, #0d7377 100%)",
            },
          },
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          "& .MuiOutlinedInput-root": {
            borderRadius: 10,
            transition: "box-shadow 0.2s",
            "&.Mui-focused": {
              boxShadow: `0 0 0 3px ${alpha("#0d7377", 0.15)}`,
            },
          },
        },
      },
    },
  },
});

export default theme;
