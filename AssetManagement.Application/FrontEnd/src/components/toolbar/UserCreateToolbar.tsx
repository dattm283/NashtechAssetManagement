import * as React from "react";
import { SaveButton, Toolbar, useRedirect, useNotify, ThemeProvider } from 'react-admin';
import {Button} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {theme} from '../../theme';
import { formToolbarStyle } from "../../styles/formToolbarStyle";

const AssetCreateToolbar = ({ disable, url }) => {
    const notify = useNotify();
    const navigate = useNavigate();
    return (
        <ThemeProvider theme={theme}>
        <Toolbar sx={formToolbarStyle.toolbarStyle} >
            <SaveButton
                label="Save"
                mutationOptions={{
                    onSuccess: () => {
                        navigate(url)
                    }}
                }
                type="button"
                variant="contained"
                icon={<></>}
                color="secondary"
                disabled={disable}
            />
            <Button
                variant="outlined"
                onClick={(e) => navigate(url)}
                color="secondary"
            >Cancel</Button>
        </Toolbar>
        </ThemeProvider>
    );
};
export default AssetCreateToolbar;