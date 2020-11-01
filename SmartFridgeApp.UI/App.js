import "react-native-gesture-handler";
import React from "react";
import { NavigationContainer } from "@react-navigation/native";
import { createStackNavigator } from "@react-navigation/stack";
import Homepage from "./components/Home";

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="SmartFridgeApp">
        <Stack.Screen name="SmartFridgeApp" component={Homepage} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
