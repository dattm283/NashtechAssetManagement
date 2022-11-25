import React, { useEffect, useState } from "react";
import {
  Datagrid,
  List,
  Pagination,
  SelectArrayInput,
  TextField,
  TextInput,
  useListContext,
  EditButton,
  useListController,
  useDataProvider,
  useGetList
} from "react-admin";
import { CustomDeleteWithConfirmButton } from "../../components/modal/confirmDeleteModal/CustomDeleteWithConfirm";
import HighlightOffIcon from "@mui/icons-material/HighlightOff";
import AssetsPagination from "../../components/pagination/AssetsPagination";
import StateFilterSelect from "../../components/select/StateFilterSelect";
import AssetShow from "./AssetShow";
import { ButtonGroup } from "@mui/material";
import CategoryFilterSelect from "../../components/select/CategoryFilterSelect";
import axios from "axios";
import axiosInstance from "../../connectionConfigs/axiosInstance";

export default () => {
  const [isOpened, setIsOpened] = useState(false);
  const [record, setRecord] = useState();
  // const { data } = useGetList("category/get", { pagination: { page: 1, perPage: 99 } })
  const dataProvider = useDataProvider();
  let data = dataProvider.getList("category/get", { pagination: { page: 1, perPage: 99 }, sort: { field: "name", order: "ASC" }, filter: {} }).then(res => res.data)

  const toggle = () => {
    setIsOpened(!isOpened);
  };
  const postRowClick = (id, resource, record) => {
    setRecord(record);
    toggle();
    return "";
  };

  const assetsFilter = [
    <TextInput label="Search" source="searchString" alwaysOn />,
    // <SelectArrayInput source="states" choices={[
    //     { id: '0', name: 'Available' },
    //     { id: '1', name: 'Not available' },
    //     { id: '2', name: 'Waiting for recycling' },
    //     { id: '3', name: 'Recyled' },
    // ]} />,
    <StateFilterSelect
      source="states"
      statesList={[
        { value: 0, text: "Available" },
        { value: 1, text: "Not Available" },
        { value: 2, text: "Waiting for recycling" },
        { value: 3, text: "Recycled" },
      ]}
    />,
    <CategoryFilterSelect
      source="categories"
      statesList={data}
    />
  ];




  return (
    <>
      <List
        perPage={5}
        pagination={<AssetsPagination />}
        filters={assetsFilter}
        exporter={false}
        sort={{ field: "name", order: "DESC" }}
      >
        <Datagrid
          rowClick={postRowClick}
          empty={
            <p>
              <h2>No Result found</h2>
            </p>
          }
          bulkActionButtons={false}
        >
          <TextField source="id" />
          <TextField source="name" />
          <TextField source="assetCode" />
          <TextField source="categoryName" />
          <TextField source="state" />
          <ButtonGroup sx={{ border: null }}>
            <EditButton variant="text" size="small" label="" />
            <CustomDeleteWithConfirmButton
              icon={<HighlightOffIcon />}
              confirmTitle="Are you sure?"
              confirmContent="Do you want to delete this asset?"
            />
          </ButtonGroup>
        </Datagrid>
      </List>
      <AssetShow isOpened={isOpened} toggle={toggle} record={record} />
    </>
  );
};
