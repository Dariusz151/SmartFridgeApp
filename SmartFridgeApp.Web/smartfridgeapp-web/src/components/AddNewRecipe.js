import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { useHistory } from "react-router-dom";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";
import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";
import FoodProductInput from "./FoodProductInput";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const AddNewRecipe = () => {
  let history = useHistory();

  const [dummyState, rerender] = useState(1);
  const [recipeName, setRecipeName] = useState("");
  const [recipeDesc, setRecipeDesc] = useState("");
  const [recipeCategory, setRecipeCategory] = useState("");
  const [levelOfDifficulty, setDifficulty] = useState("");
  const [requiredTime, setRequiredTime] = useState("");

  const [foodProducts, setFoodProducts] = useState([
    { foodProductId: 1, foodProductName: "fp" },
  ]);
  const [inputList, setInputList] = useState([
    { foodProductId: 0, foodProductName: "", amount: "" },
  ]);

  const handleRefresh = () => {
    rerender(dummyState + 1);
  };
  const handleClose = () => {
    history.push("/recipes");
  };

  useEffect(() => {
    console.log("food products fetch");
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
    setInputList([{ foodProductId: 0, foodProductName: "", amount: "" }]);
  };

  const handleAdd = () => {
    const products = [];
    inputList.map((item) => {
      const newProduct = {
        foodProductId: item.foodProductId,
        amountValue: { value: parseInt(item.amount), unit: "NotAssigned" },
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
      <p>Add Recipe</p>
      <div>
        <FoodProductInput
          foodProducts={foodProducts}
          inputList={inputList}
          setInputList={setInputList}
        />
        <TextField
          name="recipeName"
          label="Recipe name"
          fullWidth
          onChange={(e) => {
            setRecipeName(e.target.value);
          }}
          value={recipeName}
        />
        <TextField
          name="recipeDesc"
          label="Description"
          fullWidth
          onChange={(e) => {
            setRecipeDesc(e.target.value);
          }}
          value={recipeDesc}
        />
        <br />
        <br />
        <ToggleButtonGroup
          value={levelOfDifficulty}
          exclusive
          onChange={(event, diff) => {
            setDifficulty(diff);
          }}
          aria-label="text alignment"
        >
          <ToggleButton value="1" aria-label="left aligned">
            <Typography>Easy</Typography>
          </ToggleButton>
          <ToggleButton value="2" aria-label="centered">
            <Typography>Medium</Typography>
          </ToggleButton>
          <ToggleButton value="3" aria-label="right aligned">
            <Typography>Hard</Typography>
          </ToggleButton>
        </ToggleButtonGroup>
        <br /> <br />
        <ToggleButtonGroup
          value={recipeCategory}
          exclusive
          onChange={(event, cat) => {
            setRecipeCategory(cat);
          }}
          aria-label="text alignment"
        >
          <ToggleButton value="1" aria-label="centered">
            <Typography>Breakfast</Typography>
          </ToggleButton>
          <ToggleButton value="2" aria-label="left aligned">
            <Typography>Dinner</Typography>
          </ToggleButton>

          <ToggleButton value="3" aria-label="right aligned">
            <Typography>Supper</Typography>
          </ToggleButton>
          <ToggleButton value="4" aria-label="right aligned">
            <Typography>Other</Typography>
          </ToggleButton>
        </ToggleButtonGroup>
        <TextField
          name="requiredTime"
          label="Required time"
          fullWidth
          onChange={(e) => {
            setRequiredTime(e.target.value);
          }}
          value={requiredTime}
        />
      </div>
      <div>
        <Button
          onClick={handleClose}
          color="secondary"
          variant="outlined"
          startIcon={<CloseIcon />}
        >
          Cancel
        </Button>
        <Button
          onClick={handleAdd}
          color="primary"
          startIcon={<AddIcon />}
          variant="outlined"
        >
          Add
        </Button>
      </div>
    </React.Fragment>
  );
};

export default AddNewRecipe;
