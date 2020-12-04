import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableHighlight,
  Modal,
  Dimensions,
} from "react-native";
import * as RootNavigation from "../../RootNavigation";
import {
  ActivityIndicator,
  Button,
  Text,
  Colors,
  DataTable,
  IconButton,
} from "react-native-paper";

import FormInput from "../common/FormInput";
import FormButton from "../common/FormButton";
import DropDownPicker from "react-native-dropdown-picker";

const { width, height } = Dimensions.get("screen");

export default function FridgeDetail({ route, navigation }) {
  const { fridgeId, fridgeName } = route.params;
  const [dataLoading, finishLoading] = useState(true);

  const [fridgeItems, setFridgeData] = useState([]);
  const [fridgeUsers, setUsersData] = useState([]);
  const [selectedUserId, selectUser] = useState("None");

  const [modalVisible, setModalVisible] = useState(false);

  const [foodProduct, setFoodProduct] = useState(0);
  const [value, setValue] = useState(1);
  const [note, setNote] = useState("");
  const [unit, setUnit] = useState("");

  const [items, setItems] = useState([{ label: "", value: "" }]);

  useEffect(() => {
    async function getData() {
      console.log("Fetch FoodProducts from API.");
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

  useEffect(
    () => {
      // const unsubscribe = navigation.addListener("focus", () => {

      fetch("https://localhost:5001/api/fridgeUsers/" + fridgeId)
        .then((response) => response.json())
        .then((json) => {
          setUsersData([
            { id: "None", name: "None" },
            ...json,
            { id: "All", name: "All" },
          ]);
        })
        .catch((error) => console.error(error))
        .finally(() => finishLoading(false));
      console.log("Fetch FridgeUsers from API.");

      getFridgeItemsByUser(selectedUserId);
    },
    // return unsubscribe;
    // }
    [navigation, selectedUserId]
  );

  function getFridgeItemsByUser(userId) {
    if (userId === "All") {
      userId = "";
    }

    if (userId != "None") {
      fetch("https://localhost:5001/api/fridgeitems/" + fridgeId + "/" + userId)
        .then((response) => response.json())
        .then((json) => setFridgeData(json))
        .catch((error) => console.error(error))
        .finally(() => finishLoading(false));

      console.log("Fetch FridgeItems for user from API.");
    } else {
      setFridgeData([]);
    }
  }

  function renderButton(itemId, itemName) {
    if (itemId === selectedUserId) {
      return <Text style={styles.selectedUserButton}>{itemName}</Text>;
    } else {
      return <Text style={styles.userButton}>{itemName}</Text>;
    }
  }

  function ResetFormInputs() {
    setFoodProduct(0);
    setValue(1);
    setNote("");
    setUnit("");
  }

  function postData() {
    const obj = {
      userId: selectedUserId,
      fridgeItem: {
        foodProductId: foodProduct,
        value: value,
        note: note,
        unit: unit,
      },
    };
    fetch("https://localhost:5001/api/fridgeItems/" + fridgeId + "/add", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    });
    setModalVisible(!modalVisible);
    ResetFormInputs();
  }

  return (
    <View style={styles.outerContainer}>
      <Text>Hello {selectedUserId}</Text>
      {fridgeUsers.map((item) => {
        return (
          <TouchableHighlight
            key={item.id}
            onPress={() => {
              selectUser(item.id);
            }}
          >
            {renderButton(item.id, item.name)}
          </TouchableHighlight>
        );
      })}
      <View style={styles.innerContainer}>
        <View style={styles.fridgeItemsPane}>
          {dataLoading ? (
            <ActivityIndicator animating={true} color={Colors.blue300} />
          ) : (
            <View>
              <DataTable>
                <DataTable.Header>
                  <DataTable.Title>Name</DataTable.Title>
                  <DataTable.Title>Value</DataTable.Title>
                  <DataTable.Title>Unit</DataTable.Title>
                  <DataTable.Title>Category</DataTable.Title>
                </DataTable.Header>
                {fridgeItems.map((fridgeItem) => {
                  return (
                    <DataTable.Row key={fridgeItem.fridgeItemId}>
                      <DataTable.Cell>{fridgeItem.productName}</DataTable.Cell>
                      <DataTable.Cell>{fridgeItem.value}</DataTable.Cell>
                      <DataTable.Cell>{fridgeItem.unit}</DataTable.Cell>
                      <DataTable.Cell>{fridgeItem.categoryName}</DataTable.Cell>
                    </DataTable.Row>
                  );
                })}
              </DataTable>
            </View>
          )}
        </View>
      </View>
      {selectedUserId != "None" && selectedUserId != "All" ? (
        <Button onPress={() => setModalVisible(true)}>Add new item</Button>
      ) : (
        <Text>Choose a user</Text>
      )}
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          Alert.alert("Modal has been closed.");
        }}
      >
        <View style={styles.centeredView}>
          <View style={styles.modalView}>
            <Text style={styles.modalText}>siema</Text>
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

            <TouchableHighlight
              style={{ ...styles.openButton, backgroundColor: "#2196F3" }}
              onPress={() => {
                setModalVisible(!modalVisible);
              }}
            >
              <Text style={styles.textStyle}>Hide</Text>
            </TouchableHighlight>
          </View>
        </View>
      </Modal>
      <IconButton
        icon="keyboard-backspace"
        size={50}
        style={styles.navButton}
        color="#6646ee"
        onPress={() => navigation.goBack()}
      ></IconButton>
    </View>
  );
}

const styles = StyleSheet.create({
  outerContainer: {
    flex: 1,
    flexDirection: "column",
    height: "80%",
  },
  container: {
    flex: 1,
    backgroundColor: "#eee",
    alignItems: "center",
    justifyContent: "center",
    width: "100%",
    padding: 20,
  },
  innerContainer: {
    flexDirection: "row",
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
    width: "100%",
    padding: 20,
  },
  usersPane: {
    flex: 1,
  },
  fridgeItemsPane: {
    flex: 7,
  },
  button: {
    padding: 20,
  },
  navButton: {
    marginTop: 10,
    alignSelf: "center",
  },
  listings: {
    paddingTop: 15,
    paddingBottom: 15,
  },
  title: {
    paddingBottom: 10,
    fontFamily: "OpenSans",
    fontWeight: "bold",
  },
  blurb: {
    fontFamily: "OpenSans",
    fontStyle: "italic",
  },
  fridgeName: {
    fontSize: 22,
    color: Colors.green900,
  },
  selectedUserButton: {
    borderRadius: 14,
    padding: 10,
    backgroundColor: Colors.green400,
  },
  userButton: {
    borderRadius: 14,
    padding: 10,
    backgroundColor: Colors.grey600,
  },
  centeredView: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: 22,
  },
  modalView: {
    margin: 20,
    backgroundColor: "white",
    borderRadius: 20,
    padding: 35,
    alignItems: "center",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  openButton: {
    backgroundColor: Colors.green400,
    borderRadius: 20,
    padding: 10,
    elevation: 2,
  },
  textStyle: {
    color: "white",
    fontWeight: "bold",
    textAlign: "center",
  },
  modalText: {
    marginBottom: 15,
    textAlign: "center",
  },
  modalTitle: {
    marginBottom: 20,
    marginTop: 5,
    fontSize: 24,
    textAlign: "center",
  },
});
