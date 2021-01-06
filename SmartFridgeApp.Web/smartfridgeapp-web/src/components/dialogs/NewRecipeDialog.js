import React, { useState, useEffect } from "react";
import configData from "../../config_url.json";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";
import CloseIcon from "@material-ui/icons/Close";
import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";

import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const NewRecipeDialog = ({ state, handleClose }) => {
  const [recipeName, setRecipeName] = useState("");
  const [recipeDesc, setRecipeDesc] = useState("");
  const [recipeCategory, setRecipeCategory] = useState("");
  const [levelOfDifficulty, setDifficulty] = useState("");
  const [requiredTime, setRequiredTime] = useState("");

  const [foodProducts, setFoodProducts] = useState([
    { foodProductId: 1, foodProductName: "fp" },
  ]);
  const [inputList, setInputList] = useState([
    { foodProductId: 0, foodProductName: "", amount: "" },
  ]);

  const handleInputChange = (e, index) => {
    const { name, value } = e.target;
    const list = [...inputList];
    list[index][name] = value;
    setInputList(list);
  };

  const handleRemoveClick = (index) => {
    const list = [...inputList];
    list.splice(index, 1);
    setInputList(list);
  };

  const handleAddClick = () => {
    setInputList([
      ...inputList,
      { foodProductId: 0, foodProductName: "", amount: "" },
    ]);
  };

  useEffect(() => {
    console.log("food products fetch");
    fetch(configData.SERVER_URL + "/api/foodProducts")
      .then((response) => response.json())
      .then((json) => {
        const items = json.map((item, index) => ({
          foodProductId: item.foodProductId,
          foodProductName: item.foodProductName,
        }));
        setFoodProducts(items);
      })
      .catch((error) => console.error(error));
  }, []);

  //   useEffect(() => {
  //     // fetch(configData.SERVER_URL + "/api/recipes")
  //     //   .then((response) => response.json())
  //     //   .then((json) => {
  //     //     const items = json.map((item, index) => ({
  //     //       foodProductId: item.foodProductId,
  //     //       foodProductName: item.foodProductName,
  //     //     }));
  //     //     setFoodProducts(items);
  //     //   })
  //     //   .catch((error) => console.error(error));
  //   }, []);

  const refreshForm = () => {
    console.log("TODO: clear all form values.");
  };

  const handleAdd = () => {
    const products = [];
    inputList.map((item) => {
      const newProduct = {
        foodProductId: item.foodProductId,
        amountValue: { value: parseInt(item.amount), unit: "NotAssigned" },
      };
      products.push(newProduct);
      return true;
    });

    const obj = {
      name: recipeName,
      description: recipeDesc,
      recipeCategory: parseInt(recipeCategory),
      requiredTime: parseInt(requiredTime),
      levelOfDifficulty: parseInt(levelOfDifficulty),
      products: products,
    };

    console.log(obj);

    fetch(configData.SERVER_URL + "/api/recipes/", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant add recipe!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        toast.success("Added new recipe!", {
          position: "bottom-center",
          autoClose: 1500,
        });

        return response;
      })
      .then(refreshForm())
      .then(() => handleClose())
      .catch((error) => console.log(error));
  };

  return (
    <React.Fragment>
      <ToastContainer />

      <Dialog
        open={state}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">Add Recipe</DialogTitle>
        <DialogActions></DialogActions>
        <DialogContent>
          {inputList.map((x, i) => {
            return (
              <div className="box">
                <Autocomplete
                  id="foodproducts-combobox"
                  options={foodProducts}
                  getOptionLabel={(option) => option.foodProductName}
                  // style={{ width: "40%" }}
                  onChange={(e, val) => {
                    const list = [...inputList];
                    list[i].foodProductId = val.foodProductId;
                    setInputList(list);
                  }}
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      label="Select food product"
                      variant="outlined"
                    />
                  )}
                />
                <TextField
                  // style={{ width: "40%" }}
                  // className="ml10"
                  name="amount"
                  placeholder="Enter Amount"
                  value={x.amount}
                  onChange={(e) => handleInputChange(e, i)}
                />
                <div className="btn-box">
                  {inputList.length !== 1 && (
                    <Button
                      color="default"
                      variant="contained"
                      className="mr10"
                      onClick={() => handleRemoveClick(i)}
                    >
                      <RemoveIcon />
                    </Button>
                  )}
                  {inputList.length - 1 === i && (
                    <Button
                      color="default"
                      variant="contained"
                      onClick={handleAddClick}
                    >
                      <AddIcon />
                    </Button>
                  )}
                </div>
              </div>
            );
          })}
          <TextField
            name="recipeName"
            label="Recipe name"
            fullWidth
            onChange={(e) => {
              setRecipeName(e.target.value);
            }}
            value={recipeName}
          />
          <TextField
            name="recipeDesc"
            label="Description"
            fullWidth
            onChange={(e) => {
              setRecipeDesc(e.target.value);
            }}
            value={recipeDesc}
          />
          <br />
          <br />
          <ToggleButtonGroup
            value={levelOfDifficulty}
            exclusive
            onChange={(event, diff) => {
              setDifficulty(diff);
            }}
            aria-label="text alignment"
          >
            <ToggleButton value="1" aria-label="left aligned">
              <Typography>Easy</Typography>
            </ToggleButton>
            <ToggleButton value="2" aria-label="centered">
              <Typography>Medium</Typography>
            </ToggleButton>
            <ToggleButton value="3" aria-label="right aligned">
              <Typography>Hard</Typography>
            </ToggleButton>
          </ToggleButtonGroup>
          <br /> <br />
          <ToggleButtonGroup
            value={recipeCategory}
            exclusive
            onChange={(event, cat) => {
              setRecipeCategory(cat);
            }}
            aria-label="text alignment"
          >
            <ToggleButton value="1" aria-label="centered">
              <Typography>Breakfast</Typography>
            </ToggleButton>
            <ToggleButton value="2" aria-label="left aligned">
              <Typography>Dinner</Typography>
            </ToggleButton>

            <ToggleButton value="3" aria-label="right aligned">
              <Typography>Supper</Typography>
            </ToggleButton>
            <ToggleButton value="4" aria-label="right aligned">
              <Typography>Other</Typography>
            </ToggleButton>
          </ToggleButtonGroup>
          <TextField
            name="requiredTime"
            label="Required time"
            fullWidth
            onChange={(e) => {
              setRequiredTime(e.target.value);
            }}
            value={requiredTime}
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

export default NewRecipeDialog;
