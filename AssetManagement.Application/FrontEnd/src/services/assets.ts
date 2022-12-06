import axiosInstance from "../connectionConfigs/axiosInstance";
import config from '../connectionConfigs/config.json';
const baseUrl = config.api.asset;

const getAsset = async() => {
    let url = `${baseUrl}`
    const response = await axiosInstance.get(url)

    return response.data;
}
const getAssetById = async(id) => {
    let url = `${baseUrl}/${id}`
    const response = await axiosInstance.get(url)

    return response.data;
}


const createAsset = async(requestData) => {
    let url = `${baseUrl}`
    const response = await axiosInstance.post(url, requestData)

    return response.data;
}

const updateAsset = async(id, requestData) => {
    let url = `${baseUrl}/${id}`
    console.log("url", url);
    // Handle this method
    const reponse = await axiosInstance.put(url, requestData)

    return reponse.data;
}

const deleteAsset = async(id) => {
    let url = `${baseUrl}/api/Assets/${id}`
    const reponse = await axiosInstance.delete(url)

    return reponse.data;
}

const getHistoricalAssignmentsCount = async (assetCodeId) => {
    let url = `${baseUrl}/${assetCodeId}/assignmentCount`;
    const response = await axiosInstance.get(url);

    return response;
} 

export {
    getAsset,
    getAssetById,
    createAsset,
    updateAsset,
    deleteAsset,
    getHistoricalAssignmentsCount
}


