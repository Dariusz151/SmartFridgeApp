import React, { Component } from "react";
import Input from "./input";

class Form extends Component {
  state = {
    data: {},
  };

  handleSubmit = (e) => {
    e.preventDefault();

    this.doSubmit();
  };

  handleChange = ({ currentTarget: input }) => {
    const data = { ...this.state.data };
    data[input.name] = input.value;

    this.setState({ data });
  };

  renderButton(label) {
    return <button className="btn btn-primary">{label}</button>;
  }

  renderInput(name, label, value, type = "text") {
    return (
      <Input
        value={value}
        type={type}
        name={name}
        label={label}
        onChange={this.handleChange}
      />
    );
  }
}

export default Form;
