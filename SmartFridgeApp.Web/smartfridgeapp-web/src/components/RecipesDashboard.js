import React, { useEffect, useState } from "react";
import Typography from "@material-ui/core/Typography";

import { makeStyles } from "@material-ui/core/styles";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";

const useStyles = makeStyles((theme) => ({
  root: {
    width: "100%",
    maxWidth: 360,
    backgroundColor: theme.palette.background.paper,
  },
}));

export default function RecipesDashboard() {
  const classes = useStyles();
  const [dataLoading, finishLoading] = useState(true);
  const [fridgesData, setData] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/api/fridges")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  return (
    <React.Fragment>
      <Typography variant="h2" component="h1" gutterBottom>
        Recipes
      </Typography>
      <div className={classes.root}>
        {dataLoading ? (
          <div>Loading...</div>
        ) : (
          <List component="nav" aria-label="main mailbox folders">
            {fridgesData.map((item) => {
              return (
                <ListItem button>
                  <ListItemText primary={item.name}></ListItemText>
                </ListItem>
              );
            })}
          </List>
        )}
      </div>
    </React.Fragment>
  );
}
