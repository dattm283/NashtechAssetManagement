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
import * as XLSX from 'xlsx-js-style';

const exporter = posts => {
    const worksheet = XLSX.utils.json_to_sheet(posts);
    // Create new Worksheet (Formating worksheet)
    const newWorksheet:XLSX.WorkSheet = {};
    var columns:string[] = ["A", "B", "C", "D", "E", "F", "G", "H"]
    var newColumns:string[] = ["C", "D", "E", "F", "G", "H", "I", "J"]
    var rows:string[] = []
    var newRows:string[] = []
    for (let i=0; i<posts.length+1; i++){
        rows.push(`${i+1}`);
        newRows.push(`${i+3}`)
    }
    for (let C=0; C<columns.length; C++){
        for (let R=0; R<rows.length; R++){
            newWorksheet[`${newColumns[C]}${newRows[R]}`] = {...worksheet[`${columns[C]}${rows[R]}`]}
            newWorksheet[`${newColumns[C]}${newRows[R]}`].s = {
                border: {
                    top: { style: "dotted", color: "000" },
                    bottom: { style: "dotted", color: "000" },
                    left: { style: "dotted", color: "000" },
                    right: { style: "dotted", color: "000" }
                }
            }
        }
    }
    XLSX.utils.sheet_add_aoa(newWorksheet, 
        [["ID", "Category", "Total", "Assigned", "Available", "Not Available", "Waiting For Recycling", "Recycled"]], 
        { origin: "C3" }
    );
    for (var i=0; i<posts.length; i++){
        // Because skipHeading is False (Default value when using json_to_sheet)
        newWorksheet[`C${i+3+1}`] = { 
            t:'n',
            v:`${i}` ,
            s: {
                border: {
                    top: { style: "dotted", color: "000" },
                    bottom: { style: "dotted", color: "000" },
                    left: { style: "dotted", color: "000" },
                    right: { style: "dotted", color: "000" }
                }
            }
        }
    }

    for (let i=0; i<newColumns.length; i++){
        newWorksheet[`${newColumns[i]}3`].s = { 
            font: { 
                name: "Calibri",
                bold: true,
                italic: true,
                sz: "12",
                underline: false,
                strike: false,
                color: { 
                    rgb: "115c04"
                } 
            },
            fill: {
                patternType: "solid",
                fgColor: { rgb: "E9E9E9" }
            },
            border: {
                top: { style: "dotted", color: "000" },
                bottom: { style: "dotted", color: "000" },
                left: { style: "dotted", color: "000" },
                right: { style: "dotted", color: "000" }
            }
        }
    }

    newWorksheet["!ref"] = `C2:J${posts.length+3}`;
    newWorksheet["!merges"] = [{ s:{c:2,r:1}, e:{c:9,r:1} }];
    newWorksheet["C2"] = {
        t:'s',
        v:'Styled Report',
        s: {
            font: { 
                name: "Calibri",
                bold: true,
                italic: true,
                sz: "18",
                underline: false,
                strike: false,
                color: { 
                    rgb: "781818"
                } 
            },
            alignment: {
                vertical: "center",
                horizontal: "center",
                wrapText: true,
                textRotation: 0
            }
        }
    }
    newWorksheet["!cols"] = [
        {wch: 0},
        {wch: 0},
        {wch: 4},
        {wch: 20},
        {wch: 7},
        {wch: 11},
        {wch: 10},
        {wch: 15},
        {wch: 23},
        {wch: 10}
    ];;
    newWorksheet["!rows"] = [
        {hpt: 0},
        {hpt: 24}
    ];
    newWorksheet["!margins"]={left:0.25,right:0.25,top:0.75,bottom:0.75,header:0.3,footer:0.3};
    
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, newWorksheet, "Styled Report");
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