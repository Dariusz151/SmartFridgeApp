import React, { useState, useEffect } from "react";
import configData from "../../config_url.json";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import AddIcon from "@material-ui/icons/Add";
import CloseIcon from "@material-ui/icons/Close";
import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";

const NewFridgeItemDialog = ({
  fridgeId,
  selectedUserId,
  state,
  handleClose,
}) => {
  const [foodProducts, setFoodProducts] = useState([
    { foodProductId: 1, foodProductName: "fp" },
  ]);
  const [foodProductId, setFoodProductId] = useState(0);
  const [values, setValues] = useState({
    note: "",
    value: "",
  });
  const [unit, setUnit] = useState("NotAssigned");

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

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const handleNumberChange = (e) => {
    const re = /^[0-9\b]+$/;
    if (e.target.value === "" || re.test(e.target.value)) {
      const { name, value } = e.target;
      setValues({ ...values, [name]: value });
    }
  };

  const handleUnit = (event, unit) => {
    setUnit(unit);
  };

  const onSelectChange = (event, val) => {
    if (val != undefined && val != null) setFoodProductId(val.foodProductId);
  };

  const refreshForm = () => {
    setValues({
      note: "",
      unit: "",
      value: "",
    });
    setFoodProductId(0);
  };

  const handleAdd = () => {
    const obj = {
      userId: selectedUserId,
      fridgeItem: {
        foodProductId: foodProductId,
        value: parseInt(values.value),
        note: "",
        unit: unit,
      },
    };
    console.log(obj);

    //if (obj.value != NaN && obj.value > 0) {
    fetch(configData.SERVER_URL + "/api/fridgeItems/" + fridgeId + "/add", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    })
      .then(refreshForm())
      .then(console.log("Success"))
      .then(() => handleClose());
    //}
  };

  return (
    <Dialog
      open={state}
      onClose={handleClose}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle id="form-dialog-title">Add Fridge item</DialogTitle>
      <DialogContent>
        <Autocomplete
          id="foodproducts-combobox"
          options={foodProducts}
          getOptionLabel={(option) => option.foodProductName}
          style={{ width: "100%" }}
          onChange={onSelectChange}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Select food product"
              variant="outlined"
            />
          )}
        />
        <br />
        <TextField
          name="value"
          label="Value"
          fullWidth
          onChange={handleNumberChange}
          value={values.value}
        />
        <br />
        <br />
        <br />
        <ToggleButtonGroup
          value={unit}
          exclusive
          onChange={handleUnit}
          aria-label="text alignment"
        >
          <ToggleButton value="Grams" aria-label="left aligned">
            <Typography variant="body1">Grams</Typography>
          </ToggleButton>
          <ToggleButton value="Pieces" aria-label="centered">
            <Typography>Pieces</Typography>
          </ToggleButton>
          <ToggleButton value="Mililiter" aria-label="right aligned">
            <Typography>Mililiter</Typography>
          </ToggleButton>
          <ToggleButton value="NotAssigned" aria-label="justified">
            <Typography>None</Typography>
          </ToggleButton>
        </ToggleButtonGroup>
        <br />
        <br />
      </DialogContent>
      <DialogActions>
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
      </DialogActions>
    </Dialog>
  );
};

export default NewFridgeItemDialog;
