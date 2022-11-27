import React, { useState } from 'react'
import { Form, TextInput } from 'react-admin';
import { Box, IconButton, ThemeProvider, Typography } from '@mui/material'
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import { theme } from '../../theme'

function SimpleForm({ handleSubmit, handleClose, errorMessage, setErrorMessage }) {
    const [isValid, setIsValid] = useState(true);

    const requiredInput = (values) => {
        const errors: Record<string, any> = {};
        if (!values.name) {
            errors.userName = "This is required";
            setIsValid(true);
        } else if (!values.prefix) {
            errors.password = "This is required";
            setIsValid(true);
        } else {
            setIsValid(false);
            return {};
        }
        return errors;
    }

    return (
        <ThemeProvider theme={theme}>
            <Box sx={{ m:0, p:0 }} flex="true" flexDirection="column">
                <Form validate={requiredInput} onSubmit={handleSubmit}>
                    <Box
                        display="flex"
                        flexDirection="row"
                        width="430px"
                        padding="6px 16px"
                        boxSizing="border-box"
                        sx={{
                            backgroundColor:"#eff1f5",
                            // marginBlockEnd:"-10px"
                        }}
                    >
                        <TextInput
                            fullWidth
                            name="name"
                            source="name"
                            resettable 
                            onFocus={(e) => setErrorMessage("")}
                            sx={{ 
                                width:"60%", 
                                borderRadius:"0px", 
                                '& .MuiOutlinedInput-root': {
                                    '& fieldset': {
                                        borderRadius: `4px 0px 0px 4px`,
                                    },
                                }, 
                            }}
                            onKeyDown={(e) => e.stopPropagation()}
                            helperText={false}
                        />
                        <TextInput
                            fullWidth
                            name="prefix"
                            source="prefix"
                            resettable 
                            onFocus={(e) => setErrorMessage("")}
                            sx={{ 
                                width:"20%", 
                                borderRadius:"0px", 
                                '& .MuiOutlinedInput-root': {
                                    '& fieldset': {
                                        borderRadius: `0px 4px 4px 0px`,
                                    },
                                }, 
                            }}
                            onKeyDown={(e) => e.stopPropagation()}
                            helperText={false}
                        />
                        <Box display="flex" flexDirection="row">
                            <IconButton sx={{ color:theme.palette.secondary.main }} type="submit" disabled={isValid}>
                                <CheckIcon fontSize='small' />
                            </IconButton>
                            <IconButton onClick={handleClose}>
                                <ClearIcon fontSize='small' htmlColor='black'/>
                            </IconButton>
                        </Box>
                    </Box>
                </Form>
                {errorMessage!="" && 
                <Typography 
                    variant="h6" 
                    sx={{ 
                        color:theme.palette.secondary.main, fontSize:"12px", 
                        padding:"4px 16px"
                    }}
                >
                    {errorMessage}
                </Typography>}
            </Box>
        </ThemeProvider>
    )
}

export default SimpleForm;