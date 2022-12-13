import React, { Fragment, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { styled } from '@mui/material/styles';
import { Button as MUIButton } from '@mui/material';
import { Button, useRefresh, useNotify } from 'react-admin';
import * as ReturnRequestService from '../../../services/returningRequest'

const titleStype = {
    bgcolor: '#F0EBEB',
    color: "#E80E0E",
    border: "1px solid #000",
    // borderRadius: "1px 1px 0px 0px"
    borderTopLeftRadius: "4px",
    borderTopRightRadius: "4px",
    fontWeight: "bold"
}

const contentStyle = {
    border: "1px solid #000",
    borderBottomLeftRadius: '4px',
    borderBottomRightRadius: '4px',
    color: "#000"
}

const returnButtonStyle = {
    bgcolor: "#E80E0E",
    color: "#FFFFFF",
    border: "1px solid #000",
    borderRadius: 1,

    "&:hover": {
        color: "#000"
    },
}

const confirmButtonStyle = {
    bgcolor: "#F0EBEB",
    color: "#000",
    border: "1px solid #000",
    borderRadius: 1,
}


function CustomDisableWithConfirm(props) {
    const {
        icon,
        confirmTitle,
        confirmContent,
        mutationOptions,
        disabled,
        isOpen,
        setDeleting,
        record,
        ...rest
    } = props;

    const [open, setOpen] = useState(isOpen)
    const refresh = useRefresh()
    const notify = useNotify();

    return (
        <Fragment>
            <StyledButton
                variant="text"
                onClick={(e) => {
                    e.stopPropagation()
                    setDeleting(true);
                    setOpen(true);
                }}
                sx={{
                    "span": {
                        margin: 0
                    }
                }}
                key="button"
                className='ra-return-button'
                disabled={disabled}
                {...rest}
            >
                {icon}
            </StyledButton>
            <Dialog
                open={open}
                onClose={(e) => {
                    setDeleting(false);
                    setOpen(false);
                }}
                sx={{
                    "& .MuiBackdrop-root": {
                        backgroundColor: "transparent"
                    },
                    "& .css-1t1j96h-MuiPaper-root-MuiDialog-paper": {
                        boxShadow: "none"
                    }
                }}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title" sx={titleStype}>
                    {confirmTitle}
                </DialogTitle>
                <DialogContent sx={contentStyle}>
                    <DialogContentText component={"div"} id="alert-dialog-description">
                        <DialogContentText sx={{
                            padding: 3
                        }}>
                            {confirmContent}
                        </DialogContentText>
                    </DialogContentText>
                    <DialogActions>
                        <MUIButton 
                            onClick={async (e) => {
                                e.stopPropagation();
                                await ReturnRequestService.CreateReturnRequest(record.id);
                                notify("Create Returning request successfully!");
                                setDeleting(false);
                                setOpen(false);
                                refresh();
                            }} 
                            sx={returnButtonStyle} 
                        >Yes</MUIButton>
                        <MUIButton 
                            sx={confirmButtonStyle} 
                            onClick={(e) => {
                                setDeleting(false);
                                setOpen(false);
                            }}
                        >No</MUIButton>
                        <div style={{ flex: '1 0 0' }} />
                    </DialogActions>
                </DialogContent>
            </Dialog>
        </Fragment>
    )
}


const StyledButton = styled(Button, {
    name: 'RaReturnWithConfirmButton',
    overridesResolver: (props, styles) => styles.root,
})(({ theme }) => ({
    color: theme.palette.error.main,
    // '&:hover': {
    //     backgroundColor: alpha(theme.palette.error.main, 0.12),
    //     // Reset on mouse devices
    //     '@media (hover: none)': {
    //         backgroundColor: 'transparent',
    //     },
    // },
}));

export default CustomDisableWithConfirm;