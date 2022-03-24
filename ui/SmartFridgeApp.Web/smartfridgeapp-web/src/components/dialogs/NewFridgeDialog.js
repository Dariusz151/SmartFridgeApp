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

import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const NewFridgeDialog = ({ isOpen, handleClose }) => {
  const [values, setValues] = useState({
    name: "",
    address: "",
    desc: "",
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const refreshForm = () => {
    setValues({
      name: "",
      address: "",
      desc: "",
    });
  };

  const handleAdd = () => {
    const obj = {
      name: values.name,
      address: values.address,
      desc: values.desc,
    };
    fetch(configData.SERVER_URL + "/api/fridges", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant add fridge!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        toast.success("Added new fridge!", {
          position: "bottom-center",
          autoClose: 1500,
        });

        return response;
      })
      .then(refreshForm())
      .then(handleClose())
      .catch((error) => console.log(error));
  };

  return (
    <React.Fragment>
      <ToastContainer />
      <Dialog
        open={isOpen}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">Add new Fridge</DialogTitle>
        <DialogContent>
          <TextField
            name="name"
            label="Fridge name"
            fullWidth
            onChange={handleInputChange}
            value={values.name}
          />
          <TextField
            name="address"
            label="Address"
            fullWidth
            onChange={handleInputChange}
            value={values.address}
          />
          <TextField
            name="desc"
            label="Description"
            fullWidth
            onChange={handleInputChange}
            value={values.desc}
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
            onClick={handleAdd}
            color="primary"
            startIcon={<AddIcon />}
            variant="outlined"
          >
            Add
          </Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
};

export default NewFridgeDialog;
