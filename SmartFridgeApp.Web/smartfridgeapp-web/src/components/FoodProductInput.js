import React from "react";

import { Grid, TextField, Button, Checkbox } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import RemoveIcon from "@material-ui/icons/Remove";
import AddIcon from "@material-ui/icons/Add";
import UnitSelector from "./UnitSelector";

const FoodProductInput = ({ foodProducts, inputList, setInputList }) => {
  const handleChangeUnit = (val, i) => {
    const list = [...inputList];
    list[i].unit = val;
    setInputList(list);
  };

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
      {
        foodProductId: 0,
        foodProductName: "",
        amount: "",
        unit: "",
        optional: false,
      },
    ]);
  };

  const handleCheckbox = (val, i) => {
    const list = [...inputList];
    const actualState = list[i].optional;

    list[i].optional = !actualState;
    setInputList(list);
  };

  return inputList.map((fp, i) => {
    return (
      <React.Fragment>
        <div className={styles.root}>
          <Grid container spacing={2}>
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
            <Grid item xs={1}>
              <Checkbox
                checked={fp.optional}
                onChange={() => handleCheckbox(fp, i)}
              ></Checkbox>
            </Grid>
            <Grid item xs={3}>
              <Autocomplete
                id="foodproducts-combobox"
                options={foodProducts}
                getOptionLabel={(option) => option.foodProductName}
                onChange={(e, val) => {
                  if (val != null) {
                    const list = [...inputList];
                    list[i].foodProductId = val.foodProductId;
                    list[i].foodProductName = val.foodProductName;
                    setInputList(list);
                  }
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
            <Grid item xs={2}>
              <TextField
                name="amount"
                placeholder="Enter Amount"
                value={fp.amount}
                onChange={(e) => handleInputChange(e, i)}
              />
            </Grid>
            <Grid item xs={3}>
              <UnitSelector
                unit={inputList[i].unit}
                handleChangeUnit={(e) => handleChangeUnit(e, i)}
              />
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
