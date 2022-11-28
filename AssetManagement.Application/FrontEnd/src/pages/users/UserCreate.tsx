import React, { useState, useEffect } from "react";
import {
    Form,
    TextInput,
    DateInput,
    minValue,
    useCreate,
    CreateBase,
    SimpleForm,
    Title,
    SelectInput,
} from "react-admin";
import { Box, Button, Typography, Container, CssBaseline } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import { useNavigate } from "react-router-dom";
import SelectBoxWithFormInside from "../../components/custom/SelectBoxWithFormInside";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import AssetCreateToolbar from "../../components/toolbar/AssetCreateToolbar";
import * as UserService from "../../services/user";
import { formStyle } from "../../styles/formStyle";

var today = new Date();
var dd = String(today.getDate()).padStart(2, "0");
var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
var yyyy = String(today.getFullYear());
var currentDay = yyyy + "-" + mm + "-" + dd;

function NewUserCreate() {
    const [roles, setRoles] = useState([]);
    const [isValid, setIsValid] = useState(true);
    const [create, {}] = useCreate();
    const navigate = useNavigate();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);

    useEffect(() => {
        UserService
            .getRoles()
            .then((responseData) => setRoles(responseData))
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors = {
            name: "",
            categoryId: "",
            specification: "",
            installedDate: "",
            state: "",
        };
        if (!values.name) {
            errors.name = "This is required";
            setIsValid(true);
        } else if (!values.categoryId) {
            errors.categoryId = "This is required";
            setIsValid(true);
        } else if (!values.specification) {
            errors.specification = "This is required";
            setIsValid(true);
        } else if (!values.state) {
            errors.state = "This is required";
            setIsValid(true);
        } else {
            setIsValid(false);
            return {};
        }
        return errors;
    };

    return (
        <ThemeProvider theme={theme}>
            <Title title="Manage Asset > Create Asset" />
            <Container component="main">
                {/* <CssBaseline /> */}
                <Box sx={formStyle.boxTitleStyle}>
                    <Typography
                        component="h3"
                        variant="h5"
                        sx={formStyle.formTitle}
                    >
                        Create New Asset
                    </Typography>
                    <CreateBase
                        redirect="list">
                        <SimpleForm
                            validate={requiredInput}
                            toolbar={<AssetCreateToolbar disable={isValid} />}
                        >
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        First Name
                                    </Typography>
                                    <TextInput
                                        fullWidth
                                        label=""
                                        name="firstname"
                                        source="firstname"
                                        InputLabelProps={{ shrink: false }}
                                        sx={formStyle.textInputStyle}
                                        helperText={false}
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Last Name *
                                    </Typography>
                                    {/* Custom Dropdown Selection (Category) */}
                                    <TextInput
                                        fullWidth
                                        label=""
                                        name="lastname"
                                        source="lastname"
                                        InputLabelProps={{ shrink: false }}
                                        sx={formStyle.textInputStyle}
                                        helperText={false}
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Gender *
                                    </Typography>
                                    <RadioButtonGroup
                                        label=""
                                        source="gender"
                                        choices={[
                                            {
                                                gender_id: "0",
                                                gender: "Male",
                                            },
                                            {
                                                gender_id: "1",
                                                gender: "Female",
                                            },
                                        ]}
                                        row={true}
                                        sx={formStyle.textInputStyle}
                                        optionText="gender"
                                        optionValue="gender_id"
                                        helperText={false}
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Date of Birth *
                                    </Typography>
                                    <DateInput
                                        fullWidth
                                        label=""
                                        name="dob"
                                        source="dob"
                                        InputLabelProps={{ shrink: false }}
                                        onBlur={(e) => e.stopPropagation()}
                                        sx={formStyle.textInputStyle}
                                        helperText={false}
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Joined Date *
                                    </Typography>
                                    <DateInput
                                        fullWidth
                                        label=""
                                        name="joinedDate"
                                        source="joinedDate"
                                        InputLabelProps={{ shrink: false }}
                                        onBlur={(e) => e.stopPropagation()}
                                        sx={formStyle.textInputStyle}
                                        helperText={false}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Type *
                                    </Typography>
                                    {/* Custom Dropdown Selection (Category) */}
                                    <SelectInput
                                        // category={category}
                                        label=""
                                        InputLabelProps={{ shrink: false }}
                                        source="name"
                                        name="id"
                                        choices={roles}
                                        optionText="name"
                                        optionValue="id"
                                        sx={{ 
                                            width:"430px", 
                                            height:"40px",
                                            padding:"0px",
                                            boxSizing:"border-box",
                                            "& .MuiDataGrid-root": {
                                                border: "none"
                                            },
                                        }}
                                    />
                                </Box>
                        </SimpleForm>
                    </CreateBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default NewUserCreate;
