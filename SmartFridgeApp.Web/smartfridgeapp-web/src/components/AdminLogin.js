import React, { useState, useEffect } from "react";
import configData from "../config_url.json";

import Button from "@material-ui/core/Button";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";
import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";
import { Container, Box, Grid, TextField } from "@material-ui/core";

const AdminLogin = () => {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");

  return (
    <React.Fragment>
      <p>Admin Login:</p>
      <TextField
        name="login"
        label="Login"
        fullWidth
        onChange={(e) => {
          setLogin(e.target.value);
        }}
        value={login}
      />
      <TextField
        name="password"
        label="Password"
        fullWidth
        onChange={(e) => {
          setPassword(e.target.value);
        }}
        value={password}
      />
    </React.Fragment>
  );
};

export default AdminLogin;
