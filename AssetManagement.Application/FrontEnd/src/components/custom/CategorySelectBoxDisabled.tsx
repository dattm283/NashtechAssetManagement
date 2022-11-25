import React, { useState, useEffect } from 'react'
import { useInput } from 'react-admin';
import { MenuItem, Select, Typography } from '@mui/material';
import * as categoryService from '../../services/category'

function CategorySelectBoxDisabled({ source, format, parse, defaultValue = -1,disabled = false }) {
    const [addingData, setAddingData] = useState({ status:false, data:Array })
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

    return (
        <Select
            label=""
            {...field}
            value= {defaultValue}
            disabled={disabled}
            sx={{ 
                width:"430px", 
                height:"40px",
                padding:"0px",
                boxSizing:"border-box",
                "& .MuiDataGrid-root": {
                    border: "none",
                },
            }}
        >
            {Array.prototype.map.bind(addingData.data)(
                (item, index) => <MenuItem key={index} value={item.id}>{item.name}</MenuItem>)
            }
            <hr style={{ color:"gray" }} />
        </Select>
    )
}

export default CategorySelectBoxDisabled