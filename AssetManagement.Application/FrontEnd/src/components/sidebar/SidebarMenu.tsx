import { Menu, usePermissions } from 'react-admin';
import React from 'react';
import { Typography } from '@mui/material';
import CardMedia from '@mui/material/CardMedia';
import logo from '../../assets/images/logo-transparent.png';
import { sidebarMenuStyle } from '../../styles/sidebarMenuStyle';

// import 
const SidebarMenu = () => {
    const { isLoading, permissions } = usePermissions();
    return(
    <Menu 
    sx={sidebarMenuStyle.menuStyle}
    >
        <CardMedia
        component="img"
        alt="logo"
        height="auto"
        sx={sidebarMenuStyle.cardMediaStyle}
        image={logo}
        />
        <Typography variant="h3" component="h2" color="secondary" fontSize='1rem' fontWeight="bold" className="appTitleMenuBar" mb={3}>Online Asset Management</Typography>
        <li><Menu.Item to="/home" primaryText="Home"/></li>
        {permissions === 'Admin' ?<li><Menu.Item to="/user" primaryText="Manage User" /></li>:null }
        {permissions === 'Admin' ?<li><Menu.Item to="/assets" primaryText="Manage Asset" /></li>:null }
        {permissions === 'Admin' ?<li><Menu.Item to="/assignments" primaryText="Manage Assignment" /></li>:null  }
        {permissions === 'Admin' ?<li><Menu.Item to="/returning" primaryText="Request for Returning" /></li>:null  }
        {permissions === 'Admin' ?<li><Menu.Item to="/report" primaryText="Report" /></li>:null}
    </Menu >
    )
};

export default SidebarMenu;