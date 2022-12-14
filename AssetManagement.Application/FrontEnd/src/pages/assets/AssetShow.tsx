import {
  IconButton,
  Grid,
  Dialog,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import styled from "styled-components";
import { theme } from "../../theme";
import { getAssignementByAssetCodeId } from "../../services/assignment";
import {
  SimpleShowLayout,
  TextField,
  FunctionField,
  DateField,
} from "react-admin";
import DataLabel from "../../components/grid/DataLabel";
import DataText from "../../components/grid/DataText";
import CloseIcon from "@mui/icons-material/Close";

const StyledDialog = styled(Dialog)`
  .MuiBackdrop-root {
    background-color: transparent;
  }
`;

const StyledDialogTitle = styled(DialogTitle)`
  &.MuiDialogTitle-root {
    padding: 16px 38px;
    background-color: #eff1f5;
    color: #cf2338;
  }
`;

const StyledDialogContent = styled(DialogContent)`
  &.MuiDialogContent-root {
    background-color: #fafcfc;
  }
`;

const AssetShow = (props) => {
  const { isOpened, toggle, record = {} } = props;
  const [assignements, setAssignments] = useState([]);

  const style = {
    bgcolor: "#cf2338",
    fontWeight: "bold",
    color: "#fff",
  };

  useEffect(() => {
    if (record) {
      getAssignementByAssetCodeId(record.id)
        .then((response) => {
          setAssignments(response);
        })
        .catch(() => { });
    }
  }, [record]);

  function isDateAndRestyle(sentence) {
    if (sentence === null) {
      return "";
    }
    if (typeof sentence === "string") {
      var parsedDate = Date.parse(sentence);
      if (!isNaN(parsedDate)) {
        var [mm, dd, yyyy] = sentence.split("T")[0].split("/");
        console.log(`${dd}/${mm}/${yyyy}`);
        return `${dd}/${mm}/${yyyy}`;
      }
    }
    return sentence;
  }

  return (
    <>
      <StyledDialog
        maxWidth="md"
        open={isOpened}
        onClose={toggle}
        scroll="body"
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
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
            <Grid container spacing={2}>
              <DataLabel label="Asset Code" />
              <DataText>
                <TextField source="assetCode" />
              </DataText>
              <DataLabel label="Asset Name" />
              <DataText>
                <TextField source="name" />
              </DataText>
              <DataLabel label="Category" />
              <DataText>
                <TextField source="categoryName" />
              </DataText>
              <DataLabel label="Installed Date" />
              <DataText>
                <DateField source="installedDate" locales="en-GB" />
              </DataText>
              <DataLabel label="State" />
              <DataText>
                <FunctionField
                  render={(record) =>
                    record.state == "NotAvailable"
                      ? "Not available"
                      : record.state == "WaitingForRecycling"
                        ? "Waiting for recycling"
                        : record.state
                  }
                />
              </DataText>
              <DataLabel label="Location" />
              <DataText>
                <DataText>
                  <FunctionField
                    render={(record) =>
                      record.location == "HoChiMinh"
                        ? "HCM"
                        : record.location == "HaNoi"
                          ? "HN"
                          : record.location
                    }
                  />
                </DataText>
              </DataText>
              <DataLabel label="Specification" />
              <DataText>
                <TextField source="specification" />
              </DataText>
              <DataLabel label="History" />
              <DataText>
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
                      {Array.prototype.map.bind(assignements)(
                        (row: any, index) => (
                          <TableRow
                            key={index}
                            sx={{
                              "&:last-child td, &:last-child th": { border: 0 },
                            }}
                          >
                            <TableCell component="th" scope="row">
                              {isDateAndRestyle(row.assignedDate)}
                            </TableCell>
                            <TableCell align="right">
                              {row.assignedTo}
                            </TableCell>
                            <TableCell align="right">
                              {row.assignedBy}
                            </TableCell>
                            <TableCell align="right">
                              {isDateAndRestyle(row.returnedDate)}
                            </TableCell>
                          </TableRow>
                        )
                      )}
                    </TableBody>
                  </Table>
                </TableContainer>
              </DataText>
            </Grid>
          </SimpleShowLayout>
        </StyledDialogContent>
      </StyledDialog>
    </>
  );
};

export default AssetShow;
