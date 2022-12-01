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
import * as categoryService from "../../services/category";
import { formStyle } from "../../styles/formStyle";

var today = new Date();
var dd = String(today.getDate()).padStart(2, "0");
var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
var yyyy = String(today.getFullYear());
var currentDay = yyyy + "-" + mm + "-" + dd;

function NewCategoryCreate() {
    const [category, setCategory] = useState([]);
    const [isValid, setIsValid] = useState(true);
    const [create, {}] = useCreate();
    const navigate = useNavigate();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);

    useEffect(() => {
        categoryService
            .getCategory()
            .then((responseData) => setCategory(responseData.data))
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
                        redirect="show">
                        <SimpleForm
                            validate={requiredInput}
                            toolbar={<AssetCreateToolbar disable={isValid} />}
                        >
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Name *
                                    </Typography>
                                    <TextInput
                                        fullWidth
                                        label=""
                                        name="name"
                                        source="name"
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
                                        Category *
                                    </Typography>
                                    {/* Custom Dropdown Selection (Category) */}
                                    <SelectBoxWithFormInside
                                        // category={category}
                                        source="categoryId"
                                        format={(formValue) =>
                                            Array.prototype.filter.bind(
                                                category
                                            )((item) => item.id === formValue)[
                                                "name"
                                            ]
                                        }
                                        parse=""
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Specification *
                                    </Typography>
                                    <TextInput
                                        fullWidth
                                        multiline
                                        label=""
                                        rows="3"
                                        InputLabelProps={{ shrink: false }}
                                        sx={formStyle.textInputStyle}
                                        name="specification"
                                        source="specification"
                                        helperText={false}
                                    />
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Installed Date *
                                    </Typography>
                                    <DateInput
                                        fullWidth
                                        label=""
                                        name="installedDate"
                                        source="installedDate"
                                        defaultValue={currentDay}
                                        InputLabelProps={{ shrink: false }}
                                        inputProps={{ min: currentDay }}
                                        validate={minValue(currentDay)}
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
                                        State *
                                    </Typography>
                                    <RadioButtonGroup
                                        label=""
                                        source="state"
                                        choices={[
                                            {
                                                state_id: "0",
                                                state: "Available",
                                            },
                                            {
                                                state_id: "1",
                                                state: "Not available",
                                            },
                                        ]}
                                        row={false}
                                        sx={formStyle.textInputStyle}
                                        optionText="state"
                                        optionValue="state_id"
                                        helperText={false}
                                    />
                                </Box>
                        </SimpleForm>
                    </CreateBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default NewCategoryCreate;
