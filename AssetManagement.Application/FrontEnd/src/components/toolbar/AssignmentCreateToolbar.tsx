import * as React from "react";
import { SaveButton, Toolbar, useRedirect, useNotify, ThemeProvider } from 'react-admin';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { theme } from '../../theme';
import { formToolbarStyle } from "../../styles/formToolbarStyle";

export default ({ isEnable }) => {
    const notify = useNotify();
    const navigate = useNavigate();

    return (
        <ThemeProvider theme={theme}>
            <Toolbar sx={formToolbarStyle.toolbarStyle}>
                {isEnable ? (
                    <SaveButton
                        alwaysEnable
                        label="Save"
                        mutationOptions={{
                            onSuccess: () => {
                                localStorage.removeItem("RaStore.assignments.listParams");
                                notify('Assignment created successfully!');
                                navigate("/assignments")
                            }
                        }
                        }
                        type="button"
                        variant="contained"
                        icon={<></>}
                        color="secondary"
                    />
                ) : (
                    <SaveButton
                        disabled
                        label="Save"
                        mutationOptions={{
                            onSuccess: () => {
                                localStorage.removeItem("RaStore.assignments.listParams");
                                notify('Assignment created successfully!');
                                navigate("/assignments")
                            }
                        }
                        }
                        type="button"
                        variant="contained"
                        icon={<></>}
                        color="secondary"
                    />
                )}

                <Button
                    variant="outlined"
                    onClick={(e) => {
                        localStorage.removeItem("RaStore.assignments.listParams");
                        navigate("/assignments")
                    }}
                    color="secondary"
                >Cancel</Button>
            </Toolbar>
        </ThemeProvider>
    );
};