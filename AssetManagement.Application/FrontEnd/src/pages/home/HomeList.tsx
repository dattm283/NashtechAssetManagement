import * as React from "react";
import Card from '@mui/material/Card';
import { Container } from '@mui/material';
import CardContent from '@mui/material/CardContent';
import { Title } from 'react-admin';
export default () => (
    <Container component="main" sx={{padding:"20px 10px"}}>
    <Card>
        <Title title="Home" />
        <CardContent>This is home page.</CardContent>
    </Card>
    </Container>
);