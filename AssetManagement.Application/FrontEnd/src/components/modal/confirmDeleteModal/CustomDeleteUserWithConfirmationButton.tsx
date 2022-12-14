import React, { Fragment, ReactEventHandler, ReactElement, useState } from 'react';
import { styled } from '@mui/material/styles';
import PropTypes from 'prop-types';
import ActionDelete from '@mui/icons-material/Delete';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import clsx from 'clsx';
import { UseMutationOptions } from 'react-query';
import {
    MutationMode,
    RaRecord,
    DeleteParams,
    useDeleteWithConfirmController,
    useRecordContext,
    useResourceContext,
    RedirectionSideEffect,
} from 'ra-core';
import { Button as MUIButton } from '@mui/material';
import { Button, ButtonProps } from 'react-admin';
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";

export const CustomDeleteUserWithConfirmButton = <RecordType extends RaRecord = any>(
    props: DeleteWithConfirmButtonProps<RecordType>
) => {
    const {
        className,
        confirmTitle = 'ra.message.delete_title',
        confirmContent = 'ra.message.delete_content',
        icon = defaultIcon,
        // label = 'ra.action.delete',
        label = "",
        mutationMode = 'pessimistic',
        onClick,
        redirect = 'list',
        translateOptions = {},
        mutationOptions,
        isOpen,
        setDeleting,
        ...rest
    } = props;
    const record = useRecordContext(props);
    const resource = useResourceContext(props);
    const [assignementsCount, setAssignementsCount] = useState({});
    function getCount() {
        if (record) {
            setAssignementsCount(record.validAssignments);
        }
    }
    const {
        open,
        handleDialogOpen,
        handleDialogClose,
        handleDelete,
    } = useDeleteWithConfirmController({
        record,
        redirect,
        mutationMode,
        onClick,
        mutationOptions,
        resource,
    });

    const titleStype = {
        bgcolor: '#F0EBEB',
        color: "#E80E0E",
        // borderRadius: "1px 1px 0px 0px"
        borderTopLeftRadius: "4px",
        borderTopRightRadius: "4px",
        fontWeight: "bold"
    }

    const contentStyle = {
        borderBottomLeftRadius: '4px',
        borderBottomRightRadius: '4px',
        color: "#000"
    }

    const deleteButtonStyle = {
        bgcolor: "#E80E0E",
        color: "#FFFFFF",
        border: "1px solid #E80E0E",
        borderRadius: 1,

        "&:hover": {
            color: "#fff",
            bgcolor: "#424242",
            border: "1px solid #424242",
        },
    }

    const confirmButtonStyle = {
        bgcolor: "#F0EBEB",
        color: "#000",
        border: "1px solid #424242",
        borderRadius: 1,
        "&:hover": {
            color: "#fff",
            bgcolor: "#424242",
            border: "1px solid #424242",
        },
    }

    const handleOpen = (e) => {
        getCount();
        setDeleting(true);
        handleDialogOpen(e);
    }

    const handleClose = (e) => {
        setDeleting(false);
        handleDialogClose(e);
    }

    const customHandleDelete = (e) => {
        setDeleting(false);
        handleDelete(e);
    }

    return (
        <Fragment>
            <StyledButton
                variant="text"
                onClick={handleOpen}
                label={label}
                className={clsx('ra-delete-button', className)}
                key="button"
                {...rest}
                sx={{
                    "span": {
                        margin: 0
                    }
                }}
                disabled={props.disabled}
            >
                {icon}
            </StyledButton>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title" sx={titleStype}>
                    {!(assignementsCount > 0) ? confirmTitle :
                        <>Can not disable user <IconButton
                            aria-label="close"
                            onClick={handleClose}
                            sx={{
                                position: "absolute",
                                right: 8,
                                top: 8,
                                color: "#CF2338",
                            }}
                        >
                            <CloseIcon />
                        </IconButton></>}
                </DialogTitle>
                <DialogContent sx={contentStyle}>
                    {!(assignementsCount > 0) ?
                        <>
                            <DialogContentText component={"div"} id="alert-dialog-description">
                                <DialogContentText sx={{
                                    padding: 3,
                                    paddingRight: 20
                                }}>
                                    {confirmContent}
                                </DialogContentText>
                            </DialogContentText>
                            <DialogActions>
                                <MUIButton onClick={customHandleDelete} sx={deleteButtonStyle} >Disable</MUIButton>
                                <MUIButton sx={confirmButtonStyle} onClick={handleClose}>Cancel</MUIButton>
                                <div style={{ flex: '1 0 0' }} />
                            </DialogActions>
                        </> : <>
                            <DialogContentText component={"div"} id="alert-dialog-description">
                                <DialogContentText sx={{
                                    padding: 3
                                }}>
                                    There are valid assignments belonging to this user.
                                    <br />
                                    Please close all assignments before disabling user.
                                </DialogContentText>
                            </DialogContentText>
                        </>
                    }
                </DialogContent>
            </Dialog>
        </Fragment>
    );
};

const defaultIcon = <ActionDelete />;

export interface DeleteWithConfirmButtonProps<
    RecordType extends RaRecord = any,
    MutationOptionsError = unknown
> extends ButtonProps {
    confirmTitle?: string;
    confirmContent?: React.ReactNode;
    icon?: ReactElement;
    mutationMode?: MutationMode;
    onClick?: ReactEventHandler<any>;
    // May be injected by Toolbar - sanitized in Button
    translateOptions?: object;
    mutationOptions?: UseMutationOptions<
        RecordType,
        MutationOptionsError,
        DeleteParams<RecordType>
    >;
    record?: RecordType;
    redirect?: RedirectionSideEffect;
    resource?: string;
    isOpen: boolean;
    setDeleting: Function;
}

CustomDeleteUserWithConfirmButton.propTypes = {
    className: PropTypes.string,
    confirmTitle: PropTypes.string,
    confirmContent: PropTypes.string,
    label: PropTypes.string,
    mutationMode: PropTypes.oneOf(['pessimistic', 'optimistic', 'undoable']),
    record: PropTypes.any,
    redirect: PropTypes.oneOfType([
        PropTypes.string,
        PropTypes.bool,
        PropTypes.func,
    ]),
    resource: PropTypes.string,
    icon: PropTypes.element,
    translateOptions: PropTypes.object,
};

const PREFIX = 'RaDeleteWithConfirmButton';

const StyledButton = styled(Button, {
    name: PREFIX,
    overridesResolver: (styles) => styles.root,
})(({ theme }) => ({
    color: theme.palette.error.main
}));