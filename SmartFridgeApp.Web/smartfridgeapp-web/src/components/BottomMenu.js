import React from "react";
import { MDBContainer, MDBFooter, MDBBox } from "mdbreact";

export default function BottomMenu() {
  return (
    <div className="footer">
      <MDBFooter color="teal darken-2" className="font-small pt-2 mt-4">
        <div className="footer-copyright text-center teal darken-2">
          <MDBContainer style={{ minHeight: "30px" }}>
            <span>
              <a href="https://www.smartfridgeapp.pl">Smartfridgeapp.pl | </a>
              Dariusz Kozioł &copy; {new Date().getFullYear()}
            </span>
          </MDBContainer>
        </div>
      </MDBFooter>
    </div>
  );
}
