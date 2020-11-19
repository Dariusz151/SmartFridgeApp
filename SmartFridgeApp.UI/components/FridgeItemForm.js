import React, { useEffect, useState } from "react";
import FormInput from "./common/FormInput";
import FormButton from "./common/FormButton";
import { View, StyleSheet } from "react-native";
import { Title, IconButton } from "react-native-paper";

export default function FridgeItemForm({ route, navigation }) {
  const { fridgeName, fridgeId } = route.params;

  const [foodProduct, setFoodProduct] = useState("");
  const [value, setValue] = useState("");
  const [note, setNote] = useState("");
  const [unit, setUnit] = useState("");

  return (
    <View style={styles.container}>
      <Title style={styles.titleText}>
        Add item to {fridgeName} to {fridgeId}
      </Title>
      <FormInput
        labelName="FoodProduct"
        value={foodProduct}
        autoCapitalize="none"
        onChangeText={(foodProduct) => setFoodProduct(foodProduct)}
      />
      <FormInput
        labelName="Value"
        value={value}
        onChangeText={(value) => setValue(value)}
      />
      <FormInput
        labelName="Note"
        value={note}
        onChangeText={(note) => setNote(note)}
      />
      <FormInput
        labelName="Unit"
        value={unit}
        onChangeText={(unit) => setUnit(unit)}
      />
      <FormButton
        title="Add"
        modeValue="contained"
        labelStyle={styles.loginButtonLabel}
        onPress={() => {
          console.log(foodProduct);
        }}
      />
      <IconButton
        icon="keyboard-backspace"
        size={70}
        style={styles.navButton}
        color="#6646ee"
        onPress={() => navigation.goBack()}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: "#f5f5f5",
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  titleText: {
    fontSize: 24,
    marginBottom: 10,
  },
  loginButtonLabel: {
    fontSize: 22,
  },
  navButtonText: {
    fontSize: 16,
  },
  navButton: {
    marginTop: 10,
  },
});
