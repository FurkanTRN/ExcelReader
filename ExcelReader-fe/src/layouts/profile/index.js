import React, { useState, useEffect } from "react";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import { makeStyles } from "@mui/styles";
import DashboardLayout from "../../components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "../../components/Navbars/DashboardNavbar";
import { getUserByEmail, updateUser } from "Service/UserService";
import { toast } from "react-toastify";
import { toastOptions } from "Utils/ToastOptions";
import MDButton from "../../components/MDButton";
import CircularProgress from '@mui/material/CircularProgress';


const useStyles = makeStyles((theme) => ({
  form: {
    width: "100%",
    marginTop: theme.spacing(3),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
}));

const ProfileUpdate = () => {
  const classes = useStyles();
  const [isLoading, setIsLoading] = useState(false);
  
  const [formData, setFormData] = useState({
    firstname: "",
    lastname: "",
    oldPassword: "",
    newPassword: "",
    confirmPassword: ""
  });
  const [userId, setUserId] = useState(null);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const user = await getUserByEmail();
        setUserId(user.id);
        setFormData({
          firstname: user.firstName,
          lastname: user.lastName,
          oldPassword: "",
          newPassword: "",
          confirmPassword: ""
        });
      } catch (error) {
        toast.error("Error fetching user data", toastOptions);
      }
    };

    fetchUserData();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const { oldPassword, newPassword, confirmPassword } = formData;

    if (newPassword !== confirmPassword && (newPassword.length>0 && confirmPassword.length>0)) {
      toast.error("New password and confirm password do not match", toastOptions);
      return;
    }

    if (oldPassword === newPassword  && (newPassword.length>0 && confirmPassword.length>0)) {
      toast.error("New password cannot be the same as the old password", toastOptions);
      return;
    }
    if (newPassword==="" && confirmPassword==="" && oldPassword==="") {
      toast.error("Password properties can not be empty", toastOptions);
      return;
    }
    setIsLoading(true);
    try {
      const updateData = {
        firstName: formData.firstname,
        lastName: formData.lastname,
        password: formData.newPassword
      };
      await updateUser(userId, updateData);
      toast.success("Profile updated successfully", toastOptions);
      setFormData({
        firstname: formData.firstname,
        lastname: formData.lastname,
        oldPassword: "",
        newPassword: "",
        confirmPassword: ""
      });
    } catch (error) {
      toast.error("Error updating profile", toastOptions);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <DashboardLayout>
      <DashboardNavbar />
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <Typography variant="h6">Update Profile</Typography>
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            fullWidth
            id="firstname"
            type="text"
            name="firstname"
            label="First Name"
            value={formData.firstname}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            fullWidth
            type="text"
            id="lastname"
            name="lastname"
            label="Last Name"
            value={formData.lastname}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            id="oldPassword"
            name="oldPassword"
            label="Old Password"
            type="password"
            value={formData.oldPassword}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            id="newPassword"
            name="newPassword"
            label="New Password"
            type="password"
            value={formData.newPassword}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            id="confirmPassword"
            name="confirmPassword"
            label="Confirm New Password"
            type="password"
            value={formData.confirmPassword}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12}>
          <MDButton
            type="submit"
            variant="contained"
            color="info"
            size="medium"
            onClick={handleSubmit}
            disabled={isLoading}
            sx={{ minWidth: 100 }} // Added for consistent button size
          >
            {isLoading ? (
              <CircularProgress size={24} color="inherit" />
            ) : (
              "Update"
            )}
          </MDButton>
        </Grid>
      </Grid>
    </DashboardLayout>
  );
};

export default ProfileUpdate;
