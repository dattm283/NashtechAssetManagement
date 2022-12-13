import * as React from "react";
import {
    Toolbar,
    useNotify,
    ThemeProvider,
    SaveButton,
} from "react-admin";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { theme } from "../../theme";
import { formToolbarStyle } from "../../styles/formToolbarStyle";

const UserEditToolbar = ({disabled}) => {
    const navigate = useNavigate();
    return (
        <ThemeProvider theme={theme}>
            <Toolbar sx={formToolbarStyle.toolbarStyle}>
                <SaveButton
                    label="Save"
                    type="submit"
                    variant="contained"
                    onClick={() => {
                        localStorage.removeItem("RaStore.user.listParams");
                    }}
                    icon={<></>}
                    color="secondary"
                    disabled={disabled}
                />
                <Button
                    variant="outlined"
                    onClick={() => {
                        localStorage.removeItem("RaStore.user.listParams");
                        navigate("/user");
                    }}
                    color="secondary"
                    id="editUserCancelButton"
                >
                    Cancel
                </Button>
            </Toolbar>
        </ThemeProvider>
    );
};
export default UserEditToolbar;
