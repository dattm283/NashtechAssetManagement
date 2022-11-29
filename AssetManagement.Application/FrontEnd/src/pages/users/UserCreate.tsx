import React, { useState, useEffect } from "react";
import {
  Form,
  TextInput,
  DateInput,
  minValue,
  useCreate,
  required,
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
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import UserCreateToolbar from "../../components/toolbar/CreateToolbar";
import { formStyle } from "../../styles/formStyle";

// var today = new Date();
// var dd = String(today.getDate()).padStart(2, "0");
// var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
// var yyyy = String(today.getFullYear());
// var currentDay = yyyy + "-" + mm + "-" + dd;

function UserCreate() {
  const [isValid, setIsValid] = useState(true);
  let theme = createTheme();
  theme = unstable_createMuiStrictModeTheme(theme);

  const requiredInput = (values) => {
    const errors = {
      firstname: "",
      lastname: "",
      role: "",
      gender: "",
      dob: "",
      joinedDate: "",
    };

    if (!values.firstname) {
      errors.firstname = "This is required";
      setIsValid(true);
    }

    if (!values.lastname) {
      errors.lastname = "This is required";
      setIsValid(true);
    }

    if (!values.dob) {
      errors.dob = "This is required";
      setIsValid(true);
    }

    if (!values.dob) {
      errors.dob = "This is required";
      setIsValid(true);
    }

    if (!values.joinedDate) {
      errors.joinedDate = "This is required";
      setIsValid(true);
    }

    if (!values.role) {
      errors.role = "This is required";
      setIsValid(true);
    }

    if (values.firstname && values.lastname && values.gender && values.role) {
      setIsValid(false);
      return {};
    }

    return errors;
  };

  const maxLength =
    (max, message = "Too short") =>
    (value) => {
      debugger;
      value && value.length > max ? message : undefined;
    };

  return (
    <ThemeProvider theme={theme}>
      <Title title="Manage Asset > Create Asset" />
      <Container component="main">
        {/* <CssBaseline /> */}
        <Box sx={formStyle.boxTitleStyle}>
          <Typography component="h3" variant="h5" sx={formStyle.formTitle}>
            Create New User
          </Typography>
          <CreateBase redirect="list">
            <SimpleForm
              mode="onBlur"
              validate={requiredInput}
              toolbar={<UserCreateToolbar disable={isValid} url="/user" />}
            >
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  First Name
                </Typography>
                <TextInput
                  inputProps={{ maxLength: 50 }}
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
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Last Name *
                </Typography>
                {/* Custom Dropdown Selection (Category) */}
                <TextInput
                  inputProps={{ maxLength: 50 }}
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
                <Typography variant="h6" sx={formStyle.typographyStyle}>
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
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Gender *
                </Typography>
                <RadioButtonGroup
                  label=""
                  name="gender"
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
                <Typography variant="h6" sx={formStyle.typographyStyle}>
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
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Type *
                </Typography>
                <SelectInput
                  // category={category}
                  label=""
                  InputLabelProps={{ shrink: false }}
                  source="role"
                  name="role"
                  choices={[
                    {
                      name: "Admin",
                    },
                    {
                      name: "Staff",
                    },
                  ]}
                  optionText="name"
                  optionValue="name"
                  sx={{
                    width: "430px",
                    height: "40px",
                    padding: "0px",
                    boxSizing: "border-box",
                    "& .MuiDataGrid-root": {
                      border: "none",
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

export default UserCreate;
