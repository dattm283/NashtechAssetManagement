import React, { useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  Button,
  DialogContentText,
} from "@mui/material";
import {
  Form,
  maxLength,
  minLength,
  PasswordInput,
  required,
  SaveButton,
} from "react-admin";
import userService from "../../../services/users";

const UserChangePasswordModal = ({ stateChanger, ...rest }) => {
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");

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

  const validateChangePassword = [
    required("This is required"),
    minLength(6, "Must be more than 6 leters"),
    maxLength(50, "Must be less than 50 letters"),
  ];

  const style = {
    bgcolor: "#EFF0F4",
    color: "#CE2339",
    borderBottom: "0.5px solid gray",
    wordWeight: 900,
    fontWeight: "bold",
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
            <Form onSubmit={handleChangePassword} mode="onBlur" reValidateMode="onBlur">
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
                      InputLabelProps={{ shrink: false }}
                      validate={validateChangePassword}
                      onChange={(e) => setOldPassword(e.target.value)}
                      helperText={
                        error ? (
                          <p style={{ color: "#d32f2f", margin: "0" }}>{error}</p>) : ("")}
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
                      InputLabelProps={{ shrink: false }}
                      source="newPassword"
                      validate={validateChangePassword}
                      onChange={(e) => setNewPassword(e.target.value)}
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
                      disabled={oldPassword.length > 0 && newPassword.length > 0 ? false : true}
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
