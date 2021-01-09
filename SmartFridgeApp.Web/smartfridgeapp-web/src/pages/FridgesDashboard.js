import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { MDBTable, MDBTableBody, MDBTableHead } from "mdbreact";
import { Link } from "react-router-dom";
import KitchenIcon from "@material-ui/icons/Kitchen";
import AddIcon from "@material-ui/icons/Add";
import Button from "@material-ui/core/Button";
import CircularProgress from "@material-ui/core/CircularProgress";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";

import { Container, Typography, Paper } from "@material-ui/core";

import CloseIcon from "@material-ui/icons/Close";
import RefreshIcon from "@material-ui/icons/Refresh";
import DeleteIcon from "@material-ui/icons/Delete";

import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const FridgesDashboard = () => {
  const [dataLoading, finishLoading] = useState(true);
  const [dummyState, rerender] = useState(1);
  const [open, setOpen] = useState(false);
  const [values, setValues] = useState({
    name: "",
    address: "",
    desc: "",
  });
  const [rows, setRows] = useState([
    {
      id: 1,
      name: "fridge",
      address: "",
      buttons: <Link to="/" />,
      delete: <Button></Button>,
    },
  ]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const handleRefresh = () => {
    rerender(dummyState + 1);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    refreshForm();
    setOpen(false);
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
      .then(setOpen(false))
      .catch((error) => console.log(error));
  };

  const handleDelete = (fridgeId) => {
    const obj = {
      fridgeId: fridgeId,
    };

    fetch(configData.SERVER_URL + "/api/fridges", {
      method: "delete",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant delete fridge!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        toast.success("Fridge deleted!", {
          position: "bottom-center",
          autoClose: 1500,
        });

        return response;
      })
      .catch((error) => console.log(error));
  };

  const refreshForm = () => {
    setValues({
      name: "",
      address: "",
      desc: "",
    });
  };

  useEffect(() => {
    fetch(configData.SERVER_URL + "/api/fridges")
      .then((response) => response.json())
      .then((json) => {
        const rowsArray = json.map((item, index) => ({
          id: index + 1,
          name: item.name,
          address: item.address,
          buttons: (
            <Link to={`/fridgeitems/${item.id}`}>
              <Button
                variant="contained"
                color="primary"
                startIcon={<KitchenIcon />}
              >
                Details
              </Button>
            </Link>
          ),
          delete: (
            <Button
              disabled
              variant="contained"
              color="secondary"
              startIcon={<DeleteIcon />}
              onClick={() => handleDelete(item.id)}
            >
              Remove
            </Button>
          ),
        }));

        setRows(rowsArray);
      })
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, [dummyState]);

  return (
    <div>
      <ToastContainer />
      <Dialog
        open={open}
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
      <br />
      <Container fixed>
        <div className="btn-group userBtns">
          <Button
            variant="outlined"
            color="primary"
            onClick={handleClickOpen}
            startIcon={<AddIcon />}
          >
            Add new
          </Button>
          <Button
            variant="outlined"
            color="primary"
            onClick={handleRefresh}
            startIcon={<RefreshIcon />}
          >
            Refresh
          </Button>
        </div>

        <br />
        <br />
        {dataLoading ? (
          <div>
            <CircularProgress />
            <p>Loading fridges</p>
          </div>
        ) : (
          <MDBTable btn>
            <MDBTableHead columns={columns} />
            <MDBTableBody rows={rows} />
          </MDBTable>
        )}
      </Container>
    </div>
  );
};

const columns = [
  {
    label: "#",
    field: "id",
    sort: "asc",
  },
  {
    label: "Name",
    field: "name",
    sort: "asc",
  },
  {
    label: "Address",
    field: "address",
    sort: "asc",
  },
  {
    label: "",
    field: "buttons",
    sort: "asc",
  },
  {
    label: "",
    field: "delete",
    sort: "asc",
  },
];

export default FridgesDashboard;
