import * as React from "react";
import { Fragment, useState, ReactElement } from "react";
import PropTypes from "prop-types";
import ActionUpdate from "@mui/icons-material/Update";
import { alpha, styled } from "@mui/material/styles";
import {
  useTranslate,
  useRefresh,
  useNotify,
  useResourceContext,
  MutationMode,
  useDataProvider,
  useRecordContext,
} from "ra-core";
import { Button, ButtonProps } from "react-admin";
import { BulkActionProps } from "react-admin";
import { useMutation } from "react-query";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import { Button as MUIButton } from "@mui/material";

export const AdminCustomReturnAssetWithConfirm = (
  props: AdminCustomReturnAssetWithConfirm
) => {
  const notify = useNotify();
  const refresh = useRefresh();
  const translate = useTranslate();
  const resource = useResourceContext(props);
  const [returning, setReturning] = useState(false);
  const { id } = useRecordContext(props);

  const {
    confirmTitle = "ra.message.bulk_update_title",
    confirmContent = "ra.message.bulk_update_content",
    data,
    icon = defaultIcon,
    label = "ra.action.update",
    mutationMode = "pessimistic",
    onClick,
    onSuccess = () => {
      refresh();
      setReturning(false);
    },
    onError = (error: Error | string) => {
      notify(
        typeof error === "string"
          ? error
          : error.message || "ra.notification.http_error",
        {
          type: "warning",
          messageArgs: {
            _:
              typeof error === "string"
                ? error
                : error && error.message
                ? error.message
                : undefined,
          },
        }
      );
      setReturning(false);
    },
    ...rest
  } = props;

  const dataProvider = useDataProvider();
  const { mutate, isLoading } = useMutation(
    ["ReturnRequest", "update", { id: id }],
    () => dataProvider.returnAsset("ReturnRequest", { id: id }),
    {
      onSuccess,
      onError,
    }
  );

  const StyledDialog = styled(Dialog)`
    .MuiBackdrop-root {
      background-color: transparent;
    }
    .css-1t1j96h-MuiPaper-root-MuiDialog-paper {
      border-radius: 10px;
      box-shadow: none;
    }
  `;

  const titleStyle = {
    bgcolor: "#EFF1F5",
    color: "#E80E0E",
    border: "1px solid #5D5F62",
    borderBottom: "0.5px solid #ADAFB3 !important",
    borderTopLeftRadius: "10px",
    borderTopRightRadius: "10px",
    fontWeight: "bold",
  };

  const contentStyle = {
    border: "1px solid #5D5F62",
    borderTop: "0.5px solid #ADAFB3 !important",
    borderBottomLeftRadius: "10px",
    borderBottomRightRadius: "10px",
    color: "#000",
  };

  const deleteButtonStyle = {
    bgcolor: "#E80E0E",
    color: "#FFFFFF",
    border: "1px solid lightgray",
    borderRadius: "5px",

    "&:hover": {
      color: "#000",
    },
  };

  const confirmButtonStyle = {
    bgcolor: "#F0EBEB",
    color: "#000",
    border: "1px solid lightgray",
    borderRadius: "5px",
  };

  const handleClick = (e) => {
    setReturning(true);
    props.setDeleting(true);
    e.stopPropagation();
  };

  const handleUpdate = (e) => {
    mutate();
    e.stopPropagation();
    props.setDeleting(false);
    //setReturning(true);
    if (typeof onClick === "function") {
      onClick(e);
    }
  };
  const handleClose = (e) => {
    setReturning(false);
    props.setDeleting(false);
    e.stopPropagation();
  };

  return (
    <Fragment>
      <StyledButton
        onClick={handleClick}
        variant="text"
        key="button"
        {...rest}
        sx={{
          span: {
            margin: 0,
          },
          color: "blue",
        }}
        disabled={props.disabled}
        {...sanitizeRestProps(rest)}
      >
        {icon}
      </StyledButton>
      <StyledDialog
        id={id.toString()}
        open={returning}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        onClick={(e) => e.stopPropagation()}
      >
        <DialogTitle id="alert-dialog-title" sx={titleStyle}>
          {confirmTitle}
        </DialogTitle>
        <DialogContent sx={contentStyle}>
          <DialogContentText component={"div"} id="alert-dialog-description">
            <DialogContentText
              sx={{
                padding: 3,
                paddingRight: 2,
              }}
            >
              {confirmContent}
            </DialogContentText>
          </DialogContentText>
          <DialogActions>
            <MUIButton onClick={handleUpdate} sx={deleteButtonStyle}>
              Yes
            </MUIButton>
            <MUIButton sx={confirmButtonStyle} onClick={handleClose}>
              No
            </MUIButton>
            <div style={{ flex: "1 0 0" }} />
          </DialogActions>
        </DialogContent>
      </StyledDialog>
    </Fragment>
  );
};

const sanitizeRestProps = ({
  filterValues,
  label,
  onSuccess,
  onError,
  ...rest
}: Omit<AdminCustomReturnAssetWithConfirm, "resource" | "id" | "icon" | "data">) =>
  rest;

export interface AdminCustomReturnAssetWithConfirm
  extends BulkActionProps,
    ButtonProps {
  confirmContent?: React.ReactNode;
  confirmTitle?: string;
  icon?: ReactElement;
  data: any;
  onSuccess?: () => void;
  onError?: (error: any) => void;
  mutationMode?: MutationMode;
  setDeleting: React.Dispatch<React.SetStateAction<boolean>>;
}

AdminCustomReturnAssetWithConfirm.propTypes = {
  confirmTitle: PropTypes.string,
  confirmContent: PropTypes.string,
  label: PropTypes.string,
  resource: PropTypes.string,
  id: PropTypes.arrayOf(PropTypes.any),
  icon: PropTypes.element,
  data: PropTypes.any.isRequired,
  mutationMode: PropTypes.oneOf(["pessimistic", "optimistic", "undoable"]),
};

const PREFIX = "RaBulkUpdateWithConfirmButton";

const StyledButton = styled(Button, {
  name: PREFIX,
  overridesResolver: (props, styles) => styles.root,
})(({ theme }) => ({
  color: theme.palette.primary.main,
  "&:hover": {
    backgroundColor: alpha(theme.palette.primary.main, 0.12),
    // Reset on mouse devices
    "@media (hover: none)": {
      backgroundColor: "transparent",
    },
  },
}));

const defaultIcon = <ActionUpdate />;
