import React from "react";
import {
    Datagrid,
    Title,
    TextField,
    ListBase,
    ExportButton,
    downloadCSV
} from "react-admin";
import { Stack, Container } from "@mui/material";
import AssetsPagination from "../../components/pagination/AssetsPagination";
// import jsonExport from 'jsonexport/dist';
import * as XLSX from 'xlsx';

const exporter = posts => {
    const worksheet = XLSX.utils.json_to_sheet(posts);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Report");
    XLSX.writeFile(workbook, "Report.xlsx");
};

export default () => {

    return (
        <Container component="main" sx={{ padding: "20px 10px" }}>
            <Title title="Report" />
            <ListBase
                sort={{ field: "category", order: "ASC" }}
            >
                <h2 style={{ color: "#cf2338" }}>Report</h2>
                <Stack direction="row" justifyContent="end" alignContent="center">
                    <div style={{ display: "flex", alignItems: "end" }}>
                        <ExportButton
                            exporter={exporter}
                            size="large"
                            variant="contained"
                            color="secondary"
                            label="Export"
                            icon={<></>}
                        />
                    </div>
                </Stack>

                <Datagrid
                    empty={<h2>No Category found</h2>}
                    bulkActionButtons={false}
                >
                    <TextField label="Category" source="category" />
                    <TextField label="Total" source="total" />
                    <TextField label="Assigned" source="assigned" />
                    <TextField label="Available" source="available" />
                    <TextField label="Not available" source="notAvailable" />
                    <TextField label="Waiting for recycling" source="waitingForRecycling" />
                    <TextField label="Recycled" source="recycled" />
                </Datagrid>
            </ListBase>
        </Container>
    );
};