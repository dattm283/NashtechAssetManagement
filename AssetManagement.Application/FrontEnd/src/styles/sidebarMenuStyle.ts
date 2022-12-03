import { SxProps, Theme } from "@mui/material";

export const sidebarMenuStyle: Record<string, SxProps<Theme> | undefined> = {
    menuStyle: {
        // minWidth: "950px",
        margin: "10px",
        paddingTop: "40px",
        color: "#000",
        ".MuiMenuItem-root": {
            height: "50px",
            backgroundColor: "#eff1f5",
            fontWeight: "900",
            color: "#000",
            ".RaMenuItemLink-icon": {
                color: "#000",
            },
            marginBottom: "3px",
        },
        ".MuiMenuItem-root:hover": {
            height: "50px",
            backgroundColor: "#eff1f5",
            fontWeight: "900",
            color: "#cf2338",
            ".RaMenuItemLink-icon": {
                color: "#cf2338",
            },
            marginBottom: "3px",
            opacity: "0.8"
        },
        ".RaMenuItemLink-active": {
            color: "#cf2338",
            backgroundColor: '#cf2338',
            "&.RaMenuItemLink-active": {
                color: "#fff",
            },
            "& .RaMenuItemLink-icon": {
                color: "#fff",
            },
            marginBottom: "3px",
        },
        ".RaMenuItemLink-active:hover": {
            color: "#000",
            backgroundColor: '#000',
            "&.RaMenuItemLink-active": {
                color: "#fff",
            },
            "& .RaMenuItemLink-icon": {
                color: "#fff",
            },
            marginBottom: "3px",
            opacity: "0.8"
        },
        "&.RaMenu-closed": {
            width: "240px"
        }
    },
    cardMediaStyle: {
        maxWidth: "100px",
    }
}