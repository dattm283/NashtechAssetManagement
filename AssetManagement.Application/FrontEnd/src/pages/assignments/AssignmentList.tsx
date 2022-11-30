import React, { useState } from "react";
import {
    Datagrid,
    List,
    Title,
    TextField,
    TextInput,
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
// import AssetShow from "./AssetShow";
import { ButtonGroup, Stack } from "@mui/material";
import CategoryFilterSelect from "../../components/select/CategoryFilterSelect";
import { useNavigate } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import FilterSearchForm from "../../components/forms/FilterSearchForm";
import AssignmentShow from "./AssignmentShow";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [assignment, setAssignment] = useState();
    // const { data } = useGetList("category", { pagination: { page: 1, perPage: 99 } })
    const dataProvider = useDataProvider();
    let data = dataProvider.getList("category", { pagination: { page: 1, perPage: 99 }, sort: { field: "name", order: "ASC" }, filter: {} }).then(res => res.data)

    const navigate = useNavigate();
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

    const assetsFilter = [
        // <SelectArrayInput source="states" choices={[
        //     { id: '0', name: 'Available' },
        //     { id: '1', name: 'Not available' },
        //     { id: '2', name: 'Waiting for recycling' },
        //     { id: '3', name: 'Recyled' },
        // ]} />,
        <StateFilterSelect
            source="states"
            sx={{ width: "250px" }}
            statesList={[
                { value: 0, text: "Available" },
                { value: 1, text: "Not Available" },
                { value: 2, text: "Waiting for recycling" },
                { value: 3, text: "Recycled" },
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


    return (
        <>
            <Title title="Manage Assignment" />
            <ListBase
                perPage={5}
                sort={{ field: "id", order: "DESC" }}
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
                    <TextField label="Asset Code" source="assetCode" />
                    <TextField label="Asset Name" source="assetName" />
                    <TextField label="Assigned to" source="assignedTo" />
                    <TextField label="Assigned by" source="assignedBy" />
                    <TextField label="Assigned Date" source="assignedDate" />
                    <FunctionField source="state" render={(record) => record.state === 0 ? "Accepted" : "Waiting For Acceptance"} />
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
        </>
    );
};