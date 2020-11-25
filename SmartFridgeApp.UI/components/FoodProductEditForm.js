import React, { useEffect, useState } from "react";
import FormInput from "./common/FormInput";
import FormButton from "./common/FormButton";
import { View, StyleSheet, Text, Dimensions } from "react-native";
import { Title, IconButton } from "react-native-paper";
import DropDownPicker from "react-native-dropdown-picker";

const { width, height } = Dimensions.get("screen");

export default function FoodProductEditForm({ route, navigation }) {
  const { fridgeName, fridgeId } = route.params;

  const [foodProduct, setFoodProduct] = useState(0);
  const [value, setValue] = useState(1);
  const [note, setNote] = useState("");
  const [unit, setUnit] = useState("");

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

  function postData() {
    const obj = {
      userId: "72EFEE28-86A0-4929-ACE1-746D9B6B49D7",
      fridgeItem: {
        foodProductId: foodProduct,
        value: value,
        note: note,
        unit: unit,
      },
    };
    console.log(JSON.stringify(obj));
    fetch("https://localhost:5001/api/fridgeItems/" + fridgeId + "/add", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    });
  }

  return (
    <View style={styles.container}>
      <Title style={styles.titleText}>
        Add item to {fridgeName} to {fridgeId}
      </Title>
      <DropDownPicker
        items={items}
        containerStyle={{ width: width / 1.5, height: height / 15 }}
        style={{ backgroundColor: "#fafafa" }}
        itemStyle={{
          justifyContent: "flex-start",
        }}
        labelStyle={{
          fontSize: 20,
          textAlign: "left",
          color: "#000",
        }}
        dropDownStyle={{ backgroundColor: "#fafafa" }}
        searchable={true}
        searchablePlaceholder="Search for an item"
        searchablePlaceholderTextColor="gray"
        seachableStyle={{}}
        searchableError={() => <Text>Not Found</Text>}
        onChangeItem={(item) => {
          setFoodProduct(item.value);
        }}
      />
      <FormInput
        labelName="Value"
        value={value}
        onChangeText={(stringNum) => {
          setValue(parseInt(stringNum));
        }}
      />
      <FormInput
        labelName="Note"
        value={note}
        onChangeText={(note) => setNote(note)}
      />
      <DropDownPicker
        items={[
          { label: "Grams", value: "Grams" },
          { label: "Mililiter", value: "Mililiter" },
          { label: "NotAssigned", value: "NotAssigned" },
        ]}
        containerStyle={{ width: width / 1.5, height: height / 15 }}
        style={{ backgroundColor: "#fafafa" }}
        itemStyle={{
          justifyContent: "flex-start",
        }}
        labelStyle={{
          fontSize: 20,
          textAlign: "left",
          color: "#000",
        }}
        dropDownStyle={{ backgroundColor: "#fafafa" }}
        onChangeItem={(item) => {
          setUnit(item.value);
        }}
      />
      <FormButton
        title="Add"
        modeValue="contained"
        labelStyle={styles.loginButtonLabel}
        onPress={() => {
          postData();
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
  numericInputStyle: {
    width: width,
    height: height / 15,
    fontSize: 20,
  },
});
