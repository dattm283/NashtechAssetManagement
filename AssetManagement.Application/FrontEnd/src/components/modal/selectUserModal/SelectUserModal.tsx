import React, { useEffect, useState } from 'react';
import { Dialog, DialogContent, DialogTitle, Grid } from "@mui/material";
import styled from "styled-components";
import { Datagrid, FilterForm, FunctionField, ListBase, SearchInput, TextField, useListContext } from 'react-admin';
import { useFormContext } from "react-hook-form";
import RadioChoice from '../../buttons/RadioChoice';
import ChooseListPagination from '../../pagination/ChooseListPagination';
import ChooseListBotToolbar from '../../toolbar/ChooseListBotToolbar';

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

const SelectUserModal = ({ isOpened, toggle, pos, selectedUser, setSelectedUser }) => {
    const [tempChoice, setTempChoice] = useState(selectedUser);
    const postRowClick = (id, resource, record) => {
        setTempChoice(record.staffCode);
        return "";
    };

    useEffect(() => {
        setTempChoice(selectedUser);
    }, [selectedUser])

    const handleChange = (staffCode) => {
        setTempChoice(staffCode);
    };

    const handleSave = () => {
        setSelectedUser(tempChoice);
        toggle();
    }

    const handleClose = () => {
        setTempChoice(selectedUser);
        toggle();
    }

    const { setValue } = useFormContext();

    useEffect(() => {
        setValue("assignToAppUserStaffCode", selectedUser);
    }, [selectedUser])

    const usersFilter = [
        <SearchInput
            sx={{ marginRight: "-300px" }}
            InputLabelProps={{ shrink: false }}
            source="searchString"
            alwaysOn
        />
    ];

    const getStaffCode = record => record.staffCode;

    return (
        <StyledDialog
            id="abcxyz"
            open={isOpened}
            onClose={handleClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            scroll="body"
            fullWidth={true}
            maxWidth="sm"
            disableEnforceFocus
        >
            <Grid sx={{
                position: "fixed",
                top: (pos.top + 5) + "px",
                left: (pos.left + 5) + "px",
                width: "45%",
                borderRadius: "10px",
                border: "1px solid",
                maxHeight: "50vh",
                overflow: "scroll",
            }}>
                <StyledDialogContent>
                    <ListBase
                        perPage={5}
                        sort={{ field: "staffCode", order: "DESC" }}
                        resource="user"
                    >
                        <Grid container>
                            <Grid item xs={6}>
                                <h2 style={{ color: "#cf2338" }}>Select User</h2>
                            </Grid>
                            <Grid item xs={6}>
                                <div style={{ flexGrow: 1 }}><FilterForm style={{ justifyContent: "space-between" }} filters={usersFilter} /></div>
                            </Grid>
                        </Grid>

                        <Datagrid
                            rowClick={postRowClick}
                            empty={<h2>No User found</h2>}
                            bulkActionButtons={false}
                        >
                            <RadioChoice
                                handleChange={handleChange}
                                selectedValue={tempChoice}
                                propsGetter={getStaffCode}
                            />
                            <TextField label="Staff Code" source="staffCode" />
                            <TextField label="Full Name" source="fullName" />
                            <FunctionField source="type" render={data => data.type == "Admin" ? "Admin" : "Staff"} />
                        </Datagrid>
                        <ChooseListPagination />
                    </ListBase>
                    <ChooseListBotToolbar handleSave={handleSave} handleCancel={handleClose} />
                </StyledDialogContent>
            </Grid>
        </StyledDialog>
    );
}

export default SelectUserModal;