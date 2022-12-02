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
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import { ButtonGroup, Stack, Container } from "@mui/material";
import { DateAssignedFilterSelect } from "../../components/select/DateAssignedFilterSelect";
import { useNavigate } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import AssignmentShow from "./AssignmentShow";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [record, setRecord] = useState();
    const [assignment, setAssignment] = useState();

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

    const assignmentsFilter = [
        <StateFilterSelect
            source="states"
            label="Type"
            sx={{ width: "250px" }}
            statesList={[
                { value: 0, text: "Accepted" },
                { value: 1, text: "Waiting for acceptance" },
            ]}
            alwaysOn
        />,
        <DateAssignedFilterSelect source="assignedDateFilter" alwaysOn />,
        <SearchInput InputLabelProps={{ shrink: false }} source="searchString" alwaysOn />
    ];

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Manage Assignment" />
            <ListBase
                perPage={5}
                sort={{ field: "noNumber", order: "DESC" }}
            >
                <h2 style={{ color: "#cf2338" }}>Assignment List</h2>
                <Stack direction="row" justifyContent="end" alignContent="center">
                    <div style={{ flexGrow: 1 }}><FilterForm style={{ justifyContent: "space-between" }} filters={assignmentsFilter} /></div>
                    <div style={{ display: "flex", alignItems: "end" }}>
                        <CreateButton
                            size="large"
                            variant="contained"
                            color="secondary"
                            label="Create new assignment"
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
                    <TextField label="No" source="noNumber" />
                    <TextField label="Asset Code" source="assetCode" />
                    <TextField label="Asset Name" source="assetName" />
                    <TextField label="Assigned to" source="assignedTo" />
                    <TextField label="Assigned by" source="assignedBy" />
                    <DateField label="Assigned Date" source="assignedDate" />
                    <FunctionField source="state" render={(record) => record.state == "0" ? "Accepted" : "Waiting for acceptance"} />
                    <ButtonGroup sx={{ border: null }}>
                        <FunctionField render={(record) => {
                            console.log(record);
                            if (record.state === 1) {
                                return (
                                    <EditButton variant="text" size="small" label="" />
                                )
                            }
                            else {
                                return (
                                    <EditButton disabled variant="text" size="small" label="" />
                                )
                            }
                        }} />
                        <CustomDeleteWithConfirmButton
                            icon={<HighlightOffIcon />}
                            confirmTitle="Are you sure?"
                            confirmContent="Do you want to delete this asset?"
                            mutationOptions={{ onSuccess: (data) => refresh() }}
                        />
                    </ButtonGroup>
                </Datagrid>
                <AssetsPagination />
            </ListBase>
            <AssignmentShow isOpened={isOpened} toggle={toggle} assignment={assignment} />
        </Container>
    );
};