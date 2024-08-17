import React from "react";
import { Navigate } from "react-router-dom";

// eslint-disable-next-line react/prop-types
const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem("jwtToken") || sessionStorage.getItem("jwtToken");
  const isAuthenticated = !!token;

  return isAuthenticated ? children : <Navigate to="/authentication/sign-in" />;
};

export default ProtectedRoute;
