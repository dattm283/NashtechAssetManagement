import { Box, Container, createTheme, ThemeProvider, Typography, unstable_createMuiStrictModeTheme } from "@mui/material";
import React, { useEffect, useState } from "react";
import { DateInput, SelectInput, SimpleForm, TextInput, Title } from "react-admin";
import { useNavigate, useParams } from "react-router-dom";
import { assetProvider } from "../../providers/assetProvider/assetProvider";
import { formStyle } from "../../styles/formStyle";
import RadioButtonGroup from "../../components/custom/RadioButtonGroupInput";
import UserEditToolbar from "../../components/toolbar/UserEditToolbar";

const UserEdit = () => {
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme();
    const navigate = useNavigate();
    const params= useParams();
    const [isValid, setIsValid] = useState(true);
    const errors = {dob:'', joinedDate:''};
    const [user, setUser] = useState({
        firstName:'',
        lastName:'',
        staffCode:'',
        dateOfBirth:'',
        joinedDate:'',
        gender:'',
        type:''
    });
    let staffCode;

    useEffect(()=>{
        //console.log(params);
        staffCode= params.id
        assetProvider.getOne('user', { id:staffCode }).then(res => {
            setUser(res.data.result);
            //console.log(user);
        })
    },[staffCode])

    const Validate = (form) => {
        const dob = new Date(form.dob);
        const joined = new Date(form.joinedDate);
        let legal = new Date(Date.now());
        legal.setFullYear(legal.getFullYear() - 18);
        setIsValid(false);
        if(dob > legal){
            errors.dob = "User is under 18. Please select a different date";
        }
        else if(form.joined != '' && form.dob == '' ){
            errors.joinedDate = "Please select Date of Birth";
        }
        else if(joined < new Date(
            dob.getFullYear() + 18, dob.getMonth(), dob.getDate(),
            dob.getHours(), dob.getMinutes(), dob.getSeconds())){
                errors.joinedDate = "User under the age 18 may not join the company. Please select a different date";
        }
        else if(joined.getDay() > 5 || joined.getDay() < 1){
            errors.joinedDate = "Joined date is Saturday or Sunday. Please select a different date"
        }
        else{
            setIsValid(true);
            return {};
        }

        return errors;
    }

    function EditUser(e) {
        //console.log(e);
        const changes = {
            dob: e.dob,
            gender: e.gender==='Male'?0:1,
            joinedDate: e.joinedDate,
            Type: e.type
        }
        console.log(changes)
        assetProvider.update('user', {id: user.staffCode, data: changes, previousData: user}).then(
            (response) => {
                //console.log(response)
                navigate("/user", {state: {user}})
            }
        ).catch(error => console.log(error))
    }

    return(
        <ThemeProvider theme={theme}>
            <Title title="Manage User > Edit User" />
            <Container component="main">
                <Box sx={formStyle.boxTitleStyle}>
                    <Typography
                        component="h3"
                        variant="h5"
                        sx={formStyle.formTitle}
                    >
                        Edit User
                    </Typography>

                    {user.staffCode?
                        <SimpleForm
                            validate={Validate}
                            toolbar={<UserEditToolbar disabled={!isValid}/>}
                            onSubmit={(e)=>{EditUser(e)}}
                        >
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    First Name
                                </Typography>
                                <TextInput
                                    label={false}
                                    name="firstName"
                                    source="firstName"
                                    sx={formStyle.textInputStyle }
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                    defaultValue={user.firstName}
                                    fullWidth
                                    disabled
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Last Name
                                </Typography>
                                <TextInput
                                    label={false}
                                    name="lastName"
                                    source="lastName"
                                    sx={formStyle.textInputStyle }
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                    defaultValue={user.lastName}
                                    fullWidth
                                    disabled
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Date of Birth *
                                </Typography>
                                <DateInput
                                    label={false}
                                    name="dob"
                                    source="dob"
                                    sx={formStyle.textInputStyle }
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                    defaultValue={new Date(user.dateOfBirth)}
                                    fullWidth
                                    required
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Gender *
                                </Typography>
                                <RadioButtonGroup
                                    label=""
                                    sx={formStyle.textInputStyle}
                                    source="gender"
                                    choices={[
                                        { id: 0, name: "Male" },
                                        { id: 1, name: "Female" }
                                    ]}
                                    optionText="name"
                                    optionValue="name"
                                    defaultValue={user.gender}
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Joined Date *
                                </Typography>
                                <DateInput
                                    label={false}
                                    name="joinedDate"
                                    source="joinedDate"
                                    sx={formStyle.textInputStyle }
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                    defaultValue={new Date(user.joinedDate)}
                                    fullWidth
                                    required
                                />
                            </Box>
                            <Box sx={formStyle.boxStyle}>
                                <Typography
                                    variant="h6"
                                    sx={formStyle.typographyStyle}
                                >
                                    Type *
                                </Typography>

                                <SelectInput
                                    label="Type"
                                    sx={formStyle.textInputStyle}
                                    source="type"
                                    name="type"
                                    choices={[
                                        { id: 0, name: "Admin" },
                                        { id: 1, name: "Staff" }
                                    ]}
                                    optionText="name"
                                    optionValue="name"
                                    defaultValue={user.type}
                                    required
                                />
                            </Box>
                        </SimpleForm>
                    :""}
                </Box>
            </Container>
        </ThemeProvider>
    )
}

export default UserEdit;
