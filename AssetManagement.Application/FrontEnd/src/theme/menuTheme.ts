import { createTheme } from '@mui/material/styles';

export const menuTheme = createTheme({
    palette: {
        secondary: {
            light: '#d84f5f',
            main: '#cf2338',
            dark: '#901827',
            contrastText: '#fff',
        },
        primary: {
            light: '#616161',
            main: '#424242',
            dark: '#212121',
            contrastText: '#fff',
        },
        contrastThreshold: 3,
        tonalOffset: 0.2,
    },
});