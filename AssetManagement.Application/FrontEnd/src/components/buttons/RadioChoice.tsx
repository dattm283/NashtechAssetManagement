import React from "react";
import { useRecordContext } from "ra-core";
import { Radio } from "@mui/material";

const RadioChoice = ({ handleChange, selectedValue }) => {
    const record = useRecordContext();

    return (
        <Radio
            checked={selectedValue === record.assetCode}
            onChange={() => { handleChange(record.assetCode) }}
            value={record.assetCode}
            name="radio-buttons"
            inputProps={{ 'aria-label': 'A' }}
        />
    )
}

export default RadioChoice;