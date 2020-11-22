import React, { useEffect, useState } from "react";
import { StyleSheet, View, TouchableOpacity } from "react-native";
import { ActivityIndicator, Colors, DataTable } from "react-native-paper";

export default function RecipesDashboard({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [recipesData, setData] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/api/recipes")
      .then((response) => response.json())
      .then((json) => setData(json))
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, []);

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
            </DataTable.Header>
            {recipesData.map((recipe) => {
              return (
                <DataTable.Row key={recipe.recipeId}>
                  <DataTable.Cell>{recipe.recipeName}</DataTable.Cell>
                  <DataTable.Cell>{recipe.description}</DataTable.Cell>
                  <DataTable.Cell>{recipe.requiredTime}</DataTable.Cell>
                  <DataTable.Cell>{recipe.levelOfDifficulty}</DataTable.Cell>
                  <DataTable.Cell>{recipe.recipeCategory}</DataTable.Cell>
                </DataTable.Row>
              );
            })}
          </DataTable>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({});
