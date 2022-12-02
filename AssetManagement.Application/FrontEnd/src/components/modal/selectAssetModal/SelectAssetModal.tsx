import React, { useEffect, useState } from 'react';
import { Dialog, DialogContent, DialogTitle, Grid } from "@mui/material";
import styled from "styled-components";
import { Datagrid, FilterForm, ListBase, SearchInput, TextField } from 'react-admin';
import RadioChoice from '../../buttons/RadioChoice';
import { useFormContext } from "react-hook-form";
import ChooseListBotToolbar from '../../toolbar/ChooseListBotToolbar';
import ChooseListPagination from '../../pagination/ChooseListPagination';

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
    const [tempChoice, setTempChoice] = useState(selectedAsset);
    const postRowClick = (id, resource, record) => {
        setTempChoice(record.assetCode);
        return record.assetCode;
    };

    const { setValue } = useFormContext();

    useEffect(() => {
        setTempChoice(selectedAsset);
    }, [selectedAsset])

    const handleSave = () => {
        setSelectedAsset(tempChoice);
        toggle();
    }

    const handleChange = (assetCode) => {
        setTempChoice(assetCode);
    };

    const handleClose = () => {
        setTempChoice(selectedAsset);
        toggle();
    }

    useEffect(() => {
        setValue("assetCode", selectedAsset);
    }, [selectedAsset])

    const assetsFilter = [
        <SearchInput InputLabelProps={{ shrink: false }} source="searchString" alwaysOn />
    ];

    const getAssetCode = record => record.assetCode;

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
            }}>
                <StyledDialogContent>
                    <ListBase
                        perPage={5}
                        sort={{ field: "name", order: "DESC" }}
                        filter={{ "states": "0" }}
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

                        <Datagrid
                            rowClick={(postRowClick)}
                            empty={
                                <p>
                                    <h2>No Data found</h2>
                                </p>
                            }
                            bulkActionButtons={false}
                        >
                            <RadioChoice
                                handleChange={handleChange}
                                selectedValue={tempChoice}
                                propsGetter={getAssetCode}
                            />
                            <TextField source="assetCode" />
                            <TextField label="Asset Name" source="name" />
                            <TextField label="Category" source="categoryName" />
                        </Datagrid>
                        <ChooseListPagination />
                    </ListBase>
                    <ChooseListBotToolbar handleSave={handleSave} handleCancel={handleClose} />
                </StyledDialogContent>
            </Grid>
        </StyledDialog>
    );
}

export default SelectAssetModal;