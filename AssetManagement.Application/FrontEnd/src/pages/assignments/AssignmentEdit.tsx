import React, { useState, useEffect } from "react";
import { TextInput, DateInput, SimpleForm, Title, EditBase, Link, useRecordContext } from "react-admin";
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
    const [assetChoiceOpen, setAssetChoiceOpen] = useState(false);
    const [assetChoicePos, setAssetChoicePos] = useState({
        left: 0,
        top: 0,
    });
    const [selectedAsset, setSelectedAsset] = useState("");
    const { id } = useParams();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);
    // const [assignment, setAssignment] = useState({
    //     assignToAppUserStaffCode: null,
    //     assetCode: null,
    //     assignedDate: null,
    //     note: "",
    // });

    const toggleAssetChoice = () => {
        setAssetChoiceOpen(!assetChoiceOpen);
    }

    useEffect(() => {
        var assetTextBox = document.getElementById("edit_assignment_asset_choice");
        if (assetTextBox) {
            let assetTextBoxValue = assetTextBox;
            assetTextBox.setAttribute("value", selectedAsset);
        }
    }, [selectedAsset])

    useEffect(() => {
        var assetTextBox = document.getElementById("edit_assignment_asset_choice");

        if (assetTextBox) {
            let assetTextBoxPos = assetTextBox.getBoundingClientRect()
            setAssetChoicePos({
                left: assetTextBoxPos.left,
                top: assetTextBoxPos.top,
            })
        }
    }, [])

    useEffect(() => {
        assetProvider.getOne("assignments", { id: id })
            .then((response) => {
                let updatingAssignment = response.data
                console.log(updatingAssignment)
                // setAssignment({
                //     assignToAppUserStaffCode: updatingAssignment.assignToAppUserStaffCode,
                //     assetCode: updatingAssignment.assetCode,
                //     assignedDate: updatingAssignment.assignedDate,
                //     note: updatingAssignment.note,
                // });
                setSelectedAsset(updatingAssignment.assetCode);
            })
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors = {
            assignedDate: "",
            note: "",
        };
        if (!values.assignedDate) {
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
                                    User *
                                </Typography>
                                <TextInput
                                    id="edit_assignment_asset_choice"
                                    fullWidth
                                    label={false}
                                    name="assignToAppUserStaffCode"
                                    source="assignToAppUserStaffCode"
                                    disabled
                                    onClick={() => { toggleAssetChoice() }}
                                    sx={formStyle.textInputStyle}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />

                                <SelectAssetModal
                                    setSelectedAsset={setSelectedAsset}
                                    selectedAsset={selectedAsset}
                                    isOpened={assetChoiceOpen}
                                    toggle={toggleAssetChoice}
                                    pos={assetChoicePos} />
                            </Box>
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
                                    defaultValue={selectedAsset}
                                    value={selectedAsset}
                                />

                                <SelectAssetModal
                                    setSelectedAsset={setSelectedAsset}
                                    selectedAsset={selectedAsset}
                                    isOpened={assetChoiceOpen}
                                    toggle={toggleAssetChoice}
                                    pos={assetChoicePos} />
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