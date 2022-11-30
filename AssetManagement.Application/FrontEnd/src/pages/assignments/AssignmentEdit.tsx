import React, { useState, useEffect } from "react";
import { TextInput, DateInput, SimpleForm, Title, EditBase } from "react-admin";
import { useParams } from "react-router-dom";
import { Box, Typography, Container, Grid, TextField } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import * as assetService from "../../services/assets";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import * as categoryService from "../../services/category";
import CategorySelectBoxDisabled from "../../components/custom/CategorySelectBoxDisabled";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import AssignmentEditToolbar from "../../components/toolbar/AssignmentEditToolbar";
import { formStyle } from "../../styles/formStyle";
import SelectAssetModal from "../../components/modal/selectAssetModal/SelectAssetModal";

const AssignmentEdit = () => {
    const [asset, setAsset] = useState("")
    const [isInvalid, setIsInvalid] = useState(true);
    const [assetChoiceOpen, setAssetChoiceOpen] = useState(true);
    const [assetChoicePos, setAssetChoicePos] = useState({
        left: 0,
        top: 0,
        right: 0,
        bot: 0
    })
    const { id } = useParams();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);
    const [assignment, setAssignment] = useState({
        assignedTo: null,
        assetCode: null,
        assignedDate: null,
        note: "",
    });

    const toggleAssetChoice = () => {
        setAssetChoiceOpen(!assetChoiceOpen);
    }

    useEffect(() => {
        var assetTextBox = document.getElementById("edit_assignment_asset_choice");

        if (assetTextBox) {
            let assetTextBoxPos = assetTextBox.getBoundingClientRect()
            console.log("Asset text box pos: ", assetTextBoxPos);
            setAssetChoicePos({
                left: assetTextBoxPos.left,
                top: assetTextBoxPos.top,
                right: assetTextBoxPos.right,
                bot: assetTextBoxPos.bottom
            })
        }

    }, [])

    useEffect(() => {
        assetProvider.getOne("assignments", { id: id })
            .then((response) => {
                let updatingAssignment = response.data
                console.log("AssignmentEdit_ updating assignment: ", updatingAssignment)
                setAssignment({
                    assignedTo: updatingAssignment.assignedTo,
                    assetCode: updatingAssignment.assetCode,
                    assignedDate: updatingAssignment.assignedDate,
                    note: updatingAssignment.note,
                });
            })
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors = {
            assignedTo: "",
            assetCode: "",
            assignedDate: "",
            note: "",
        };
        if (!values.assignedTo) {
            errors.assignedTo = "This is required";
            setIsInvalid(true);
        } else if (!values.assetCode) {
            errors.assetCode = "This is required";
            setIsInvalid(true);
        } else if (!values.assignedDate) {
            errors.assignedDate = "This is required";
            setIsInvalid(true);
        } else if (!values.note) {
            errors.note = "This is required";
            setIsInvalid(true);
        } else {
            setIsInvalid(false);
            return {};
        }
        return errors;
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
                        Edit Assignment
                    </Typography>
                    <EditBase
                        sx={formStyle.editBaseStyle}
                        mutationMode="pessimistic"
                    >
                        <SimpleForm
                            validate={requiredInput}
                            toolbar={<AssignmentEditToolbar disable={isInvalid} />}
                        >
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Asset *
                                </Typography>
                                <TextInput
                                    id="edit_assignment_asset_choice"
                                    fullWidth
                                    label={false}
                                    name="assetCode"
                                    source="assetCode"
                                    disabled
                                    onClick={() => { toggleAssetChoice() }}
                                    sx={formStyle.textInputStyle}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />
                                <SelectAssetModal pos={assetChoicePos} isOpened={assetChoiceOpen} toggle={toggleAssetChoice} />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Assigned Date *
                                </Typography>
                                <DateInput
                                    fullWidth
                                    label=""
                                    name="assignedDate"
                                    source="assignedDate"
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
                                    Note *
                                </Typography>
                                <TextInput
                                    fullWidth
                                    label={false}
                                    name="note"
                                    source="note"
                                    sx={formStyle.textInputStyle}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />
                            </Box>
                        </SimpleForm>
                    </EditBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default AssignmentEdit;