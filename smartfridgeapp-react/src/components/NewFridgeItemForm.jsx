import React from "react";
import Form from "./common/form";

class NewFridgeItemForm extends Form {
  state = {
    data: {
      foodProductId: "",
      categoryId: "",
      value: "",
      unit: "",
      userId: "",
    },
  };

  constructor(props) {
    super(props);
  }

  async doSubmit() {
    console.log(this.state.data);

    // await post(url, formData, config)
    //   .then((response) => {
    //     imageSentSuccess = true;
    //     //if (response.statusText === "OK")
    //   })
    //   .catch((msg) => {
    //     imageSentSuccess = false;
    //   });
    // this.setState({
    //   file: {},
    //   data: {
    //     firstname: "",
    //     lastname: "",
    //   },
    // });
  }

  render() {
    return (
      <div>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput(
            "foodProductId",
            "Food Product Id",
            this.state.data.foodProductId
          )}
          {this.renderInput(
            "categoryId",
            "Category Id",
            this.state.data.categoryId
          )}
          {this.renderInput("value", "Value", this.state.data.value)}
          {this.renderInput("unit", "Unit", this.state.data.unit)}
          {this.renderInput("userId", "User Id", this.state.data.userId)}
          {this.renderButton("Submit")}
        </form>
      </div>
    );
  }
}

export default NewFridgeItemForm;
