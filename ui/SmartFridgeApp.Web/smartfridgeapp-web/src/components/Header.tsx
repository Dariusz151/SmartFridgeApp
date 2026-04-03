import { useState, useEffect, useCallback } from "react";
import { Link as RouterLink, useLocation, useNavigate } from "react-router-dom";
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  IconButton,
  Menu,
  MenuItem,
  Box,
  Container,
  useMediaQuery,
  Drawer,
  List,
  ListItemButton,
  ListItemText,
  Badge,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Stack,
  Chip,
} from "@mui/material";
import { useTheme } from "@mui/material/styles";
import MenuIcon from "@mui/icons-material/Menu";
import PersonIcon from "@mui/icons-material/Person";
import LogoutIcon from "@mui/icons-material/Logout";
import LoginIcon from "@mui/icons-material/Login";
import MailIcon from "@mui/icons-material/Mail";
import CheckIcon from "@mui/icons-material/Check";
import CloseIcon from "@mui/icons-material/Close";
import RestaurantMenuIcon from "@mui/icons-material/RestaurantMenu";
import { useAuth } from "@/context/AuthContext";
import { useMainKitchen, clearMainKitchen } from "@/hooks/useMainKitchen";
import { api } from "@/services/api";
import { toast } from "react-toastify";
import type { KitchenInvite } from "@/types";

const navLinks = [
  { label: "Kitchens", to: "/Kitchens" },
  { label: "Products", to: "/foodProducts" },
  { label: "Recipes", to: "/recipes", icon: <RestaurantMenuIcon fontSize="small" /> },
];

