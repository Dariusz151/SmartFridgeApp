import React, { useState } from "react";
import configData from "../../config_url.json";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import Autocomplete from "@material-ui/lab/Autocomplete";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const NewFoodProductDialog = ({ categories, state, handleClose }) => {
  const [values, setValues] = useState({
    name: "",
  });
  const [categoryId, setCategory] = useState(0);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const refreshForm = () => {
    setValues({
      name: "",
      category: "",
    });
  };

  const onSelectChange = (event, val) => {
    console.log(val);
    if (val != undefined && val != null) setCategory(val.categoryId);
  };

  const handleAdd = () => {
    const obj = {
      name: values.name,
      category: categoryId,
    };
    console.log(sessionStorage.getItem("token"));
    fetch(configData.SERVER_URL + "/api/foodProducts", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
        "Authorization": "Bearer " + sessionStorage.getItem("token"),
      },
      body: JSON.stringify(obj),
    })
      .then(function (response) {
        if (!response.ok) {
          if (response.status === 401) {
            toast.error("Unauthorized operation!", {
              position: "bottom-center",
              autoClose: 1500,
            });
          } else {
            toast.error("Cant add food product!", {
              position: "bottom-center",
              autoClose: 1500,
            });
          }
          throw Error(response.statusText);
        }
        toast.success("Food product added!", {
          position: "bottom-center",
          autoClose: 1500,
        });
        return response;
      })
      .then(refreshForm())
      .catch((error) => {
        console.log(error);
      })
      .then(() => handleClose());
  };

  return (
    <React.Fragment>
      <ToastContainer />

      <Dialog
        open={state}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">Add food product</DialogTitle>
        <DialogContent>
          <TextField
            name="name"
            label="Name"
            fullWidth
            onChange={handleInputChange}
            value={values.name}
          />
          <br />
          <br />
          <Autocomplete
            id="categories-combobox"
            options={categories}
            getOptionLabel={(option) => option.name}
            style={{ width: "100%" }}
            onChange={onSelectChange}
            renderInput={(params) => (
              <TextField
                {...params}
                label="Select category"
                variant="outlined"
              />
            )}
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
    </React.Fragment>
  );
};

export default NewFoodProductDialog;
