import * as React from 'react';
import { AppBar } from 'react-admin';
import Typography from '@mui/material/Typography';

const Header = (props) => (
    <AppBar
        sx={{
            "& .RaAppBar-title": {
                flex: 1,
                display: "flex",
                justifyContent: "space-between",
                // Cant add these props (Ctrl+Click to see more which props this component get)
                // backgroundColor: "black",
                textOverflow: "ellipsis",
                whiteSpace: "nowrap",
                overflow: "hidden",
            },
        }}
        {...props}
    >
        <Typography
            variant="h6"
            color="inherit"
            className="Header"
            id="react-admin-title"
        />
        <span />
    </AppBar>
);

export default Header;