import React, { useState, useEffect, useRef } from "react";
import { TextInput, DateInput, SimpleForm, Title, EditBase, useRecordContext, FunctionField } from "react-admin";
import { useNavigate, useParams } from "react-router-dom";
import { Box, Typography, Container, Grid } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import { theme } from "../../theme";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import AssignmentEditToolbar from "../../components/toolbar/AssignmentEditToolbar";
import { formStyle } from "../../styles/formStyle";
import SelectAssetModal from "../../components/modal/selectAssetModal/SelectAssetModal";
import SelectUserModal from "../../components/modal/selectUserModal/SelectUserModal";
import InputWithSelectModal from "../../components/custom/InputWithSelectModal";

const AssignmentEdit = () => {
    const [isInvalid, setIsInvalid] = useState(false);
    const [assetChoiceOpen, setAssetChoiceOpen] = useState(false);
    const [assetChoicePos, setAssetChoicePos] = useState({
        left: 0,
        top: 0,
        height: 0,
    });
    const [selectedAsset, setSelectedAsset] = useState({
        assetCode: "",
        assetName: ""
    });
    const [userChoiceOpen, setUserChoiceOpen] = useState(false);
    const [userChoicePos, setUserChoicePos] = useState({
        left: 0,
        top: 0,
        height: 0
    })
    const assetRef = useRef<HTMLElement>(null);
    const userRef = useRef<HTMLElement>(null);
    const [selectedUser, setSelectedUser] = useState({
        staffCode: "",
        fullname: ""
    });
    const { id } = useParams();
    let appTheme = createTheme(theme);
    appTheme = unstable_createMuiStrictModeTheme(appTheme);
    const navigate = useNavigate();
    const [changed, setChanged] = useState(false);

    const toggleUserChoice = () => {
        setUserChoiceOpen(!userChoiceOpen);
    }

    const toggleAssetChoice = () => {
        setAssetChoiceOpen(!assetChoiceOpen);
    }

    useEffect(() => {
        let assetTextBox = assetRef.current;
        let userTextBox = userRef.current;

        if (assetTextBox) {
            let assetTextBoxPos = assetTextBox.getBoundingClientRect()
            setAssetChoicePos({
                left: assetTextBoxPos.left,
                top: assetTextBoxPos.top,
                height: assetTextBox.offsetHeight
            })
        }
        if (userTextBox) {
            let userTextBoxPos = userTextBox.getBoundingClientRect();
            setUserChoicePos({
                left: userTextBoxPos.left,
                top: userTextBoxPos.top,
                height: userTextBox.offsetHeight
            });
        }
    }, [])

    useEffect(() => {
        assetProvider.getOne("assignments", { id: id })
            .then((response) => {
                let updatingAssignment = response.data
                console.log("updating assignment ", updatingAssignment);
                setSelectedAsset({
                    assetCode: updatingAssignment.assetCode,
                    assetName: updatingAssignment.assetName
                });
                setSelectedUser({
                    staffCode: updatingAssignment.assignToAppUserStaffCode,
                    fullname: updatingAssignment.assignToAppUserFullName
                });
            })
            .catch((error) => console.log(error));
    }, []);

    const requiredInput = (values) => {
        const errors = {
            note: "",
            assignedDate: ""
        };
        let displayErrors = {}
        let today = new Date();
        today.setDate(today.getDate() - 1);
        let yesterday = today.toISOString();
        if (!values.note || values.note.trim().length === 0) {
            errors.note = "This is required";
            setIsInvalid(true);
        } else if (values.note.trim().length > 500) {
            errors.note = "This is field must be least than 500 characters";
        }
        else if (!values.assignedDate) {
            errors.assignedDate = "This is required";
            setIsInvalid(true);
        } else if (values.assignedDate < yesterday) {
            errors.assignedDate = "Please select only current or future date";
            setIsInvalid(false);
        } else {
            setIsInvalid(false);
            return {};
        }
        if (errors.note) {
            displayErrors = { ...displayErrors, note: errors.note }
        }
        if (errors.assignedDate) {
            displayErrors = { ...displayErrors, assignedDate: errors.assignedDate }
        }
        return displayErrors;
    };

    return (
        <ThemeProvider theme={appTheme}>
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
                            mode="onChange"
                            validate={requiredInput}
                            reValidateMode="onChange"
                            toolbar={<AssignmentEditToolbar changed={changed} isEnable={!isInvalid} />}
                        >
                            <FunctionField render={(record) => {
                                if (record.state !== 1) {
                                    navigate("/assignments");
                                }
                            }} />
                            <Grid container>
                                <Box sx={formStyle.boxStyle}>
                                    <Grid item xs={4}>
                                        <Typography
                                            variant="h6"
                                            sx={formStyle.typographyStyle}
                                        >
                                            User <span className="red">*</span>
                                        </Typography>
                                    </Grid>

                                    <InputWithSelectModal
                                        handleClick={toggleUserChoice}
                                        source="assignToAppUserFullName"
                                        innerRef={userRef}
                                    />

                                    <SelectUserModal
                                        setSelectedUser={setSelectedUser}
                                        selectedUser={selectedUser}
                                        isOpened={userChoiceOpen}
                                        toggle={toggleUserChoice}
                                        pos={userChoicePos}
                                        setChanged={setChanged}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Grid item xs={4}>
                                        <Typography
                                            variant="h6"
                                            sx={formStyle.typographyStyle}
                                        >
                                            Asset <span className="red">*</span>
                                        </Typography>
                                    </Grid>

                                    <InputWithSelectModal
                                        handleClick={toggleAssetChoice}
                                        source="assetName"
                                        innerRef={assetRef}
                                    />

                                    <SelectAssetModal
                                        setSelectedAsset={setSelectedAsset}
                                        selectedAsset={selectedAsset}
                                        isOpened={assetChoiceOpen}
                                        toggle={toggleAssetChoice}
                                        pos={assetChoicePos}
                                        setChanged={setChanged}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Grid item xs={4}>
                                        <Typography
                                            variant="h6"
                                            sx={formStyle.typographyStyle}
                                        >
                                            Assigned Date <span className="red">*</span>
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={8}>
                                        <DateInput
                                            onChange={() => { setChanged(true); }}
                                            fullWidth
                                            label=""
                                            name="assignedDate"
                                            source="assignedDate"
                                            InputLabelProps={{ shrink: false }}
                                            onBlur={(e) => e.stopPropagation()}
                                            sx={formStyle.textInputStyle}
                                            helperText={false}
                                        />
                                    </Grid>
                                </Box>

                                <Box sx={formStyle.boxStyle}>
                                    <Grid item xs={4}>
                                        <Typography
                                            variant="h6"
                                            sx={formStyle.typographyStyle}
                                        >
                                            Note <span className="red">*</span>
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={8}>
                                        <TextInput
                                            onChange={() => { setChanged(true) }}
                                            fullWidth
                                            label={false}
                                            multiline
                                            rows={3}
                                            name="note"
                                            source="note"
                                            sx={formStyle.textInputStyle}
                                            helperText={false}
                                            InputLabelProps={{ shrink: false }}
                                        />
                                    </Grid>
                                </Box>
                            </Grid>
                        </SimpleForm>
                    </EditBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default AssignmentEdit;