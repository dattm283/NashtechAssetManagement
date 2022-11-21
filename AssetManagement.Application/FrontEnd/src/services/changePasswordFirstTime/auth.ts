import axiosInstance from "../../connectionConfigs/axiosInstance";

const baseUrl = "https://localhost:50569/api/auth/";

const getUserProfile = async () => {
    let url = baseUrl + "user-profile";
    const response = await axiosInstance.get(url);

    return response.data;
}

const changePassword = async (changePasswordRequest) => {
    let url = baseUrl + "change-password";
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