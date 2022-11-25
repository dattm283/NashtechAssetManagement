import axiosInstance from "../connectionConfigs/axiosInstance";
import config from '../connectionConfigs/config.json';
const baseUrl = config.api.category;

const getCategory = async() => {
    let url = `${baseUrl}/Get`
    const response = await axiosInstance.get(url)

    return response.data;
}

const createCategory = async(data) => {
    let url = `${baseUrl}/Create`
    const response = await axiosInstance.post(url, data)

    return response.data;
}

export {
    getCategory,
    createCategory
}