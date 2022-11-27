import React, { useState, useEffect } from 'react'
import { useInput, useNotify } from 'react-admin';
import { Box, MenuItem, Select, Typography } from '@mui/material';
import { theme } from '../../theme'
import * as categoryService from '../../services/category'
import SimpleForm from './SimpleForm'

function SelectBoxWithFormInside({ source, format, parse }) {
    const [addingData, setAddingData] = useState({ status:false, data:Array })
    const [errorMessage, setErrorMessage] = useState("")
    const notify = useNotify();
    
    const {
        field,
        fieldState: { isTouched, invalid, error },
        formState: { isSubmitted }
    } = useInput({ source, format, parse })

    useEffect(() => {
        categoryService.getCategory()
            .then(responseData => setAddingData({ status:false, data:responseData.data }) )
            .catch(error => console.log(error))
    }, [])

    // Handle Click to generate a form for creating new Category
    const handleClick = (e) => {
        setAddingData({ status:true, data:addingData.data })
    }
    // Handle Submit to create new Category
    const handleSubmit = (formData) => {
        categoryService.createCategory(formData)
            .then(response => categoryService.getCategory())
            .then(responseData => setAddingData({ status:false, data:responseData.data }) )
            .catch(error => setErrorMessage(error.response.data))
    }
    // Handle Close to close form for createing new Category
    const handleClose = (e) => {
        setAddingData({ status:false, data:addingData.data })
    }

    return (
        <Select
            label=""
            {...field}
            sx={{ 
                width:"430px", 
                height:"40px",
                padding:"0px",
                boxSizing:"border-box",
                "& .MuiDataGrid-root": {
                    border: "none"
                },
            }}
        >
            {Array.prototype.map.bind(addingData.data)(
                (item, index) =>
                <MenuItem key={index} value={item.id}>
                    {item.name}
                </MenuItem>)
            }
            <hr style={{ margin:"0", color:"gray", backgroundColor:"whitesmoke" }} />
            <Box sx={{backgroundColor:"#eff1f5"}}>
                {addingData.status==false && <Typography 
                    color={theme.palette.secondary.main} 
                    sx={{ 
                        cursor:"pointer", 
                        fontStyle:"italic", 
                        textDecoration:"underline",
                        padding:"6px 16px",
                        // marginBlockEnd:"-10px"
                    }} 
                    onClick={handleClick}
                >
                    Add new category
                </Typography>}
                {addingData.status==true && 
                <SimpleForm 
                    handleSubmit={handleSubmit} 
                    handleClose={handleClose} 
                    errorMessage={errorMessage} 
                    setErrorMessage={setErrorMessage}
                /> }
            </Box>
        </Select>
    )
}

export default SelectBoxWithFormInside