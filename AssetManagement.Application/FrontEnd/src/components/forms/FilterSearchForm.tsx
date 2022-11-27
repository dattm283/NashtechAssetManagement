import { useInput } from "react-admin";
import React from "react";
import { Divider, IconButton, InputAdornment, InputBase, Paper, TextField } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
export default (props) => {
   const {
      field,
      fieldState: { isTouched, invalid, error },
      formState: { isSubmitted }
   } = useInput(props);



   return <Paper
      component="form"
      sx={{ p: '2px 4px', display: 'flex', alignItems: 'center', width: 250 }}
   >
      <InputBase
         sx={{ ml: 1, flex: 1 }}
         placeholder="Search"
         inputProps={{ 'aria-label': 'search' }}
      />
      <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
      <IconButton type="button" sx={{ p: '10px' }} aria-label="search">
         <SearchIcon />
      </IconButton>
   </Paper>
}