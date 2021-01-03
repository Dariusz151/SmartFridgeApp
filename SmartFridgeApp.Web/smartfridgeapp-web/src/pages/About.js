import React from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

function About() {
  return (
    <div className="About">
      <h1>About page</h1>
      {/* <Link
        className="btn btn-primary"
        to={{
          pathname: "/fridgeitems",
          state: 1,
        }}
      >
        Send 1
      </Link> */}
      <Link to="/">Go to Home page</Link>
    </div>
  );
}

export default About;
