import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { MDBTable, MDBTableBody, MDBTableHead } from "mdbreact";
import Button from "@material-ui/core/Button";
import VisibilityIcon from "@material-ui/icons/Visibility";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import CloseIcon from "@material-ui/icons/Close";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import DialogContentText from "@material-ui/core/DialogContentText";

import CircularProgress from "@material-ui/core/CircularProgress";

const Recipes = () => {
  const [dataLoading, finishLoading] = useState(true);
  const [open, setOpen] = useState(false);
  const [rows, setRows] = useState([
    {
      id: 1,
      name: "name",
      category: "cat",
      reqTime: "req",
      difficulty: "diff",
      details: (
        <Button gradient="aqua" size="sm">
          Details
        </Button>
      ),
    },
  ]);

  const [recipeDetails, setRecipeDetails] = useState({
    recipeName: "recipe",
    recipeDescription: "desc",
    foodProducts: "items",
  });
  const [foodProductsFormatted, setFoodProductsFormatted] = useState([
    {
      foodProductId: "1",
      foodProductName: "fpName1",
      AmountValue: { Value: "1", Unit: "Grams" },
    },
  ]);

  function formatFoodProducts(foodProds) {
    var foodProductsString = foodProds;
    var jsonObj = JSON.parse(foodProductsString);
    var foodProductsList = jsonObj.ArrayOfFoodProductDetails.FoodProductDetails;

    setFoodProductsFormatted([]);
    for (const [key, value] of Object.entries(foodProductsList)) {
      setFoodProductsFormatted((oldArray) => [
        ...oldArray,
        {
          FoodProductId: value.FoodProductId,
          FoodProductName: value.FoodProductName,
          AmountValue: {
            Value: value.AmountValue.Value,
            Unit: value.AmountValue.Unit,
          },
        },
      ]);
    }
  }

  const handleClickOpen = (recipeName, desc, foodProducts) => {
    setRecipeDetails({
      recipeName: recipeName,
      recipeDescription: desc,
      foodProducts: foodProducts,
    });

    formatFoodProducts(foodProducts);
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  const handleAdd = () => {
    console.log("use this recipe");
  };

  useEffect(() => {
    fetch(configData.SERVER_URL + "/api/recipes")
      .then((response) => response.json())
      .then((json) => {
        const rowsArray = json.map((item, index) => ({
          id: index + 1,
          name: item.recipeName,
          category: item.recipeCategory,
          reqTime: item.requiredTime,
          difficulty: item.levelOfDifficulty,
          details: (
            <Button
              variant="contained"
              color="primary"
              startIcon={<VisibilityIcon />}
              onClick={() =>
                handleClickOpen(
                  item.recipeName,
                  item.description,
                  item.foodProducts
                )
              }
            >
              Details
            </Button>
          ),
        }));

        setRows(rowsArray);
      })
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  return (
    <div>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">
          {recipeDetails.recipeName}
        </DialogTitle>
        <DialogContent dividers>
          <List component="nav" aria-label="secondary mailbox folders">
            {foodProductsFormatted.map((foodProduct) => {
              return (
                <ListItem key={foodProduct.FoodProductId}>
                  <ListItemText>
                    - {foodProduct.foodProductName}{" "}
                    {foodProduct.AmountValue.Value}{" "}
                    {foodProduct.AmountValue.Unit}
                  </ListItemText>
                </ListItem>
              );
            })}
          </List>
          <br />
          <DialogContentText>
            {recipeDetails.recipeDescription}
          </DialogContentText>
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
          <Button onClick={handleAdd} color="primary" variant="outlined">
            Use this recipe
          </Button>
        </DialogActions>
      </Dialog>
      {dataLoading ? (
        <div>
          <CircularProgress />
          <p>Loading recipes</p>
        </div>
      ) : (
        <MDBTable btn>
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
    label: "Name",
    field: "name",
    sort: "asc",
  },
  {
    label: "Category",
    field: "category",
    sort: "asc",
  },
  {
    label: "Required time",
    field: "reqTime",
    sort: "asc",
  },
  {
    label: "Difficulty",
    field: "difficulty",
    sort: "asc",
  },
  {
    label: "#",
    field: "details",
    sort: "asc",
  },
];

export default Recipes;
