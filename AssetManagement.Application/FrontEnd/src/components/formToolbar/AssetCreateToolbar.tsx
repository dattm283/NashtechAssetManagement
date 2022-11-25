import * as React from "react";
import { SaveButton, Toolbar, useRedirect, useNotify, ThemeProvider } from 'react-admin';
import {Button} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {theme} from '../../theme';

const AssetCreateToolbar = ({ disable }) => {
    const notify = useNotify();
    const navigate = useNavigate();
    return (
        <ThemeProvider theme={theme}>
        <Toolbar sx={{display:"flex", justifyContent:"end", mt:"20px", backgroundColor:"#fff"}} >
            <SaveButton
                style={{ margin:"10px"}}
                label="Save"
                mutationOptions={{
                    onSuccess: () => {
                        // notify('Element updated');
                        navigate("/assets")
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
                onClick={(e) => navigate("/assets")}
                style={{ margin:"10px"}}
                color="secondary"
            >Cancel</Button>
        </Toolbar>
        </ThemeProvider>
    );
};
export default AssetCreateToolbar;