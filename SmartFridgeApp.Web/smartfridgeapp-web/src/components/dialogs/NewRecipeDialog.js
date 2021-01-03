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
  //   const [foodProducts, setFoodProducts] = useState([
  //     { foodProductId: 1, foodProductName: "fp" },
  //   ]);
  //   const [foodProductId, setFoodProductId] = useState(0);
  //   const [inputCount, incrementInputs] = useState(1);
  const [foodProductInputs, addFoodProductInput] = useState([
    {
      input: (
        <React.Fragment>
          <TextField
            name="input"
            label="input"
            fullWidth
            //   onChange={(e) => {
            //     setRecipeName(e.target.value);
            //   }}
            //   value={recipeName}
          />
        </React.Fragment>
      ),
    },
  ]);

  const [recipeName, setRecipeName] = useState("");
  const [recipeDesc, setRecipeDesc] = useState("");

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

  //   const refreshForm = () => {
  //     setValues({
  //       note: "",
  //       unit: "",
  //       value: "",
  //     });
  //     setFoodProductId(0);
  //   };

  const handleAdd = () => {
    console.log("add");
  };

  const handleAddNewInput = () => {
    addFoodProductInput((oldArray) => [
      ...oldArray,
      {
        input: (
          <React.Fragment>
            <TextField
              name=""
              label=""
              fullWidth
              //   onChange={(e) => {
              //     setRecipeName(e.target.value);
              //   }}
              //   value={recipeName}
            />
          </React.Fragment>
        ),
      },
    ]);
    //incrementInputs(inputCount + 1);
  };

  const handleDeleteInput = () => {
    //const newArray = foodProductInputs;
    //newArray.pop();
    addFoodProductInput([]);
    //addFoodProductInput(newArray);
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
        <DialogActions>
          <Button
            onClick={handleDeleteInput}
            color="primary"
            variant="outlined"
          >
            <RemoveIcon />
          </Button>
          <Button
            onClick={handleAddNewInput}
            color="primary"
            // startIcon={}
            variant="outlined"
          >
            <AddIcon />
          </Button>
        </DialogActions>
        <DialogContent>
          {foodProductInputs.map((item, index) => {
            return <React.Fragment key={index}>{item.input}</React.Fragment>;
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
