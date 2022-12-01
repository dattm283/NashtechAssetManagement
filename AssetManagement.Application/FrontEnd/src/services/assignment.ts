import axiosInstance from "../connectionConfigs/axiosInstance";
import config from "../connectionConfigs/config.json"
const baseUrl = config.api.assignement;

export const getAssignementByAssetCodeId = async (assetCodeId) => {
    let url = `${baseUrl}/assets/${assetCodeId}`;
    const response = await axiosInstance.get(url);

    return response.data;
} 