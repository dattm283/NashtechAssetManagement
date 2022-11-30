import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Box, Button, ButtonGroup, Dialog, DialogContent, DialogTitle, Grid, Radio, Stack } from "@mui/material";
import styled from "styled-components";
import SearchIcon from '@mui/icons-material/Search';
import { CreateButton, Datagrid, EditButton, FilterForm, FunctionField, ListBase, SearchInput, TextField, TextInput, useDataProvider, useRefresh } from 'react-admin';
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

const SelectAssetModal = ({ isOpened, toggle, pos }) => {
    // const [selectedValue, setSelectedValue] = useState(1);

    // const handleChange = (id) => {
    //     setSelectedValue(id);
    //     toggle();
    // };

    // useEffect(() => {
    //     var modal = document.getElementById("abcxyz");
    //     if (modal) {
    //         console.log("modal pos: ", modal.getBoundingClientRect());
    //     }
    // }, [isOpened])

    // const columns: GridColDef[] = [
    //     {
    //         field: 'id',
    //         headerName: '',
    //         width: 70,
    //         renderCell: (cellValue) => {
    //             return (
    //                 <Radio
    //                     checked={selectedValue === cellValue.id}
    //                     onChange={() => { handleChange(cellValue.id) }}
    //                     value={cellValue.id}
    //                     name="radio-buttons"
    //                     inputProps={{ 'aria-label': 'A' }}
    //                 />
    //             )
    //         }
    //     },
    //     {
    //         field: 'assetCode',
    //         headerName: 'Asset Code',
    //         flex: 1,
    //     },
    //     {
    //         field: 'assetName',
    //         headerName: 'Asset Name',
    //         flex: 1,
    //     },
    //     {
    //         field: 'category',
    //         headerName: 'Category',
    //     },
    // ];

    const [record, setRecord] = useState();
    // const { data } = useGetList("category", { pagination: { page: 1, perPage: 99 } })
    const dataProvider = useDataProvider();
    let data = dataProvider.getList("category", { pagination: { page: 1, perPage: 99 }, sort: { field: "name", order: "ASC" }, filter: {} }).then(res => res.data)

    const navigate = useNavigate();
 
    const postRowClick = (id, resource, record) => {
        setRecord(record);
        toggle();
        return "";
    };

    const refresh = useRefresh();

    const assetsFilter = [
        <StateFilterSelect
            source="states"
            sx={{ width: "250px" }}
            statesList={[
                { value: "0", text: "Available" },
                { value: "1", text: "Not Available" },
                { value: "2", text: "Waiting for recycling" },
                { value: "3", text: "Recycled" },
            ]}
            alwaysOn
        />,
        <CategoryFilterSelect
            source="categories"
            statesList={data}
            alwaysOn
        />,
        <SearchInput InputLabelProps={{ shrink: false }} source="searchString" alwaysOn />
    ];

    const rows = [
        { id: 1, assetCode: 'Snow', assetName: 'Jon', category: "cate 1" },
        { id: 2, assetCode: 'Lannister Lannister Lannister', assetName: 'Cersei', category: "cate 2" },
        { id: 3, assetCode: 'Lannister', assetName: 'Jaime', category: "cate 3" },
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
            open={true}
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
                        filterDefaultValues={{ states: ["0", "1", "4"] }}
                    >
                        <h2 style={{ color: "#cf2338" }}>Asset List</h2>
                        <Stack direction="row" justifyContent="end" alignContent="center">
                            <div style={{ flexGrow: 1 }}><FilterForm style={{ justifyContent: "space-between" }} filters={assetsFilter} /></div>
                            <div style={{ display: "flex", alignItems: "end" }}>
                                <CreateButton
                                    size="large"
                                    variant="contained"
                                    color="secondary"
                                    label="Create new asset"
                                    sx={{
                                        width: "250px",
                                    }}
                                />
                            </div>
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
                            <TextField source="assetCode" />
                            <TextField label="Asset Name" source="name" />
                            <TextField label="Category" source="categoryName" />
                            <FunctionField source="state" render={(record) => record.state == "NotAvailable" ? "Not available" : record.state} />
                        </Datagrid>
                        <AssetsPagination />
                    </ListBase>
                </StyledDialogContent>
            </Grid>
        </StyledDialog>

    );
}

export default SelectAssetModal;