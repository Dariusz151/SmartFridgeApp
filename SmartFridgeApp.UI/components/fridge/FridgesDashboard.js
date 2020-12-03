import React, { useEffect, useState } from "react";
import * as RootNavigation from "../../RootNavigation";
import configData from "../../config.dev.json";
import { StyleSheet, View, Button, TouchableHighlight } from "react-native";
import { ActivityIndicator, Colors, Text, DataTable } from "react-native-paper";

export default function FridgesDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [fridgesData, setData] = useState([]);

  useEffect(() => {
    fetch("https://" + configData.SERVER_URL + "/api/fridges")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));

    // const mockData = [
    //   {
    //     id: "B3BE3EF2-5A86-4705-D13D-A67135DDA55F",
    //     name: "Fridge1",
    //     desc: "Fridge1",
    //   },
    //   {
    //     id: "A3BE7EF8-9A86-4705-B87D-A16990DDA55F",
    //     name: "Fridge2",
    //     desc: "Fridge2",
    //   },
    //   {
    //     id: "A3BF4EF8-9A86-4705-B87D-B16990DDA55F",
    //     name: "Fridge3",
    //     desc: "Fridge3",
    //   },
    // ];
    // setData(mockData);
    // finishLoading(false);
  }, []);

  return (
    <View>
      <View style={styles.mainContainer}>
        {dataLoading ? (
          <ActivityIndicator animating={true} color={Colors.blue300} />
        ) : (
          <View>
            <DataTable>
              <DataTable.Header>
                <DataTable.Title>Id</DataTable.Title>
                <DataTable.Title>Name</DataTable.Title>
                <DataTable.Title>Desc</DataTable.Title>
                <DataTable.Title></DataTable.Title>
              </DataTable.Header>
              {fridgesData.map((item, index) => {
                return (
                  <DataTable.Row key={item.id}>
                    <DataTable.Cell>{index + 1}</DataTable.Cell>
                    <DataTable.Cell>
                      <Text>{item.name}</Text>
                    </DataTable.Cell>
                    <DataTable.Cell>{item.desc}</DataTable.Cell>
                    <DataTable.Cell>
                      <TouchableHighlight
                        style={styles.openButton}
                        onPress={() =>
                          navigation.navigate("FridgeDetail", {
                            fridgeId: item.id,
                            fridgeName: item.name,
                          })
                        }
                      >
                        <Text style={styles.textStyle}>Edit</Text>
                      </TouchableHighlight>
                    </DataTable.Cell>
                  </DataTable.Row>
                );
              })}
            </DataTable>
            <Button
              style={styles.newButton}
              onPress={() => {
                navigation.navigate("CreateFridgeForm");
              }}
              title="Add new"
              color={Colors.blueGrey600}
            >
              Add new
            </Button>
          </View>
        )}
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
  newButton: { width: "50%" },
});
