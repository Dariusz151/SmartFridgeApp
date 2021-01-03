import React, { useState, useEffect } from "react";
import configData from "../config_url.json";
import { MDBTable, MDBTableBody, MDBTableHead } from "mdbreact";
import NewFoodProductDialog from "../components/dialogs/NewFoodProductDialog";
import CircularProgress from "@material-ui/core/CircularProgress";
import RefreshIcon from "@material-ui/icons/Refresh";
import SnackBar from "@material-ui/core/Snackbar";
import Button from "@material-ui/core/Button";

const FoodProducts = () => {
  const [dataLoading, finishLoading] = useState(true);
  const [newFoodProductDialog, setNewFoodProductDialog] = useState(false);
  const [categories, setCategories] = useState({
    name: "categoryName",
    categoryId: 1,
  });

  const [dummyState, rerender] = useState(1);
  const handleRefresh = () => {
    rerender(dummyState + 1);
  };

  const [rows, setRows] = useState([
    {
      id: 1,
      name: "foodProducts",
      category: "category",
    },
  ]);

  useEffect(() => {
    fetch(configData.SERVER_URL + "/api/foodproducts")
      .then((response) => response.json())
      .then((json) => {
        const rowsArray = json.map((item, index) => ({
          id: index + 1,
          name: item.foodProductName,
          category: item.foodProductCategory,
        }));

        setRows(rowsArray);
      })
      .catch((error) => console.error(error))
      .finally(() => finishLoading(false));

    fetch(configData.SERVER_URL + "/api/foodproducts/categories")
      .then((response) => response.json())
      .then((json) => {
        const rowsArray = json.map((item) => ({
          name: item.name,
          categoryId: item.categoryId,
        }));

        setCategories(rowsArray);
      })
      .catch((error) => console.error(error));
  }, [dummyState]);

  const handleCloseNewFoodProductDialog = () => {
    setNewFoodProductDialog(false);
  };

  return (
    <div>
      <NewFoodProductDialog
        categories={categories}
        state={newFoodProductDialog}
        handleClose={handleCloseNewFoodProductDialog}
      />
      <Button
        variant="outlined"
        color="primary"
        onClick={() => setNewFoodProductDialog(true)}
      >
        Add new food product
      </Button>
      <Button
        variant="outlined"
        color="primary"
        onClick={handleRefresh}
        startIcon={<RefreshIcon />}
      >
        Refresh
      </Button>
      {dataLoading ? (
        <div>
          <CircularProgress />
          <p>Loading food products</p>
        </div>
      ) : (
        <MDBTable btn>
          <MDBTableHead columns={columns} />
          <MDBTableBody rows={rows} />
        </MDBTable>
      )}
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
];

export default FoodProducts;
