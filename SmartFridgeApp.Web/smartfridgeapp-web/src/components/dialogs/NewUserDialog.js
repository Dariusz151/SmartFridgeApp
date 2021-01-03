import React, { useState } from "react";
import configData from "../../config_url.json";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";

const NewUserDialog = ({ fridgeId, state, handleClose }) => {
  const [values, setValues] = useState({
    name: "",
    email: "",
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const refreshForm = () => {
    setValues({
      name: "",
      email: "",
    });
  };

  const handleAdd = () => {
    const obj = {
      user: { name: values.name, email: values.email },
    };

    fetch(configData.SERVER_URL + "/api/fridgeUsers/" + fridgeId, {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then(refreshForm())
      .then(() => handleClose());
  };

  return (
    <Dialog
      open={state}
      onClose={handleClose}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle id="form-dialog-title">Add user</DialogTitle>
      <DialogContent>
        <TextField
          name="name"
          label="Name"
          fullWidth
          onChange={handleInputChange}
          value={values.name}
        />
        <TextField
          name="email"
          label="Email"
          fullWidth
          onChange={handleInputChange}
          value={values.email}
        />
      </DialogContent>
      <DialogActions>
        <Button
          onClick={handleClose}
          color="secondary"
          variant="outlined"
          startIcon={<CloseIcon />}
        >
          Cancel
        </Button>
        <Button
          color="primary"
          startIcon={<AddIcon />}
          variant="outlined"
          onClick={handleAdd}
        >
          Add
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default NewUserDialog;
