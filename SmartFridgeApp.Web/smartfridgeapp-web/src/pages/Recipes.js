import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import configData from "../config_url.json";
import { MDBDataTable } from "mdbreact";
import Button from "@material-ui/core/Button";
import VisibilityIcon from "@material-ui/icons/Visibility";
import RefreshIcon from "@material-ui/icons/Refresh";
import AddIcon from "@material-ui/icons/Add";
import CircularProgress from "@material-ui/core/CircularProgress";

import { Container } from "@material-ui/core";

import { AuthContext } from "../App";
import RecipeDetails from "../components/dialogs/RecipeDetails";

const Recipes = () => {
  const { state, dispatch } = React.useContext(AuthContext);
  let history = useHistory();
  const [dataLoading, finishLoading] = useState(true);
  const [dummyState, rerender] = useState(1);
  const [recipeDetailsDialogOpened, openDetailsDialog] = useState(false);

  const [rows, setRows] = useState([
    {
      id: 1,
      name: "name",
      category: "cat",
      reqTime: "req",
      difficulty: "diff",
      details: (
        <Button gradient="aqua" size="sm">
          Details
        </Button>
      ),
    },
  ]);

  const [recipeDetails, setRecipeDetails] = useState({
    recipeName: "recipe",
    recipeDescription: "desc",
    foodProducts: JSON.stringify({
      ArrayOfFoodProductDetails: {
        FoodProductDetails: [
          {
            FoodProductId: 1,
            FoodProductName: "fp1",
            AmountValue: {
              Value: 10,
              Unit: "Grams",
            },
          },
        ],
      },
    }),
  });

  // useEffect(() => {
  //   console.log(recipeDetails.foodProducts);
  // }, [recipeDetails]);

  const closeDetailsDialog = () => {
    openDetailsDialog(false);
  };

  useEffect(() => {
    fetch(configData.SERVER_URL + "/api/recipes")
      .then((response) => response.json())
      .then((json) => {
        const rowsArray = json.map((item, index) => ({
          id: index + 1,
          name: item.recipeName,
          category: item.recipeCategory,
          reqTime: item.requiredTime,
          difficulty: item.levelOfDifficulty,
          details: (
            <Button
              variant="contained"
              color="primary"
              startIcon={<VisibilityIcon />}
              onClick={() => {
                setRecipeDetails({
                  recipeName: item.recipeName,
                  recipeDescription: item.description,
                  foodProducts: item.foodProducts,
                });
                openDetailsDialog(true);
              }}
            >
              Details
            </Button>
          ),
        }));
        setRows(rowsArray);
      })
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));
  }, [dummyState]);

  return (
    <div>
      <RecipeDetails
        isDialogOpened={recipeDetailsDialogOpened}
        handleClose={closeDetailsDialog}
        recipeDetails={recipeDetails}
      />
      <Container fixed>
        <div className="btn-group">
          <Button
            disabled={!state.isAuthenticated}
            style={styles.topButton}
            variant="outlined"
            color="primary"
            onClick={() => history.push("/recipes/add")}
            startIcon={<AddIcon />}
          >
            Add new
          </Button>
          <Button
            style={styles.topButton}
            variant="outlined"
            color="primary"
            onClick={() => rerender(dummyState + 1)}
            startIcon={<RefreshIcon />}
          >
            Refresh
          </Button>
        </div>

        {dataLoading ? (
          <div>
            <CircularProgress />
            <p>Loading recipes</p>
          </div>
        ) : (
          <MDBDataTable
            paging={true}
            hover
            entriesOptions={[5, 10, 20, 30]}
            entries={10}
            pagesAmount={3}
            data={{ columns: columns, rows: rows }}
          ></MDBDataTable>
        )}
      </Container>
    </div>
  );
};

const columns = [
  {
    label: "#",
    field: "id",
    sort: "asc",
  },
  {
    label: "Name",
    field: "name",
    sort: "asc",
  },
  {
    label: "Category",
    field: "category",
    sort: "asc",
  },
  {
    label: "Required time",
    field: "reqTime",
    sort: "asc",
  },
  {
    label: "Difficulty",
    field: "difficulty",
    sort: "asc",
  },
  {
    label: "#",
    field: "details",
    sort: "asc",
  },
];

const styles = {
  topButton: {
    paddingTop: "10px",
    paddingBottom: "10px",
    marginTop: "30px",
    // marginRight: "8px",
    // marginLeft: "20px",
  },
};

export default Recipes;
