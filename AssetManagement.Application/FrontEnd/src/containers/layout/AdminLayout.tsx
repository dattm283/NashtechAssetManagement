import React, { useState, useEffect } from 'react';
import jsonServerProvider from 'ra-data-json-server';
import simpleRestProvider from 'ra-data-simple-rest';
import {
    Admin,
    Resource,
    NotFound,
    useRefresh,
    CustomRoutes,
    ListGuesser,
} from 'react-admin';
import { theme } from '../../theme';
import Layout from '../Layout';
import LoginPage from './LoginLayout';
import AuthProvider from '../../providers/authenticationProvider/authProvider';
import authService from '../../services/changePasswordFirstTime/auth';
import ChangePasswordModal from "../../components/modal/changePasswordModal/ChangePasswordModal";
import HomeList from '../../pages/home/HomeList';
import * as CryptoJS from 'crypto-js';
import config from "../../connectionConfigs/config.json";
import { assetProvider } from '../../providers/assetProvider/assetProvider';
import AssetList from '../../pages/assets/AssetList';
import AssetEdit from '../../pages/assets/AssetEdit';
import AssetCreate from '../../pages/assets/AssetCreate';
import UserList from '../../pages/users/UserList';
import UserCreate from '../../pages/users/UserCreate';
import AssignmentList from '../../pages/assignments/AssignmentList';
import AssignmentEdit from '../../pages/assignments/AssignmentEdit';
// import AssetManager from '../../pages/asset/AssetManager';
import SelectAssetModal from '../../components/modal/selectAssetModal/SelectAssetModal';
import EditUser from '../../pages/users/UserEdit';
import { Route } from 'react-router-dom';
import AssignmentCreate from '../../pages/assignments/AssignmentCreate';
import ReturnRequestList from '../../pages/returnRequest/ReturnRequestList';
import ReportList from '../../pages/report/ReportList'

// You will fix this API-URL
const authProvider = AuthProvider(config.api.base);
const encryptKey = config.encryption.key;

const App = () => {
    const [loginFirstTime, setLoginFirstTime] = useState(false);
    const refresh = useRefresh();

    const encrypt = (text) => {
        return CryptoJS.AES
            .encrypt(text, encryptKey)
            .toString();
    }

    const [permissions, setPermissions] = useState(localStorage.getItem("permissions") || '')
    useEffect(() => {
        setPermissions(localStorage.getItem("permissions") || '')
    })

    console.log("permissionsAdminlayout", permissions)
    const checkIsLoginFirstTime = (currentPassword) => {
        authService.getUserProfile()
            .then(data => {
                if (data.isLoginFirstTime) {
                    setLoginFirstTime(true);
                    localStorage.setItem("currentPassword", encrypt(currentPassword))
                    localStorage.setItem('loginFirstTime', "new");
                } else {
                    setPermissions(localStorage.getItem("permissions") || '')
                    refresh();
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

    useEffect(() => {
        let expTick = localStorage.getItem("expTime");
        if (expTick) {
            var exp = new Date(parseInt(expTick) * 1000);
            var now = new Date();
            if (now > exp) {
                localStorage.removeItem("auth");
                authProvider.logout();
            }
        }
    }, [])

    return (
        <>
            <Admin
                title="Nashtech"
                dataProvider={assetProvider}
                authProvider={authProvider}
                theme={theme}
                layout={Layout}
                catchAll={NotFound}
                loginPage={<LoginPage checkIsLoginFirstTime={checkIsLoginFirstTime} />}
                requireAuth={true}
            >
                <Resource name="home" options={{ label: 'Home' }} list={HomeList} />
                {permissions == 'Admin' ? <Resource name="assets" list={AssetList} edit={AssetEdit} create={AssetCreate} options={{ label: 'Manage Asset' }} /> : null}
                {permissions == 'Admin' ? <Resource name="assignments" list={AssignmentList} edit={AssignmentEdit} create={AssignmentCreate} options={{ label: 'Manage Assignments' }} /> : null}
                {permissions == 'Admin' ? <Resource name="user" list={UserList} create={UserCreate} edit={EditUser} options={{ label: 'Manage User' }} /> : null}
                {permissions == 'Admin' ? <Resource name="returnRequest" list={ReturnRequestList} options={{ label: 'Manage ReturnRequest' }} /> : null}
                {permissions == 'Admin' ? <Resource name="report" list={ReportList} /> : null}
            </Admin>

            <ChangePasswordModal
                loginFirstTime={loginFirstTime}
                setLoginFirstTime={setLoginFirstTime}
                logout={authProvider.logout}
            />
        </>
    )
};

export default App;