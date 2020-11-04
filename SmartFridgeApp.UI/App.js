import "react-native-gesture-handler";
import React from "react";
import { NavigationContainer } from "@react-navigation/native";
import { createStackNavigator } from "@react-navigation/stack";
import Homepage from "./components/Homepage";
import { Platform } from "react-native";
import { useFonts } from "expo-font";
import { AppLoading } from "expo";
import Header from "./components/Header";
import { StatusBar } from "expo-status-bar";
import Footer from "./components/Footer";
import { navigationRef } from "./RootNavigation";
import FridgesDashboard from "./components/FridgesDashboard";

const Stack = createStackNavigator();

export default function App() {
  let [fontsLoaded] = useFonts({
    OpenSans: require("./assets/OpenSans-Regular.ttf"),
  });

  if (!fontsLoaded) {
    return <AppLoading />;
  } else {
    return (
      <NavigationContainer
        style={{
          paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0,
        }}
        ref={navigationRef}
      >
        <Stack.Navigator initialRouteName="Homepage" headerMode="screen">
          <Stack.Screen
            name="Homepage"
            component={Homepage}
            options={{
              header: () => <Header headerDisplay="SmartfridgeHeader" />,
            }}
          />
          <Stack.Screen
            name="FridgesPage"
            component={FridgesDashboard}
            options={{
              header: () => <Header headerDisplay="SmartfridgeHeader" />,
            }}
          />
        </Stack.Navigator>
        <Footer />
      </NavigationContainer>
    );
  }
}
