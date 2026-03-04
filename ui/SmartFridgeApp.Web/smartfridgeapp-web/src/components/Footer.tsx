import { Box, Typography, Link } from "@mui/material";

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
      <Typography variant="body2">
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
    </Box>
  );
}
