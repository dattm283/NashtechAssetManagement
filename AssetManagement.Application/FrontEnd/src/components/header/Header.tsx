import React, { useState, useEffect } from "react";
import { AppBar, UserMenu, useShowContext, Button } from "react-admin";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import { headerStyle } from "../../styles/headerStyle";

const Header = () => {
    const [userName, setUserName] = useState(
        localStorage.getItem("userName") || ""
    );
    useEffect(() => {
        setUserName(localStorage.getItem("userName") || "");
    });
    const button = (
        <Button
            color="inherit"
            endIcon={<ArrowDropDownIcon />}
            label={userName}
            sx={headerStyle.userMenuButtonStyle}
        />
    );
    return (
        <AppBar
            userMenu={<UserMenu icon={button} />}
            open={true}
            sx={headerStyle.appBarStyle}
        />
    );
};

export default Header;
