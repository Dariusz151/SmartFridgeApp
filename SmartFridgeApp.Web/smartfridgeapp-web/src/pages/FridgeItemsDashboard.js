import React, { useState, useEffect, useRef } from "react";
import configData from "../config_url.json";
import { MDBTable, MDBTableBody, MDBTableHead } from "mdbreact";
import { useParams } from "react-router-dom";
import Button from "@material-ui/core/Button";
import FastfoodIcon from "@material-ui/icons/Fastfood";
import CircularProgress from "@material-ui/core/CircularProgress";
import RefreshIcon from "@material-ui/icons/Refresh";
import NumericInput from "react-numeric-input";
import NewUserDialog from "../components/dialogs/NewUserDialog";
import NewFridgeItemDialog from "../components/dialogs/NewFridgeItemDialog";

import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const FridgeItemsDashboard = () => {
  const { fridgeId } = useParams();
  const [dummyState, rerender] = useState(1);
  const [newUserDialogState, setNewUserDialogState] = useState(false);
  const [newFridgeItemDialogState, setNewFridgeItemDialogState] = useState(
    false
  );
  const amount = useRef(0);
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
        <span>
          <NumericInput min={0} max={100} />
          <Button
            variant="contained"
            color="primary"
            startIcon={<FastfoodIcon />}
          >
            Consume
          </Button>
        </span>
      ),
    },
  ]);

  useEffect(() => {
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
                <NumericInput
                  min={0}
                  max={10000}
                  value={amount.curent}
                  onChange={handleChange}
                />
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

  const handleChange = (e) => {
    const re = /^[0-9\b]+$/;
    if (e === "" || re.test(e)) {
      amount.current = e;
    } else {
      amount.current = 0;
    }
  };
  const handleConsume = (fridgeItemId, unit) => {
    console.log(amount.current);
    if (amount.current < 1) {
      toast.error("Invalid amount!", {
        position: "bottom-center",
        autoClose: 1500,
      });
      return;
    }
    const obj = {
      fridgeItemId: fridgeItemId,
      userId: selectedUserId,
      amountValue: {
        value: amount.current,
        unit: unit,
      },
    };

    fetch(configData.SERVER_URL + "/api/fridgeItems/" + fridgeId + "/consume", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant consume fridge item!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        toast.success("Consumed!", {
          position: "bottom-center",
          autoClose: 1500,
        });
        amount.current = 0;
        rerender(dummyState + 1);
        return response;
      })
      .catch((error) => console.log(error));
  };

  return (
    <div>
      <ToastContainer />
      <NewUserDialog
        fridgeId={fridgeId}
        state={newUserDialogState}
        handleClose={() => {
          setNewUserDialogState(false);
        }}
      />
      <NewFridgeItemDialog
        fridgeId={fridgeId}
        selectedUserId={selectedUserId}
        state={newFridgeItemDialogState}
        handleClose={() => {
          setNewFridgeItemDialogState(false);
        }}
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
            onClick={() => {
              rerender(dummyState + 1);
            }}
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
