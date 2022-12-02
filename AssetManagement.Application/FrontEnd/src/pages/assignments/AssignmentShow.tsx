import React from "react";
import { Dialog, DialogContent, DialogTitle, Grid } from "@mui/material";
import styled from "styled-components";
import { DateField, FunctionField, SimpleShowLayout, TextField, useRecordContext } from "react-admin";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import DataLabel from "../../components/grid/DataLabel";
import DataText from "../../components/grid/DataText";

const StyledDialog = styled(Dialog)`
.MuiBackdrop-root {
  background-color: transparent;
}
`;

const StyledDialogTitle = styled(DialogTitle)`
&.MuiDialogTitle-root {
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
                        <DataLabel label="Assigned To" />
                        <DataText><TextField source="assignToAppUser" /></DataText>
                        <DataLabel label="Assigned by" />
                        <DataText><TextField source="assignByAppUser" /></DataText>
                        <DataLabel label="Assigned Date" />
                        <DataText><DateField source="assignedDate" /></DataText>
                        <DataLabel label="State" />
                        <DataText><FunctionField source="stateName" render={(record) => record.state == "0" ? "Accepted" : "Waiting for acceptance"} /></DataText>
                        <DataLabel label="Note" />
                        <DataText><TextField source="note" /></DataText>
                    </Grid>
                </SimpleShowLayout>
            </StyledDialogContent>
        </StyledDialog>
    );
}

export default AssignmentShow;