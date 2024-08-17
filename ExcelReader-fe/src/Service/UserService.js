import apiClient from "./ApiService";

export const updateUser = async (userId, data) => {
  try {
    await apiClient.put(`/user/${userId}`, data);
  } catch (error) {
    throw new Error("Failed to update user");
  }
};

export const getUserByEmail = async () => {
  try {
    const response = await apiClient.get(`/user`);
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch user ID");
  }
};