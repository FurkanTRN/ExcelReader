import apiClient from "./ApiService";
import { toast } from "react-toastify";

export const getPrintHistory = async () => {
  try {
    const response = await apiClient.get('/history/all');
    return response.data;
  } catch (error) {
    console.error("Error fetching saved charts:", error);
    throw error;
  }
};

export const deletePrintHistory = async (id) => {
  try {
    const response = await apiClient.delete(`/history/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting saved chart:", error);
    throw error;
  }
};
export const getPrintHistoryDetails = async (id) => {
  try {
    const response = await apiClient.get(`/history/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching chart details:", error);
    throw error;
  }
};

export const saveToHistory = async (fileId, devices,startDate,endDate) => {
  try {
    await apiClient.post(`/history`, {
      fileId,
      devices,
      startDate: startDate.toISOString(),
      endDate : endDate.toISOString()
    });
    console.log(`Start Date: ${startDate.toISOString()}`);
    console.log(`End Date: ${endDate.toISOString()};`)
  } catch (error) {
    console.error("Failed to save data to history");
  }
};