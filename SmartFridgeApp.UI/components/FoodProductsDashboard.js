import React, { useEffect, useState } from "react";
import { StyleSheet, View, TouchableHighlight, Text } from "react-native";
import { ActivityIndicator, Colors, DataTable } from "react-native-paper";

export default function FoodProductsDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [foodProductsData, setData] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    fetch("https://localhost:5001/api/foodProducts")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  return (
    <View>
      {dataLoading ? (
        <ActivityIndicator animating={true} color={Colors.blue300} />
      ) : (
        <View style={styles.foodProductsContainer}>
          <DataTable>
            <DataTable.Header>
              <DataTable.Title>Name</DataTable.Title>
              <DataTable.Title>Category</DataTable.Title>
              <DataTable.Title></DataTable.Title>
            </DataTable.Header>
            {foodProductsData.map((item) => {
              return (
                <DataTable.Row key={item.foodProductId}>
                  <DataTable.Cell>{item.foodProductName}</DataTable.Cell>
                  <DataTable.Cell>{item.foodProductCategory}</DataTable.Cell>
                  <DataTable.Cell style={styles.cellStyle}>
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
          </DataTable>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  foodProductsContainer: {
    paddingLeft: 30,
    paddingRight: 30,
    paddingTop: 10,
    paddingBottom: 10,
    backgroundColor: Colors.grey50,
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
  cellStyle: {
    width: 100,
  },
});
