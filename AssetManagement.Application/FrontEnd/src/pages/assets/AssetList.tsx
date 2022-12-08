import React, { useEffect, useState } from "react";
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
} from "react-admin";
import { CustomDeleteAssetWithConfirmButton } from "../../components/modal/confirmDeleteModal/CustomDeleteAssetWithConfirm";
import HighlightOffIcon from "@mui/icons-material/HighlightOff";
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import AssetShow from "./AssetShow";
import { ButtonGroup, Stack, Container } from "@mui/material";
import CategoryFilterSelect from "../../components/select/CategoryFilterSelect";
import { useNavigate } from "react-router-dom";
import FilterSearchForm from "../../components/forms/FilterSearchForm";
import { listStyle } from "../../styles/listStyle";


export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [deleting, setDeleting] = useState(false);
    const [record, setRecord] = useState();
    // const { data } = useGetList("category", { pagination: { page: 1, perPage: 99 } })
    const dataProvider = useDataProvider();
    let data = dataProvider
        .getList("category", {
            pagination: { page: 1, perPage: 99 },
            sort: { field: "name", order: "ASC" },
            filter: {},
        })
        .then((res) => res.data);

    useEffect(() => {
        window.addEventListener("beforeunload", () => localStorage.removeItem("item"));
        window.addEventListener("click", () => localStorage.removeItem("item"));
    }, [])

    const toggle = () => {
        setIsOpened(!isOpened);
    };
    const postRowClick = (id, resource, record) => {
        setRecord(record);
        toggle();
        return "";
    };

    const refresh = useRefresh();

    const assetsFilter = [
        <StateFilterSelect
            source="states"
            label="State"
            sx={{ width: "240px" }}
            statesList={[
                { value: "4", text: "Assigned" },
                { value: "0", text: "Available" },
                { value: "1", text: "Not available" },
                { value: "2", text: "Waiting for recycling" },
                { value: "3", text: "Recycled" },

            ]}
            defaultSelect={["0", "1", "4"]}
            alwaysOn
        />,
        <CategoryFilterSelect source="categories" statesList={data} alwaysOn />,
        <SearchInput
            InputLabelProps={{ shrink: false }}
            source="searchString"
            alwaysOn
        />,
    ];

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Manage Asset" />
            <ListBase
                perPage={5}
                sort={{ field: "assetCode", order: "ASC" }}
                filterDefaultValues={{ states: ["0", "1", "4"] }}
            >
                <h2 style={{ color: "#cf2338" }}>Asset List</h2>
                <Stack
                    direction="row"
                    justifyContent="end"
                    alignContent="center"
                >
                    <div style={{ flexGrow: 1 }}>
                        <FilterForm
                            style={{ justifyContent: "space-between" }}
                            filters={assetsFilter}
                        />
                    </div>
                    <div style={{ display: "flex", alignItems: "end" }}>
                        <CreateButton
                            size="large"
                            variant="contained"
                            color="secondary"
                            label="Create new asset"
                            icon={<></>}
                        />
                    </div>
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
                    <TextField source="assetCode" />
                    <TextField label="Asset Name" source="name" />
                    <TextField label="Category" source="categoryName" />
                    <FunctionField
                        source="state"
                        label="State"
                        render={(record) =>
                            record.state == "NotAvailable"
                                ? "Not available"
                                : record.state == "WaitingForRecycling"
                                    ? "Waiting for recycling"
                                    : record.state
                        }
                    />
                    <ButtonGroup sx={{ border: null }}>
                        <FunctionField render={(record) => {
                            if (record.isEditable == true) {
                                return (
                                    <EditButton variant="text" size="small" label="" sx={listStyle.buttonToolbar}/>
                                )
                            }
                            else {
                                return (
                                    <EditButton disabled variant="text" size="small" label="" sx={listStyle.buttonToolbar} />
                                )
                            }
                        }} />

                        <FunctionField render={(record) => {
                            if (!(record.state == "Assigned")) {
                                return (
                                    <CustomDeleteAssetWithConfirmButton
                                        icon={<HighlightOffIcon />}
                                        confirmTitle="Are you sure?"
                                        confirmContent="Do you want to delete this asset?"
                                        mutationOptions={{ onSuccess: (data) => refresh() }}
                                        isOpen={deleting}
                                        setDeleting={setDeleting}
                                    />
                                )
                            }
                            else {
                                return (
                                    <CustomDeleteAssetWithConfirmButton
                                        icon={<HighlightOffIcon />}
                                        confirmTitle="Are you sure?"
                                        confirmContent="Do you want to delete this asset?"
                                        mutationOptions={{ onSuccess: (data) => refresh() }}
                                        disabled={true}
                                        isOpen={deleting}
                                        setDeleting={setDeleting}
                                    />
                                )
                            }
                        }} />

                    </ButtonGroup>
                </Datagrid>
                <AssetsPagination />
            </ListBase>
            {isOpened && <AssetShow isOpened={isOpened} toggle={toggle} record={record} />}
        </Container>
    );
};
