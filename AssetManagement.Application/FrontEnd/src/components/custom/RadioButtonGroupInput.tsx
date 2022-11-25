import React from 'react'
import { RadioButtonGroupInput } from 'react-admin'

function RadioButtonGroup(props) {
    return (
        <RadioButtonGroupInput 
            // fullwidth="true"
            {...props}
            sx={{
                ".css-1m9pwf3 + .css-hyxlzm .css-1hbvpl3-MuiSvgIcon-root":{
                    width:"20px",
                    height:"20px",
                    color:'#000',
                    backgroundColor: '#fff',
                    borderRadius: '50%',
                },
                ".css-1m9pwf3:checked + .css-hyxlzm .css-1hbvpl3-MuiSvgIcon-root":{
                    width:"20px",
                    height:"20px",
                    color:'#cf2338',
                    backgroundColor: '#cf2338',
                    borderRadius: '50%',
                },
                ".css-1m9pwf3 + .css-hyxlzm .css-11zohuh-MuiSvgIcon-root":{
                    width:"20px",
                    height:"20px",
                    color:'#fff',
                },
                ".css-1m9pwf3:checked + .css-hyxlzm .css-11zohuh-MuiSvgIcon-root":{
                    width:"20px",
                    height:"20px",
                    color:'#fff',
                }
            }}
        />
    )
}

export default React.memo(RadioButtonGroup);