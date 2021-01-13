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
function Header() {
  const [isAdmin, loginAdmin] = React.useState(false);
  const [isOpen, open] = React.useState(false);
  const [actualPath, setActualPath] = React.useState("fridges");
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
          {isAdmin ? (
            <MDBNavbarNav right>
              <MDBNavItem>
                <MDBDropdown>
                  <MDBDropdownToggle nav caret>
                    <MDBIcon icon="user" />
                  </MDBDropdownToggle>
                  <MDBDropdownMenu className="dropdown-default">
                    <MDBDropdownItem href="#!">Logout</MDBDropdownItem>
                  </MDBDropdownMenu>
                </MDBDropdown>
              </MDBNavItem>
            </MDBNavbarNav>
          ) : (
            <MDBNavbarNav right>
              <MDBNavItem>
                <MDBNavLink to="/admin">Admin</MDBNavLink>
              </MDBNavItem>
            </MDBNavbarNav>
          )}
        </MDBCollapse>
      </MDBNavbar>
    </React.Fragment>
  );
}
export default Header;
