import React from 'react'
import { RadioButtonGroupInput } from 'react-admin'
import {radioButtonGroupInputStyle} from '../../styles/radioButtonGroupInputStyle'

function RadioButtonGroup(props) {
    return (
        <RadioButtonGroupInput 
            // fullwidth="true"
            {...props}
            sx={radioButtonGroupInputStyle.radioButtonGroupInput}
        />
    )
}

export default React.memo(RadioButtonGroup);