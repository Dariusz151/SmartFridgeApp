import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { MDBTable, MDBTableBody, MDBTableHead } from "mdbreact";
import { useParams } from "react-router-dom";
import Button from "@material-ui/core/Button";
import FastfoodIcon from "@material-ui/icons/Fastfood";
import CircularProgress from "@material-ui/core/CircularProgress";
import RefreshIcon from "@material-ui/icons/Refresh";

import TextField from "@material-ui/core/TextField";
import NewUserDialog from "../components/dialogs/NewUserDialog";
import NewFridgeItemDialog from "../components/dialogs/NewFridgeItemDialog";

const FridgeItemsDashboard = () => {
  const { fridgeId } = useParams();
  const [dummyState, rerender] = useState(1);
  const [newUserDialogState, setNewUserDialogState] = useState(false);
  const [newFridgeItemDialogState, setNewFridgeItemDialogState] = useState(
    false
  );
  const [amount, setAmount] = useState("");
  const [usersLoading, finishUsersLoading] = useState(true);
  const [fridgeItemsLoading, finishFridgeItemsLoading] = useState(true);
  const [fridgeUsers, setUsersData] = useState([]);
  const [selectedUserId, selectUser] = useState("None");
  const [rows, setRows] = useState([
    {
      id: 1,
      categoryName: "category",
      productName: "product",
      value: 10,
      unit: "unit",
      consume: (
        <div>
          <Button gradient="aqua" size="sm">
            Consume
          </Button>
        </div>
      ),
    },
  ]);

  useEffect(() => {
    console.log("useEffect fridgeItem");
    if (selectedUserId != "None") {
      fetch(
        configData.SERVER_URL +
          "/api/fridgeItems/" +
          fridgeId +
          "/" +
          selectedUserId
      )
        .then((response) => response.json())
        .then((json) => {
          const rowsArray = json.map((item, index) => ({
            id: index + 1,
            productName: item.productName,
            categoryName: item.categoryName,
            value: item.value,
            unit: item.unit,
            consume: (
              <span>
                <TextField
                  name="amount"
                  label="Amount"
                  fullWidth
                  onChange={handleNumberChange}
                  value={amount}
                ></TextField>

                <Button
                  variant="contained"
                  color="primary"
                  startIcon={<FastfoodIcon />}
                  onClick={() => handleConsume(item.fridgeItemId, item.unit)}
                >
                  Consume
                </Button>
              </span>
            ),
          }));

          setRows(rowsArray);
        })
        .catch((error) => console.error(error))
        .finally(() => finishFridgeItemsLoading(false));
    }
  }, [selectedUserId, dummyState]);

  useEffect(() => {
    console.log("useEffect users");
    fetch(configData.SERVER_URL + "/api/fridgeUsers/" + fridgeId)
      .then((response) => response.json())
      .then((json) => {
        setUsersData([
          { id: "None", name: "None" },
          ...json,
          // { id: "All", name: "All" },
        ]);
      })
      .catch((error) => console.error(error))
      .finally(() => finishUsersLoading(false));
  }, [dummyState]);

  const handleRefresh = () => {
    rerender(dummyState + 1);
  };

  const handleNumberChange = (e) => {
    const re = /^[0-9\b]+$/;
    if (e.target.value === "" || re.test(e.target.value)) {
      console.log(e.target.value);
      //setAmount("");
    }
  };

  const handleConsume = (fridgeItemId, unit) => {
    //TODO: get value from user (dialog)

    const obj = {
      fridgeItemId: fridgeItemId,
      userId: selectedUserId,
      amountValue: {
        value: 0,
        unit: unit,
      },
    };

    console.log(obj);

    fetch(configData.SERVER_URL + "/api/fridgeItems/" + fridgeId + "/consume", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    }).then(console.log("Success Consume"));
  };

  const handleCloseUserDialog = () => {
    setNewUserDialogState(false);
  };

  const handleCloseFridgeItemDialog = () => {
    setNewFridgeItemDialogState(false);
  };

  return (
    <div>
      <NewUserDialog
        fridgeId={fridgeId}
        state={newUserDialogState}
        handleClose={handleCloseUserDialog}
      />
      <NewFridgeItemDialog
        fridgeId={fridgeId}
        selectedUserId={selectedUserId}
        state={newFridgeItemDialogState}
        handleClose={handleCloseFridgeItemDialog}
      />
      {usersLoading ? (
        <div>
          <CircularProgress />
          <p>Loading users</p>
        </div>
      ) : (
        <div className="btn-group userBtns">
          {fridgeUsers.map((item) => {
            return (
              <Button
                variant="contained"
                color={selectedUserId === item.id ? "secondary" : "primary"}
                onClick={() => selectUser(item.id)}
              >
                {item.name}
              </Button>
            );
          })}
        </div>
      )}

      <div>
        <div className="btn-group userBtns">
          <Button
            variant="outlined"
            color="primary"
            onClick={() => setNewFridgeItemDialogState(true)}
          >
            Add new fridge item
          </Button>
          <Button
            variant="outlined"
            color="primary"
            onClick={() => setNewUserDialogState(true)}
          >
            Add new user
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
      </div>

      <br />
      {fridgeItemsLoading ? (
        <p>Select user</p>
      ) : (
        <MDBTable>
          <MDBTableHead columns={columns} />
          <MDBTableBody rows={rows} />
        </MDBTable>
      )}
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
    label: "Product",
    field: "productName",
    sort: "asc",
  },
  {
    label: "Category",
    field: "categoryName",
    sort: "asc",
  },
  {
    label: "Value",
    field: "value",
    sort: "asc",
  },
  {
    label: "Unit",
    field: "unit",
    sort: "asc",
  },
  {
    label: "#",
    field: "consume",
    sort: "asc",
  },
];

export default FridgeItemsDashboard;
