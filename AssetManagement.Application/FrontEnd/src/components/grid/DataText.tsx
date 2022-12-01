import React from "react";
import { Grid } from "@mui/material";

const DataText = ({ xs = 9, children }) => {
    return (
        <Grid item alignSelf={"right"} xs={xs}>{children}</Grid>
    )
}

export default DataText;