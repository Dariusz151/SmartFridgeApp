import React from "react";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import Link from "@material-ui/core/Link";

import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import FridgesDashboard from "./components/FridgesDashboard";
import RecipesDashboard from "./components/RecipesDashboard";
import Home from "./components/Home";

export default function App() {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <Router>
        <Switch>
          <Container component="main" className={classes.main} maxWidth="sm">
            <Home />
            <Route path="/fridges">
              <FridgesDashboard />
            </Route>
            <Route path="/recipes">
              <RecipesDashboard />
            </Route>
          </Container>
        </Switch>
        <footer className={classes.footer}>
          <Container maxWidth="xs">
            <Typography>
              <Link className={classes.linkStyle} href="/fridges">
                Fridges
              </Link>
              <Link className={classes.linkStyle} href="/foodProducts">
                FoodProducts
              </Link>
              <Link className={classes.linkStyle} href="/recipes">
                Recipes
              </Link>
            </Typography>
          </Container>
        </footer>
      </Router>
    </div>
  );
}

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    flexDirection: "column",
    minHeight: "100vh",
  },
  main: {
    marginTop: theme.spacing(8),
    marginBottom: theme.spacing(2),
  },
  linkStyle: {
    marginleft: 15,
    marginRight: 15,
  },
  footer: {
    padding: theme.spacing(3, 2),
    marginTop: "auto",
    backgroundColor:
      theme.palette.type === "light"
        ? theme.palette.grey[200]
        : theme.palette.grey[800],
  },
}));
