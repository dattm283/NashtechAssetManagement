import React, { useState } from "react";
import {
    Form,
    TextInput,
    useLogin,
    useNotify,
    PasswordInput,
} from "react-admin";
import {
    Button,
    Box,
    CssBaseline,
    Typography,
    Container,
    Grid,
} from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { theme } from "../../theme";
import logo from "../../assets/images/logo-white-transparent.svg";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import AdbIcon from "@mui/icons-material/Adb";
import { formStyle } from "../../styles/formStyle";

const LoginPage = ({ checkIsLoginFirstTime }) => {
    const [isValid, setIsValid] = useState(true);
    const login = useLogin();
    const notify = useNotify();

    const handleFormSubmit = ({ userName, password }: any) => {
        login({ username: userName, password: password })
            .then(data => {
                checkIsLoginFirstTime(password);
            })
            .catch((error) => {
                console.log(error);
                notify(error.response.data.message);
            });
    };

    const requiredInput = (values) => {
        const errors: Record<string, any> = {};
        if (!values.userName || values.userName.trim().length === 0) {
            errors.userName = "This is required";
            setIsValid(true);
        } else if (!values.password || values.password.trim().length === 0) {
            errors.password = "This is required";
            setIsValid(true);
        } else {
            setIsValid(false);
            return {};
        }
        return errors;
    };

    return (
        <ThemeProvider theme={theme}>
            {/* Form container */}
            <Container component="main">
                <CssBaseline />
                <AppBar className="loginHeaderBackground">
                    <Container maxWidth="lg">
                        <Toolbar disableGutters>
                            <Box
                                component="img"
                                sx={{
                                    width: 40,
                                    border: "",
                                    mr: 4,
                                }}
                                alt="NashTech logo login page"
                                src={logo}
                            />
                            <Typography
                                variant="h6"
                                noWrap
                                component="a"
                                href="/"
                                sx={{
                                    mr: 2,
                                    display: { xs: "none", md: "flex" },
                                    fontWeight: "bold",
                                    fontSize: "1rem",
                                    color: "inherit",
                                    textDecoration: "none",
                                }}
                            >
                                Online Asset Management
                            </Typography>
                        </Toolbar>
                    </Container>
                </AppBar>
                <Box
                    sx={{
                        margin: "auto",
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                    }}
                >
                    <Box className="loginBorder" sx={{ mt: "100px" }}>
                        <Form
                            className="loginFormBorder"
                            mode="onBlur"
                            validate={requiredInput}
                            onSubmit={handleFormSubmit}
                        >
                            <Grid container maxWidth="md">
                                <Grid item xs={12}>
                                    <Typography
                                        component="h1"
                                        variant="h5"
                                        mb="5%"
                                        className="loginFormHeader"
                                        textAlign="center"
                                        fontWeight="700"
                                    >
                                        Welcome to Online Asset Management
                                    </Typography>
                                </Grid>
                                <Grid item xs={12}>
                                    <Box>
                                        <Grid container>
                                            <Grid item xs={12} sm={3}>
                                                <Typography variant="h6">
                                                    Username <span className="red">*</span>
                                                </Typography>
                                            </Grid>
                                            <Grid item xs={12} sm={9}>
                                                <TextInput
                                                    fullWidth
                                                    id="userName"
                                                    label={false}
                                                    name="userName"
                                                    autoComplete="current-userName"
                                                    autoFocus
                                                    inputProps={{
                                                        maxLength: "50",
                                                    }}
                                                    source="username"
                                                    InputLabelProps={{ shrink: false }}
                                                />
                                            </Grid>
                                        </Grid>
                                    </Box>
                                    <Box>
                                        <Grid container>
                                            <Grid item xs={12} sm={3}>
                                                <Typography variant="h6">
                                                    Password <span className="red">*</span>
                                                </Typography>
                                            </Grid>
                                            <Grid item xs={12} sm={9}>
                                                <PasswordInput
                                                    fullWidth
                                                    id="password"
                                                    label={false}
                                                    name="password"
                                                    autoComplete="current-password"
                                                    inputProps={{
                                                        maxLength: "50",
                                                    }}
                                                    source="password"
                                                    InputLabelProps={{ shrink: false }}
                                                />
                                            </Grid>
                                        </Grid>
                                    </Box>
                                </Grid>
                                <Grid item xs={12}>
                                    <Box
                                        display="flex"
                                        justifyContent="end"
                                        mt="20px"
                                    >
                                        <Button
                                            type="submit"
                                            color="neutral"
                                            variant="contained"
                                            disabled={isValid}
                                            className="loginButton"
                                            sx={{ textTransform: "none" }}
                                        >
                                            Login
                                        </Button>
                                    </Box>
                                </Grid>
                            </Grid>
                        </Form>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
};

export default LoginPage;
