import React from "react";
import { MDBContainer, MDBFooter } from "mdbreact";

export default function BottomMenu() {
  return (
    <React.Fragment>
      <MDBFooter color="teal darken-2" className="font-small pt-4 mt-4">
        <div className="footer-copyright text-center py-3 teal darken-2">
          <MDBContainer fluid>
            <a href="https://www.smartfridgeapp.pl">Smartfridgeapp.pl</a>
            <p>&copy; {new Date().getFullYear()} Copyright: Dariusz Kozioł</p>
          </MDBContainer>
        </div>
      </MDBFooter>
    </React.Fragment>
  );
}
