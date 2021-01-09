import React, { useState, useEffect } from "react";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import DialogContentText from "@material-ui/core/DialogContentText";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";

import ListItemIcon from "@material-ui/core/ListItemIcon";
import CloseIcon from "@material-ui/icons/Close";
import DoneIcon from "@material-ui/icons/Done";

const RecipeDetails = ({ isDialogOpened, handleClose, recipeDetails }) => {
  const [foodProductsFormatted, setFoodProductsFormatted] = useState([
    {
      foodProductId: "1",
      foodProductName: "fpName1",
      amountValue: { value: "1", unit: "Grams" },
    },
  ]);

  useEffect(() => {
    var jsonObj = JSON.parse(recipeDetails.foodProducts);
    var foodProductsList = jsonObj.ArrayOfFoodProductDetails.FoodProductDetails;
    setFoodProductsFormatted([]);
    for (const [key, value] of Object.entries(foodProductsList)) {
      setFoodProductsFormatted((oldArray) => [
        ...oldArray,
        {
          foodProductId: value.FoodProductId,
          foodProductName: value.FoodProductName,
          amountValue: {
            value: value.AmountValue.Value,
            unit: value.AmountValue.Unit,
          },
        },
      ]);
    }
  }, [recipeDetails]);

  return (
    <React.Fragment>
      <Dialog
        open={isDialogOpened}
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
                <ListItem key={foodProduct.foodProductId}>
                  <ListItemIcon>
                    <DoneIcon fontSize="small" />
                  </ListItemIcon>
                  <ListItemText>
                    {foodProduct.foodProductName}{" "}
                    <span style={{ color: "#345131", paddingLeft: 10 }}>
                      {foodProduct.amountValue.value}{" "}
                      {foodProduct.amountValue.unit}
                    </span>
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
          <Button
            disabled
            onClick={() => {
              console.log("Use this recipe.");
            }}
            color="primary"
            variant="outlined"
          >
            Use this recipe
          </Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
};

export default RecipeDetails;
