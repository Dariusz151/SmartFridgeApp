import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { useHistory } from "react-router-dom";
import Button from "@material-ui/core/Button";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";
import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";
import { Container, Box, Grid, TextField } from "@material-ui/core";

import FoodProductInput from "./FoodProductInput";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const AddNewRecipe = () => {
  let history = useHistory();

  const [recipeName, setRecipeName] = useState("");
  const [recipeDesc, setRecipeDesc] = useState("");
  const [recipeCategory, setRecipeCategory] = useState("");
  const [levelOfDifficulty, setDifficulty] = useState("");
  const [requiredTime, setRequiredTime] = useState("");

  const [foodProducts, setFoodProducts] = useState([
    { foodProductId: 1, foodProductName: "fp" },
  ]);
  const [inputList, setInputList] = useState([
    {
      foodProductId: 0,
      foodProductName: "",
      amount: "",
      unit: "",
      optional: false,
    },
  ]);

  const handleClose = () => {
    history.push("/recipes");
  };

  useEffect(() => {
    fetch(configData.SERVER_URL + "/api/foodProducts")
      .then((response) => response.json())
      .then((json) => {
        const items = json.map((item, index) => ({
          foodProductId: item.foodProductId,
          foodProductName: item.foodProductName,
        }));
        setFoodProducts(items);
      })
      .catch((error) => console.error(error));
  }, []);

  const refreshForm = () => {
    setRecipeName("");
    setRecipeDesc("");
    setRecipeCategory("");
    setDifficulty("");
    setRequiredTime("");
    setInputList([
      {
        foodProductId: 0,
        foodProductName: "",
        amount: "",
        unit: "",
        optional: false,
      },
    ]);
  };

  const handleAdd = () => {
    const products = [];
    inputList.map((item) => {
      const newProduct = {
        foodProductId: item.foodProductId,
        amountValue: { value: parseInt(item.amount), unit: item.unit },
        optional: item.optional,
      };
      products.push(newProduct);
      return true;
    });

    const obj = {
      name: recipeName,
      description: recipeDesc,
      recipeCategory: parseInt(recipeCategory),
      requiredTime: parseInt(requiredTime),
      levelOfDifficulty: parseInt(levelOfDifficulty),
      products: products,
    };

    console.log(obj);

    fetch(configData.SERVER_URL + "/api/recipes/", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then((response) => {
        if (!response.ok) {
          toast.error("Cant add recipe!", {
            position: "bottom-center",
            autoClose: 1500,
          });
          throw Error(response.statusText);
        }
        toast.success("Added new recipe!", {
          position: "bottom-center",
          autoClose: 1500,
        });

        return response;
      })
      .then(refreshForm())
      .then(() => handleClose())
      .catch((error) => console.log(error));
  };

  return (
    <React.Fragment>
      <ToastContainer />
      <Container fixed style={styles.container}>
        <Typography variant="h3" component="h3" style={styles.header}>
          Add Recipe
        </Typography>
        <FoodProductInput
          foodProducts={foodProducts}
          inputList={inputList}
          setInputList={setInputList}
        />
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <TextField
              name="recipeName"
              label="Recipe name"
              fullWidth
              onChange={(e) => {
                setRecipeName(e.target.value);
              }}
              value={recipeName}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              name="requiredTime"
              label="Required time"
              fullWidth
              onChange={(e) => {
                setRequiredTime(e.target.value);
              }}
              value={requiredTime}
            />
          </Grid>
          <Grid item xd={1} style={styles.diffToggleBtnGrp}>
            <Typography>Difficulty</Typography>
          </Grid>
          <Grid
            style={styles.diffToggleBtnGrp}
            item
            direction="row"
            justify="flex-start"
            alignItems="flex-start"
            xs={2}
          >
            <ToggleButtonGroup
              value={levelOfDifficulty}
              exclusive
              onChange={(event, diff) => {
                setDifficulty(diff);
              }}
              aria-label="text alignment"
            >
              <ToggleButton value="1" aria-label="left aligned">
                <Typography style={styles.toggleBtn}>Easy</Typography>
              </ToggleButton>
              <ToggleButton value="2" aria-label="centered">
                <Typography style={styles.toggleBtn}>Medium</Typography>
              </ToggleButton>
              <ToggleButton value="3" aria-label="right aligned">
                <Typography style={styles.toggleBtn}>Hard</Typography>
              </ToggleButton>
            </ToggleButtonGroup>
          </Grid>
          <Grid item xs={9} style={styles.diffToggleBtnGrp}></Grid>
          <Grid item xd={1} style={styles.categoryToggleBtnGrp}>
            <Typography>Category</Typography>
          </Grid>
          <Grid
            style={styles.categoryToggleBtnGrp}
            direction="row"
            justify="flex-start"
            alignItems="flex-start"
            item
            xs={2}
          >
            <ToggleButtonGroup
              value={recipeCategory}
              exclusive
              onChange={(event, cat) => {
                setRecipeCategory(cat);
              }}
              aria-label="text alignment"
            >
              <ToggleButton value="1" aria-label="centered">
                <Typography style={styles.toggleBtn}>Breakfast</Typography>
              </ToggleButton>
              <ToggleButton value="2" aria-label="left aligned">
                <Typography style={styles.toggleBtn}>Dinner</Typography>
              </ToggleButton>
              <ToggleButton value="3" aria-label="right aligned">
                <Typography style={styles.toggleBtn}>Supper</Typography>
              </ToggleButton>
            </ToggleButtonGroup>
          </Grid>
          <Grid item xs={9} style={styles.categoryToggleBtnGrp}></Grid>
          <Grid item xs={12}>
            <TextField
              name="recipeDesc"
              label="Description"
              multiline
              rows={8}
              variant="outlined"
              fullWidth
              value={recipeDesc}
              onChange={(e) => {
                setRecipeDesc(e.target.value);
              }}
            />
          </Grid>
        </Grid>
        <Box style={styles.bottomButtonsGrp}>
          <Button
            size="large"
            style={styles.bottomBtn}
            onClick={handleClose}
            color="secondary"
            variant="outlined"
            startIcon={<CloseIcon />}
          >
            Cancel
          </Button>
          <Button
            size="large"
            style={styles.bottomBtn}
            onClick={handleAdd}
            color="primary"
            startIcon={<AddIcon />}
            variant="outlined"
          >
            Create
          </Button>
        </Box>
      </Container>
    </React.Fragment>
  );
};

const styles = {
  header: {
    paddingTop: "40px",
    paddingBottom: "40px",
  },
  bottomButtonsGrp: {
    paddingTop: "50px",
    paddingBottom: "50px",
  },
  bottomBtn: {
    marginLeft: "30px",
    marginRight: "30px",
    minWidth: "200px",
    minHeight: "70px",
  },
  toggleBtn: {
    color: "#494949",
    textAlign: "center",
  },
  diffToggleBtnGrp: {
    marginTop: "20px",
    marginbottom: "20px",
  },
  categoryToggleBtnGrp: {
    marginTop: "20px",
    marginBottom: "30px",
  },
  container: {
    backgroundColor: "#FFFFFF",
  },
};

export default AddNewRecipe;
