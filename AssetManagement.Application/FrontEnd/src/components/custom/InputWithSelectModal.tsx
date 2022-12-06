import { Grid, IconButton, InputAdornment } from "@mui/material";
import React, { forwardRef } from "react";
import { TextInput } from "react-admin";
import SearchIcon from '@mui/icons-material/Search';

const InputWithSelectModal = ({ handleClick, source, innerRef }) => {
    return (
        <Grid item xs={8}
            ref={innerRef}
            sx={{
                margin: 0,
                padding: 0
            }}
        >
            <TextInput
                fullWidth
                label={false}
                name={source}
                source={source}
                disabled
                helperText={false}
                InputLabelProps={{ shrink: false }}
                InputProps={{
                    endAdornment:
                        <InputAdornment position="end">
                            <IconButton onClick={handleClick}>
                                <SearchIcon />
                            </IconButton>
                        </InputAdornment>,
                }}
            />
        </Grid>
    )
}

export default InputWithSelectModal;