import React, { useState, useEffect } from "react";
import { AppBar, UserMenu, useShowContext, Button, Logout } from "react-admin";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import { headerStyle } from "../../styles/headerStyle";
import { Link } from "react-router-dom";
import MenuItem from "@mui/material/MenuItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import KeyIcon from '@mui/icons-material/Key';

const Header = () => {
  const [userName, setUserName] = useState(
    localStorage.getItem("userName") || ""
  );
  useEffect(() => {
    setUserName(localStorage.getItem("userName") || "");
  });

  const ChangePassswordMenu = React.forwardRef((props, ref) => {
    return (
      <MenuItem
        component={Link}
        // It's important to pass the props to allow MUI to manage the keyboard navigation
        {...props}
        to="/change-password"
      >
        <ListItemIcon>
          <KeyIcon />
        </ListItemIcon>
        <ListItemText>Change password</ListItemText>
      </MenuItem>
    );
  });

  const button = (
    <Button
      color="inherit"
      endIcon={<ArrowDropDownIcon />}
      label={userName}
      sx={headerStyle.userMenuButtonStyle}
    />
  );

  const MyUserMenu = (props) => (
    <UserMenu {...props}>
      <ChangePassswordMenu />
      <Logout />
    </UserMenu>
  );

  return (
    <AppBar
      userMenu={<MyUserMenu icon={button} />}
      open={true}
      sx={headerStyle.appBarStyle}
    />
  );
};

export default Header;
