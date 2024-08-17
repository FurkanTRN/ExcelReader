
import React from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "App";
import { MaterialUIControllerProvider } from "context";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


const container = document.getElementById("app");
const root = createRoot(container);

root.render(
  <BrowserRouter>
    <MaterialUIControllerProvider>
        <App />
      <ToastContainer />
    </MaterialUIControllerProvider>
  </BrowserRouter>
);
