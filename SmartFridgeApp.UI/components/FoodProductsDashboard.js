import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableHighlight,
  Text,
  ScrollView,
} from "react-native";
import { ActivityIndicator, Colors, DataTable } from "react-native-paper";

export default function FoodProductsDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [foodProductsData, setData] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    // fetch("https://localhost:5001/api/foodProducts")
    //   .then((response) => response.json())
    //   .then((json) => console.log(json))
    //   .catch((error) => console.error(error))
    //   .finally(() => finishLoading(false));

    const mockData = [
      {
        foodProductId: 1,
        foodProductName: "Kurczak",
        foodProductCategory: "Mięso",
      },
      {
        foodProductId: 2,
        foodProductName: "Mięso mielone",
        foodProductCategory: "Mięso",
      },
      {
        foodProductId: 3,
        foodProductName: "Mięso wołowe",
        foodProductCategory: "Mięso",
      },
      {
        foodProductId: 4,
        foodProductName: "Cebula",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 5,
        foodProductName: "Ziemniaki",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 6,
        foodProductName: "Marchew",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 7,
        foodProductName: "Ogórek",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 8,
        foodProductName: "Pomidor",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 9,
        foodProductName: "Sałata",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 10,
        foodProductName: "Kapusta",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 11,
        foodProductName: "Banan",
        foodProductCategory: "Owoce",
      },
      {
        foodProductId: 12,
        foodProductName: "Jabłko",
        foodProductCategory: "Owoce",
      },
      {
        foodProductId: 13,
        foodProductName: "Cytryna",
        foodProductCategory: "Owoce",
      },
      {
        foodProductId: 14,
        foodProductName: "Gruszka",
        foodProductCategory: "Owoce",
      },
      {
        foodProductId: 15,
        foodProductName: "Mleko",
        foodProductCategory: "Nabiał",
      },
      {
        foodProductId: 16,
        foodProductName: "Ser",
        foodProductCategory: "Nabiał",
      },
      {
        foodProductId: 17,
        foodProductName: "Twarog",
        foodProductCategory: "Nabiał",
      },
      {
        foodProductId: 18,
        foodProductName: "Makaron spaghetti",
        foodProductCategory: "Makarony",
      },
      {
        foodProductId: 19,
        foodProductName: "Ryż basmati",
        foodProductCategory: "Ryże",
      },
      {
        foodProductId: 20,
        foodProductName: "Ryż biały",
        foodProductCategory: "Ryże",
      },
      {
        foodProductId: 21,
        foodProductName: "Marchew",
        foodProductCategory: "Warzywa",
      },
      {
        foodProductId: 22,
        foodProductName: "Roszponka",
        foodProductCategory: "Warzywa",
      },
    ];
    setData(mockData);
    finishLoading(false);
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
                          console.log("siema");
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
});
