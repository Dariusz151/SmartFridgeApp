import React, { useEffect, useState } from "react";
import { StyleSheet, View, TouchableOpacity, FlatList } from "react-native";
import * as RootNavigation from "../RootNavigation";
import {
  ActivityIndicator,
  Button,
  Text,
  Colors,
  DataTable,
  IconButton,
} from "react-native-paper";
import FridgeUsers from "./FridgeUsers";

export default function FridgeDetail({ route, navigation }) {
  const { fridgeId, fridgeName } = route.params;

  const [dataLoading, finishLoading] = useState(true);
  const [fridgeItems, setData] = useState([]);

  useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      fetch("https://localhost:5001/api/fridgeitems/" + fridgeId)
        .then((response) => response.json())
        .then((json) => setData(json))
        .catch((error) => console.error(error))
        .finally(() => finishLoading(false));
    });

    return unsubscribe;
  }, [navigation]);

  return (
    <View style={styles.outerContainer}>
      <View style={styles.innerContainer}>
        <View style={styles.fridgeItemsPane}>
          <TouchableOpacity>
            <Text>Hello {fridgeName}</Text>
          </TouchableOpacity>
          {dataLoading ? (
            <ActivityIndicator animating={true} color={Colors.blue300} />
          ) : (
            <View>
              <DataTable>
                <DataTable.Header>
                  <DataTable.Title>FridgeItemId</DataTable.Title>
                  <DataTable.Title>Name</DataTable.Title>
                  <DataTable.Title>Category</DataTable.Title>
                </DataTable.Header>
                {fridgeItems.map((fridgeItem) => {
                  return (
                    <DataTable.Row key={fridgeItem.fridgeItemId}>
                      <DataTable.Cell>{fridgeItem.fridgeItemId}</DataTable.Cell>
                      <DataTable.Cell>{fridgeItem.productName}</DataTable.Cell>
                      <DataTable.Cell>{fridgeItem.categoryName}</DataTable.Cell>
                    </DataTable.Row>
                  );
                })}
              </DataTable>
            </View>
          )}
        </View>
        <FridgeUsers style={styles.usersPane} fridgeId={fridgeId} />
      </View>
      <Button
        onPress={() =>
          navigation.navigate("FridgeItemForm", {
            fridgeId: fridgeId,
            fridgeName: fridgeName,
          })
        }
      >
        Add
      </Button>
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
});
