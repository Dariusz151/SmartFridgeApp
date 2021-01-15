import React from "react";
import { Card, makeStyles } from "@material-ui/core";

export default function CarouselSlide(props) {
  const { recipe } = props.content;

  const useStyles = makeStyles(() => ({
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

  const classes = useStyles();

  return (
    <Card className={classes.card}>
      <h1>{recipe.description}</h1>
      <p>{recipe.levelOfDifficulty}</p>
      <p>{recipe.requiredTime}</p>
    </Card>
  );
}
