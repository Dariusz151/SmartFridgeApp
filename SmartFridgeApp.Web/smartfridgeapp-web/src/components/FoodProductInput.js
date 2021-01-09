import React, { useState, useEffect } from "react";

import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";

import Autocomplete from "@material-ui/lab/Autocomplete";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";

const FoodProductInput = ({ foodProducts, inputList, setInputList }) => {
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
        <div>
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
          <TextField
            name="amount"
            placeholder="Enter Amount"
            value={x.amount}
            onChange={(e) => handleInputChange(e, i)}
          />
        </div>
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
      </React.Fragment>
    );
  });
};

export default FoodProductInput;
