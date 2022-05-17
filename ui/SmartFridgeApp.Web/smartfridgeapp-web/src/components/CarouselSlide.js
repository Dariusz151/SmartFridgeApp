import React, { useEffect } from "react";
import { Card, makeStyles } from "@material-ui/core";

import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import CloseIcon from "@material-ui/icons/Close";
import DoneIcon from "@material-ui/icons/Done";

import DialogContentText from "@material-ui/core/DialogContentText";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
  },
  card: {
    borderRadius: 5,
    padding: "75px 50px",
    margin: "0px 25px",
    width: "500px",
    boxShadow: "20px 20px 20px black",
    display: "flex",
    justifyContent: "center",
  },
}));

export default function CarouselSlide(props) {
  const { recipe } = props.content;
  const classes = useStyles();

  // useEffect(() => {
  //   console.log("recipe");
  //   console.log(recipe.foodProducts);
  // }, []);

  return (
    <React.Fragment>
      <List component="nav" aria-label="secondary">
        {recipe.foodProducts.map((fp) => {
          return (
            <ListItem key={fp.foodProductId}>
              <ListItemIcon>
                <DoneIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText>
                {" "}
                {fp.foodProductName}{" "}
                <span style={{ color: "#345131", paddingLeft: 10 }}>
                  {fp.amountValue.value} {fp.amountValue.unit}
                </span>
              </ListItemText>
            </ListItem>
          );
        })}
      </List>
      {/* <DialogContentText>{recipe.description}</DialogContentText> */}
      {/* <div className={classes.root}>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <Paper className={classes.paper}>xs=12 sm=6</Paper>
          </Grid>
          <Grid item xs={12} sm={6}>
            <Paper className={classes.paper}>xs=12 sm=6</Paper>
          </Grid>
          <Grid item xs={6} sm={3}>
            <Paper className={classes.paper}>xs=6 sm=3</Paper>
          </Grid>
          <Grid item xs={6} sm={3}>
            <Paper className={classes.paper}>xs=6 sm=3</Paper>
          </Grid>
          <Grid item xs={6} sm={3}>
            <Paper className={classes.paper}>xs=6 sm=3</Paper>
          </Grid>
          <Grid item xs={6} sm={3}>
            <Paper className={classes.paper}>xs=6 sm=3</Paper>
          </Grid>{" "}
          <Grid item xs={12}>
            <h1>{recipe.description}</h1>
          </Grid>
        </Grid>
      </div> */}
      {/* <Card className={classes.card}>
        <p>{recipe.levelOfDifficulty}</p>
        <p>{recipe.requiredTime}</p>
      </Card> */}
    </React.Fragment>
  );
}
