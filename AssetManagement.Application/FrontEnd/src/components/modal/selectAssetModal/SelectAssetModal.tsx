import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Box, Button, ButtonGroup, Dialog, DialogContent, DialogTitle, Grid, Radio } from "@mui/material";
import styled from "styled-components";
import SearchIcon from '@mui/icons-material/Search';
import { TextInput } from 'react-admin';
import { BloodtypeOutlined } from '@mui/icons-material';

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
    const [selectedValue, setSelectedValue] = useState(1);

    const handleChange = (id) => {
        setSelectedValue(id);
        toggle();
    };

    useEffect(() => {
        var modal = document.getElementById("abcxyz");
        if (modal) {
            console.log("modal pos: ", modal.getBoundingClientRect());
        }
    }, [isOpened])

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: '',
            width: 70,
            renderCell: (cellValue) => {
                return (
                    <Radio
                        checked={selectedValue === cellValue.id}
                        onChange={() => { handleChange(cellValue.id) }}
                        value={cellValue.id}
                        name="radio-buttons"
                        inputProps={{ 'aria-label': 'A' }}
                    />
                )
            }
        },
        {
            field: 'assetCode',
            headerName: 'Asset Code',
            flex: 1,
        },
        {
            field: 'assetName',
            headerName: 'Asset Name',
            flex: 1,
        },
        {
            field: 'category',
            headerName: 'Category',
        },
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
                {/* <StyledDialogTitle id="alert-dialog-title" sx={style}>
                    
                </StyledDialogTitle> */}
                <StyledDialogContent>
                    <Grid container alignContent="right">
                        <Grid xs={6} sx={style}>
                            Select Asset
                        </Grid>
                        <Grid xs={6}>
                                <TextInput source="hha" />
                                <Button><SearchIcon /></Button>
                        </Grid>
                    </Grid>
                    <div style={{ height: 400, width: '100%' }}>
                        <DataGrid
                            rows={rows}
                            columns={columns}
                            pageSize={5}
                            rowsPerPageOptions={[5]}
                        />
                    </div>
                </StyledDialogContent>
            </Grid>
        </StyledDialog>

    );
}

export default SelectAssetModal;