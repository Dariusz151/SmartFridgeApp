import React from "react";
import { Link } from "react-router-dom";

function Home() {
  return (
    <div className="Home">
      <h1>Home page</h1>
      <Link to="/about">Go to About page</Link>
      <h1>Fridges</h1>
      <Link to="/fridges">Go to Fridges page</Link>
    </div>
  );
}

export default Home;
