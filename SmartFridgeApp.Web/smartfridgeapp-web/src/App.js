import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import "./App.css";

import React from "react";

import FridgesDashboard from "./pages/FridgesDashboard";
import FridgeItemsDashboard from "./pages/FridgeItemsDashboard";
import FoodProducts from "./pages/FoodProducts";
import AddNewRecipe from "./components/AddNewRecipe";
import Recipes from "./pages/Recipes";
import Header from "./components/Header";
import BottomMenu from "./components/BottomMenu";
import AdminLogin from "./components/AdminLogin";
import Login from "./components/Login";
import Register from "./components/Register";

export const AuthContext = React.createContext();

const initialState = {
  isAdmin: false,
  token: null,
  loggedInUser: "",
};

const reducer = (state, action) => {
  switch (action.type) {
    case "LOGIN_ADMIN":
      sessionStorage.setItem("token", action.payload);
      return {
        ...state,
        isAdmin: true,
        token: action.payload,
      };
    case "LOGOUT_ADMIN":
      sessionStorage.clear();
      return {
        ...state,
        isAdmin: false,
        token: null,
      };
    default:
      return state;
  }
};

function App() {
  const [state, dispatch] = React.useReducer(reducer, initialState);

  React.useEffect(() => {
    const token = sessionStorage.getItem("token") || null;
    if (token) {
      dispatch({
        type: "LOGIN_ADMIN",
        payload: sessionStorage.getItem("token"),
      });
    }
  }, []);

  return (
    <div className="App">
      <Router>
        <AuthContext.Provider
          value={{
            state,
            dispatch,
          }}
        >
          <Header />

          <Switch>
            <Route exact path="/">
              <Redirect to="/fridges" />
            </Route>
            <Route path="/recipes/add">
              <AddNewRecipe />
            </Route>
            <Route path="/recipes">
              <Recipes />
            </Route>
            <Route path="/admin">
              <AdminLogin />
            </Route>
            {/* <Route path="/login">
              <Login />
            </Route>
            <Route path="/register">
              <Register />
            </Route> */}
            <Route path="/fridges">
              <FridgesDashboard />
            </Route>
            <Route path="/foodProducts">
              <FoodProducts />
            </Route>
            <Route path="/fridgeitems/:fridgeId">
              <FridgeItemsDashboard />
            </Route>
          </Switch>

          <BottomMenu />
        </AuthContext.Provider>
      </Router>
    </div>
  );
}

export default App;
