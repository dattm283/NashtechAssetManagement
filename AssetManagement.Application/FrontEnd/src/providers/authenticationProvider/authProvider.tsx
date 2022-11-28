import simpleRestProvider from 'ra-data-simple-rest';
import { fetchUtils } from 'react-admin';
import axiosInstance from '../../connectionConfigs/axiosInstance';
import decodeJwt from 'jwt-decode';

// Customize Request header
const httpClient = (url, options) => {
    if (!options.headers) {
        options.headers = new Headers({ Accept: 'application/json' });
    }
    const { token } = JSON.parse(localStorage.getItem('auth') as string);
    options.headers.set('Authorization', `Bearer ${token}`);
    return fetchUtils.fetchJson(url, options);
};

// Setup AuthProvider
function AuthProvider(authURL) {
    const dataPorvider = simpleRestProvider(authURL);

    return ({
        ...dataPorvider,
        // send username and password to the auth server and get back credentials
        login: ({ username, password }) => {
            return axiosInstance.post(`${authURL}/api/auth/token`, { Username: username, Password: password }, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    if (response.status < 200 || response.status >= 300) {
                        throw new Error("Username or password is incorrect. Please try again");
                    }
                    localStorage.setItem('auth', response.data.result.token);
                    localStorage.setItem('role', response.data.result.role);

                    interface MyToken {
                        name: string;
                        exp: number;
                        // whatever else is in the JWT.
                    }

                    const decodedToken: MyToken = decodeJwt(response.data.result.token);
                    localStorage.setItem("expTime", decodedToken.exp.toString());
                    // localStorage.setItem('token', response.data.result.token);
                    localStorage.setItem('permissions', response.data.result.role);
                })
        },

        // when the dataProvider returns an error, check if this is an authentication error
        checkError: (error) => {
            const status = error.status;
            console.log("error", error)
            if (status === 401 || status === 403) {
                localStorage.removeItem('auth');
                return Promise.reject();
            }

            // other error code (404, 500, etc): no need to log out
            return Promise.resolve();
        },

        // when the user navigates, make sure that their credentials are still valid
        checkAuth: () => localStorage.getItem('auth')
            ? Promise.resolve()
            // react-admin passes the error message to the translation layer
            : Promise.reject(null),

        // remove local credentials and notify the auth server that the user logged out
        logout: () => {
            localStorage.removeItem('auth');
            return Promise.resolve('/Login');
        },

        // get the user's profile
        getIdentity: () => {
            try {
                const { id, fullName, avatar } = JSON.parse(localStorage.getItem('auth') as string);
                return Promise.resolve({ id, fullName, avatar });
            } catch (error) {
                return Promise.reject(error);
            }
        },

        // get the user permissions (optional)
        getPermissions: () => {
            const role = localStorage.getItem('permissions');
            return role ? Promise.resolve(role) : Promise.reject();
        }
    })
};

export default AuthProvider