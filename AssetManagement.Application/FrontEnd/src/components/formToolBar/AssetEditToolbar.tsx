import * as React from "react";
import { SaveButton, Toolbar, useRedirect, useNotify, ThemeProvider } from 'react-admin';
import {Button} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {theme} from '../../theme';
import { useUpdate, useRecordContext } from 'react-admin';


const AssetEditToolbar = () => {
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
                        notify('Element updated');
                    }}
                }
                // type="button"
                variant="contained"
                icon={<></>}
                color="secondary"
            
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
export default AssetEditToolbar;