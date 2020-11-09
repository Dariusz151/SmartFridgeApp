import React, { useEffect, useState } from "react";
import { StyleSheet, View, TouchableOpacity, FlatList } from "react-native";
import * as RootNavigation from "../RootNavigation";
import { ActivityIndicator, Button, Text, Colors } from "react-native-paper";
import FridgeUsers from "./FridgeUsers";

export default function FridgeDetail({ route, navigation }) {
  const { fridgeId, fridgeName } = route.params;

  const [dataLoading, finishLoading] = useState(true);
  const [fridgeItems, setData] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/api/fridgeitems/" + fridgeId)
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

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
              <FlatList
                data={fridgeItems}
                renderItem={({ item }) => (
                  <View>
                    <Text>{item.productName}</Text>
                  </View>
                )}
                keyExtractor={(item) => item.fridgeItemId}
              />
            </View>
          )}
        </View>
        <FridgeUsers style={styles.usersPane} fridgeId={fridgeId} />
      </View>
      <Button onPress={() => navigation.goBack()}>Back</Button>
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
});
