import React, { useState, useEffect } from "react";
import configData from "../config_url.json";

import { MDBDataTable } from "mdbreact";
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

import { Container } from "@material-ui/core";

import CloseIcon from "@material-ui/icons/Close";
import RefreshIcon from "@material-ui/icons/Refresh";
import DeleteIcon from "@material-ui/icons/Delete";
import MoreVertIcon from "@material-ui/icons/MoreVert";

import NewFridgeDialog from "../components/dialogs/NewFridgeDialog";
import { AuthContext } from "../App";

import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const FridgesDashboard = () => {
  const { state, dispatch } = React.useContext(AuthContext);
  const [dataLoading, finishLoading] = useState(true);
  const [dummyState, rerender] = useState(1);
  const [newFridgeDialogOpen, newFridgeDialog] = useState(false);

  const [rows, setRows] = useState([
    {
      id: 1,
      name: "fridge",
      address: "",
      buttons: <Link to="/" />,
      delete: <Button></Button>,
    },
  ]);

  const handleRefresh = () => {
    rerender(dummyState + 1);
  };

  const handleClose = () => {
    newFridgeDialog(false);
  };

  const handleDelete = (fridgeId) => {
    const obj = {
      fridgeId: fridgeId,
    };

    if (state.isAuthenticated) {
      fetch(configData.SERVER_URL + "/api/fridges", {
        method: "delete",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + state.token,
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
    }
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
                startIcon={<MoreVertIcon />}
              >
                Details
              </Button>
            </Link>
          ),
          delete: (
            <Button
              disabled={!state.isAuthenticated}
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
  }, [dummyState, state]);

  return (
    <React.Fragment>
      <ToastContainer />
      <NewFridgeDialog isOpen={newFridgeDialogOpen} handleClose={handleClose} />
      <Container fixed>
        <Button
          style={styles.topButton}
          variant="outlined"
          color="primary"
          onClick={() => {
            newFridgeDialog(true);
          }}
          startIcon={<AddIcon />}
        >
          Add new
        </Button>
        <Button
          style={styles.topButton}
          variant="outlined"
          color="primary"
          onClick={handleRefresh}
          startIcon={<RefreshIcon />}
        >
          Refresh
        </Button>

        {dataLoading ? (
          <div>
            <CircularProgress />
            <p>Loading fridges</p>
          </div>
        ) : (
          <MDBDataTable
            paging={true}
            hover
            entriesOptions={[5, 10, 20]}
            entries={5}
            pagesAmount={3}
            data={{ columns: columns, rows: rows }}
          ></MDBDataTable>
        )}
      </Container>
    </React.Fragment>
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

const styles = {
  topButton: {
    paddingTop: "10px",
    paddingBottom: "10px",
    marginTop: "30px",
    // marginRight: "8px",
    // marginLeft: "20px",
  },
};

export default FridgesDashboard;
