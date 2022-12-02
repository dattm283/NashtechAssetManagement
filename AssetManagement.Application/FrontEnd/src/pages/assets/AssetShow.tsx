import {
  Box,
  Modal,
  Typography,
  IconButton,
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
import CloseRoundedIcon from "@mui/icons-material/CloseRounded";
import { getAssignementByAssetCodeId } from "../../services/assignment";

// Style for Modal
const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  m: "0px",
  package: "0px",
  transform: "translate(-50%, -50%)",
  display: "flex",
  flexDirection: "column",
  width: 650,
  bgcolor: "background.paper",
  border: "0px solid white",
  borderRadius: "10px",
  boxShadow: 24,
};

const StyledModal = styled(Modal)`
  .MuiBackdrop-root {
    background-color: transparent;
  }
`;

const AssetShow = (props) => {
  const { isOpened, toggle, record = {} } = props;
  const [assignements, setAssignments] = useState([]);

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
    <>
      <StyledModal
        open={isOpened}
        onClose={toggle}
        sx={{}}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style} border="none">
          <Box
            sx={{
              m: "0px",
              padding: "15px 0px",
              display: "flex",
              bgcolor: "#EFF1F5",
              borderTopLeftRadius: "10px",
              borderTopRightRadius: "10px",
            }}
          >
            <Box
              sx={{
                m: "auto",
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                width: "540px",
              }}
            >
              <Typography
                id="modal-modal-title"
                variant="h5"
                component="h5"
                color={theme.palette.secondary.main}
                width="280"
                fontWeight="bold"
              >
                Detailed Asset Information
              </Typography>

              <IconButton
                onClick={toggle}
                sx={{
                  p: "0px",
                  color: theme.palette.secondary.main
                }}
              >
                <CloseRoundedIcon sx={{ fontWeight: "bold" }} />
              </IconButton>
            </Box>
          </Box>

          <Box
            sx={{
              display: "flex",
              flexDirection: "row",
              m: "10px 80px",
              borderBottomLeftRadius: "10px",
              borderBottomRightRadius: "10px",
            }}
          >
            <Box flex="true" flexDirection="column">
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                Asset Code
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                Asset Name
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                Category
              </Typography>
              <Typography
                variant="subtitle1"
                sx={{ m: "20px 0px" }}
                noWrap={true}
              >
                Installed Date
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                State
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                Location
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                Specification
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                History
              </Typography>
            </Box>

            <Box flex="true" flexDirection="column">
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {record.assetCode}
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {record.name}
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {record.categoryName}
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {new Date(record.installedDate).toLocaleDateString()}
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {record.state == "NotAvailable"
                  ? "Not available"
                  : record.state == "WaitingForRecycling"
                  ? "Waiting for recycling"
                  : record.state}
              </Typography>
              <Typography variant="subtitle1" sx={{ m: "20px 50px" }}>
                {record.location == "HoChiMinh"
                  ? "HCM"
                  : record.location == "HaNoi"
                  ? "HN"
                  : record.location}
              </Typography>
              <Typography
                variant="subtitle1"
                sx={{ m: "20px 50px" }}
                noWrap={true}
              >
                {record.specification}
              </Typography>
              <TableContainer sx={{ mt: "20px", mb: "20px"  }} component={Paper}>
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
                    {Array.prototype.map.bind(assignements)((row: any, index) => (
                      <TableRow
                        key={index}
                        sx={{
                          "&:last-child td, &:last-child th": { border: 0 },
                        }}
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
            </Box>
          </Box>
        </Box>
      </StyledModal>
    </>
  );
};

export default AssetShow;
