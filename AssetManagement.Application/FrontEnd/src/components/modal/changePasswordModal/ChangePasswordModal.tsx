import React from 'react';
import { Grid } from '@mui/material';
import { DeleteButton, Form, PasswordInput, SaveButton, TextInput, useNotify } from 'react-admin';
import authService from "../../../services/changePasswordFirstTime/auth";
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import * as CryptoJS from 'crypto-js';
import config from "../../../connectionConfigs/config.json";
import AuthProvider from '../../../providers/authenticationProvider/authProvider';

const encryptKey = config.encryption.key;

const ChangePasswordModal = ({
    loginFirstTime,
    setLoginFirstTime,
    logout
}) => {
    const notify = useNotify();

    const handleLogout = () => {
        setLoginFirstTime(false);
        localStorage.removeItem("loginFirstTime");
        logout();
        window.location.href = ".";
    }

    const decrypt = (text) => {
        console.log(encryptKey)
        return CryptoJS.AES.decrypt(text, encryptKey).toString(CryptoJS.enc.Utf8);
    }

    const requiredInput = (values) => {
        let errors = {
            newPassword: "",
        };
        if (!values.newPassword) {
            errors.newPassword = "This is required";
        } else {
            return {};
        }
        return errors;
    }

    const handleChangePassword = data => {
        const newPassword = data.newPassword;
        const confirmPassword = data.newPassword;
        const currentPassword = decrypt(localStorage.getItem("currentPassword"));

        console.log(newPassword, confirmPassword, currentPassword)

        const changePasswordRequest = {
            newPassword,
            confirmPassword,
            currentPassword
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
        color: "#fff",
    }

    const buttonStyle = {
        bgcolor: '#cf2338',
        color: "#fff",
        marginRight: 3
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
                                <PasswordInput source="newPassword" fullWidth />
                            </Grid>
                            <Grid alignItems="left" item xs={12}>
                                <SaveButton label='Save' sx={buttonStyle} type="submit" icon={<></>}/>
                                <Button type="button" onClick={handleLogout} sx={buttonStyle}>Logout</Button>
                            </Grid>
                        </Grid>
                    </Form>
                </DialogContentText>
            </DialogContent>
        </Dialog>
    )
}

export default ChangePasswordModal;