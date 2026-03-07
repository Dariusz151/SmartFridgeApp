import { ToggleButton, ToggleButtonGroup } from "@mui/material";
import type { Unit } from "@/types";

interface UnitSelectorProps {
  value: Unit | "";
  onChange: (unit: Unit) => void;
  size?: "small" | "medium";
}

const units: { value: Unit; label: string }[] = [
  { value: "Grams", label: "Grams" },
  { value: "Pieces", label: "Pieces" },
  { value: "Mililiter", label: "ml" },
  { value: "NotAssigned", label: "None" },
];

export default function UnitSelector({ value, onChange, size = "small" }: UnitSelectorProps) {
  return (
    <ToggleButtonGroup
      value={value}
      exclusive
      onChange={(_, unit) => unit && onChange(unit as Unit)}
      size={size}
    >
      {units.map((u) => (
        <ToggleButton key={u.value} value={u.value}>
          {u.label}
        </ToggleButton>
      ))}
    </ToggleButtonGroup>
  );
}
