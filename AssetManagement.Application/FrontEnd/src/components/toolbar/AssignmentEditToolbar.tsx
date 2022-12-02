import React, { useEffect } from "react";
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
                {isEnable ? (
                    <SaveButton
                        alwaysEnable
                        label="Save"
                        mutationOptions={{
                            onSuccess: () => {
                                localStorage.setItem("RaStore.assets.listParams",
                                    `{"displayedFilters":{},"filter":{},"order":"DESC","page":1,"perPage":5,"sort":"noNumber"}`)
                                notify('Element updated');
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
                                notify('Element updated');
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
                    onClick={(e) => navigate("/assignments")}
                    color="secondary"
                >Cancel</Button>
            </Toolbar>
        </ThemeProvider>
    );
};
export default AssignmentEditToolbar;