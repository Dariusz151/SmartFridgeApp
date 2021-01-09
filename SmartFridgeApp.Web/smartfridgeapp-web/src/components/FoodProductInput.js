import React, { useState } from "react";

import { Grid, TextField, Box, Button } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";

import RemoveIcon from "@material-ui/icons/Remove";
import AddIcon from "@material-ui/icons/Add";

import UnitSelector from "./UnitSelector";

const FoodProductInput = ({ foodProducts, inputList, setInputList }) => {
  const [unit, setUnit] = useState("NotAssigned");
  const handleInputChange = (e, index) => {
    const { name, value } = e.target;
    const list = [...inputList];
    list[index][name] = value;
    setInputList(list);
  };

  const handleRemoveClick = (index) => {
    const list = [...inputList];
    list.splice(index, 1);
    setInputList(list);
  };

  const handleAddClick = () => {
    setInputList([
      ...inputList,
      { foodProductId: 0, foodProductName: "", amount: "" },
    ]);
  };

  return inputList.map((x, i) => {
    return (
      <React.Fragment>
        <div className={styles.root}>
          <Grid container spacing={3}>
            <Grid item xs={2}>
              <div className="btn-box">
                {inputList.length !== 1 && (
                  <Button
                    color="default"
                    variant="contained"
                    className="mr10"
                    onClick={() => handleRemoveClick(i)}
                  >
                    <RemoveIcon />
                  </Button>
                )}
                {inputList.length - 1 === i && (
                  <Button
                    color="default"
                    variant="contained"
                    onClick={handleAddClick}
                  >
                    <AddIcon />
                  </Button>
                )}
              </div>
            </Grid>
            <Grid item xs={3}>
              <Autocomplete
                id="foodproducts-combobox"
                options={foodProducts}
                getOptionLabel={(option) => option.foodProductName}
                onChange={(e, val) => {
                  const list = [...inputList];
                  list[i].foodProductId = val.foodProductId;
                  setInputList(list);
                }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Select food product"
                    variant="outlined"
                  />
                )}
              />
            </Grid>
            <Grid item xs={3}>
              <TextField
                name="amount"
                placeholder="Enter Amount"
                value={x.amount}
                onChange={(e) => handleInputChange(e, i)}
              />
            </Grid>
            <Grid item xs={4}>
              <UnitSelector unit={unit} setUnit={setUnit} />
            </Grid>
          </Grid>
        </div>
      </React.Fragment>
    );
  });
};

const styles = {
  root: {
    flexGrow: 1,
  },
  paper: {
    textAlign: "center",
  },
};

export default FoodProductInput;
