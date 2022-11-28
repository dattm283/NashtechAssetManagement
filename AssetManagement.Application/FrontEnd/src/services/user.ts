import axiosInstance from "../connectionConfigs/axiosInstance";
import config from '../connectionConfigs/config.json';
const baseUrl = config.api.role;

export const getRoles = async() => {
    let url = `${baseUrl}`
    const response = await axiosInstance.get(url)

    return response.data;
}