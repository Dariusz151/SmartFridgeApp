import { Box, Typography, Link, Stack } from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";

export default function Footer() {
  return (
    <Box
      component="footer"
      sx={{
        py: 2.5,
        px: 2,
        mt: "auto",
        background: "linear-gradient(135deg, #095456 0%, #0d7377 40%, #14a3a8 100%)",
        color: "#fff",
        textAlign: "center",
      }}
    >
      <Stack direction="row" spacing={1} justifyContent="center" alignItems="center">
        <Typography variant="body2">
          Made with
        </Typography>
        <FavoriteIcon sx={{ fontSize: 16, color: "#ff6f3c" }} />
        <Typography variant="body2">
          by{" "}
          <Link
            href="https://www.smartfridgeapp.pl"
            color="inherit"
            underline="hover"
            sx={{ fontWeight: 600 }}
          >
            smartfridgeapp.pl
          </Link>
          {" "}© {new Date().getFullYear()}
        </Typography>
      </Stack>
    </Box>
  );
}
