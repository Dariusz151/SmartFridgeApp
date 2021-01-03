import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import "./App.css";

import Home from "./pages/Home";
import About from "./pages/About";
import FridgesDashboard from "./pages/FridgesDashboard";
import FridgeItemsDashboard from "./pages/FridgeItemsDashboard";
import FoodProducts from "./pages/FoodProducts";
import Recipes from "./pages/Recipes";
import Header from "./components/Header";

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
            <Route path="/about">
              <About />
            </Route>
          </Switch>
        </div>
      </Router>
    </div>
  );
}

export default App;
