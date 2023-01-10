import axiosInstance from "../connectionConfigs/axiosInstance";
import config from '../connectionConfigs/config.json';
const baseUrl = config.api.asset;


const importAssets = async(data) => {
    let url = `${baseUrl}/import`;
    const formData = new FormData();
    formData.append("file", data);
    formData.append("fileName", data.name);
    console.log(data);
    await axiosInstance.post(url, formData).then((res) => {console.log(res.data); return res.data});
    
    return null;
}

export {
    importAssets
}