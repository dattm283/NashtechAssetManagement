import React, { useEffect, useState } from 'react';
import { Box, Button, ButtonGroup, Dialog, DialogContent, DialogTitle, Grid, Radio, Stack } from "@mui/material";
import styled from "styled-components";
import SearchIcon from '@mui/icons-material/Search';
import { CreateButton, Datagrid, EditButton, FilterForm, FunctionField, ListBase, SearchInput, TextField, TextInput, useDataProvider, useListContext, useRecordContext, useRecordSelection, useRefresh } from 'react-admin';
import { BloodtypeOutlined } from '@mui/icons-material';
import { CustomDeleteWithConfirmButton } from '../confirmDeleteModal/CustomDeleteWithConfirm';
import AssetsPagination from '../../pagination/AssetsPagination';
import CategoryFilterSelect from '../../select/CategoryFilterSelect';
import StateFilterSelect from '../../select/StateFilterSelect';
import { useNavigate } from 'react-router-dom';
import RadioChoice from '../../buttons/RadioChoice';
import { useFormContext } from "react-hook-form";

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
    const postRowClick = (id, resource, record) => {
        setSelectedUser(record.staffCode);
        toggle();
        return "";
    };

    const handleChange = (staffCode) => {
        setSelectedUser(staffCode);
    };

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

    // const style = {
    //     color: "#cf2338",
    //     paddingLeft: "50px",
    //     fontWeight: "bold",
    //     fontSize: "20px"
    // };

    return (
        <StyledDialog
            id="abcxyz"
            open={isOpened}
            onClose={toggle}
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
                                selectedValue={selectedUser}
                                propsGetter={getStaffCode}
                            />
                            <TextField label="Staff Code" source="staffCode" />
                            <TextField label="Full Name" source="fullName" />
                            <FunctionField source="Type" render={data => data.type == "Admin" ? "Admin" : "Staff"} />
                        </Datagrid>
                        <AssetsPagination />
                    </ListBase>
                </StyledDialogContent>
            </Grid>
        </StyledDialog>
    );
}

export default SelectUserModal;