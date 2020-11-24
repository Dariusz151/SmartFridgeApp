import React, { useEffect, useState } from "react";
import * as RootNavigation from "../RootNavigation";
import configData from ".././config.dev.json";
import { StyleSheet, View, FlatList } from "react-native";
import {
  Button,
  ActivityIndicator,
  Colors,
  Text,
  Title,
  Divider,
} from "react-native-paper";

export default function FridgesDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [fridgesData, setData] = useState([]);

  useEffect(() => {
    fetch("https://" + configData.SERVER_URL + "/api/fridges")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  const storyItem = ({ item }) => {
    return (
      <Button
        mode="outlined"
        compact={true}
        onPress={() =>
          navigation.navigate("FridgeDetail", {
            fridgeId: item.id,
            fridgeName: item.name,
          })
        }
      >
        <View style={styles.listings}>
          <Text style={styles.fridgeName}>{item.name}</Text>
          <Divider></Divider>
          <Text style={styles.blurb}>{item.desc}</Text>
        </View>
      </Button>
    );
  };
  return (
    <View style={styles.container}>
      {dataLoading ? (
        <ActivityIndicator animating={true} color={Colors.blue300} />
      ) : (
        <View>
          <Title style={styles.title}>Select fridge</Title>
          <FlatList
            data={fridgesData}
            renderItem={storyItem}
            keyExtractor={(item) => item.id}
          />
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#eee",
    alignItems: "center",
    justifyContent: "center",
    width: "100%",
    padding: 20,
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
