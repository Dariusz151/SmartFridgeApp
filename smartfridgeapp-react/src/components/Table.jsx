import React, { Component } from "react";
import { getFridgeItems } from "../services/smartfridgeappService";

class Table extends Component {
  state = {
    fridgeItems: [],
  };

  async componentDidMount() {
    await getFridgeItems(1, 1)
      .then((response) => {
        this.setState({ fridgeItems: response.data });
        console.log(response);
      })
      .catch((msg) => console.log(msg));
  }

  render() {
    return (
      <React.Fragment>
        <table className="table table-hover">
          <thead>
            <tr>
              <th scope="col" className="tableHeader">
                Id
              </th>
              <th scope="col" className="tableHeader">
                Product name
              </th>
              <th scope="col" className="tableHeader">
                Category
              </th>
              <th scope="col" className="tableHeader">
                Value
              </th>
              <th scope="col" className="tableHeader">
                Unit
              </th>
            </tr>
          </thead>
          <tbody>
            {this.state.fridgeItems.map((elem) => (
              <tr>
                <th scope="row">#</th>
                <td>{elem.productName}</td>
                <td>{elem.categoryName}</td>
                <td>{elem.value}</td>
                <td>{elem.unit}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </React.Fragment>
    );
  }
}

export default Table;
