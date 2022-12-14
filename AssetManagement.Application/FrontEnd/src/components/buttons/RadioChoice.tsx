import React from "react";
import { useRecordContext } from "ra-core";
import { Radio } from "@mui/material";

const RadioChoice = ({ handleChange, selectedValue, propsGetter }) => {
    const record = useRecordContext();

    return (
        <Radio
            checked={selectedValue === propsGetter(record)}
            onChange={() => {
                handleChange(record);
            }}
            value={propsGetter(record)}
            name="radio-buttons"
            inputProps={{ 'aria-label': 'A' }}
        />
    )
}

export default RadioChoice;