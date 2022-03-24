import React from "react";
import {
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarNav,
  MDBNavItem,
  MDBNavLink,
  MDBNavbarToggler,
  MDBCollapse,
  MDBIcon,
  MDBDropdown,
  MDBDropdownItem,
  MDBDropdownToggle,
  MDBDropdownMenu,
} from "mdbreact";

import { AuthContext } from "../App";

function Header() {
  const { state, dispatch } = React.useContext(AuthContext);

  const [isOpen, open] = React.useState(false);
  const [actualPath, setActualPath] = React.useState("");
  const toggleCollapse = () => {
    open(!isOpen);
  };
  return (
    <React.Fragment>
      <MDBNavbar color="teal darken-2" dark expand="md">
        <MDBNavbarBrand>
          <strong className="white-text">SmartFridgeApp</strong>
        </MDBNavbarBrand>
        <MDBNavbarToggler onClick={toggleCollapse} />
        <MDBCollapse id="navbarCollapse3" isOpen={isOpen} navbar>
          <MDBNavbarNav left>
            <MDBNavItem active={actualPath === "fridges"}>
              <MDBNavLink
                to="/fridges"
                onClick={() => setActualPath("fridges")}
              >
                Fridges
              </MDBNavLink>
            </MDBNavItem>
            <MDBNavItem active={actualPath === "foodproducts"}>
              <MDBNavLink
                to="/foodProducts"
                onClick={() => setActualPath("foodproducts")}
              >
                FoodProducts
              </MDBNavLink>
            </MDBNavItem>
            <MDBNavItem active={actualPath === "recipes"}>
              <MDBNavLink
                to="/recipes"
                onClick={() => setActualPath("recipes")}
              >
                Recipes
              </MDBNavLink>
            </MDBNavItem>
            <MDBNavItem></MDBNavItem>
          </MDBNavbarNav>
          {state.isAdmin ? (
            <MDBNavbarNav right>
              <MDBNavItem>
                <MDBDropdown>
                  <MDBDropdownToggle nav caret>
                    <MDBIcon icon="user" /> Admin
                  </MDBDropdownToggle>
                  <MDBDropdownMenu className="dropdown-default">
                    <MDBDropdownItem
                      href="/fridges"
                      onClick={() =>
                        dispatch({
                          type: "LOGOUT_ADMIN",
                        })
                      }
                    >
                      Logout
                    </MDBDropdownItem>
                  </MDBDropdownMenu>
                </MDBDropdown>
              </MDBNavItem>
            </MDBNavbarNav>
          ) : (
            <MDBNavbarNav right>
              <MDBNavItem>
                <MDBNavLink to="/admin">Login Admin</MDBNavLink>
              </MDBNavItem>
            </MDBNavbarNav>
          )}
        </MDBCollapse>
      </MDBNavbar>
    </React.Fragment>
  );
}
export default Header;
