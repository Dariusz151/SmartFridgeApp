import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableOpacity,
  TouchableHighlight,
  Modal,
  Text,
} from "react-native";
import {
  ActivityIndicator,
  Colors,
  DataTable,
  Divider,
} from "react-native-paper";

export default function RecipesDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [recipesData, setData] = useState([]);
  const [recipeDetails, setRecipeDetails] = useState({
    recipeDescription: "desc",
    foodProducts: "items",
  });
  const [foodProductsFormatted, setFoodProductsFormatted] = useState([
    { foodProductId: "1" },
    { foodProductId: "2" },
  ]);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    fetch("https://localhost:5001/api/recipes")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

  function formatFoodProducts(foodProds) {
    var foodProductsString = foodProds;
    var jsonObj = JSON.parse(foodProductsString);
    var foodProductsList = jsonObj.ArrayOfFoodProductDetails.FoodProductDetails;

    setFoodProductsFormatted([]);
    for (const [key, value] of Object.entries(foodProductsList)) {
      setFoodProductsFormatted((oldArray) => [
        ...oldArray,
        { foodProductId: value.FoodProductId },
      ]);
    }
  }

  return (
    <View>
      <TouchableOpacity></TouchableOpacity>
      {dataLoading ? (
        <ActivityIndicator animating={true} color={Colors.blue300} />
      ) : (
        <View>
          <DataTable>
            <DataTable.Header>
              <DataTable.Title>Name</DataTable.Title>
              <DataTable.Title>Description</DataTable.Title>
              <DataTable.Title>Required time</DataTable.Title>
              <DataTable.Title>Level of difficulty</DataTable.Title>
              <DataTable.Title>Category</DataTable.Title>
              <DataTable.Title>Details</DataTable.Title>
            </DataTable.Header>
            {recipesData.map((recipe) => {
              return (
                <DataTable.Row key={recipe.recipeId}>
                  <DataTable.Cell>{recipe.recipeName}</DataTable.Cell>
                  <DataTable.Cell>{recipe.description}</DataTable.Cell>
                  <DataTable.Cell>{recipe.requiredTime}</DataTable.Cell>
                  <DataTable.Cell>{recipe.levelOfDifficulty}</DataTable.Cell>
                  <DataTable.Cell>{recipe.recipeCategory}</DataTable.Cell>
                  <DataTable.Cell>
                    <TouchableHighlight
                      style={styles.openButton}
                      onPress={() => {
                        setRecipeDetails((item) => ({
                          recipeDescription: recipe.description,
                          foodProducts: recipe.foodProducts,
                        }));
                        setModalVisible(true);
                        formatFoodProducts(recipe.foodProducts);
                      }}
                    >
                      <Text style={styles.textStyle}>Details</Text>
                    </TouchableHighlight>
                  </DataTable.Cell>
                </DataTable.Row>
              );
            })}
          </DataTable>
        </View>
      )}
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          Alert.alert("Modal has been closed.");
        }}
      >
        <View style={styles.centeredView}>
          <View style={styles.modalView}>
            <Text style={styles.modalTitle}>
              {recipeDetails.recipeDescription}
            </Text>
            <Divider></Divider>
            {foodProductsFormatted.map((foodProduct) => {
              return <Text>{foodProduct.foodProductId}</Text>;
            })}

            <TouchableHighlight
              style={{ ...styles.openButton, backgroundColor: "#2196F3" }}
              onPress={() => {
                setModalVisible(!modalVisible);
              }}
            >
              <Text style={styles.textStyle}>Hide</Text>
            </TouchableHighlight>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  centeredView: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: 22,
  },
  modalView: {
    margin: 20,
    backgroundColor: "white",
    borderRadius: 20,
    padding: 35,
    alignItems: "center",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  openButton: {
    backgroundColor: "blue",
    borderRadius: 20,
    padding: 10,
    elevation: 2,
  },
  textStyle: {
    color: "white",
    fontWeight: "bold",
    textAlign: "center",
  },
  modalText: {
    marginBottom: 15,
    textAlign: "center",
  },
  modalTitle: {
    marginBottom: 20,
    marginTop: 5,
    fontSize: 24,
    textAlign: "center",
  },
});
