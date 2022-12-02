import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { Pagination, PaginationItem } from '@mui/material';
import React from 'react';
import { Toolbar, useListContext, Button } from 'react-admin';
import PreviousButton from '../buttons/PreviousButton';
import NextButton from '../buttons/NextButton';

const ChooseListBotToolbar = ({ handleSave, handleCancel }) => {
    return (
        <div style={{ float: "right", marginTop: "12px" }}>
            <Button
                onClick={handleSave}
                color='secondary'
                variant="contained"
                label='Save'
                size="medium"
            />
            <Button
                onClick={handleCancel}
                sx={{
                    marginLeft: 5,
                }}
                variant="outlined"
                label='Cancel'
                size="medium"
            />
        </div>
    );
}

export default ChooseListBotToolbar;