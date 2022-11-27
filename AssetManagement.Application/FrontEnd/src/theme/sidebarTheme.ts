export const sidebarTheme = {
    palette: {
        // ...
    },
  components: {
    // ... 
    RaMenuItemLink: {
        styleOverrides: {
            root: {
                // invisible border when not active, to avoid position flashs
                borderLeft: '3px solid transparent', 
                '&.RaMenuItemLink-active': {
                    borderLeft: '10px solid #4f3cc9',
                },
                '& .RaMenuItemLink-icon': {
                    color: '#EFC44F',
                },
            },
        },
    },
}
}