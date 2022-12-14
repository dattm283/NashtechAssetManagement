import React, { useState, useEffect } from "react";
import { TextInput, DateInput, SimpleForm, Title, EditBase } from "react-admin";
import { useParams } from "react-router-dom";
import { Box, Typography, Container } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import * as assetService from "../../services/assets";
import * as categoryService from "../../services/category";
import CategorySelectBoxDisabled from "../../components/custom/CategorySelectBoxDisabled";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import AssetEditToolbar from "../../components/toolbar/AssetEditToolbar";
import { formStyle } from "../../styles/formStyle";

function EditAssetInformations() {
    const [category, setCategory] = useState([]);
    const [isValid, setIsValid] = useState(false);
    const { id } = useParams();
    const [asset, setAsset] = useState({
        name: null,
        specification: null,
        installedDate: null,
        state: -1,
        categoryId: 0,
    });
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);
    useEffect(() => {
        categoryService
            .getCategory()
            .then((responseData) => setCategory(responseData))
            .catch((error) => console.log(error));
        assetService
            .getAssetById(id)
            .then((responseData) => {
                setAsset(responseData);
            })
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors: Record<string, any> = {};
        if (!values.name || values.name.trim().length === 0) {
            errors.name = "This is required";
        } else if (values.name.trim().length > 100) {
            errors.name = "This field must be least than 100 characters";
        }

        if (!values.categoryId) {
            errors.categoryId = "This is required";
        }
        if (values.installedDate == null) {
            errors.installedDate = "This is required";
        }
        if (!values.specification || values.specification.trim().length === 0) {
            errors.specification = "This is required";
        } else if (values.specification.trim().length > 500) {
            errors.specification = "This field must be least than 500 characters";
        }

        if (!values.state) {
            errors.state = "This is required";
        }
        if (Object.keys(errors).length === 0) {
            setIsValid(false);
        } else {
            setIsValid(false);
        }
        if (errors.name == "This field must be least than 100 characters") {
            return { name: "This field must be least than 100 characters" };
        } else if (errors.specification == "This field must be least than 500 characters") {
            return { specification: "This field must be least than 500 characters" }
        } else {
            return {};
        }
    };

    return (
        <ThemeProvider theme={theme}>
            <Title title="Manage Asset > Edit Asset" />
            <Container component="main">
                {/* <CssBaseline /> */}
                <Box sx={formStyle.boxTitleStyle}>
                    <Typography
                        component="h3"
                        variant="h5"
                        sx={formStyle.formTitle}
                    >
                        Edit Asset
                    </Typography>
                    <EditBase
                        sx={formStyle.editBaseStyle}
                        mutationMode="pessimistic"
                    >
                        <SimpleForm
                            validate={requiredInput}
                            toolbar={<AssetEditToolbar disable={isValid} />}
                        >
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Name <span className="red">*</span>
                                </Typography>
                                <TextInput
                                    fullWidth
                                    label={false}
                                    name="name"
                                    source="name"
                                    sx={formStyle.textInputStyle}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Category <span className="red">*</span>
                                </Typography>
                                <CategorySelectBoxDisabled
                                    source="category"
                                    format={(formValue) =>
                                        Array.prototype.filter.bind(category)(
                                            (item) => item.id === formValue
                                        )["name"]
                                    }
                                    parse=""
                                    defaultValue={asset.categoryId}
                                    disabled={true}
                                />
                            </Box>

                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Specification <span className="red">*</span>
                                </Typography>
                                <TextInput
                                    label={false}
                                    fullWidth
                                    multiline
                                    rows="3"
                                    sx={formStyle.textInputStyle}
                                    name="specification"
                                    source="specification"
                                    helperText={false}
                                    defaultValue={asset.specification}
                                    InputLabelProps={{ shrink: false }}
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Installed Date <span className="red">*</span>
                                </Typography>
                                <DateInput
                                    fullWidth
                                    label={false}
                                    name="installedDate"
                                    source="installedDate"
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
                                    State <span className="red">*</span>
                                </Typography>
                                <RadioButtonGroup
                                    label=""
                                    row={false}
                                    sx={formStyle.textInputStyle}
                                    source="state"
                                    choices={[
                                        { id: 0, name: "Available" },
                                        { id: 1, name: "Not available" },
                                        { id: 2, name: "Waiting for recycling" },
                                        { id: 3, name: "Recycled" }
                                    ]}
                                    optionText="name"
                                    optionValue="id"
                                />
                            </Box>
                        </SimpleForm>
                    </EditBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default EditAssetInformations;
