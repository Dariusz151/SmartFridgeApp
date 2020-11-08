import React from "react";
import { StyleSheet, View, TouchableOpacity } from "react-native";
import * as RootNavigation from "../RootNavigation";
import { Button, Text } from "react-native-paper";

export default function FridgeDetail({ route, navigation }) {
  const { fridgeId } = route.params;

  return (
    <View style={styles.container}>
      <TouchableOpacity style={styles.button}>
        <Button onPress={() => console.log(fridgeId)}>
          Console fridge item
        </Button>
        <Text>Hello {fridgeId}</Text>
        {/* <Text>itemId: {JSON.stringify(itemId)}</Text> */}
      </TouchableOpacity>
      <Button onPress={() => navigation.goBack()}>Back</Button>
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
