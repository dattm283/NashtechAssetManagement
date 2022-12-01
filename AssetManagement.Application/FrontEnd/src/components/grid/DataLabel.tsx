import React from "react";
import { Grid } from "@mui/material";

const DataLabel = ({ label, xs = 3 }) => {
    return (
        <Grid item alignSelf={"left"} xs={xs}>{label}</Grid>
    )
}

export default DataLabel;