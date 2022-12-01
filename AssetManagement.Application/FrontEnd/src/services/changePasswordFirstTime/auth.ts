import axiosInstance from "../../connectionConfigs/axiosInstance";
import config from "../../connectionConfigs/config.json";
const baseUrl = config.api.auth;

const getUserProfile = async () => {
    let url = baseUrl + "/user-profile";
    const response = await axiosInstance.get(url);
    localStorage.setItem('userName', response.data.username);
    return response.data;
}

const changePassword = async (changePasswordRequest) => {
    let url = baseUrl + "/change-password";
    const response = await axiosInstance.post(url, changePasswordRequest);
    return response.data;
}

const loginFailError = "Username or password is incorrect. Please try again";
const loginFirstTimeError = "User login first time";

const exportObject = {
    getUserProfile,
    changePassword,
    loginFailError,
    loginFirstTimeError,
}

export default exportObject;