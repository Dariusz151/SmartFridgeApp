import React from "react";

import {
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarNav,
  MDBNavItem,
  MDBNavLink,
  MDBNavbarToggler,
  MDBCollapse,
} from "mdbreact";
function Header() {
  const [isOpen, open] = React.useState(false);
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
        <MDBCollapse id="navbarCollapse3" isOpen={true} navbar>
          <MDBNavbarNav left>
            <MDBNavItem active>
              <MDBNavLink to="/fridges">Fridges</MDBNavLink>
            </MDBNavItem>
            <MDBNavItem>
              <MDBNavLink to="/foodProducts">FoodProducts</MDBNavLink>
            </MDBNavItem>
            <MDBNavItem>
              <MDBNavLink to="/recipes">Recipes</MDBNavLink>
            </MDBNavItem>
            <MDBNavItem></MDBNavItem>
          </MDBNavbarNav>
          <MDBNavbarNav right>
            <MDBNavItem>
              <MDBNavLink to="/">Admin</MDBNavLink>
            </MDBNavItem>
          </MDBNavbarNav>
        </MDBCollapse>
      </MDBNavbar>
    </React.Fragment>
  );
}
export default Header;
