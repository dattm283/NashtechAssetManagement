import axiosInstance from "../connectionConfigs/axiosInstance";
import config from "../connectionConfigs/config.json"
const baseUrl = config.api.assignement;

const getAssignementByAssetCodeId = async (assetCodeId) => {
    let url = `${baseUrl}/assets/${assetCodeId}`;
    const response = await axiosInstance.get(url);

    return response.data;
} 

const acceptAssignment =async (assignmentId) => {
    let url = `${baseUrl}/${assignmentId}/accept`;
    const response = await axiosInstance.put(url);
    return response.data;
}

const declineAssignment =async (assignmentId) => {
    let url = `${baseUrl}/${assignmentId}/decline`;
    const response = await axiosInstance.put(url);
    return response.data;
}

export {
    getAssignementByAssetCodeId,
    acceptAssignment,
    declineAssignment
}