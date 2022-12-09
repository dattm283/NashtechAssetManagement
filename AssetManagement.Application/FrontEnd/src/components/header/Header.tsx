import React, { useState, useEffect } from "react";
import { AppBar, UserMenu, useShowContext, Button, Logout } from "react-admin";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import { headerStyle } from "../../styles/headerStyle";
import MenuItem from "@mui/material/MenuItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import KeyIcon from "@mui/icons-material/Key";
import UserChangePasswordModal from "../modal/changePasswordModal/UserChangePasswordModal";
import { Dialog, Modal } from "@mui/material";

const Header = () => {
  const [userName, setUserName] = useState(
    localStorage.getItem("userName") || ""
  );
  const [state, setState] = useState(false);

  useEffect(() => {
    setUserName(localStorage.getItem("userName") || "");
  });

  const ChangePassswordMenu = () => {
    return (
      <MenuItem
        onClick={() => {
          setState(true);
        }}
      >
        <ListItemIcon>
          <KeyIcon />
        </ListItemIcon>
        <ListItemText>Change password</ListItemText>
      </MenuItem>
    );
  };

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
      <Logout id="logoutButton" sx={{
        color: "black"
      }} />
    </UserMenu>
  );

  return (
    <>
      <AppBar
        userMenu={<MyUserMenu icon={button} />}
        open={true}
        sx={headerStyle.appBarStyle}
      />
      <Dialog
        open={state}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <UserChangePasswordModal stateChanger={setState} />
      </Dialog>
    </>
  );
};

export default Header;
