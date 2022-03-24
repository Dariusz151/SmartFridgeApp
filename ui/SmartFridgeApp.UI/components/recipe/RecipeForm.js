import React, { useEffect, useState } from "react";
import FormInput from "../common/FormInput";
import FormButton from "../common/FormButton";
import { View, StyleSheet, Text, Dimensions } from "react-native";
import { Title, IconButton } from "react-native-paper";
import DropDownPicker from "react-native-dropdown-picker";

const { width, height } = Dimensions.get("screen");

export default function RecipeForm({ navigation }) {
  const [name, setName] = useState("");
  const [desc, setDesc] = useState("");
  const [reqTime, setReqTime] = useState(0);
  const [difficulty, setDifficulty] = useState(0);
  const [category, setCategory] = useState(0);
  const [categories, setCategories] = useState([{ label: "", value: "" }]);

  useEffect(() => {
    // async function getData() {
    //   const response = await fetch(
    //     "https://localhost:5001/api/recipes/categories"
    //   );
    //   const body = await response.json();
    //   setCategories(
    //     body.map((item) => ({
    //       label: item.name,
    //       value: item.recipeCategoryId,
    //     }))
    //   );
    // }
    // getData();

    const mockData = [
      {
        name: "Kolacja",
        recipeCategoryId: 1,
      },
      {
        name: "Obiad",
        recipeCategoryId: 2,
      },
      {
        name: "Śniadanie",
        recipeCategoryId: 3,
      },
    ];
    setCategories(
      mockData.map((item) => ({
        label: item.name,
        value: item.recipeCategoryId,
      }))
    );
  }, []);

  function postData() {
    const obj = {
      name: name,
      description: desc,
      recipeCategory: category,
      requiredTime: reqTime,
      levelOfDifficulty: difficulty,
      products: [
        {
          foodProductId: 1,
          amountValue: {
            value: 1,
            unit: "NotAssigned",
          },
        },
        {
          foodProductId: 2,
          amountValue: {
            value: 3,
            unit: "NotAssigned",
          },
        },
      ],
    };
    console.log(JSON.stringify(obj));
    fetch("https://localhost:5001/api/recipes/", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    });
  }

  return (
    <View style={styles.container}>
      <Title style={styles.titleText}>Add new recipe</Title>
      <FormInput
        labelName="Name"
        value={name}
        onChangeText={(name) => setName(name)}
      />
      <FormInput
        labelName="Desc"
        value={desc}
        onChangeText={(desc) => setDesc(desc)}
      />
      <DropDownPicker
        items={categories}
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
          setCategory(parseInt(item.value));
        }}
      />
      <FormInput
        labelName="Required time"
        value={reqTime}
        onChangeText={(stringNum) => {
          setReqTime(parseInt(stringNum));
        }}
      />
      <DropDownPicker
        items={[
          { label: "Easy", value: 0 },
          { label: "Medium", value: 1 },
          { label: "Hard", value: 2 },
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
          setDifficulty(parseInt(item.value));
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
