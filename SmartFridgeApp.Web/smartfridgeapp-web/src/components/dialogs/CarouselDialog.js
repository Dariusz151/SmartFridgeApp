import React from "react";
import { Card, makeStyles } from "@material-ui/core";

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import CloseIcon from "@material-ui/icons/Close";
import AddIcon from "@material-ui/icons/Add";

import CarouselSlide from "../CarouselSlide";

import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";

function Arrow(props) {
  const { direction, clickFunction } = props;
  const icon =
    direction === "left" ? <ChevronLeftIcon /> : <ChevronRightIcon />;

  return <div onClick={clickFunction}>{icon}</div>;
}

const CarouselDialog = ({ state, handleClose, recipes }) => {
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

  return (
    <React.Fragment>
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
            onClick={() => {
              console.log("use this recipe");
            }}
          >
            Use this recipe
          </Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
};

export default CarouselDialog;
