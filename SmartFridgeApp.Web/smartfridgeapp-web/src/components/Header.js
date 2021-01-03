import React from "react";
import { NavLink } from "react-router-dom";

function Header() {
  return (
    <nav>
      {/* <NavLink exact activeClassName="active" to="/" className="header-links">
        Home
      </NavLink> */}
      <NavLink activeClassName="active" to="/fridges" className="header-links">
        Fridges
      </NavLink>
      <NavLink
        activeClassName="active"
        to="/foodProducts"
        className="header-links"
      >
        FoodProducts
      </NavLink>
      <NavLink activeClassName="active" to="/recipes" className="header-links">
        Recipes
      </NavLink>
    </nav>
  );
}
export default Header;
