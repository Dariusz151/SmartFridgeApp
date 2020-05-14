import React, { Component } from "react";
import { getFridges } from "../services/smartfridgeappService";
import NewFridgeItemForm from "./NewFridgeItemForm";

class FridgesDashboard extends Component {
  state = {
    fridges: [],
  };

  async componentDidMount() {
    await getFridges()
      .then((response) => {
        this.setState({ fridges: response.data });
        console.log(response);
      })
      .catch((msg) => console.log(msg));
  }

  render() {
    return (
      <React.Fragment>
        {this.state.fridges.map((elem) => (
          <h1>{elem.name}</h1>
        ))}
        <NewFridgeItemForm></NewFridgeItemForm>
      </React.Fragment>
    );
  }
}

export default FridgesDashboard;
