import { Menu, usePermissions  } from 'react-admin';
import React from 'react';
import { Typography } from '@mui/material';
import PeopleIcon from '@mui/icons-material/People';
import LabelIcon from '@mui/icons-material/Label';
import AssignmentIcon from '@mui/icons-material/Assignment';
import RedoIcon from '@mui/icons-material/Redo';
import PieChartIcon from '@mui/icons-material/PieChart';
import logo from '../../assets/images/logo-transparent.png';
import HomeIcon from '@mui/icons-material/Home';
import { createTheme } from '@mui/material/styles';

const theme = createTheme();

const SidebarMenu = () => {
    const { isLoading, permissions } = usePermissions();
    return(
    <Menu>
        <img src={logo} alt="logo" className="logo" />
        <Typography textAlign="center" variant="h3" component="h2" color="#cf2338" fontSize='1rem' fontWeight="bold" mb={3}>Online Asset Management</Typography>
        <Menu.Item to="/home" primaryText="Home" leftIcon={<HomeIcon />} />
            {permissions === 'Admin' ?<Menu.Item to="/users" primaryText="Manage User" leftIcon={<PeopleIcon />} />:null }
            {permissions === 'Admin' ?<Menu.Item to="/assets" primaryText="Manage Asset" leftIcon={<LabelIcon />} />:null }
            {permissions === 'Admin' ?<Menu.Item to="/assignments" primaryText="Manage Assignment" leftIcon={<AssignmentIcon />} />:null  }
            {permissions === 'Admin' ?<Menu.Item to="/returning" primaryText="Request for Returning" leftIcon={<RedoIcon />} />:null  }
            {permissions === 'Admin' ?<Menu.Item to="/report" primaryText="Report" leftIcon={<PieChartIcon />} />:null}
    </Menu >
    )
};

export default SidebarMenu;