import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { Pagination } from '@mui/material';
import React from 'react';
import { Button, Toolbar, useListContext } from 'react-admin';

export default () => {
   const { page, hasPreviousPage, hasNextPage, setPage, total, perPage } = useListContext();
   if (!hasPreviousPage && !hasNextPage) return null;
   const pageNumber = Math.ceil(total / perPage);

   const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
      setPage(value);
   };
   return (
      <div style={{ marginLeft: "auto", marginTop: "12px" }}>
         <Pagination color={"secondary"} shape='rounded' count={pageNumber} page={page} onChange={handleChange} />
      </div>
   );
}