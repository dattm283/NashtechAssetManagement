import Select, { SelectChangeEvent } from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import { useInput, useListContext } from 'react-admin';
import React, { useEffect, useState } from 'react';
import { Checkbox, FormControl, InputLabel, ListItemText } from '@mui/material';
import FilterAltIcon from '@mui/icons-material/FilterAlt';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
   PaperProps: {
      style: {
         maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
         width: "250px",
      },
   },
};

export default (props) => {
   const {
      field,
      fieldState: { isTouched, invalid, error },
      formState: { isSubmitted }
   } = useInput(props);

   const { setFilters, displayedFilters, setPerPage, filterValues } = useListContext();

   const [states, setStates] = useState<string[]>([]);

   const handleChange = (event: SelectChangeEvent<typeof states>) => {
      const {
         target: { value },
      } = event;
      if (value.includes("all")) {
         if (states.length == 4) {
            setStates([]);
         } else {
            handleSelectAll();
         }
      } else {

         setStates(
            // On autofill we get a stringified value.
            typeof value === 'string' ? value.split(',') : value,
         );
      }
   };

   useEffect(() => {
      var tmp = filterValues.categories;
      setFilters({ states: states, categories: tmp }, displayedFilters);
   }, [states])

   useEffect(() => {
      setPerPage(5)
   }, [])

   const handleSelectAll = () => {
      let arr: Array<string> = [];
      props.statesList.forEach(({ value, text }) => {
         arr.push(value);
      });
      setStates(arr);
   }
   return (
      <FormControl variant='standard' sx={{ m: 1, width: 120 }}>
         <InputLabel id="demo-multiple-name-label">State</InputLabel>
         <Select
            labelId="demo-multiple-name-label"
            {...field}
            multiple
            value={states}
            renderValue={(selected) => {
               if (selected.length == 4) {
                  return "States";
               } else {
                  selected.map(key => { return props.statesList[key].text ? props.statesList[key].text : "" }).join(', ');
               }
            }}
            onChange={handleChange}
            MenuProps={MenuProps}
            autoWidth={false}
            IconComponent={() => <FilterAltIcon />}
         >
            <MenuItem value={"all"}>
               <Checkbox sx={{
                  color: "#cf2338",
                  '&.Mui-checked': {
                     color: "#cf2338",
                  },
               }} checked={states.length == 4} />
               <ListItemText primary="All" />
            </MenuItem>
            {props.statesList.map((state) => (
               <MenuItem key={state.value} value={state.value}>
                  <Checkbox sx={{
                     color: "#cf2338",
                     '&.Mui-checked': {
                        color: "#cf2338",
                     },
                  }} checked={states.indexOf(state.value) > -1} />
                  <ListItemText primary={state.text} />
               </MenuItem>
            ))}
         </Select>
      </FormControl >
   );
}