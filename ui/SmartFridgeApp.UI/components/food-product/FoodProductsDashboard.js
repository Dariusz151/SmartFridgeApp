import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableHighlight,
  Text,
  ScrollView,
  Modal,
  Button,
} from "react-native";
import { ActivityIndicator, Colors, DataTable } from "react-native-paper";

export default function FoodProductsDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [foodProductsData, setData] = useState([]);
  const [selectedFoodProduct, selectFoodProduct] = useState({
    foodProductId: 1,
    foodProductName: "FoodProduct",
  });
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    fetch("https://localhost:5001/api/foodProducts")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));

    // const mockData = [
    //   {
    //     foodProductId: 1,
    //     foodProductName: "Kurczak",
    //     foodProductCategory: "Mięso",
    //   },
    //   {
    //     foodProductId: 2,
    //     foodProductName: "Mięso mielone",
    //     foodProductCategory: "Mięso",
    //   },
    //   {
    //     foodProductId: 3,
    //     foodProductName: "Mięso wołowe",
    //     foodProductCategory: "Mięso",
    //   },
    //   {
    //     foodProductId: 4,
    //     foodProductName: "Cebula",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 5,
    //     foodProductName: "Ziemniaki",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 6,
    //     foodProductName: "Marchew",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 7,
    //     foodProductName: "Ogórek",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 8,
    //     foodProductName: "Pomidor",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 9,
    //     foodProductName: "Sałata",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 10,
    //     foodProductName: "Kapusta",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 11,
    //     foodProductName: "Banan",
    //     foodProductCategory: "Owoce",
    //   },
    //   {
    //     foodProductId: 12,
    //     foodProductName: "Jabłko",
    //     foodProductCategory: "Owoce",
    //   },
    //   {
    //     foodProductId: 13,
    //     foodProductName: "Cytryna",
    //     foodProductCategory: "Owoce",
    //   },
    //   {
    //     foodProductId: 14,
    //     foodProductName: "Gruszka",
    //     foodProductCategory: "Owoce",
    //   },
    //   {
    //     foodProductId: 15,
    //     foodProductName: "Mleko",
    //     foodProductCategory: "Nabiał",
    //   },
    //   {
    //     foodProductId: 16,
    //     foodProductName: "Ser",
    //     foodProductCategory: "Nabiał",
    //   },
    //   {
    //     foodProductId: 17,
    //     foodProductName: "Twarog",
    //     foodProductCategory: "Nabiał",
    //   },
    //   {
    //     foodProductId: 18,
    //     foodProductName: "Makaron spaghetti",
    //     foodProductCategory: "Makarony",
    //   },
    //   {
    //     foodProductId: 19,
    //     foodProductName: "Ryż basmati",
    //     foodProductCategory: "Ryże",
    //   },
    //   {
    //     foodProductId: 20,
    //     foodProductName: "Ryż biały",
    //     foodProductCategory: "Ryże",
    //   },
    //   {
    //     foodProductId: 21,
    //     foodProductName: "Marchew",
    //     foodProductCategory: "Warzywa",
    //   },
    //   {
    //     foodProductId: 22,
    //     foodProductName: "Roszponka",
    //     foodProductCategory: "Warzywa",
    //   },
    // ];
    // setData(mockData);
    // finishLoading(false);
  }, []);

  return (
    <View style={styles.mainContainer}>
      {dataLoading ? (
        <ActivityIndicator animating={true} color={Colors.blue300} />
      ) : (
        <View style={styles.foodProductsContainer}>
          <DataTable style={styles.tableStyle}>
            <DataTable.Header>
              <DataTable.Title>Id</DataTable.Title>
              <DataTable.Title>Name</DataTable.Title>
              <DataTable.Title>Category</DataTable.Title>
              <DataTable.Title></DataTable.Title>
            </DataTable.Header>
            <ScrollView>
              {foodProductsData.map((item, index) => {
                return (
                  <DataTable.Row key={item.foodProductId}>
                    <DataTable.Cell>{index + 1}</DataTable.Cell>
                    <DataTable.Cell>
                      <Text>{item.foodProductName}</Text>
                    </DataTable.Cell>
                    <DataTable.Cell>{item.foodProductCategory}</DataTable.Cell>
                    <DataTable.Cell>
                      <TouchableHighlight
                        style={styles.openButton}
                        onPress={() => {
                          selectFoodProduct({
                            foodProductId: item.foodProductId,
                            foodProductName: item.foodProductName,
                          });
                          setModalVisible(true);
                        }}
                      >
                        <Text style={styles.textStyle}>Edit</Text>
                      </TouchableHighlight>
                    </DataTable.Cell>
                  </DataTable.Row>
                );
              })}
            </ScrollView>
          </DataTable>
        </View>
      )}
      <View style={styles.centeredView}>
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
              <Text style={styles.modalText}>
                Edit {selectedFoodProduct.foodProductName} with index{" "}
                {selectedFoodProduct.foodProductId}
              </Text>
              <Text style={styles.modalText}>TODO FORM</Text>
              <TouchableHighlight
                style={{ ...styles.openButton, backgroundColor: "#2196F3" }}
                onPress={() => {
                  setModalVisible(!modalVisible);
                }}
              >
                <Text style={styles.textStyle}>Close</Text>
              </TouchableHighlight>
            </View>
          </View>
        </Modal>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  mainContainer: {
    height: "80%",
    paddingLeft: 20,
    paddingRight: 20,
    paddingTop: 10,
    paddingBottom: 10,
  },
  openButton: {
    backgroundColor: Colors.green400,
    borderRadius: 14,
    padding: 10,
    elevation: 2,
  },
  textStyle: {
    color: Colors.grey50,
    fontWeight: "bold",
    textAlign: "center",
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
  modalText: {
    marginBottom: 15,
    textAlign: "center",
  },
});
