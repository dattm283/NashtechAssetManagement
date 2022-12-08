import Select, { SelectChangeEvent } from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import { useInput, useListContext } from 'react-admin';
import React, { useEffect, useState } from 'react';
import { Checkbox, FormControl, InputLabel, ListItemText } from '@mui/material';
import FilterAltIcon from '@mui/icons-material/FilterAlt';

const ITEM_HEIGHT = 100;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
   PaperProps: {
      style: {
         maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
         width: 250,
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
   const [categoriesList, setCategoriesList] = useState<any>();
   const [filterElement, setFilterElement] = useState<any>();
   const handleChange = (event: SelectChangeEvent<typeof states>, list) => {
      const {
         target: { value },
      } = event
      if (value.includes("all")) {
         if (states.length == list.length) {
            setStates([]);
         } else {
            handleSelectAll(list);
         }
      } else {

         setStates(
            // On autofill we get a stringified value.
            typeof value === 'string' ? value.split(',') : value,
         );
      }
   };

   useEffect(() => {
      var filterstates = filterValues.states;
      var searchString = filterValues.searchString;
      console.log(filterValues);
      setFilters({ categories: states, states: filterstates, searchString: searchString }, displayedFilters);
   }, [states])

   useEffect(() => {
      setPerPage(5)
      props.statesList.then(res => {
         var mappedId = res.map(s => s.id);
         setStates(mappedId);
      })
   }, [])

   const handleSelectAll = (list) => {
      let arr: Array<string> = [];
      list.forEach(({ id, name }) => {
         arr.push(id);
      });
      setStates(arr);
   }
   useEffect(() => {
      props.statesList.then(res => {
         setCategoriesList(res);

         var tmp = (<FormControl variant='standard' sx={{ m: 1, width: 250 }}>
            <InputLabel id="demo-multiple-name" shrink={false}>Category</InputLabel>
            <Select
               autoWidth={false}
               labelId="demo-multiple-name"
               {...field}
               multiple
               value={states}
               renderValue={(selected) => ""}
               onChange={(e) => handleChange(e, res)}
               MenuProps={MenuProps}
               IconComponent={() => <FilterAltIcon />}
            // inputProps={InputLabel}
            >
               <MenuItem value={"all"}>
                  <Checkbox sx={{
                     color: "#cf2338",
                     '&.Mui-checked': {
                        color: "#cf2338",
                     },
                  }} checked={states.length == res.length} />
                  <ListItemText primary="All" />
               </MenuItem>
               {res.map((state) => (
                  <MenuItem key={state.id} value={state.id}>
                     <Checkbox sx={{
                        color: "#cf2338",
                        '&.Mui-checked': {
                           color: "#cf2338",
                        },
                     }} checked={states.indexOf(state.id) > -1} />
                     <ListItemText primary={state.name} />
                  </MenuItem>
               ))}
            </Select>
         </FormControl >)
         setFilterElement(tmp);
      });
   }, [states])

   return categoriesList ? filterElement : <></>
}