import {
Box,
Modal,
Typography,
IconButton,
} from "@mui/material";
import React, { useEffect, useState } from "react";
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

function isDateAndRestyle(sentence) {
    if (sentence === null){
        return "Unknown"
    }
    if (typeof sentence === 'string'){
        var parsedDate = Date.parse(sentence);
        if (!isNaN(parsedDate)) {
            var [yyyy, mm, dd] = sentence.split("T")[0].split("-")
            return `${dd}/${mm}/${yyyy}`;
        }
    }
    return sentence
}

const UserShow = ({ openDetail, setOpenDetail, label }) => {
    return (
        <>
            <StyledModal
                open={openDetail.status}
                onClose={() => setOpenDetail({ status:false, data:{} })}
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
                            m: "auto 80px",
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
                            Detailed User Information
                        </Typography>

                        <IconButton
                            onClick={(e) => setOpenDetail({ status:false, data:{} })}
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
                                Staff Code
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                                Full Name
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                                User Name
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }} noWrap={true}>
                                Date of Birth
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                                Gender
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }} noWrap={true}>
                                Joined Date
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                                Type
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 0px" }}>
                                Location
                            </Typography>
                        </Box>

                        <Box flex="true" flexDirection="column">
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["staffCode"]}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["fullName"]}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["userName"]}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {isDateAndRestyle(openDetail.data["dateOfBirth"])}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["gender"]}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {isDateAndRestyle(openDetail.data["joinedDate"])}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["type"]}
                            </Typography>
                            <Typography variant="subtitle1" sx={{ m: "20px 60px" }}>
                                {openDetail.data["location"] == "HoChiMinh" ? "HCM" : openDetail.data["location"] == "HaNoi" ? "HN" : openDetail.data["location"]}
                            </Typography>
                        </Box>
                    </Box>
                </Box>
            </StyledModal>
        </>
    );
};

export default UserShow;
