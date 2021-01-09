import React from "react";

import ToggleButton from "@material-ui/lab/ToggleButton";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import Typography from "@material-ui/core/Typography";

const UnitSelector = ({ unit, handleChangeUnit }) => {
  return (
    <ToggleButtonGroup
      value={unit}
      exclusive
      onChange={(event, unit) => {
        handleChangeUnit(unit);
      }}
      aria-label="text alignment"
    >
      <ToggleButton value="Grams" aria-label="left aligned">
        <Typography variant="button">Grams</Typography>
      </ToggleButton>
      <ToggleButton value="Pieces" aria-label="centered">
        <Typography variant="button">Pieces</Typography>
      </ToggleButton>
      <ToggleButton value="Mililiter" aria-label="right aligned">
        <Typography variant="button">Mililiter</Typography>
      </ToggleButton>
      <ToggleButton value="NotAssigned" aria-label="justified">
        <Typography variant="button">None</Typography>
      </ToggleButton>
    </ToggleButtonGroup>
  );
};

export default UnitSelector;
