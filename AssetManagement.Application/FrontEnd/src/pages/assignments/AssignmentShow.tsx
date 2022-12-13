import React, { useEffect, useState } from "react";
import { Dialog, DialogContent, DialogTitle, Grid, Typography } from "@mui/material";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import styled from "styled-components";
import { SimpleShowLayout, TextField, FunctionField, DateField } from "react-admin";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import { getAssignementByAssetCodeId } from "../../services/assignment";
import DataLabel from "../../components/grid/DataLabel";
import DataText from "../../components/grid/DataText";

const StyledDialog = styled(Dialog)`
.MuiBackdrop-root {
  background-color: transparent;
}
`;

const StyledDialogTitle = styled(DialogTitle)`
&.MuiDialogTitle-root {
    padding: 16px 38px;
    background-color: #EFF1F5;
    color: #CF2338;
}
`;

const StyledDialogContent = styled(DialogContent)`
&.MuiDialogContent-root {
  background-color: #FAFCFC;
}
`;

const AssignmentShow = ({ isOpened, toggle, assignment }) => {
    const style = {
        bgcolor: "#cf2338",
        fontWeight: "bold",
        color: "#fff",
    };

    return (
        <StyledDialog
            open={isOpened}
            onClose={toggle}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            scroll="body"
        >
            <StyledDialogTitle id="alert-dialog-title" sx={style}>
                {"Detail Assignment Information"}
                <IconButton
                    aria-label="close"
                    onClick={toggle}
                    sx={{
                        position: "absolute",
                        right: 8,
                        top: 8,
                        color: "#CF2338",
                    }}
                >
                    <CloseIcon />
                </IconButton>
            </StyledDialogTitle>
            <StyledDialogContent>
                <SimpleShowLayout record={assignment}>
                    <Grid container spacing={2} >
                        <DataLabel label="Asset Code" />
                        <DataText><TextField source="assetCode" /></DataText>
                        <DataLabel label="Asset Name" />
                        <DataText><TextField source="assetName" /></DataText>
                        <DataLabel label="Specification" />
                        <DataText><TextField source="specification" /></DataText>
                        <DataLabel label="Assigned to" />
                        <DataText><TextField source="assignToAppUser" /></DataText>
                        <DataLabel label="Assigned by" />
                        <DataText><TextField source="assignByAppUser" /></DataText>
                        <DataLabel label="Assigned Date" />
                        <DataText><DateField source="assignedDate" locales="en-GB" /></DataText>
                        <DataLabel label="State" />
                        <DataText>
                            <FunctionField render={record => {
                                switch (record.state) {
                                    case 0: {
                                        return "Accepted";
                                    }
                                    case 1: {
                                        return "Waiting For Acceptance";
                                    }
                                    case 2: {
                                        return "Returned";
                                    }
                                    case 3: {
                                        return "Waiting For Returning";
                                    }
                                    case 4: {
                                        return "Declined";
                                    }
                                }
                            }} />
                        </DataText>
                        <DataLabel label="Note" />
                        <DataText><TextField source="note" /></DataText>
                    </Grid>
                </SimpleShowLayout>
            </StyledDialogContent>
        </StyledDialog>
    );
}

export default AssignmentShow;