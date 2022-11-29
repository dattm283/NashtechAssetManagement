import { createTheme } from '@mui/material/styles';
declare module '@mui/material/styles' {
  interface Palette {
    neutral: Palette['primary'];
  }

  // allow configuration using `createTheme`
  interface PaletteOptions {
    neutral?: PaletteOptions['primary'];
  }
}

declare module '@mui/material/Button' {
  interface ButtonPropsColorOverrides {
    neutral: true;
  }
}
export const theme = createTheme({
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
    neutral: {
      light: '#874b94',
      main: '#6a1f7a',
      dark: '#4a1555',
      contrastText: '#fff',
    },
    contrastThreshold: 3,
    tonalOffset: 0.2,
  },
});