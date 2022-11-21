import React, { useState, useEffect } from 'react';
import jsonServerProvider from 'ra-data-json-server';
import simpleRestProvider from 'ra-data-simple-rest';
import {
    Admin,
    Resource,
    NotFound,
    ListGuesser,
    ShowGuesser,
    usePermissions
} from 'react-admin';
import { theme } from '../../theme';
import Layout from '../Layout';
import LoginPage from './LoginLayout';
import AuthProvider from '../../providers/authenticationProvider/authProvider';
import authService from '../../services/changePasswordFirstTime/auth';
import ChangePasswordModal from "../../components/modal/changePasswordModal/ChangePasswordModal";
import HomeList from '../../pages/home/HomeList';

import config from "../../connectionConfigs/config.json";

const dataProvider = jsonServerProvider("https://jsonplaceholder.typicode.com");

// You will fix this API-URL
const authProvider = AuthProvider(config.api.base);

const App = () => {
    const [loginFirstTime, setLoginFirstTime] = useState(false);
    const { permissions } = usePermissions();
    // const permissions: string = "Admin";

    const checkIsLoginFirstTime = () => {
        authService.getUserProfile()
            .then(data => {
                console.log("User data", data);
                if (data.isLoginFirstTime) {
                    setLoginFirstTime(true);
                    localStorage.setItem('loginFirstTime', "new");
                }
            })
            .catch(error => {
                console.log(error)
            })
    }

    useEffect(() => {
        if (localStorage.getItem('loginFirstTime') == "new") {
            setLoginFirstTime(true);
        }
    })

    return (
        <>
            <Admin
                title="Nashtech"
                dataProvider={dataProvider}
                authProvider={authProvider}
                theme={theme}
                layout={Layout}
                catchAll={NotFound}
                loginPage={<LoginPage checkIsLoginFirstTime={checkIsLoginFirstTime} />}
                requireAuth={true}
            >
                <Resource name="home" options={{ label: 'Home' }} list={HomeList} />
                {permissions == 'Admin' ? <Resource name="assets" options={{ label: 'Manage Asset' }} /> : null}
                {permissions == 'Admin' ? <Resource name="users" options={{ label: 'Manage User' }} list={ListGuesser} show={ShowGuesser} /> : null}
            </Admin>

            <ChangePasswordModal
                loginFirstTime={loginFirstTime}
                setLoginFirstTime={setLoginFirstTime}
            />
        </>
    )
};

export default App;