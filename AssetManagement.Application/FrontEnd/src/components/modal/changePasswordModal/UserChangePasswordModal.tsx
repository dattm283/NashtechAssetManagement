import React, { useEffect, useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  Button,
  DialogContentText,
  FormHelperText,
  styled,
} from "@mui/material";
import {
  Form,
  PasswordInput,
  required,
  SaveButton,
  useNotify,
} from "react-admin";
import userService from "../../../services/users";
import { red } from "@mui/material/colors";
import { ImportantDevices } from "@mui/icons-material";

const UserChangePasswordModal = ({ stateChanger, ...rest }) => {
  const [modalState, setModalState] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);
  const notify = useNotify();

  const requiredInput = (values) => {
    let errors = {
      currentPassword: "",
      newPassword: "",
    };
    if (!values.newPassword || !values.currentPassword) {
      if (!values.currentPassword && !values.newPassword) {
        errors.currentPassword = "This is required";
        errors.newPassword = "This is required";
      }
      if (!values.currentPassword) errors.newPassword = "This is required";
      else if (!values.newPassword) errors.currentPassword = "This is required";
      else errors.newPassword = error;
    } else {
      if (values.currentPassword.length < 6) {
        errors.currentPassword = "Must be more than 6 leters";
      }
      if (values.newPassword.length < 6) {
        errors.newPassword = "Must be more than 6 leters";
      } else return {};
    }
    return errors;
  };

  const handleChangePassword = (data) => {
    const newPassword = data.newPassword;
    const currentPassword = data.currentPassword;
    console.log(newPassword, currentPassword);

    const changePasswordRequest = {
      newPassword,
      currentPassword,
    };

    userService
      .changePassword(changePasswordRequest)
      .then(
        () => {
          setSuccess(true);
        },
        (err) => {
          console.log(err.response.data.message);
          setError(err.response.data.message);
        }
      )
      .catch((err) => {
        console.log(err);
        setError(err.response.data.message);
      });
  };

  const style = {
    bgcolor: "#cf2338",
    color: "#fff",
  };

  const buttonSaveStyle = {
    bgcolor: "#cf2338",
    color: "#fff",
    marginRight: 3,
  };
  if (!success)
    return (
      <Dialog open={stateChanger}>
        <DialogTitle id="alert-dialog-title" sx={style}>
          {"Change Password"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText
            component={"div"}
            id="alert-dialog-description"
            sx={{ width: "410px" }}
          >
            <br />
            <Form
              onSubmit={handleChangePassword}
              validate={requiredInput}
              {...(error && { error: true })}
            >
              <Grid container alignItems="center">
                <Grid item>
                  <label style={{ marginRight: "37px" }}>Old password:</label>
                  <PasswordInput
                    label={false}
                    source="currentPassword"
                    helperText={error ? error : ""}
                  />
                </Grid>
                <Grid item>
                  <label style={{ marginRight: "30px" }}>New password:</label>
                  <PasswordInput label={false} source="newPassword" />
                </Grid>
                <Grid item justifyContent="flex-end" marginLeft="59%">
                  <SaveButton
                    label="Save"
                    sx={buttonSaveStyle}
                    type="submit"
                    icon={<></>}
                  />
                  <Button
                    type="button"
                    onClick={() => stateChanger(false)}
                    sx={{ border: "1px solid lightgray" }}
                  >
                    Cancel
                  </Button>
                </Grid>
              </Grid>
            </Form>
          </DialogContentText>
        </DialogContent>
      </Dialog>
    );
  else {
    return (
      <Dialog
        open={success}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title" sx={style}>
          {"Change Password"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText component={"div"} id="alert-dialog-description">
            <DialogContentText>
              <br />
              Your password has been changed successfully
            </DialogContentText>
          </DialogContentText>
          <DialogContent>
            <DialogContentText
              component={"div"}
              id="alert-dialog-description"
              marginLeft="80%"
            >
              <Button
                type="button"
                onClick={() => {
                  stateChanger(false);
                  setSuccess(false);
                }}
                sx={{ border: "1px solid lightgray" }}
              >
                Close
              </Button>
            </DialogContentText>
          </DialogContent>
        </DialogContent>
      </Dialog>
    );
  }
};

export default UserChangePasswordModal;
