import React from "react";

const Input = ({ name, label, ...rest }) => {
  return (
    <div className="form-group">
      <input
        {...rest}
        name={name}
        id={name}
        placeholder={label}
        className="form-control"
      />
    </div>
  );
};

export default Input;
