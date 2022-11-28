import { SxProps, Theme } from "@mui/material";

export const headerStyle: Record<string, SxProps<Theme> | undefined> = {
    userMenuButtonStyle: {
        textTransform: "none",  
        font: "20px Roboto,Helvetica,Arial,sans-serif",
        fontWeight: "bold"
    },
    appBarStyle: {
        color:"white",
        padding: "5px 10px",
        ".RaAppBar-menuButton" : {
            display: "none"
        },
        ".RaLoadingIndicator-root": {
            display:"none"
        },
        ".RaLoadingIndicator-loadedIcon": {
            display:"none"
        },
        ".RaAppBar-title": {
            paddingLeft: "10px",
            fontWeight: "bold"
        }
    }
}