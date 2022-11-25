import React from 'react';
import { Grid } from '@mui/material';
import { Form, PasswordInput, SaveButton, TextInput, useNotify } from 'react-admin';
import authService from "../../../services/changePasswordFirstTime/auth";
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';

const ChangePasswordModal = ({
    loginFirstTime,
    setLoginFirstTime,
}) => {
    const notify = useNotify();

    const requiredInput = (values) => {
        let errors = {
            currentPassword: "",
            newPassword: "",
            confirmPassword: "",
        };
        if (!values.currentPassword) {
            errors.currentPassword = "This is required";
        } else if (!values.newPassword) {
            errors.newPassword = "This is required";
        } else if (!values.confirmPassword) {
            errors.confirmPassword = "This is required";
        } else if (values.confirmPassword !== values.newPassword) {
            errors.confirmPassword = "Confirm password not match";
        } else {
            return {};
        }
        return errors;
    }

    const handleChangePassword = data => {
        const newPassword = data.newPassword;
        const confirmPassword = data.confirmPassword;
        const currentPassword = data.currentPassword;

        console.log(newPassword, confirmPassword, currentPassword)

        const changePasswordRequest = {
            newPassword: data.newPassword,
            confirmPassword: data.confirmPassword,
            currentPassword: data.currentPassword
        }

        authService.changePassword(changePasswordRequest)
            .then(() => {
                localStorage.removeItem("loginFirstTime");
                setLoginFirstTime(false);
            }, (err) => console.log(err))
            .catch(() => {
                notify('Invalid password');
            })
    }

    const style = {
        bgcolor: '#cf2338',
        color: "#fff"
    }

    return (
        <Dialog
            open={loginFirstTime}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
        >
            <DialogTitle id="alert-dialog-title" sx={style}>
                {"Change Password"}
            </DialogTitle>
            <DialogContent>
                <DialogContentText component={"div"} id="alert-dialog-description">
                    <DialogContentText>
                        This is the first time you login. <br />
                        You have to change the password to continue
                    </DialogContentText>

                    <Form onSubmit={handleChangePassword} validate={requiredInput}>
                        <Grid container>
                            <Grid item xs={12}>
                                <TextInput type="password" source="currentPassword" fullWidth />
                            </Grid>
                            <Grid item xs={12}>
                                <TextInput type="password" source="newPassword" fullWidth />
                            </Grid>
                            <Grid item xs={12}>
                                <TextInput type="password" source="confirmPassword" fullWidth />
                            </Grid>
                            <Grid item xs={12}>
                                <SaveButton label='Change Password' sx={style} type="submit" />
                            </Grid>
                        </Grid>
                    </Form>
                </DialogContentText>
            </DialogContent>
        </Dialog>
    )
}

export default ChangePasswordModal;