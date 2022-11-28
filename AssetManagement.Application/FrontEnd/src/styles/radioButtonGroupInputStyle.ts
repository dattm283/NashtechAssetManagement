import { SxProps, Theme } from "@mui/material";

export const radioButtonGroupInputStyle: Record<string, SxProps<Theme> | undefined> = {
    radioButtonGroupInput: {
        ".PrivateSwitchBase-input + span":{
            ".MuiSvgIcon-root:nth-of-type(1)" : {
                width:"20px",
                height:"20px",
                color:'#000',
                backgroundColor: '#fff',
                borderRadius: '50%',
            },
            ".MuiSvgIcon-root:nth-of-type(2)":{
                width:"20px",
                height:"20px",
                color:'#fff',
            },
        },
        ".PrivateSwitchBase-input:checked + span ":{
            ".MuiSvgIcon-root:nth-of-type(1)":{
            width:"20px",
            height:"20px",
            color:'#cf2338',
            backgroundColor: '#cf2338',
            borderRadius: '50%',
            },
            ".MuiSvgIcon-root:nth-of-type(2)":{
                width:"20px",
                height:"20px",
                color:'#fff',
            }

        }
    },
};
