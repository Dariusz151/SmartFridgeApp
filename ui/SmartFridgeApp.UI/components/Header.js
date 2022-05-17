import React from "react";
import { StyleSheet, Text, View, Image } from "react-native";
import { Colors } from "react-native-paper";
import logo from "../assets/fridge2.png";

export default function Header(props) {
  return (
    <View style={styles.header}>
      <View>
        <Text style={styles.text}>{props.headerDisplay}</Text>
      </View>
      {/* <Image source={logo} style={{ width: 35, height: 35 }} /> */}
    </View>
  );
}

const styles = StyleSheet.create({
  header: {
    width: "100%",
    height: 70,
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: Colors.blueGrey50,
  },
  text: {
    fontFamily: "OpenSans",
    fontSize: 20,
    paddingTop: 10,
  },
});
