import React, { useState, useEffect } from "react";
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
    DeleteButton
} from "react-admin";
import { AcceptAssignment } from "./AcceptAssignment";
import { DeclineAssignment } from "./DeclineAssignment";
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import ReplayIcon from '@mui/icons-material/Replay';
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import { ButtonGroup, Stack, Container, Typography } from "@mui/material";
import { DateAssignedFilterSelect } from "../../components/select/DateAssignedFilterSelect";
import { useNavigate } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { listStyle } from "../../styles/listStyle";
import AssignmentShow from "../assignments/AssignmentShow";
// import CustomDisableWithConfirm from '../../components/modal/confirmReturnModal/CustomDisableWithConfirm'
import { AdminCustomReturnAssetWithConfirm } from "../../components/modal/confirmReturnModal/AdminCustomReturnAssetWithConfirm";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [record, setRecord] = useState();
    const [assignment, setAssignment] = useState();
    const [deleting, setDeleting] = useState(false);

    useEffect(() => {
        window.addEventListener("beforeunload", () => localStorage.removeItem("item"));
        window.addEventListener("click", () => localStorage.removeItem("item"));
    }, [])

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

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Manage Assignment" />
            <ListBase
                perPage={5}
                sort={{ field: "noNumber", order: "ASC" }}
                filterDefaultValues={{ states: [0, 1] }}
            >
                <h2 style={{ color: "#cf2338" }}>My Assignment</h2>

                <Datagrid
                    rowClick={!deleting ? postRowClick : (id, resource) => ""}
                    empty={
                        <p>
                            <h2>No Data found</h2>
                        </p>
                    }
                    bulkActionButtons={false}
                >
                    <TextField label="Asset Code" source="assetCode" />
                    <TextField label="Asset Name" source="assetName" />
                    <TextField label="Category" source="categoryName" />
                    <DateField label="Assigned Date" source="assignedDate" locales="en-GB" />
                    <FunctionField source="state" render={(record) => {
                        switch (record.state) {
                            case 0: {
                                return "Accepted";
                            }
                            case 1: {
                                return "Waiting For Acceptance";
                            }
                            case 2: {
                                return "Returned";
                            }
                            case 3: {
                                return "Waiting For Returning";
                            }
                        }
                    }} />
                    <ButtonGroup sx={{ border: null }}>
                        <FunctionField render={(record) => {
                            if (record.state === 1) {
                                return (
                                    <ButtonGroup>
                                        <AcceptAssignment 
                                            icon={<CheckIcon/>}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to accept this assignment?"
                                            mutationOptions={{ onSuccess: () => refresh() }} isOpen={deleting} 
                                            setDeleting={setDeleting} 
                                        />
                                        <DeclineAssignment
                                            icon={<ClearIcon />}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to decline this assignment?"
                                            mutationOptions={{ onSuccess: () => refresh() }} isOpen={deleting} setDeleting={setDeleting} />
                                        {/* <CustomDisableWithConfirm
                                            icon={<ReplayIcon style={{ color:"#bdbdbd" }} />}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to create a returning request for this asset?"
                                            mutationOptions={{ onSuccess: () => refresh() }}
                                            disabled={true}
                                            isOpen={false}
                                            setDeleting={setDeleting}
                                            record={record}
                                        /> */}
                                        <AdminCustomReturnAssetWithConfirm
                                            icon={<ReplayIcon />}
                                            confirmTitle=""
                                            confirmContent=""
                                            setDeleting={setDeleting}
                                            disabled
                                        />
                                    </ButtonGroup>

                                )
                            }
                            else {
                                return (
                                    <ButtonGroup>
                                        <EditButton icon={<CheckIcon/>} disabled variant="text" size="small" label=""
                                            sx={listStyle.buttonToolbar} />
                                        <DeleteButton icon={<ClearIcon />} disabled variant="text" size="small" label=""
                                            sx={listStyle.buttonToolbar} />
                                        {/* <CustomDisableWithConfirm
                                            icon={<ReplayIcon style={{ color: !(record.state === 0) ? "#bdbdbd" : "#5f73e4" }} />}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to create a returning request for this asset?"
                                            mutationOptions={{ onSuccess: () => refresh() }}
                                            disabled={!(record.state === 0)}
                                            isOpen={false}
                                            setDeleting={setDeleting}
                                            record={record}
                                        /> */}
                                        <AdminCustomReturnAssetWithConfirm
                                            icon={<ReplayIcon />}
                                            confirmTitle="Are you sure?"
                                            confirmContent="Do you want to create a returning request for this asset?"
                                            setDeleting={setDeleting}
                                            disabled={!(record.state === 0)}
                                        />
                                    </ButtonGroup>
                                )
                            }
                        }} />
                    </ButtonGroup>
                </Datagrid>
                <AssetsPagination />
                <AssignmentShow isOpened={isOpened} toggle={toggle} assignment={assignment} />
            </ListBase>
        </Container>
    )
};