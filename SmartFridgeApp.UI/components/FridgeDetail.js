import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableOpacity,
  FlatList,
  Switch,
} from "react-native";
import * as RootNavigation from "../RootNavigation";
import {
  ActivityIndicator,
  Button,
  Text,
  Colors,
  DataTable,
  IconButton,
} from "react-native-paper";

export default function FridgeDetail({ route, navigation }) {
  const { fridgeId, fridgeName } = route.params;
  const [dataLoading, finishLoading] = useState(true);

  const [isEnabled, setIsEnabled] = useState(false);
  const toggleSwitch = () => setIsEnabled((previousState) => !previousState);

  const [fridgeItems, setFridgeData] = useState([]);
  const [fridgeUsers, setUsersData] = useState([]);

  useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      fetch("https://localhost:5001/api/fridgeitems/" + fridgeId)
        .then((response) => response.json())
        .then((json) => setFridgeData(json))
        .catch((error) => console.error(error))
        .finally(() => finishLoading(false));

      fetch("https://localhost:5001/api/fridgeUsers/" + fridgeId)
        .then((response) => response.json())
        .then((json) => setUsersData(json))
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
        <View style={styles.container}>
          {dataLoading ? (
            <ActivityIndicator animating={true} color={Colors.blue300} />
          ) : (
            <View>
              <FlatList
                data={fridgeUsers}
                renderItem={({ item }) => (
                  <View>
                    <Switch
                      trackColor={{ false: "#767577", true: "#81b0ff" }}
                      thumbColor={isEnabled ? "#f5dd4b" : "#f4f3f4"}
                      onValueChange={toggleSwitch}
                      value={isEnabled}
                    />
                    <Text>{item.name}</Text>
                  </View>
                )}
                keyExtractor={(item) => item.id}
              />
            </View>
          )}
        </View>
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
});
