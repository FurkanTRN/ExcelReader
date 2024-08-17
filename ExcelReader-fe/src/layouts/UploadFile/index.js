import React, { useCallback, useState } from "react";
import {useNavigate } from "react-router-dom";
import { useDropzone } from "react-dropzone";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import MDButton from "components/MDButton";
import MDAlert from "components/MDAlert";
import DashboardLayout from "../../components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "../../components/Navbars/DashboardNavbar";
import { makeStyles } from "@mui/styles";
import Icon from "@mui/material/Icon";
import { uploadFile } from "Service/FileService";
import { toast } from "react-toastify";
import {toastOptions} from "Utils/ToastOptions";
import CircularProgress from '@mui/material/CircularProgress';



const useStyles = makeStyles((theme) => ({
  dropzone: {
    border: `2px dashed ${theme.palette.primary.main}`,
    padding: theme.spacing(3),
    textAlign: "center",
    cursor: "pointer",
    transition: "background-color 0.3s ease-in-out",
    "&:hover": {
      backgroundColor: theme.palette.action.hover,
    },
  },
  dropzoneActive: {
    backgroundColor: theme.palette.action.selected,
  },
  typography: {
    marginTop: theme.spacing(2),
    marginBottom: theme.spacing(2),
  },
  fileInfo: {
    marginTop: theme.spacing(2),
  },
  button: {
    marginTop: theme.spacing(2),
  },
  alert: {
    marginTop: theme.spacing(2),
  },
}));

function FileUpload() {
  const classes = useStyles();
  const [isDragging, setIsDragging] = useState(false);
  const [files, setFiles] = useState([]);
  const [alert, setAlert] = useState({ show: false, message: "" });
  const [isInvalidFile, setIsInvalidFile] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const onDrop = useCallback((acceptedFiles, fileRejections) => {
    setIsDragging(false);

    if (fileRejections.length > 0) {
      setAlert({ show: true, message: "Invalid file type. Please upload a .xlsx file." });
      setIsInvalidFile(true);
      return;
    }

    setFiles(acceptedFiles);
    setAlert({ show: false, message: "" });
    setIsInvalidFile(false);
  }, []);

  const onDragEnter = useCallback(() => {
    setIsDragging(true);
  }, []);

  const onDragLeave = useCallback(() => {
    setIsDragging(false);
  }, []);

  const { getRootProps, getInputProps } = useDropzone({
    onDrop,
    onDragEnter,
    onDragLeave,
    accept: {
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": [".xlsx"],
    },
  });

  const handleProceed = async () => {
    if (files.length === 0 || isInvalidFile || isLoading) return;
    setIsLoading(true);
    try {
      const formData = new FormData();
      formData.append("file", files[0]);
      await uploadFile(formData);
      toast.success("File uploaded successfully!", toastOptions);
    } catch (error) {
      toast.error("Error uploading file. Please try again.", toastOptions);
    } finally {
      setIsLoading(false);
      setFiles([]);
    }
  };

  return (
    <DashboardLayout>
      <DashboardNavbar />
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Paper
            {...getRootProps()}
            className={`${classes.dropzone} ${isDragging ? classes.dropzoneActive : ""}`}>
            {/* eslint-disable-next-line react/jsx-props-no-spreading */}
            <input {...getInputProps()} />
            <Typography variant="h6" className={classes.typography}>
              Drag and drop files here, or click to select files. Only accepted .xlsx files
            </Typography>
          </Paper>
          {alert.show && (
            <MDAlert color="error" className={classes.alert}>
              <Typography variant="body2">{alert.message}</Typography>
            </MDAlert>
          )}
          <div className={classes.fileInfo}>
            {files.length > 0 && (
              <Typography variant="body2">
                <strong>Selected file:</strong> {files[0].name} ({(files[0].size / 1024).toFixed(2)}{" "}
                KB)
              </Typography>
            )}
          </div>
          <MDButton
            variant="contained"
            size="large"
            color="info"
            className={classes.button}
            disabled={files.length === 0 || isInvalidFile || isLoading}
            onClick={handleProceed}
            sx={{ height: 30, marginLeft: 1, minWidth: 100 }}
          >
            {isLoading ? (
              <CircularProgress size={24} color="inherit" />
            ) : (
              <>
                <Icon>upgrade</Icon>&nbsp;Proceed
              </>
            )}
          </MDButton>
        </Grid>
      </Grid>
    </DashboardLayout>
  );
}

export default FileUpload;
