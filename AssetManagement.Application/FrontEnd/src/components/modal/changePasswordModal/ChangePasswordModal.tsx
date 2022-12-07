import React from 'react';
import { Grid, Typography, ThemeProvider } from '@mui/material';
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
import { theme } from '../../../theme';

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
        } else if (values.newPassword.length < 6) {
            errors.newPassword = "Password must have more than 5 characters"
        } else if (values.newPassword.length > 50) {
            errors.newPassword = "Password must have least than 50 characters"
        }
        else {
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
        bgcolor: "#EFF0F4",
        color: "#CE2339",
        borderBottom: "0.5px solid gray",
        fontWeight: "bold",
        padding: "20px 30px"
    }

    const buttonStyle = {
        bgcolor: '#cf2338',
        color: "#fff",
    }
    const container = {
        padding: "20px"
    }
    const typographyStyle = {
        marginTop: "5px",
        padding: "0",
        alignSelf: "center",
    }

    const dialogContentTextStyle = {
        margin: "10px 0 20px"
    }

    const logoutButtonStyle = {
        border: "1px solid #212121",
        color: "#212121",
    }

    return (
        <ThemeProvider theme={theme}>
            <Dialog
                open={loginFirstTime}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title" sx={style}>
                    {"Change Password"}
                </DialogTitle>
                <DialogContent>
                    <DialogContentText component={"div"} id="alert-dialog-description" sx={container}>
                        <DialogContentText sx={dialogContentTextStyle}>
                            This is the first time you login. <br />
                            You have to change the password to continue
                        </DialogContentText>

                        <Form onSubmit={handleChangePassword} validate={requiredInput}>
                            <Grid container>
                                <Grid container>
                                    <Grid item sm={4} xs={12}>
                                        <Typography
                                            sx={typographyStyle}
                                        >New password:</Typography>
                                    </Grid>
                                    <Grid item sm={8} xs={12}>
                                        <PasswordInput
                                            fullWidth
                                            label={false}
                                            InputLabelProps={{ shrink: false }}
                                            source="newPassword"
                                        />
                                    </Grid>
                                </Grid>
                                <Grid container columnSpacing={2} justifyContent="flex-end">
                                    <Grid item sm={2.5} xs={6}>
                                        <SaveButton label='Save' color="secondary" sx={buttonStyle} type="submit" icon={<></>} />
                                    </Grid>
                                    <Grid item sm={2.5} xs={6}>
                                        <Button type="button" color="primary" onClick={handleLogout} sx={logoutButtonStyle}>Logout</Button>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </Form>
                    </DialogContentText>
                </DialogContent>
            </Dialog>
        </ThemeProvider>
    )
}

export default ChangePasswordModal;