export default function Header() {
  const { state, dispatch } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const [drawerOpen, setDrawerOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [invites, setInvites] = useState<KitchenInvite[]>([]);
  const [inviteDialogOpen, setInviteDialogOpen] = useState(false);
  const mainKitchen = useMainKitchen();

  const fetchInvites = useCallback(async () => {
    if (!state.token) return;
    try {
      const data = await api.getPendingInvites();
      setInvites(data ?? []);
    } catch {
      // silent
    }
  }, [state.token]);

  useEffect(() => {
    fetchInvites();
    const interval = setInterval(fetchInvites, 300_000);
    return () => clearInterval(interval);
  }, [fetchInvites]);

  const handleAccept = async (inviteId: number) => {
    try {
      await api.acceptInvite(inviteId);
      toast.success("Invite accepted!", { position: "bottom-center", autoClose: 1500 });
      fetchInvites();
    } catch {
      toast.error("Failed to accept invite.", { position: "bottom-center", autoClose: 2000 });
    }
  };

  const handleDecline = async (inviteId: number) => {
    try {
      await api.declineInvite(inviteId);
      toast.info("Invite declined.", { position: "bottom-center", autoClose: 1500 });
      fetchInvites();
    } catch {
      toast.error("Failed to decline invite.", { position: "bottom-center", autoClose: 2000 });
    }
  };

  const handleLogout = async () => {
    try {
      await api.logout();
    } catch {
      // Server-side logout is best-effort; always clear local state
    }
    clearMainKitchen();
    dispatch({ type: "LOGOUT_ADMIN" });
    setAnchorEl(null);
    navigate("/login");
  };

  const displayName = state.name || state.email || (state.isAdmin ? "Admin" : "User");

  return (
    <AppBar position="sticky" color="primary" elevation={0}>
      <Container maxWidth="lg">
        <Toolbar disableGutters sx={{ gap: 1 }}>
          <Box sx={{ fontSize: 28, lineHeight: 1, mr: 0.5 }}>🍳</Box>
          <Typography
            variant="h6"
            component={RouterLink}
            to="/Kitchens"
            sx={{
              color: "inherit",
              textDecoration: "none",
              fontWeight: 700,
              mr: 4,
            }}
          >
            SmartFridgeApp
          </Typography>

          {isMobile ? (
            <>
              <Box sx={{ flexGrow: 1 }} />
              {state.token && invites.length > 0 && (
                <IconButton color="inherit" onClick={() => setInviteDialogOpen(true)}>
                  <Badge badgeContent={invites.length} color="error">
                    <MailIcon />
                  </Badge>
                </IconButton>
              )}
              <IconButton color="inherit" onClick={() => setDrawerOpen(true)}>
                <MenuIcon />
              </IconButton>
              <Drawer
                anchor="right"
                open={drawerOpen}
                onClose={() => setDrawerOpen(false)}
              >
                <Box sx={{ width: 260, pt: 2 }}>
                  <Box sx={{ textAlign: "center", mb: 2 }}>
                    <Box sx={{ fontSize: 40, lineHeight: 1 }}>🍳</Box>
                    <Typography variant="subtitle2" color="text.secondary" sx={{ mt: 0.5 }}>SmartFridgeApp</Typography>
                  </Box>
                  <List>
                    {navLinks.map((link) => (
                      <ListItemButton
                        key={link.to}
                        component={RouterLink}
                        to={link.to}
                        selected={location.pathname.startsWith(link.to)}
                        onClick={() => setDrawerOpen(false)}
                      >
                        <ListItemText primary={link.label} />
                      </ListItemButton>
                    ))}
                    {state.token ? (
                      <ListItemButton onClick={handleLogout}>
                        <ListItemText primary="Logout" />
                      </ListItemButton>
                    ) : (
                      <ListItemButton
                        component={RouterLink}
                        to="/login"
                        onClick={() => setDrawerOpen(false)}
                      >
                        <ListItemText primary="Sign in" />
                      </ListItemButton>
                    )}
                  </List>
                </Box>
              </Drawer>
            </>
          ) : (
            <>
              <Box sx={{ display: "flex", gap: 0.5, flexGrow: 1 }}>
                {navLinks.map((link) => (
                  <Button
                    key={link.to}
                    component={RouterLink}
                    to={link.to}
                    color="inherit"
                    startIcon={link.icon}
                    sx={{
                      borderRadius: 2,
                      px: 2,
                      backgroundColor: location.pathname.startsWith(link.to)
                        ? "rgba(255,255,255,0.18)"
                        : "transparent",
                      backdropFilter: location.pathname.startsWith(link.to) ? "blur(4px)" : "none",
                      "&:hover": { backgroundColor: "rgba(255,255,255,0.12)" },
                    }}
                  >
                    {link.label}
                  </Button>
                ))}
              </Box>

              {state.token && mainKitchen && (
                <Chip
                  label={`🏠 ${mainKitchen.name}`}
                  size="small"
                  component={RouterLink}
                  to={`/KitchenItems/${mainKitchen.id}`}
                  clickable
                  sx={{
                    color: "inherit",
                    borderColor: "rgba(255,255,255,0.4)",
                    mr: 1,
                    "&:hover": { backgroundColor: "rgba(255,255,255,0.12)" },
                  }}
                  variant="outlined"
                />
              )}

              {state.token ? (
                <>
                  {invites.length > 0 && (
                    <IconButton color="inherit" onClick={() => setInviteDialogOpen(true)}>
                      <Badge badgeContent={invites.length} color="error">
                        <MailIcon />
                      </Badge>
                    </IconButton>
                  )}
                  <Button
                    color="inherit"
                    startIcon={<PersonIcon />}
                    onClick={(e) => setAnchorEl(e.currentTarget)}
                  >
                    {displayName}
                  </Button>
                  <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={() => setAnchorEl(null)}
                  >
                    <MenuItem onClick={handleLogout}>
                      <LogoutIcon sx={{ mr: 1 }} fontSize="small" />
                      Logout
                    </MenuItem>
                  </Menu>
                </>
              ) : (
                <Button
                  color="inherit"
                  component={RouterLink}
                  to="/login"
                  startIcon={<LoginIcon />}
                >
                  Sign in
                </Button>
              )}
            </>
          )}
        </Toolbar>
      </Container>

      {/* Pending Invites Dialog */}
      <Dialog
        open={inviteDialogOpen}
        onClose={() => setInviteDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>Pending Kitchen Invites</DialogTitle>
        <DialogContent dividers>
          {invites.length === 0 ? (
            <Typography color="text.secondary">No pending invites.</Typography>
          ) : (
            <Stack spacing={2}>
              {invites.map((inv) => (
                <Box
                  key={inv.id}
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    p: 1.5,
                    borderRadius: 2,
                    bgcolor: "grey.50",
                  }}
                >
                  <Box>
                    <Typography fontWeight={600}>{inv.kitchenName}</Typography>
                    <Typography variant="body2" color="text.secondary">
                      Invited by {inv.inviterName || inv.inviterEmail}
                    </Typography>
                  </Box>
                  <Box sx={{ display: "flex", gap: 1 }}>
                    <Chip
                      icon={<CheckIcon />}
                      label="Accept"
                      color="success"
                      variant="outlined"
                      clickable
                      onClick={() => handleAccept(inv.id)}
                    />
                    <Chip
                      icon={<CloseIcon />}
                      label="Decline"
                      color="error"
                      variant="outlined"
                      clickable
                      onClick={() => handleDecline(inv.id)}
                    />
                  </Box>
                </Box>
              ))}
            </Stack>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setInviteDialogOpen(false)}>Close</Button>
        </DialogActions>
      </Dialog>
    </AppBar>
  );
}
