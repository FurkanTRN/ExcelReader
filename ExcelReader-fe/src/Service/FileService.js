import apiClient from 'Service/ApiService';
import { toast } from 'react-toastify';


export const getFiles = async () => {
  try {
    const response = await apiClient.get('/file');
    return response.data;
  } catch (error) {
    toast.error("Failed to fetch files. Please try again.");
    throw error;
  }
};

export const deleteFile = async (fileId) => {
  try {
    await apiClient.delete(`/file/${fileId}`);
    toast.success("File deleted successfully.");
  } catch (error) {
    toast.error("Failed to delete file. Please try again.");
    throw error;
  }
};

export const getDevicesByFileId = async (fileId) => {
  try {
    const response = await apiClient.get(`/device/user/${fileId}`);
    return response.data;
  } catch (error) {
   toast.error("Error fetching devices:");
    throw error;
  }
};
export const uploadFile = (formData) => {
  return apiClient.post("/file/upload", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};

export const getSensorsByDeviceAndFileId = async (fileId, devices) => {
  try {
    const response = await apiClient.post(`/device/${fileId}/sensors`,devices);
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch sensor data");
  }
};


