import { SxProps, Theme } from "@mui/material";

export const formToolbarStyle: Record<string, SxProps<Theme> | undefined> = {
    toolbarStyle: {
        display: "flex",
        justifyContent: "end",
        mt: "20px",
        backgroundColor: "#fff",
        "button" : {margin: "10px"},
    }
};
