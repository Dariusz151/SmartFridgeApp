import React, { useEffect, useState } from "react";
import FormInput from "./common/FormInput";
import FormButton from "./common/FormButton";
import { View, StyleSheet } from "react-native";
import { Title, IconButton } from "react-native-paper";
import Icon from "react-native-vector-icons/Feather";
import DropDownPicker from "react-native-dropdown-picker";

export default function FridgeItemForm({ route, navigation }) {
  const { fridgeName, fridgeId } = route.params;

  const [foodProduct, setFoodProduct] = useState(0);
  const [value, setValue] = useState("");
  const [note, setNote] = useState("");
  const [unit, setUnit] = useState("");
  const [country, setCountry] = useState("uk");

  const [items, setItems] = useState([{ label: "", value: "" }]);

  useEffect(() => {
    async function getData() {
      const response = await fetch("https://localhost:5001/api/foodProducts");
      const body = await response.json();
      setItems(
        body.map((item) => ({
          label: item.foodProductName,
          value: item.foodProductId,
        }))
      );
    }
    getData();
  }, []);

  return (
    <View style={styles.container}>
      <Title style={styles.titleText}>
        Add item to {fridgeName} to {fridgeId}
      </Title>
      <DropDownPicker
        items={items}
        containerStyle={{ height: 40 }}
        style={{ backgroundColor: "#fafafa" }}
        itemStyle={{
          justifyContent: "flex-start",
        }}
        dropDownStyle={{ backgroundColor: "#fafafa" }}
        onChangeItem={(item) => {
          setFoodProduct(item.value);
        }}
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
