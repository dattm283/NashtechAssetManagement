import React from 'react';
import './App.css';
import Admin from './layout/AdminLayout';
import { QueryClient, QueryClientProvider } from 'react-query';

function App() {
    console.log("App");
    return (
        // <QueryClientProvider client={new QueryClient()}>
        <Admin />
        /* </QueryClientProvider> */
        // title={<AppTitle />}
    );
}

export default App;
