import React, { useState, useEffect, useRef } from "react";
import {
    TextInput,
    DateInput,
    SimpleForm,
    Title,
    EditBase,
    useRefresh,
    Edit,
    CreateBase,
    useListContext,
    SearchInput,
    Button
} from "react-admin";
import { useParams } from "react-router-dom";
import { Box, Typography, Container, Grid } from "@mui/material";
import {
    createTheme,
    ThemeProvider,
    unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { formStyle } from "../../styles/formStyle";
import SelectUserModal from "../../components/modal/selectUserModal/SelectUserModal";
import AssignmentEditToolbar from "../../components/toolbar/AssignmentEditToolbar";
import SelectAssetModal from "../../components/modal/selectAssetModal/SelectAssetModal";
import { theme } from "../../theme";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from '@mui/icons-material/Search';
import IconButton from "@mui/material/IconButton";
import AssignmentCreateToolbar from "../../components/toolbar/AssignmentCreateToolbar";
import InputWithSelectModal from "../../components/custom/InputWithSelectModal";

const AssignmentCreate = () => {
    const userChoiceRef = useRef<HTMLElement>(null);
    const assetChoiceRef = useRef<HTMLElement>(null);
    const [asset, setAsset] = useState("")
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
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, "0");
    var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
    var yyyy = String(today.getFullYear());
    var currentDay = yyyy + "-" + mm + "-" + dd;
    const { setSort } = useListContext();
    const toggleAssetChoice = () => {
        setAssetChoiceOpen(!assetChoiceOpen);
    }

    const toggleUserChoice = () => {
        setUserChoiceOpen(!userChoiceOpen);
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
        const errors: Record<string, any> = {};
        let today = new Date(currentDay);
        today.setDate(today.getDate() - 1);
        let yesterday = today.toISOString();
        if (!values.note || values.note.trim().length === 0) {
            errors.note = "This is required";
        } else if (values.note.trim().length > 500) {
            errors.note = "This field must be least than 500 characters";
        }
        if (!values.assignedDate) {
            errors.assignedDate = "This is required";
        }
        if (values.assignedDate < yesterday) {
            errors.assignedDate = "Please select only current or future date";
        }
        if (!values.assetCode) {
            errors.assetCode = "This is required";
        }
        if (!values.assignToAppUserStaffCode) {
            errors.assignToAppUserStaffCode = "This is required";
        }
        if (Object.keys(errors).length === 0) {
            setIsInvalid(false);
        } else if (errors.assignedDate) {
            setIsInvalid(true);
            return { assignedDate: errors.assignedDate };
        } else if (errors.note == "This field must be least than 500 characters") {
            setIsInvalid(true);
            return { note: "This field must be least than 500 characters" };
        }
        else {
            setIsInvalid(true);
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
                        Create Assignment
                    </Typography>
                    <CreateBase
                    // sx={formStyle.editBaseStyle}
                    // mutationMode="pessimistic"
                    >
                        <SimpleForm
                            mode="onChange"
                            validate={requiredInput}
                            toolbar={<AssignmentCreateToolbar isEnable={!isInvalid} />}
                        >
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
                                        setChanged={() => { }}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Asset <span className="red">*</span>
                                    </Typography>
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
                                        setChanged={() => { }}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Assigned Date <span className="red">*</span>
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
                                        defaultValue={currentDay}
                                        inputProps={{ min: currentDay }}
                                    />
                                </Box>
                                <Box sx={formStyle.boxStyle}>
                                    <Typography
                                        variant="h6"
                                        sx={formStyle.typographyStyle}
                                    >
                                        Note <span className="red">*</span>
                                    </Typography>
                                    <TextInput
                                        fullWidth
                                        label={false}
                                        name="note"
                                        source="note"
                                        multiline
                                        rows={3}
                                        sx={formStyle.textInputStyle}
                                        helperText={false}
                                        InputLabelProps={{ shrink: false }}
                                    />
                                </Box>
                            </Grid>
                        </SimpleForm>
                    </CreateBase>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default AssignmentCreate;