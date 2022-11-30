import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
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

const SelectAssetModal = ({ isOpened, toggle, pos, selectedAsset, setSelectedAsset }) => {
    const [record, setRecord] = useState();
    // const { data } = useGetList("category", { pagination: { page: 1, perPage: 99 } })
    const dataProvider = useDataProvider();
    let data = dataProvider.getList("category", { pagination: { page: 1, perPage: 99 }, sort: { field: "name", order: "ASC" }, filter: {} }).then(res => res.data)

    const postRowClick = (id, resource, record) => {
        setSelectedAsset(record.assetCode);
        toggle();
        return "";
    };

    const handleChange = (assetCode) => {
        setSelectedAsset(assetCode);
    };

    const assetsFilter = [
        <SearchInput InputLabelProps={{ shrink: false }} source="searchString" alwaysOn />
    ];

    const style = {
        color: "#cf2338",
        paddingLeft: "50px",
        fontWeight: "bold",
        fontSize: "20px"
    };

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
                        sort={{ field: "name", order: "DESC" }}
                        resource="assets"
                    >
                        <Grid container>
                            <Grid item xs={6}>
                                <h2 style={{ color: "#cf2338" }}>Select Asset</h2>
                            </Grid>
                            <Grid item xs={6}>
                                <div style={{ flexGrow: 1 }}><FilterForm style={{ justifyContent: "space-between" }} filters={assetsFilter} /></div>
                            </Grid>
                        </Grid>

                        <Stack direction="row" justifyContent="end" alignContent="center">

                        </Stack>

                        <Datagrid
                            rowClick={postRowClick}
                            empty={
                                <p>
                                    <h2>No Data found</h2>
                                </p>
                            }
                            bulkActionButtons={false}
                        >
                            <RadioChoice handleChange={handleChange} selectedValue={selectedAsset} />
                            <TextField source="assetCode" />
                            <TextField label="Asset Name" source="name" />
                            <TextField label="Category" source="categoryName" />
                        </Datagrid>
                        <AssetsPagination />
                    </ListBase>
                </StyledDialogContent>
            </Grid>
        </StyledDialog>

    );
}

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

export default SelectAssetModal;