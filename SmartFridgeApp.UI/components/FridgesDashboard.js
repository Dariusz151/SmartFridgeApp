import React, { useEffect, useState } from "react";
import { StyleSheet, View, FlatList, TouchableOpacity } from "react-native";
import {
  Button,
  ActivityIndicator,
  Colors,
  Text,
  Title,
} from "react-native-paper";

export default function FridgesDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [fridgesData, setData] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/api/fridges")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  const storyItem = ({ item }) => {
    return (
      <Button
        mode="outlined"
        icon="fridge-outline"
        compact={true}
        onPress={() => console.log("Pressed")}
        // onPress={() => navigation.navigate("NewsDetail", { url: item.url })}
      >
        <View style={styles.listings}>
          <Text style={styles.fridgeName}>{item.name}</Text>
          {/* <Text style={styles.blurb}>{item.desc}</Text> */}
          {/* <Text style={styles.blurb}>{item.address}</Text> */}
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
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
    width: "100%",
    padding: 20,
  },
  thumbnail: {
    height: 100,
    width: "98%",
  },
  listings: {
    paddingTop: 15,
    paddingBottom: 25,
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
