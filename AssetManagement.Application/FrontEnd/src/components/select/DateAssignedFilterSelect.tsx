import * as React from 'react';
import { styled } from '@mui/material/styles';
import SearchIcon from '@mui/icons-material/Search';
import { InputAdornment } from '@mui/material';
import { useTranslate } from 'ra-core';

import { CommonInputProps, DateInput } from 'react-admin';
import { TextInput, TextInputProps } from 'react-admin';

export const DateAssignedFilterSelect = (props: SearchInputProps) => {
    const translate = useTranslate();

    return (
        <DateInput
            // hiddenLabel
            label={"Assigned Date"}
            resettable
            // placeholder={""}
            // InputLabelProps={{ shrink: false }}
            size="small"
            {...props}
        />
    ); 
};

export type SearchInputProps = CommonInputProps & TextInputProps;

const PREFIX = 'RaSearchInput';

const StyledTextInput = styled(TextInput, {
    name: PREFIX,
    overridesResolver: (props, styles) => styles.root,
})({
    marginTop: 0,
});
