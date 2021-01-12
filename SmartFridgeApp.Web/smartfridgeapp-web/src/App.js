import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import "./App.css";

import FridgesDashboard from "./pages/FridgesDashboard";
import FridgeItemsDashboard from "./pages/FridgeItemsDashboard";
import FoodProducts from "./pages/FoodProducts";
import AddNewRecipe from "./components/AddNewRecipe";
import Recipes from "./pages/Recipes";
import Header from "./components/Header";
import BottomMenu from "./components/BottomMenu";

function App() {
  return (
    <div className="App">
      <Router>
        <div>
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
        </div>
      </Router>
    </div>
  );
}

export default App;
