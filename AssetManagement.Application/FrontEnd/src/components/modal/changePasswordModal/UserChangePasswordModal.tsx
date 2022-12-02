import React, { useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  Button,
  DialogContentText,
} from "@mui/material";
import { Form, PasswordInput, SaveButton, useNotify } from "react-admin";
import userService from "../../../services/users";

const UserChangePasswordModal = ({ stateChanger, ...rest }) => {
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

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
    if (error) errors.currentPassword = error;
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
    bgcolor: "#EFF0F4",
    color: "#CE2339",
    borderBottom: "0.5px solid gray",
    wordWeight: 900,
  };

  const buttonSaveStyle = {
    bgcolor: "#cf2338",
    color: "#fff",
    // marginRight: 3,
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
            sx={{ maxWidth: "410px" }}
          >
            <br />
            <Form onSubmit={handleChangePassword} validate={requiredInput}>
              <Grid container alignItems="center">
                <Grid container>
                  <Grid item sm={4} xs={12}>
                    <label>Old password:</label>
                  </Grid>
                  <Grid item sm={8} xs={12}>
                    <PasswordInput
                      fullWidth
                      label={false}
                      source="currentPassword"
                      helperText={error ? <p style={{color: "#d32f2f", margin: "0"}}>{error}</p> : ""} 
                    />
                  </Grid>
                </Grid>
                <Grid container>
                  <Grid item sm={4} xs={12}>
                    <label>New password:</label>
                  </Grid>
                  <Grid item sm={8} xs={12}>
                    <PasswordInput
                      fullWidth
                      label={false}
                      source="newPassword"
                    />
                  </Grid>
                </Grid>
                <Grid container columnSpacing={2} justifyContent="flex-end">
                  <Grid item sm={2.5} xs={6}>
                    <SaveButton
                      label="Save"
                      sx={buttonSaveStyle}
                      type="submit"
                      icon={<></>}
                    />
                  </Grid>
                  <Grid item sm={2.5} xs={6}>
                    <Button
                      type="button"
                      onClick={() => stateChanger(false)}
                      sx={{ border: "1px solid lightgray" }}
                    >
                      Cancel
                    </Button>
                  </Grid>
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
