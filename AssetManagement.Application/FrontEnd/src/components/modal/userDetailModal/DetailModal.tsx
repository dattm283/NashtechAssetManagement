import React from "react";
import CloseRoundedIcon from '@mui/icons-material/CloseRounded';
import { Box, Typography, Modal, IconButton } from '@mui/material';
import { theme } from '../../../theme'

// Style for Modal
const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    m: "0px",
    package: "0px",
    transform: 'translate(-50%, -50%)',
    display:'flex', flexDirection:'column',
    width: 600,
    bgcolor: 'background.paper',
    border: '0px solid white',
    borderRadius: '10px',
    boxShadow: 24,
};

// Capitalize string
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

// Params: openDetail:object, setOpenDetail:function, label:string, data:object
function DetailModal({ openDetail, setOpenDetail, label }) {
    return (
        <Modal
            open={openDetail.status}
            onClose={() => setOpenDetail({ status:false, data:{} })}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Box sx={style} border='none'>
                <Box 
                    sx={{ 
                        m:"0px", 
                        padding:"15px 0px", 
                        display:"flex", 
                        bgcolor:"lightgray", 
                        borderBottom:"1px solid black",
                        borderTopLeftRadius:"10px",
                        borderTopRightRadius:"10px"
                    }}
                >
                    <Box 
                        sx={{
                            m:"auto",
                            display: "flex",
                            flexDirection: "row",
                            justifyContent: "space-between",
                            width: "440px",
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
                            {label}
                        </Typography>

                        <IconButton
                            onClick={(e) => setOpenDetail({ status:false, data:{} })}
                            sx={{
                                p:'0px',
                                color:theme.palette.secondary.main,
                                border:`2px solid ${theme.palette.secondary.main}`,
                                borderRadius:"3px",
                            }}
                        >
                            <CloseRoundedIcon sx={{ fontWeight:"bold" }}  />
                        </IconButton>
                    </Box>
                </Box>

                <Box 
                    sx={{ 
                        display:"flex", 
                        flexDirection:"row", 
                        m:"10px 80px",
                        borderBottomLeftRadius:"10px",
                        borderBottomRightRadius:"10px"
                    }}
                >
                    <Box flex="true" flexDirection="column" >
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Staff Code</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Full Name</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>User Name</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Date of Birth</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Gender</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Joined Date</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Type</Typography>
                        <Typography variant="h6" sx={{ m:"10px 0px"}}>Location</Typography>
                    </Box>
                    
                    <Box flex="true" flexDirection="column" >
                        {Array.prototype.map.bind(Object.values(openDetail.data))((item, index) => {
                            return (
                                <Typography key={index} variant="h6" sx={{ m:"10px 50px"}}>{isDateAndRestyle(item)}</Typography>
                            )
                        })}
                    </Box>
                </Box>
            </Box>
        </Modal>
    )
}

export default DetailModal