import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { Pagination, PaginationItem } from '@mui/material';
import React from 'react';
import { Toolbar, useListContext, Button } from 'react-admin';
import PreviousButton from '../buttons/PreviousButton';
import NextButton from '../buttons/NextButton';

const ChooseListPagination = () => {
    const { page, hasPreviousPage, hasNextPage, setPage, total, perPage } = useListContext();
    if (!hasPreviousPage && !hasNextPage) return null;
    const pageNumber = Math.ceil(total / perPage);

    const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
    };
    return (
        <div style={{ float: "left", marginTop: "12px" }}>
            <Pagination
                color={"secondary"}
                shape='rounded'
                count={pageNumber}
                page={page}
                onChange={handleChange}
                renderItem={(item) => (
                    <PaginationItem
                        slots={{ previous: PreviousButton, next: NextButton }}
                        {...item}
                    />
                )}
            />
        </div>
    );
}

export default ChooseListPagination;