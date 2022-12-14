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
    useRecordContext,
} from "react-admin";
import { CustomDeleteWithConfirmButton } from "../../components/modal/confirmDeleteModal/CustomDeleteWithConfirm";
import CheckIcon from "@mui/icons-material/Check";
import ClearIcon from "@mui/icons-material/Clear";
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import { ButtonGroup, Stack, Container, Typography } from "@mui/material";
import { DateAssignedFilterSelect } from "../../components/select/DateAssignedFilterSelect";
import { useNavigate } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { listStyle } from "../../styles/listStyle";
import DoneIcon from "@mui/icons-material/Done";
import { CustomCompleteReturnRequestWithConfirm } from "../../components/modal/confirmCompleteModal/CustomCompleteReturnRequestWithConfirm";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [record, setRecord] = useState();
    const [assignment, setAssignment] = useState();
    const [deleting, setDeleting] = useState(false);
    const [complete, setComplete] = useState(false);

    const toggle = () => {
        setIsOpened(!isOpened);
    };
    const postRowClick = (id, resource) => {
        assetProvider
            .getOne("assignments", { id: id })
            .then((response) => {
                setAssignment(response.data);
            })
            .catch((err) => {
                console.log(err);
            });
        toggle();
        return "";
    };

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, "0");
    var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
    var yyyy = String(today.getFullYear());
    var currentDay = yyyy + "-" + mm + "-" + dd;

    const refresh = useRefresh();

    const returnRequestFilter = [
        <StateFilterSelect
            source="states"
            label="State"
            sx={{ width: "250px" }}
            statesList={[
                { value: 0, text: "Waiting for returning" },
                { value: 1, text: "Completed" },
            ]}
            defaultSelect={[0, 1]}
            alwaysOn
        />,
        <DateAssignedFilterSelect
            source="returnedDateFilter"
            alwaysOn
            id="ReturnedDateFilterReturnRequest"
            label="Returned Date"
            inputProps={{ max: currentDay }}
        />,
        <SearchInput
            InputLabelProps={{ shrink: false }}
            source="searchString"
            alwaysOn
        />,
    ];

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Manage Assignment" />
            <ListBase perPage={5} sort={{ field: "noNumber", order: "ASC" }}>
                <h2 style={{ color: "#cf2338" }}>Request List</h2>
                <Stack direction="row" justifyContent="end" alignContent="center">
                    <Typography
                        sx={{
                            flexGrow: 1,
                            form: {
                                "div:nth-of-type(2)": {
                                    marginRight: "auto",
                                },
                            },
                        }}
                    >
                        <FilterForm filters={returnRequestFilter} />
                    </Typography>
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
                    <DateField
                        label="Assigned Date"
                        source="assignedDate"
                        locales="en-GB"
                    />
                    <TextField label="Accepted By" source="acceptedBy" />
                    <DateField
                        label="Returned Date"
                        source="returnedDate"
                        locales="en-GB"
                    />
                    <FunctionField
                        source="state"
                        render={(record) => {
                            switch (record.state) {
                                case 0: {
                                    return "Waiting for returning";
                                }
                                case 1: {
                                    return "Completed";
                                }
                            }
                        }}
                    />
                    <ButtonGroup sx={{ border: null }}>
                        <FunctionField
                            render={(record) => {
                                if (record.state === 0) {
                                    return (
                                        <CustomCompleteReturnRequestWithConfirm
                                            icon={<DoneIcon />}
                                            record={record}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to mark this returning request as 'Completed'?"
                                            onSuccess={refresh}
                                            mutationOptions={{ onSuccess: () => refresh() }}
                                            isOpen={complete}
                                            setComplete={setComplete}
                                        />
                                    );
                                } else {
                                    return (
                                        <CustomCompleteReturnRequestWithConfirm
                                            icon={<DoneIcon />}
                                            record={record}
                                            disabled={true}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to mark this returning request as 'Completed'?"
                                            onSuccess={refresh}
                                            mutationOptions={{ onSuccess: () => refresh() }}
                                            isOpen={complete}
                                            setComplete={setComplete}
                                        />
                                    );
                                }
                            }}
                        />
                        <FunctionField
                            render={(record) => (
                                <CustomDeleteWithConfirmButton
                                    icon={<ClearIcon sx={{ color: record.state == 0 ? "black" : "grey" }} />}
                                    confirmTitle="Are you sure?"
                                    confirmContent="Do you want to cancel this returning request?"
                                    mutationOptions={{ onSuccess: (data) => refresh() }}
                                    isOpen={deleting}
                                    setDeleting={setDeleting}
                                    disabled={record.state == 0 ? false : true}
                                    acceptButtonLabel="Yes"
                                    cancelButtonLabel="No"
                                />
                            )}
                        />
                    </ButtonGroup>
                </Datagrid>
                <AssetsPagination />
            </ListBase>
        </Container>
    );
};
