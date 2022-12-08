import React, { useState } from "react";
import {
    Datagrid,
    List,
    Title,
    TextField,
    TextInput,
    DateField,
    EditButton,
    useDataProvider,
    FunctionField,
    useRefresh,
    ListBase,
    FilterForm,
    CreateButton,
    Button,
    SearchInput,
    useRecordContext
} from "react-admin";
import { CustomDeleteWithConfirmButton } from "../../components/modal/confirmDeleteModal/CustomDeleteWithConfirm";
import HighlightOffIcon from "@mui/icons-material/HighlightOff";
import ReplayIcon from '@mui/icons-material/Replay';
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import { ButtonGroup, Stack, Container, Typography } from "@mui/material";
import { DateAssignedFilterSelect } from "../../components/select/DateAssignedFilterSelect";
import { useNavigate } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { listStyle } from "../../styles/listStyle";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [record, setRecord] = useState();
    const [assignment, setAssignment] = useState();
    const [deleting, setDeleting] = useState(false);

    const toggle = () => {
        setIsOpened(!isOpened);
    };
    const postRowClick = (id, resource) => {
        assetProvider.getOne("assignments", { id: id })
            .then(response => {
                setAssignment(response.data);
            })
            .catch(err => {
                console.log(err);
            })
        toggle();
        return "";
    };

    const refresh = useRefresh();

    const returnRequestFilter = [
        <StateFilterSelect
            source="states"
            label="State"
            sx={{ width: "250px" }}
            statesList={[
                { value: 2, text: "Completed" },
                { value: 4, text: "Waiting for returning" },
            ]}
            defaultSelect={[2, 4]}
            alwaysOn
        />,
        <DateAssignedFilterSelect source="returnedDateFilter" alwaysOn id="ReturnedDateFilterReturnRequest" />,
        <SearchInput InputLabelProps={{ shrink: false }} source="searchString" alwaysOn />
    ];

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Manage Assignment" />
            <ListBase
                perPage={5}
                sort={{ field: "noNumber", order: "ASC" }}
            >
                <h2 style={{ color: "#cf2338" }}>Assignment List</h2>
                <Stack direction="row" justifyContent="end" alignContent="center">
                    <Typography
                        sx={{
                            flexGrow: 1,
                            "form": {
                                "div:nth-of-type(2)": {
                                    marginRight: "auto"
                                }
                            }
                        }}><FilterForm filters={returnRequestFilter} /></Typography>
                    {/* <div style={{ display: "flex", alignItems: "end" }}>
                        <CreateButton
                            size="large"
                            variant="contained"
                            color="secondary"
                            label="Create new assignment"
                            id="createNewAssignmentBtn"
                            icon={<></>}
                        />
                    </div> */}
                </Stack>

                <Datagrid
                    rowClick={!deleting ? postRowClick : (id, resource) => ""}
                    empty={
                        <p>
                            <h2>No Data found</h2>
                        </p>
                    }
                    bulkActionButtons={false}
                >
                    <TextField label="No" source="noNumber" />
                    <TextField label="Asset Code" source="assetCode" />
                    <TextField label="Asset Name" source="assetName" />
                    <TextField label="Requested By" source="requestedBy" />
                    <DateField label="Assigned Date" source="assignedDate" locales="en-GB" />
                    <TextField label="Accepted By" source="acceptedBy" />
                    <DateField label="Returned Date" source="returnedDate" locales="en-GB" />
                    <FunctionField source="state" render={(record) => {
                        switch (record.state) {
                            case 0: {
                                return "Accepted";
                            }
                            case 1: {
                                return "Waiting for acceptance";
                            }
                            case 2: {
                                return "Completed";
                            }
                            case 3: {
                                return "Returned";
                            }
                            case 4: {
                                return "Waiting for returning";
                            }
                        }
                    }} />
                    <ButtonGroup sx={{ border: null }}>
                        <FunctionField render={(record) => {
                            if (record.state === 1) {
                                return (
                                    <EditButton variant="text" size="small" label="" sx={listStyle.buttonToolbar} />
                                )
                            }
                            else {
                                return (
                                    <EditButton disabled variant="text" size="small" label=""
                                        sx={listStyle.buttonToolbar} />
                                )
                            }
                        }} />
                        <FunctionField render={record => (
                            <CustomDeleteWithConfirmButton
                                icon={<HighlightOffIcon />}
                                confirmTitle="Are you sure?"
                                confirmContent="Do you want to cancel this returning request?"
                                mutationOptions={{ onSuccess: (data) => refresh() }}
                                isOpen={deleting}
                                setDeleting={setDeleting}
                                disabled={record.state == 4? false: true}
                            />
                        )} />
                    </ButtonGroup>
                </Datagrid>
                <AssetsPagination />
            </ListBase>

        </Container>
    );
};