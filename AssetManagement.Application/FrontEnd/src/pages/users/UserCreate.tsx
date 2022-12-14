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
import { Box, Button, Typography, Container } from "@mui/material";
import {
  createTheme,
  ThemeProvider,
  unstable_createMuiStrictModeTheme,
} from "@mui/material/styles";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import UserCreateToolbar from "../../components/toolbar/UserCreateToolbar";
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
    const errors: Record<string, any> = {};

    if (!values.firstname || values.firstname.trim().length === 0) {
      errors.firstname = "This is required";
    }

    if (values.firstname) {
      if (values.firstname.length > 50) {
        errors.firstname = "First name must be between 1 and 50 characters!";
      }
    }

    if (!values.lastname || values.lastname.trim().length === 0) {
      errors.lastname = "This is required";
    }

    if (values.lastname) {
      if (values.lastname.length > 50) {
        errors.lastname = "Last name must be between 1 and 50 characters!";
      }
    }

    if (!values.dob) {
      errors.dob = "This is required";
    }

    let ageDob = getAge(values.dob);
    if (values.dob) {
      if (ageDob < 18) {
        errors.dob = "User is under 18. Please select a different date";
      }
    }

    if (!values.joinedDate) {
      errors.joinedDate = "This is required";
    }

    if (values.joinedDate) {
      if (!values.dob) {
        errors.joinedDate = "Please Select Date of Birth";
      }

      let condition = new Date(values.dob);
      condition.setFullYear(condition.getFullYear() + 18);
      let joinedDate = new Date(values.joinedDate);
      if (joinedDate < condition) {
        errors.joinedDate =
          "User under the age of 18 may not join company. Please select a different date";
      } else if (isWeekend(values.joinedDate)) {
        errors.joinedDate =
          "Joined date is Saturday or Sunday. Please select a different date";
      }
    }

    if (!values.role) {
      errors.role = "This is required";
    }

    if (Object.keys(errors).length === 0) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    return errors;
  };

  function getAge(DOB) {
    var today = new Date();
    var birthDate = new Date(DOB);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  }

  function isWeekend(joinedDate) {
    let date = new Date(joinedDate);
    return date.getDay() === 6 || date.getDay() === 0;
  }

  return (
    <ThemeProvider theme={theme}>
      <Title title="Manage Asset > Create User" />
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
              toolbar={<UserCreateToolbar disable={isValid} />}
            >
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  First Name <span className="red">*</span>
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
                  Last Name <span className="red">*</span>
                </Typography>
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
                  Date of Birth <span className="red">*</span>
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
                  Gender <span className="red">*</span>
                </Typography>
                <RadioButtonGroup
                  label=""
                  defaultValue="1"
                  name="gender"
                  source="gender"
                  choices={[
                    {
                      gender_id: "1",
                      gender: "Female",
                    },
                    {
                      gender_id: "0",
                      gender: "Male",
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
                  Joined Date <span className="red">*</span>
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
                  Type <span className="red">*</span>
                </Typography>
                <SelectInput
                  validate={required()}
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
