import Dashboard from "layouts/dashboard";
import SignIn from "layouts/authentication/sign-in";
import SignUp from "layouts/authentication/sign-up";
import Icon from "@mui/material/Icon";
import FileUpload from "./layouts/UploadFile";
import ProfileUpdate from "layouts/profile";
import {logout} from "Service/AuthService";
const routes = [
  {
    type: "collapse",
    name: "Dashboard",
    key: "dashboard",
    icon: <Icon fontSize="small">dashboard</Icon>,
    route: "/dashboard",
    component: <Dashboard />,
  },
  {
    type: "collapse",
    name: "Upload File",
    key: "Upload File",
    icon: <Icon fontSize="small">upload</Icon>,
    route: "/upload",
    component: <FileUpload />,
  },
  {
    type: "collapse",
    name: "Profile",
    key: "profile",
    icon: <Icon fontSize="small">person</Icon>,
    route: "/profile",
    component: <ProfileUpdate />,
  },
  {
    type: "collapse",
    name: "Sign In",
    key: "sign-in",
    icon: <Icon fontSize="small">login</Icon>,
    route: "/authentication/sign-in",
    component: <SignIn />,
  },
  {
    type: "collapse",
    name: "Sign Up",
    key: "sign-up",
    icon: <Icon fontSize="small">assignment</Icon>,
    route: "/authentication/sign-up",
    component: <SignUp />,
  },
  {
    type: "collapse",
    name: "Logout",
    key: "logout",
    icon: <Icon fontSize="small">logout</Icon>,
    route: "/logout",
    action:logout
  }
];

export default routes;
