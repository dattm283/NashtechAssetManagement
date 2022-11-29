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
    SearchInput
} from "react-admin";
import { CustomDeleteWithConfirmButton } from "../../components/modal/confirmDeleteModal/CustomDeleteWithConfirm";
import HighlightOffIcon from "@mui/icons-material/HighlightOff";
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import AssetShow from "./AssetShow";
import { ButtonGroup, Stack } from "@mui/material";
import CategoryFilterSelect from "../../components/select/CategoryFilterSelect";
import { useNavigate } from "react-router-dom";
import FilterSearchForm from "../../components/forms/FilterSearchForm";

export default () => {
    const [isOpened, setIsOpened] = useState(false);
    const [record, setRecord] = useState();
    // const { data } = useGetList("category", { pagination: { page: 1, perPage: 99 } })
    const dataProvider = useDataProvider();
    let data = dataProvider.getList("category", { pagination: { page: 1, perPage: 99 }, sort: { field: "name", order: "ASC" }, filter: {} }).then(res => res.data)

    const navigate = useNavigate();
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
            sx={{ width:"250px" }}
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




  return (
      <>
      <Title title="Manage Asset"/>
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
                    <ButtonGroup sx={{ border: null }}>
                        <EditButton variant="text" size="small" label="" />
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
            <AssetShow isOpened={isOpened} toggle={toggle} record={record} />
        </>
    );
};