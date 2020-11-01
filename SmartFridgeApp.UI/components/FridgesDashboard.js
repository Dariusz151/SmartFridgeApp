import React, { Component } from "react";
import { View } from "react-native";
import { getFridges } from "../services/FridgesService";

class FridgesDashboard extends Component {
  state = {
    imageFile: [],
    imageId: {},
    answers: [],
    score: [],
  };

  // async handleClick() {
  //   var fridges = await getFridges()
  //     .then((response) => {
  //       console.log(response);
  //     })
  //     .catch((err) => console.log(err));
  // }

  render() {
    return (
      <View>
        {/* <Button onPress={this.handleClick.bind(this)}>Siema</Button> */}
        <Button>siema</Button>
      </View>
    );
  }
}
