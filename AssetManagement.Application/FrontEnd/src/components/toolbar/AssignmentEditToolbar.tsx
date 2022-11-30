import * as React from "react";
import { SaveButton, Toolbar, useRedirect, useNotify, ThemeProvider } from 'react-admin';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { theme } from '../../theme';
import { formToolbarStyle } from "../../styles/formToolbarStyle";

const AssignmentEditToolbar = ({ isEnable }) => {
    const notify = useNotify();
    const navigate = useNavigate();
    
    return (
        <ThemeProvider theme={theme}>
            <Toolbar sx={formToolbarStyle.toolbarStyle}>
                <SaveButton
                    {... isEnable && { 'alwaysEnable' : 'true' }}
                    {... !isEnable && { 'disabled' : 'true' }}
                    alwaysEnable
                    label="Save"
                    mutationOptions={{
                        onSuccess: () => {
                            // notify('Element updated');
                            navigate("/assignments")
                        }
                    }
                    }
                    type="button"
                    variant="contained"
                    icon={<></>}
                    color="secondary"
                    disabled={false}
                />
                <Button
                    variant="outlined"
                    onClick={(e) => navigate("/assignments")}
                    color="secondary"
                >Cancel</Button>
            </Toolbar>
        </ThemeProvider>
    );
};
export default AssignmentEditToolbar;