import React, { useState } from "react";
import configData from "../config_url.json";
import { useHistory } from "react-router-dom";

import { MDBTypography } from "mdbreact";
import { Container, TextField } from "@material-ui/core";

import Button from "@material-ui/core/Button";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";

import { AuthContext } from "../App";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Login = () => {
  const { dispatch } = React.useContext(AuthContext);
  let history = useHistory();
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");

  const refreshForm = () => {
    setLogin("");
    setPassword("");
  };

  const handleClose = () => {
    history.push("/fridges");
  };

  const handleLogin = () => {
    console.log("Login");
    // const obj = {
    //   login: login,
    //   password: password,
    // };
    // fetch(configData.SERVER_URL + "/api/auth", {
    //   method: "post",
    //   headers: {
    //     "Content-Type": "application/json",
    //   },
    //   body: JSON.stringify(obj),
    // })
    //   .then((response) => {
    //     if (!response.ok) {
    //       toast.error("Cant login to admin account!", {
    //         position: "bottom-center",
    //         autoClose: 1500,
    //       });
    //       throw Error(response.statusText);
    //     }
    //     toast.success("Logged in as admin!", {
    //       position: "bottom-center",
    //       autoClose: 1500,
    //     });
    //     return response;
    //   })
    //   .then((response) => response.json())
    //   .then((data) => {
    //     dispatch({
    //       type: "LOGIN",
    //       payload: data.value,
    //     });
    //   })
    //   .then(refreshForm())
    //   .then(handleClose())
    //   .catch((error) => console.log(error));
  };

  return (
    <React.Fragment>
      <ToastContainer />
      <Container style={{ minHeight: "75vh", maxWidth: "50vw" }}>
        <MDBTypography tag="h2" style={{ marginTop: "20px" }}>
          Login:
        </MDBTypography>
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
          type="password"
          fullWidth
          onChange={(e) => {
            setPassword(e.target.value);
          }}
          value={password}
        />

        <Button
          onClick={handleClose}
          color="secondary"
          variant="outlined"
          style={{ marginTop: "50px", marginRight: "10px" }}
          startIcon={<CloseIcon />}
        >
          Cancel
        </Button>
        <Button
          onClick={handleLogin}
          color="primary"
          style={{ marginTop: "50px", marginRight: "10px" }}
          startIcon={<AddIcon />}
          variant="contained"
        >
          Login
        </Button>
      </Container>
    </React.Fragment>
  );
};

export default Login;
