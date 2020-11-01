import React from "react";
import { StyleSheet, Text, View } from "react-native";

export default function Homepage() {
  return (
    <View style={styles.container}>
      <Text>Home Page</Text>
      <Text>Siemano</Text>
      <Text>Siemano2</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
  },
});
