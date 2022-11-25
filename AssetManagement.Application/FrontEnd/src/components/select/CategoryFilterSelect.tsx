import Select, { SelectChangeEvent } from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import { useInput, useListContext } from 'react-admin';
import React, { useEffect, useState } from 'react';
import { Checkbox, FormControl, InputLabel, ListItemIcon, ListItemText } from '@mui/material';


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
      console.log(states);
      var tmp = filterValues.states;
      setFilters({ categories: states, states: tmp }, displayedFilters);
   }, [states])

   useEffect(() => {
      setPerPage(5)
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


         var tmp = (<FormControl variant='standard' sx={{ m: 1, minWidth: 120 }}>
            <InputLabel id="demo-multiple-name">Categories</InputLabel>
            <Select
               labelId="demo-multiple-name"
               {...field}
               multiple
               value={states}
               renderValue={(selected) => selected.map(key => { return res.find((o) => o.id == key).name ? res.find((o) => o.id == key).name : "" }).join(', ')}
               onChange={(e) => handleChange(e, res)}
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

   // return (props.statesList as Array<{ value, text }>).length > 0 ? <FormControl variant='standard' sx={{ m: 1, minWidth: 120 }}>
   //    <InputLabel id="demo-multiple-name-label">States</InputLabel>
   //    <Select
   //       labelId="demo-multiple-name-label"
   //       {...field}
   //       multiple
   //       value={states}
   //       renderValue={(selected) => selected.map(key => { return props.statesList[key].name ? props.statesList[key].name : "" }).join(', ')}
   //       onChange={handleChange}
   //    >
   //       <MenuItem value={"all"}>
   //          <Checkbox sx={{
   //             color: "#cf2338",
   //             '&.Mui-checked': {
   //                color: "#cf2338",
   //             },
   //          }} checked={states.length == 4} />
   //          <ListItemText primary="All" />
   //       </MenuItem>
   //       {props.statesList.map((state) => (
   //          <MenuItem key={state.id} value={state.id}>
   //             <Checkbox sx={{
   //                color: "#cf2338",
   //                '&.Mui-checked': {
   //                   color: "#cf2338",
   //                },
   //             }} checked={states.indexOf(state.id) > -1} />
   //             <ListItemText primary={state.name} />
   //          </MenuItem>
   //       ))}
   //    </Select>
   // </FormControl > : <></>;
}