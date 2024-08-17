import { useState, useEffect } from "react";
import { useLocation, NavLink, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import Icon from "@mui/material/Icon";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import Collapse from "@mui/material/Collapse";
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
import MDBox from "../MDBox";
import MDTypography from "../MDTypography";
import SidenavCollapse from "./SidenavCollapse";
import SidenavRoot from "./SidenavRoot";
import sidenavLogoLabel from "./styles/sidenav";
import { useMaterialUIController, setMiniSidenav, setUserInfo, setSelectedItem } from "../../context";
import { logout as performLogout } from "../../Service/AuthService";
import { getPrintHistory, deletePrintHistory, getPrintHistoryDetails } from "../../Service/HistoryPrintService";
import DeleteIcon from "@mui/icons-material/Delete";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import { Flip, toast } from "react-toastify";
import { toastOptions } from "../../Utils/ToastOptions";
import { IconButton, Zoom } from "@mui/material";
import ArchiveIcon from "@mui/icons-material/Archive";
import Tooltip from "@mui/material/Tooltip";


function Sidenav({ color, brand, brandName, ...rest }) {
  const [controller, dispatch] = useMaterialUIController();
  const { miniSidenav, transparentSidenav, whiteSidenav, darkMode, userInfo } = controller;
  const location = useLocation();
  const [openHistory, setOpenHistory] = useState(false);
  const [printHistory, setPrintHistory] = useState([]);
  const [openConfirmDialog, setOpenConfirmDialog] = useState(false);
  const [deleteItemId, setDeleteItemId] = useState(null);
  let textColor = "whitesmoke";
  const navigate = useNavigate();


  const closeSidenav = () => setMiniSidenav(dispatch, true);

  const handleLogout = async () => {
    try {
      await performLogout();
      setUserInfo(dispatch, null);
      window.location.href = "/authentication/sign-in";
    } catch (error) {
      toast.error("Logout failed", toastOptions);
    }
  };


  useEffect(() => {
    function handleMiniSidenav() {
      setMiniSidenav(dispatch, window.innerWidth < 1200);
    }

    window.addEventListener("resize", handleMiniSidenav);
    handleMiniSidenav();
    return () => window.removeEventListener("resize", handleMiniSidenav);
  }, [dispatch, location]);

  const fetchPrintHistory = async () => {
    try {
      const history = await getPrintHistory();
      setPrintHistory(history);
    } catch (error) {
      toast.error("History is empty!", toastOptions);
    }
  };
  const handleHistoryClick = () => {
    fetchPrintHistory();
    setOpenHistory(!openHistory);
  };

  const handleItemClick = async (id) => {
    try {
      const history = await getPrintHistoryDetails(id);
      const { fileId, devices,startDate,endDate } = history;
      const selectedItemData = { fileId, devices: Array.isArray(devices) ? devices : [] ,startDate,endDate };
      setSelectedItem(dispatch, selectedItemData);
      navigate("/dashboard", { state: { selectedItemData } });
    } catch (error) {
      console.error("Error fetching item details:", error);
    }
    ;
  };
  const formatDate = (dateString) => {
    const options = { year: "numeric", month: "short", day: "numeric", hour: "2-digit", minute: "2-digit" };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const handleDeleteClick = (id) => {
    setDeleteItemId(id);
    setOpenConfirmDialog(true);
  };


  const handleDeleteConfirm = async () => {
    try {
      await deletePrintHistory(deleteItemId);
      setPrintHistory(printHistory.filter(item => item.id !== deleteItemId));
      setOpenConfirmDialog(false);
    } catch (error) {
      toast.error("Error deleting print history"), toastOptions;
    }
  };

  const handleDeleteCancel = () => {
    setOpenConfirmDialog(false);
  };

  return (
    <SidenavRoot
      variant="permanent"
      ownerState={{ transparentSidenav, whiteSidenav, miniSidenav, darkMode }}
    >
      <MDBox pt={3} pb={1} px={4} textAlign="center">
        <MDBox
          display={{ xs: "block", xl: "none" }}
          position="absolute"
          top={0}
          right={0}
          p={1.625}
          onClick={closeSidenav}
          sx={{ cursor: "pointer" }}
        >
          <MDTypography variant="h6" color="secondary">
            <Icon sx={{ fontWeight: "bold" }}>close</Icon>
          </MDTypography>
        </MDBox>
        <MDBox component={NavLink} to="/" display="flex" alignItems="center">
          {brand && <MDBox component="img" src={brand} alt="Brand" width="2rem" />}
          <MDBox width={!brandName && "100%"} sx={(theme) => sidenavLogoLabel(theme, { miniSidenav })}>
            <MDTypography component="h6" variant="button" fontWeight="medium" color="white">
              {brandName}
            </MDTypography>
          </MDBox>
        </MDBox>
      </MDBox>
      <Divider />
      <List>
        <NavLink to="/dashboard" key="dashboard" style={{ textDecoration: "none" }}>
          <SidenavCollapse
            name="Dashboard"
            icon={<Icon>dashboard</Icon>}
            active={location.pathname === "/dashboard"}
          />
        </NavLink>
        <NavLink to="/upload" key="upload" style={{ textDecoration: "none" }}>
          <SidenavCollapse
            name="Upload File"
            icon={<Icon>upload</Icon>}
            active={location.pathname === "/upload"}
          />
        </NavLink>
        <NavLink to="/profile" key="profile" style={{ textDecoration: "none" }}>
          <SidenavCollapse
            name="Profile"
            icon={<Icon>person</Icon>}
            active={location.pathname === "/profile"}
          />
        </NavLink>

        <MDBox
          component="button"
          onClick={handleLogout}
          sx={{ width: "100%", textAlign: "left", background: "none", border: "none", cursor: "pointer" }}
        >
          <SidenavCollapse
            name="Logout"
            icon={<Icon>logout</Icon>}
            active={false}
          />
        </MDBox>
        <ListItemButton onClick={handleHistoryClick} sx={{ pl: 3 }}>
          <ArchiveIcon sx={{ mr: 2, color: "whitesmoke" }} />
          <ListItemText
            primary="Archive"
            primaryTypographyProps={{
              variant: "button",
              fontWeight: "small",
              color: "whitesmoke"
            }}
          />
          <IconButton size="small" sx={{ color: "whitesmoke" }}>
            {openHistory ? <ExpandLess /> : <ExpandMore />}
          </IconButton>
        </ListItemButton>
        <Collapse in={openHistory} timeout="auto" unmountOnExit>
          <List component="div" disablePadding>
            {printHistory.map((item) => (
              <MDBox key={item.id} display="flex" alignItems="center" pl={1}>
               <Tooltip title={item.devices.join(', ')} arrow TransitionComponent={Zoom}>
                 <ListItemButton onClick={() => handleItemClick(item.id)}>
                   <ListItemText
                     primary={`- ${item.fileName}`}
                     secondary={`Printed: ${new Date(item.printedAt).toLocaleString()}`}
                     primaryTypographyProps={{
                       variant: "body2",
                       style: { color: textColor }
                     }}
                     secondaryTypographyProps={{
                       variant: "caption",
                       style: { color: textColor, opacity: 0.7 }
                     }}
                   />

                 </ListItemButton>
               </Tooltip>
                <DeleteIcon sx={{ cursor: "pointer", ml: 1, color: textColor }} onClick={() => handleDeleteClick(item.id)} />
              </MDBox>
            ))}
          </List>
        </Collapse>
      </List>

      <Dialog open={openConfirmDialog} onClose={handleDeleteCancel}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          Are you sure you want to delete this item?
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} color="primary">
            Cancel
          </Button>
          <Button onClick={handleDeleteConfirm} color="secondary">
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </SidenavRoot>
  );
}

Sidenav.defaultProps = {
  color: "info",
  brand: ""
};

Sidenav.propTypes = {
  color: PropTypes.oneOf([
    "primary",
    "secondary",
    "info",
    "success",
    "warning",
    "error",
    "dark"
  ]),
  brand: PropTypes.string,
  brandName: PropTypes.string.isRequired
};

export default Sidenav;
