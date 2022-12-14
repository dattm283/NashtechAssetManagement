import {
  Box,
  Container,
  createTheme,
  ThemeProvider,
  Typography,
  unstable_createMuiStrictModeTheme,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import {
  DateInput,
  minValue,
  required,
  SelectInput,
  SimpleForm,
  TextInput,
  Title,
  useNotify,
} from "react-admin";
import { useNavigate, useParams } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { formStyle } from "../../styles/formStyle";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import UserEditToolbar from "../../components/toolbar/UserEditToolbar";

const UserEdit = () => {
  let theme = createTheme();
  theme = unstable_createMuiStrictModeTheme();
  const navigate = useNavigate();
  const params = useParams();
  const [isValid, setIsValid] = useState(false);
  const [minJd, setMinJd] = useState("");
  const notify = useNotify();
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    staffCode: "",
    dateOfBirth: "",
    joinedDate: "",
    gender: "",
    type: "",
  });
  let staffCode;

  useEffect(() => {
    staffCode = params.id;
    assetProvider.getOne("user", { id: staffCode }).then((res) => {
      setUser(res.data.result);
      setMinJd(res.data.result.dateOfBirth);
    });
  }, [staffCode]);

  const validate = (form) => {
    if (
      form.firstName === null ||
      form.lastName === null ||
      form.dob === null ||
      form.joinedDate === null ||
      form.Gender === null ||
      form.type === null ||
      form.firstName.trim().length === 0 ||
      form.lastName.trim().length === 0
    ) {
      setIsValid(false);
    }

    const dob = new Date(form.dob);
    setMinJd(form.dob);
    const joined = new Date(form.joinedDate);
    let legal = new Date(Date.now());
    legal.setFullYear(legal.getFullYear() - 18);
    let err = {};
    let dobE = "";
    let jdE = "";
    if (dob > legal) {
      dobE = "User is under 18. Please select a different date";
    }

    if (form.joined !== null && form.dob === null) {
      jdE = "Please select Date of Birth";
    }

    dob.setFullYear(dob.getFullYear() + 18);
    if (joined < dob) {
      jdE =
        "User under the age 18 may not join the company. Please select a different date";
    }

    if (joined.getDay() < 1 || joined.getDay() > 5) {
      jdE = "Joined date is Saturday or Sunday. Please select a different date";
    }

    if (
      form.dob === user.dateOfBirth &&
      form.joinedDate === user.joinedDate &&
      form.Gender === user.gender &&
      form.type === user.type
    ) {
      setIsValid(false);
      return {};
    }

    if (dobE === "" && jdE === "") {
      setIsValid(true);
      return {};
    }

    if (dobE !== "") err = { ...err, dob: dobE };
    if (jdE != "") err = { ...err, joinedDate: jdE };
    return err;
  };

  function editUser(e) {
    const changes = {
      dob: e.dob.toString(),
      gender: e.gender === "Male" ? 0 : 1,
      joinedDate: e.joinedDate.toString(),
      Type: e.type,
    };
    assetProvider
      .update("user", { id: user.staffCode, data: changes, previousData: user })
      .then((response) => {
        localStorage.setItem("item", JSON.stringify(response.data));
        if (response.data.username == localStorage.getItem("userName")) {
          localStorage.clear();
        }
        navigate("/user");
        notify("User edited successfully!");
      })
      .catch((error) => console.log(error));
  }

  return (
    <ThemeProvider theme={theme}>
      <Title title="Manage User > Edit User" />
      <Container component="main">
        <Box sx={formStyle.boxTitleStyle}>
          <Typography component="h3" variant="h5" sx={formStyle.formTitle}>
            Edit User
          </Typography>

          {user.staffCode ? (
            <SimpleForm
              mode="onBlur"
              validate={validate}
              toolbar={<UserEditToolbar disabled={!isValid} />}
              onSubmit={(e) => {
                editUser(e);
              }}
            >
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  First Name <span className="red">*</span>
                </Typography>
                <TextInput
                  label={false}
                  name="firstName"
                  source="firstName"
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  InputLabelProps={{ shrink: false }}
                  defaultValue={user.firstName}
                  fullWidth
                  disabled
                />
              </Box>
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Last Name <span className="red">*</span>
                </Typography>
                <TextInput
                  label={false}
                  name="lastName"
                  source="lastName"
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  InputLabelProps={{ shrink: false }}
                  defaultValue={user.lastName}
                  fullWidth
                  disabled
                />
              </Box>
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Date of Birth <span className="red">*</span>
                </Typography>
                <DateInput
                  label={false}
                  name="dob"
                  source="dob"
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  InputLabelProps={{ shrink: false }}
                  defaultValue={user.dateOfBirth.toString()}
                  fullWidth
                />
              </Box>
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Gender <span className="red">*</span>
                </Typography>
                <RadioButtonGroup
                  label={false}
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  source="gender"
                  choices={[
                    { id: 1, name: "Female" },
                    { id: 0, name: "Male" },
                  ]}
                  optionText="name"
                  optionValue="name"
                  defaultValue={user.gender}
                />
              </Box>
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Joined Date <span className="red">*</span>
                </Typography>
                <DateInput
                  label={false}
                  name="joinedDate"
                  source="joinedDate"
                  InputLabelProps={{ shrink: false }}
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  inputProps={{ min: minJd ? minJd.split("T")[0] : "" }}
                  defaultValue={user.joinedDate.toString()}
                  fullWidth
                />
              </Box>
              <Box sx={formStyle.boxStyle}>
                <Typography variant="h6" sx={formStyle.typographyStyle}>
                  Type <span className="red">*</span>
                </Typography>

                <SelectInput
                  label={false}
                  validate={required()}
                  InputLabelProps={{ shrink: false }}
                  sx={formStyle.textInputStyle}
                  helperText={false}
                  source="type"
                  name="type"
                  choices={[
                    { id: 0, name: "Admin" },
                    { id: 1, name: "Staff" },
                  ]}
                  optionText="name"
                  optionValue="name"
                  defaultValue={user.type}
                />
              </Box>
            </SimpleForm>
          ) : (
            ""
          )}
        </Box>
      </Container>
    </ThemeProvider>
  );
};

export default UserEdit;
