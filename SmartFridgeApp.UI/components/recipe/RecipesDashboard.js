import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableHighlight,
  Modal,
  Text,
  ScrollView,
  Button,
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
    recipeName: "recipe",
    recipeDescription: "desc",
    foodProducts: "items",
  });
  const [foodProductsFormatted, setFoodProductsFormatted] = useState([
    {
      foodProductId: "1",
      foodProductName: "fpName1",
      AmountValue: { Value: "1", Unit: "Grams" },
    },
    {
      foodProductId: "2",
      foodProductName: "fpName2",
      AmountValue: { Value: "2", Unit: "Grams" },
    },
  ]);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    fetch("https://localhost:5001/api/recipes")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));

    // const mockData = [
    //   {
    //     recipeId: "8aef7086-398e-470d-bcba-0b2f7c0bc777",
    //     recipeName: "Recipe1",
    //     description: "Recipe1",
    //     requiredTime: 40,
    //     levelOfDifficultyId: 2,
    //     levelOfDifficulty: "Hard",
    //     recipeCategory: "Kolacja",
    //     foodProducts:
    //       '{"?xml":{"@version":"1.0","@encoding":"utf-16"},"ArrayOfFoodProductDetails":{"FoodProductDetails":[{"FoodProductId":"1","FoodProductName":"fpName3","AmountValue":{"Value":"3","Unit":"Grams"}},{"FoodProductId":"15","FoodProductName":"fpName3","AmountValue":{"Value":"12","Unit":"Grams"}},{"FoodProductId":"16","FoodProductName":"fpName3","AmountValue":{"Value":"30","Unit":"Grams"}},{"FoodProductId":"7","FoodProductName":"fpName3","AmountValue":{"Value":"20","Unit":"Grams"}},{"FoodProductId":"4","FoodProductName":"fpName3","AmountValue":{"Value":"8","Unit":"Grams"}}]}}',
    //   },
    //   {
    //     recipeId: "897e6753-61db-4c14-a933-16c88c995fa0",
    //     recipeName: "Recipe2",
    //     description: "Recipe2",
    //     requiredTime: 50,
    //     levelOfDifficultyId: 2,
    //     levelOfDifficulty: "Hard",
    //     recipeCategory: "Kolacja",
    //     foodProducts:
    //       '{"?xml":{"@version":"1.0","@encoding":"utf-16"},"ArrayOfFoodProductDetails":{"FoodProductDetails":[{"FoodProductId":"4","FoodProductName":"fpName3","AmountValue":{"Value":"3","Unit":"Grams"}},{"FoodProductId":"5","FoodProductName":"fpName3","AmountValue":{"Value":"2","Unit":"Grams"}},{"FoodProductId":"6","FoodProductName":"fpName3","AmountValue":{"Value":"3","Unit":"Grams"}},{"FoodProductId":"13","FoodProductName":"fpName3","AmountValue":{"Value":"2","Unit":"Grams"}},{"FoodProductId":"14","FoodProductName":"fpName3","AmountValue":{"Value":"8","Unit":"Grams"}}]}}',
    //   },
    //   {
    //     recipeId: "5f3f3e34-49c7-4960-b238-c3ded7edd8b3",
    //     recipeName: "Recipe 3",
    //     description: "Recipe 3",
    //     requiredTime: 25,
    //     levelOfDifficultyId: 0,
    //     levelOfDifficulty: "Easy",
    //     recipeCategory: null,
    //     foodProducts:
    //       '{"?xml":{"@version":"1.0","@encoding":"utf-16"},"ArrayOfFoodProductDetails":{"FoodProductDetails":[{"FoodProductId":"2","FoodProductName":"fpName3","AmountValue":{"Value":"10","Unit":"NotAssigned"}},{"FoodProductId":"5","FoodProductName":"fpName1","AmountValue":{"Value":"60","Unit":"NotAssigned"}}]}}',
    //   },
    // ];
    // setData(mockData);
    // finishLoading(false);
  }, []);

  function formatFoodProducts(foodProds) {
    var foodProductsString = foodProds;
    var jsonObj = JSON.parse(foodProductsString);
    var foodProductsList = jsonObj.ArrayOfFoodProductDetails.FoodProductDetails;

    console.log(foodProductsList);

    setFoodProductsFormatted([]);
    for (const [key, value] of Object.entries(foodProductsList)) {
      setFoodProductsFormatted((oldArray) => [
        ...oldArray,
        {
          FoodProductId: value.FoodProductId,
          FoodProductName: value.FoodProductName,
          AmountValue: {
            Value: value.AmountValue.Value,
            Unit: value.AmountValue.Unit,
          },
        },
      ]);
    }
  }

  return (
    <View style={styles.mainContainer}>
      {dataLoading ? (
        <ActivityIndicator animating={true} color={Colors.blue300} />
      ) : (
        <View style={styles.dataTableStyle}>
          <DataTable>
            <DataTable.Header>
              <DataTable.Title>Id</DataTable.Title>
              <DataTable.Title>Name</DataTable.Title>
              {/* <DataTable.Title>Description</DataTable.Title> */}
              {/* <DataTable.Title>Required time</DataTable.Title> */}
              {/* <DataTable.Title>Level of difficulty</DataTable.Title> */}
              <DataTable.Title>Category</DataTable.Title>
              <DataTable.Title>Details</DataTable.Title>
            </DataTable.Header>
            <ScrollView>
              {recipesData.map((recipe, index) => {
                return (
                  <DataTable.Row key={recipe.recipeId}>
                    <DataTable.Cell>{index + 1}</DataTable.Cell>
                    <DataTable.Cell>{recipe.recipeName}</DataTable.Cell>
                    {/* <DataTable.Cell>{recipe.description}</DataTable.Cell> */}
                    {/* <DataTable.Cell>{recipe.requiredTime}</DataTable.Cell> */}
                    {/* <DataTable.Cell>{recipe.levelOfDifficulty}</DataTable.Cell> */}
                    <DataTable.Cell>{recipe.recipeCategory}</DataTable.Cell>
                    <DataTable.Cell>
                      <TouchableHighlight
                        style={styles.openButton}
                        onPress={() => {
                          setRecipeDetails((item) => ({
                            recipeName: recipe.recipeName,
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
            </ScrollView>
          </DataTable>
          <Button
            onPress={() => {
              navigation.navigate("RecipeForm");
            }}
            title="Add new"
            color={Colors.blueGrey600}
          ></Button>
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
            <Text style={styles.modalTitle}>{recipeDetails.recipeName}</Text>
            <Text style={styles.modalText}>
              {recipeDetails.recipeDescription}
            </Text>
            <Divider></Divider>
            {foodProductsFormatted.map((foodProduct) => {
              return (
                <Text key={foodProduct.FoodProductId}>
                  {foodProduct.FoodProductName} {foodProduct.AmountValue.Value}{" "}
                  {foodProduct.AmountValue.Unit}
                </Text>
              );
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
  mainContainer: {
    height: "80%",
    paddingLeft: 20,
    paddingRight: 20,
    paddingTop: 10,
    paddingBottom: 10,
  },
  dataTableStyle: {
    flex: 1,
  },
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
    backgroundColor: Colors.green400,
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
