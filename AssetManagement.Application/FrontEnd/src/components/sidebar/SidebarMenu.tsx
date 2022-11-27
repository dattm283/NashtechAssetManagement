import { Menu, usePermissions  } from 'react-admin';
import React from 'react';
import { Typography } from '@mui/material';
import CardMedia from '@mui/material/CardMedia';
import logo from '../../assets/images/logo-transparent.png';
import HomeIcon from '@mui/icons-material/Home';

// import 
const SidebarMenu = () => {
    const { isLoading, permissions } = usePermissions();
    return(
    <Menu 
    sx={{
        width: "950px",
        margin: "10px",
        paddingTop: "40px",
        color: "#000",
        ".MuiMenuItem-root" :{
            height:"50px",
            backgroundColor: "#eff1f5",
            fontWeight: "900",
            color: "#000",
            ".RaMenuItemLink-icon" :{
                color: "#000",
            },
            marginBottom: "3px",
        },
        ".RaMenuItemLink-active": {
            color: "#cf2338",
            backgroundColor: '#cf2338',
            "&.RaMenuItemLink-active" : {
                color: "#fff",
            },
            "& .RaMenuItemLink-icon" :{
                color: "#fff",
            },
            marginBottom: "3px",
        }
    }}
    >
        <CardMedia
        component="img"
        alt="logo"
        height="auto"
        sx={
            {maxWidth:"100px",
        }
        }
        image={logo}
        />
        <Typography variant="h3" component="h2" color="secondary" fontSize='1rem' fontWeight="bold" className="appTitleMenuBar" mb={3}>Online Asset Management</Typography>
        <Menu.Item to="/home" primaryText="Home"/>
            {permissions === 'Admin' ?<Menu.Item to="/users" primaryText="Manage User" />:null }
            {permissions === 'Admin' ?<Menu.Item to="/assets" primaryText="Manage Asset" />:null }
            {permissions === 'Admin' ?<Menu.Item to="/assignments" primaryText="Manage Assignment" />:null  }
            {permissions === 'Admin' ?<Menu.Item to="/returning" primaryText="Request for Returning" />:null  }
            {permissions === 'Admin' ?<Menu.Item to="/report" primaryText="Report" />:null}
    </Menu >
    )
};

export default SidebarMenu;