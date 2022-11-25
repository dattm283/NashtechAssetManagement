import { Dialog, DialogContent, DialogTitle } from "@mui/material";
import React, { useEffect, useState } from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import styled from "styled-components";
import { SimpleShowLayout, TextField } from "react-admin";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import { getAssignementByAssetCodeId } from "../../services/assignment";

const StyledDialog = styled(Dialog)`
  .MuiBackdrop-root {
    background-color: transparent;
  }
`;

const StyledDialogTitle = styled(DialogTitle)`
  &.MuiDialogTitle-root {
    background-color: #EFF1F5;
    color: #CF2338;
  }
`;

const StyledDialogContent = styled(DialogContent)`
  &.MuiDialogContent-root {
    background-color: #FAFCFC;
  }
`;

const AssetShow = (props) => {
  const { isOpened, toggle, record } = props;
  const [assignements, setAssignments] = useState([]);
  const style = {
    bgcolor: "#cf2338",
    color: "#fff",
  };

  useEffect(() => {
    if (record) {
      getAssignementByAssetCodeId(record.id)
        .then((response) => {
          setAssignments(response);
        })
        .catch(() => {});
    }
  }, [record]);

  return (
    <StyledDialog
      open={isOpened}
      onClose={toggle}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <StyledDialogTitle id="alert-dialog-title" sx={style}>
        {"Detail Asset Information"}
        <IconButton
          aria-label="close"
          onClick={toggle}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: "#CF2338",
          }}
        >
          <CloseIcon />
        </IconButton>
      </StyledDialogTitle>
      <StyledDialogContent>
        <SimpleShowLayout record={record}>
          <TextField label="Asset Code" source="assetCode" />
          <TextField label="Asset Name" source="name" />
          <TextField label="Category" source="categoryName" />
          <TextField label="Installed Date" source="installedDate" />
          <TextField label="Available" source="state" />
          <TextField label="Location" source="location" />
          <TextField label="Specification" source="specification" />
          <TextField label="History" />
          <TableContainer component={Paper}>
            <Table aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell>Date</TableCell>
                  <TableCell align="right">Assigned to</TableCell>
                  <TableCell align="right">Assigned by</TableCell>
                  <TableCell align="right">Returned Date</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {assignements.map((row: any, index) => (
                  <TableRow
                    key={index}
                    sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                  >
                    <TableCell component="th" scope="row">
                      {row.assignedDate}
                    </TableCell>
                    <TableCell align="right">{row.assignedTo}</TableCell>
                    <TableCell align="right">{row.assignedBy}</TableCell>
                    <TableCell align="right">{row.returnedDate}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </SimpleShowLayout>
      </StyledDialogContent>
    </StyledDialog>
  );
};

export default AssetShow;
