import React, { useState, useEffect } from "react";
import { TextInput, DateInput, SimpleForm, Title, EditBase, useRefresh } from "react-admin";
import { useParams } from "react-router-dom";
import { Box, Typography, Container } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import AssignmentEditToolbar from "../../components/toolbar/AssignmentEditToolbar";
import { formStyle } from "../../styles/formStyle";
import SelectAssetModal from "../../components/modal/selectAssetModal/SelectAssetModal";
import SelectUserModal from "../../components/modal/selectUserModal/SelectUserModal";

const AssignmentEdit = () => {
    const [asset, setAsset] = useState("")
    const [isInvalid, setIsInvalid] = useState(false);
    const [assetChoiceOpen, setAssetChoiceOpen] = useState(false);
    const [assetChoicePos, setAssetChoicePos] = useState({
        left: 0,
        top: 0,
    });
    const [selectedAsset, setSelectedAsset] = useState("");
    const [userChoiceOpen, setUserChoiceOpen] = useState(false);
    const [userChoicePos, setUserChoicePos] = useState({
        left: 0,
        top: 0,
    })
    const [selectedUser, setSelectedUser] = useState("");
    const { id } = useParams();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);

    const toggleAssetChoice = () => {
        setAssetChoiceOpen(!assetChoiceOpen);
    }

    const toggleUserChoice = () => {
        setUserChoiceOpen(!userChoiceOpen);
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
        var userTextBox = document.getElementById("edit_assignment_user_choice");

        if (assetTextBox) {
            let assetTextBoxPos = assetTextBox.getBoundingClientRect()
            setAssetChoicePos({
                left: assetTextBoxPos.left,
                top: assetTextBoxPos.top,
            })
        }
        if (userTextBox) {
            let userTextBoxPos = userTextBox.getBoundingClientRect();
            setUserChoicePos({
                left: userTextBoxPos.left,
                top: userTextBoxPos.top
            })
        }
    }, [])

    useEffect(() => {
        assetProvider.getOne("assignments", { id: id })
            .then((response) => {
                let updatingAssignment = response.data
                setSelectedAsset(updatingAssignment.assetCode);
                setSelectedUser(updatingAssignment.assignToAppUserStaffCode);
            })
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors = {
            note: "",
        };
        console.log("values: ", values);
        if (!values.note) {
            errors.note = "This is required";
            setIsInvalid(true);
        } else {
            setIsInvalid(false);
            return {
                note: "",
            }
        }
        console.log(errors)
        console.log(isInvalid)
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
                            toolbar={<AssignmentEditToolbar />}
                            mode="onChange"
                        >
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    User *
                                </Typography>
                                <TextInput
                                    id="edit_assignment_user_choice"
                                    fullWidth
                                    label={false}
                                    name="assignToAppUserStaffCode"
                                    source="assignToAppUserStaffCode"
                                    disabled
                                    onClick={() => { toggleUserChoice() }}
                                    sx={formStyle.textInputStyle}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />

                                <SelectUserModal
                                    setSelectedUser={setSelectedUser}
                                    selectedUser={selectedUser}
                                    isOpened={userChoiceOpen}
                                    toggle={toggleUserChoice}
                                    pos={userChoicePos} />
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