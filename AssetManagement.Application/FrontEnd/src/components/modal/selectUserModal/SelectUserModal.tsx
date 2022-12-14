import React, { forwardRef, useEffect, useRef, useState } from 'react';
import { Dialog, DialogContent, DialogTitle, Grid } from "@mui/material";
import styled from "styled-components";
import { Datagrid, FilterForm, FunctionField, ListBase, SearchInput, TextField, useListContext } from 'react-admin';
import { useFormContext } from "react-hook-form";
import RadioChoice from '../../buttons/RadioChoice';
import ChooseListPagination from '../../pagination/ChooseListPagination';
import ChooseListBotToolbar from '../../toolbar/ChooseListBotToolbar';
// import RootRef from '@material-ui/core/RootRef';

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

const SelectUserModal = ({ isOpened, toggle, pos, selectedUser, setSelectedUser, setChanged }) => {
    const [tempChoice, setTempChoice] = useState(selectedUser);
    const postRowClick = (id, resource, record) => {
        console.log("user record", record)
        setTempChoice({
            staffCode: record.staffCode,
            fullname: record.fullName
        });
        return "";
    };

    useEffect(() => {
        setTempChoice(selectedUser);
    }, [selectedUser])

    const handleChange = (user) => {
        console.log("user change", user);
        setTempChoice({
            staffCode: user.staffCode,
            fullname: user.fullName
        });
    };

    const handleSave = () => {
        setSelectedUser(tempChoice);
        if (tempChoice !== selectedUser) {
            setChanged(true);
        }
        toggle();
    }

    const handleClose = () => {
        setTempChoice(selectedUser);
        toggle();
    }

    const { setValue } = useFormContext();

    useEffect(() => {
        setValue("assignToAppUserFullName", selectedUser.fullname);
        setValue("assignToAppUserStaffCode", selectedUser.staffCode);
    }, [selectedUser])

    const usersFilter = [
        <SearchInput
            sx={{ marginRight: "-300px" }}
            InputLabelProps={{ shrink: false }}
            source="searchString"
            alwaysOn
        />
    ];

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
            <Grid item xs={0}>
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
                                selectedValue={tempChoice.staffCode}
                                propsGetter={record => record.staffCode}
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

export default forwardRef(SelectUserModal);