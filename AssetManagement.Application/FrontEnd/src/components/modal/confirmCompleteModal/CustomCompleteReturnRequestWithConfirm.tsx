import React, { Fragment, ReactEventHandler, ReactElement } from 'react';
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
    useTranslate,
    RedirectionSideEffect,
} from 'ra-core';
import { Button as MUIButton, ButtonGroup } from '@mui/material';
import { Button, ButtonProps, useNotify } from 'react-admin';
import { PutComplete } from '../../../services/returningRequest';

const StyledDialogContent = styled(DialogContent)`
  &.MuiDialogContent-root {
    background-color: #fafcfc;
  }
`;

const StyledDialog = styled(Dialog)`
  .MuiBackdrop-root {
    background-color: transparent;
  }
`;

export const CustomCompleteReturnRequestWithConfirm = <RecordType extends RaRecord = any>(
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
        setComplete,
        onSuccess,
        ...rest
    } = props;
    const translate = useTranslate();
    const record = useRecordContext(props);
    const resource = useResourceContext(props);
    const notify = useNotify();

    const {
        open,
        isLoading,
        handleDialogOpen,
        handleDialogClose,
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

    const deleteButtonStyle = {
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

    const handleOpen = (e) => {
        setComplete(true);
        handleDialogOpen(e);
    } 

    const handleClose = (e) => {
        setComplete(false);
        handleDialogClose(e);
    }

    const handleComplete = (e) => {
        setComplete(false);
        handleDialogClose(e);
        PutComplete(record.id)
        .then((response) => {
            onSuccess();
        }).catch((error) => {
            notify(error.response.data.message);
        });
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
                sx={{"span": { 
                    margin: 0
                }}}
                disabled={props.disabled}
            >
                {icon}
            </StyledButton>
            <StyledDialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title" sx={titleStype}>
                    {confirmTitle}
                </DialogTitle>
                <StyledDialogContent sx={contentStyle}>
                    <DialogContentText component={"div"} id="alert-dialog-description">
                        <DialogContentText sx={{
                            padding: 3,
                        }}>
                            {confirmContent}
                        </DialogContentText>
                    </DialogContentText>
                    <DialogActions>
                        <MUIButton onClick={handleComplete} sx={deleteButtonStyle} >Yes</MUIButton>
                        <MUIButton sx={confirmButtonStyle} onClick={handleClose}>No</MUIButton>
                        <div style={{ flex: '1 0 0' }} />
                    </DialogActions>
                </StyledDialogContent>
            </StyledDialog>
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
    setComplete: Function;
    onSuccess: Function;
}

CustomCompleteReturnRequestWithConfirm.propTypes = {
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