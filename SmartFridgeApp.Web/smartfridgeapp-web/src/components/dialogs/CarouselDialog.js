import React from "react";
import { Card, makeStyles } from "@material-ui/core";

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import CloseIcon from "@material-ui/icons/Close";
import AddIcon from "@material-ui/icons/Add";

import configData from "../../config_url.json";

import CarouselSlide from "../CarouselSlide";

import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Arrow(props) {
  const { direction, clickFunction } = props;
  const icon =
    direction === "left" ? <ChevronLeftIcon /> : <ChevronRightIcon />;

  return <div onClick={clickFunction}>{icon}</div>;
}

const CarouselDialog = ({ state, handleClose, recipes, userId, fridgeId }) => {
  const [index, setIndex] = React.useState(0);
  const recipeContent = recipes[index];
  const numSlides = recipes.length;

  const onArrowClick = (direction) => {
    console.log(recipeContent);
    console.log(direction);
    const increment = direction === "left" ? -1 : 1;
    const newIndex = (index + increment + numSlides) % numSlides;
    setIndex(newIndex);
  };

  const handleUseThisRecipe = () => {
    const obj = {
      userId: userId,
      fridgeId: fridgeId,
      foodProducts: recipeContent.foodProducts,
    };

    fetch(configData.SERVER_URL + "/api/fridgeItems/ConsumeRecipe", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant consume this recipe!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        return response;
      })
      .then((response) => response.json())
      .then((body) => {
        console.log(body);
        handleClose();
      })
      .catch((error) => console.log(error));
    console.log("Recipe used by" + userId);
  };

  return (
    <React.Fragment>
      <ToastContainer />
      <Dialog
        open={state}
        onClose={handleClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">{recipeContent.name}</DialogTitle>
        <DialogContent>
          <span>
            <Arrow
              direction="left"
              clickFunction={() => onArrowClick("left")}
            />
            <CarouselSlide content={{ recipe: recipeContent }} />
            <Arrow
              direction="right"
              clickFunction={() => onArrowClick("right")}
            />
          </span>
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
            onClick={handleUseThisRecipe}
          >
            Use this recipe
          </Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
};

export default CarouselDialog;
