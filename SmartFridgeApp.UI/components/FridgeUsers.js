import React, { useEffect, useState } from "react";
import * as RootNavigation from "../RootNavigation";
import { StyleSheet, View, FlatList } from "react-native";
import { ActivityIndicator, Colors, Text, Switch } from "react-native-paper";

export default function FridgeUsers(props) {
  const { fridgeId } = props;

  const [isEnabled, setIsEnabled] = useState(false);
  const toggleSwitch = () => setIsEnabled((previousState) => !previousState);

  const [dataLoading, finishLoading] = useState(true);
  const [fridgeUsers, setData] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/api/fridgeUsers/" + fridgeId)
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  return (
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
                  ios_backgroundColor="#3e3e3e"
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
