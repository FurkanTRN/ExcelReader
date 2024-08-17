import apiClient from "./ApiService";

export const login = async (email, password, rememberMe) => {
  try {
    const response = await apiClient.post("auth/login", { email, password });
    const token = response.data.token;

    if (rememberMe) {
      localStorage.setItem("jwtToken", token);
    } else {
      sessionStorage.setItem("jwtToken", token);
    }
    return response.data;
  } catch (error) {
    console.error("Error logging in:", error);
    throw error;
  }
};


export const register = async (firstName, lastName, email, password) => {
  try {
    const response = await apiClient.post('auth/register', {
      firstName,
      lastName,
      email,
      password,
    });
    return response.data;
  } catch (error) {
    console.error('Error registering:', error);
    throw error;
  }
};

export const logout = () => {
  localStorage.removeItem("jwtToken");
  sessionStorage.removeItem("jwtToken");
  window.location.href = "/authentication/sign-in"; // Redirect to login page
};