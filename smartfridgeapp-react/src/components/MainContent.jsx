import React, { Component } from "react";
import { getFridges } from "../services/smartfridgeappService";
import FridgesDashboard from "./FridgesDashboard.jsx";
import FridgesPanel from "./FridgesPanel.jsx";
import Table from "./Table.jsx";

class MainContent extends Component {
  state = {};

  async componentDidMount() {}

  render() {
    return (
      <React.Fragment>
        <div className="flex-container">
          <div className="main-panel">
            <Table></Table>
          </div>
        </div>
        <div class-name="fridges-panel">
          <FridgesPanel></FridgesPanel>
        </div>
        <div className="bottom-panel">
          <FridgesDashboard></FridgesDashboard>
        </div>
      </React.Fragment>
    );
  }
}

export default MainContent;
