import React, { useEffect, useState } from "react";
import { StyleSheet, View, Text, TouchableOpacity, Switch } from "react-native";
import * as RootNavigation from "../RootNavigation";

export default function Homepage({ navigation }) {
  return (
    <View style={styles.container}>
      <TouchableOpacity
        style={styles.button}
        onPress={() => RootNavigation.navigate("FridgesPage")}
      >
        <Text style={styles.title}>Home</Text>
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
  title: {
    fontSize: 20,
    fontWeight: "bold",
  },
});
