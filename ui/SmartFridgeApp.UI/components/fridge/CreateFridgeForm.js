import React, { useEffect, useState } from "react";
import FormInput from "../common/FormInput";
import FormButton from "../common/FormButton";
import { View, StyleSheet, Text, Dimensions, ToastAndroid } from "react-native";
import { Title, IconButton } from "react-native-paper";

const { width, height } = Dimensions.get("screen");

export default function CreateFridgeForm({ navigation }) {
  const [name, setName] = useState("");
  const [desc, setDesc] = useState("");
  const [address, setAddress] = useState("");

  useEffect(() => {}, []);

  function postData() {
    const obj = {
      name: name,
      desc: desc,
      address: address,
    };
    fetch("https://localhost:5001/api/fridges/", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      //   .then(ToastAndroid.show("Fridge created!", ToastAndroid.SHORT))
      .then(navigation.goBack())
      .catch((error) => {
        setName("");
        setDesc("");
        setAddress("");
      });
  }

  return (
    <View style={styles.container}>
      <Title>Add new fridge</Title>
      <FormInput
        labelName="Name"
        value={name}
        onChangeText={(name) => setName(name)}
      />
      <FormInput
        labelName="Description"
        value={desc}
        onChangeText={(desc) => setDesc(desc)}
      />
      <FormInput
        labelName="Address"
        value={address}
        onChangeText={(address) => setAddress(address)}
      />
      <FormButton
        title="Add"
        modeValue="contained"
        labelStyle={styles.addButtonLabel}
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
  addButtonLabel: {
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
