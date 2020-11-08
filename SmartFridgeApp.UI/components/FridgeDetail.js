import React from "react";
import { StyleSheet, View, Text, TouchableOpacity } from "react-native";
import * as RootNavigation from "../RootNavigation";

export default function FridgeDetail({ navigation }) {
  return (
    <View style={styles.container}>
      <TouchableOpacity style={styles.button}>
        <Text>Hello</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
    width: "100%",
    padding: 20,
  },
  button: {
    padding: 20,
  },
});
