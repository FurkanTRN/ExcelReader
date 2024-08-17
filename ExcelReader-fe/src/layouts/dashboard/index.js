import React, { useState, useEffect, useRef, useMemo } from "react";
import { Line } from "react-chartjs-2";
import "chart.js/auto";
import {
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Checkbox,
  ListItemText,
  OutlinedInput,
  Icon
} from "@mui/material";
import MDBox from "components/MDBox";
import DashboardLayout from "../../components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "../../components/Navbars/DashboardNavbar";
import { getFiles, getDevicesByFileId, deleteFile, getSensorsByDeviceAndFileId } from "Service/FileService";
import { toast } from "react-toastify";
import { format } from "date-fns";
import MDButton from "components/MDButton";
import { useReactToPrint } from "react-to-print";
import logoImage from "assets/images/biocoder-logo.png";
import { useMaterialUIController } from "context";
import { saveToHistory } from "../../Service/HistoryPrintService";
import { toastOptions } from "Utils/ToastOptions";
import { DateTimePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFnsV3";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import HumiditySensorTable from "../../components/HumiditySensorTable";
import TemperatureSensorTable from "../../components/TemperatureSensorTable";


const randomColor = require("randomcolor");

function Dashboard() {
  const [controller] = useMaterialUIController();
  const { selectedItem } = controller;
  const [dropdownValue, setDropdownValue] = useState("");
  const [fileList, setFileList] = useState([]);
  const [deviceList, setDeviceList] = useState([]);
  const [selectedDevices, setSelectedDevices] = useState([]);
  const [sensorData, setSensorData] = useState([]);
  const [openDialog, setOpenDialog] = useState(false);
  const temperatureChartRef = useRef(null);
  const humidityChartRef = useRef(null);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [minDate, setMinDate] = useState(null);
  const [maxDate, setMaxDate] = useState(null);
  const [summaryData, setSummaryData] = useState([]);
  const [isPrinting, setIsPrinting] = useState(false);



  useEffect(() => {
    fetchFiles();
  }, []);
  useEffect(() => {
    if (selectedItem) {
      const { fileId, devices, startDate, endDate } = selectedItem;
      setDropdownValue(fileId);
      setSelectedDevices(devices);
      setStartDate(startDate || minDate);  // Tarihleri günceller
      setEndDate(endDate || maxDate);      // Tarihleri günceller
      fetchDevices(fileId);
    }
  }, [selectedItem, minDate, maxDate]);

  useEffect(() => {
    if (dropdownValue) {
      fetchDevices(dropdownValue);
    } else {
      setDeviceList([]);
    }
  }, [dropdownValue]);

  useEffect(() => {
    if (dropdownValue && selectedDevices.length > 0) {
      fetchSensorData(dropdownValue, selectedDevices);
    } else {
      setSensorData([]);
    }
  }, [dropdownValue, selectedDevices]);


  const fetchFiles = async () => {

    try {
      const files = await getFiles();
      setFileList(files);
    } catch (error) {
    }
  };
  const filterSensorData = (data) => {
    if (!startDate || !endDate) return [];
    return data.filter(item => {
      const itemDate = new Date(item.recordDate);
      return itemDate >= startDate && itemDate <= endDate;
    });
  };
  const fetchDevices = async (fileId) => {
    if (!fileId) return;
    try {
      let devices = await getDevicesByFileId(fileId);
      devices = devices.sort((a, b) => a.name.localeCompare(b.name)); // A-Z sıralama
      setDeviceList(devices);
    } catch (error) {

    }
  };

  const fetchSensorData = async (fileId, devices) => {
    if (!fileId || devices.length === 0) return;
    try {
      const data = await getSensorsByDeviceAndFileId(fileId, devices);
      setSensorData(data);
      setSummaryData(data);
      if (data.length > 0) {
        const dates = data.map(item => new Date(item.recordDate));
        setMinDate(new Date(Math.min(...dates)));
        setMaxDate(new Date(Math.max(...dates)));
      }
    } catch (error) {
      toast.error(error.message, toastOptions);
    }
  };


  const handlePrintTemperature = useReactToPrint({
    documentTitle: `Temperature Chart ${Date(Date.now())}`,
    onPrintError: () => toast.error("Temperature chart printing failed", toastOptions),
    onBeforeGetContent: () => {
      setIsPrinting(true);
      return new Promise((resolve) => {
        setTimeout(() => {
          resolve();
        }, 0);
      });
    },
    onAfterPrint: async () => {
      try {
        setIsPrinting(false);
        const devicesArray = selectedDevices.map(device => device.trim());
        await saveToHistory(dropdownValue, devicesArray, startDate, endDate);
        toast.success("Temperature chart printed and saved to history successfully", toastOptions);
      } catch (error) {
        toast.error("Failed to save temperature chart to history", toastOptions);
      }
    },
    content: () => temperatureChartRef.current,
    pageStyle: `
  @page {
    size: landscape;
    margin: 5mm;
  }
  @media print {
    body * {
      visibility: hidden;
    }
    #temperature-chart, #temperature-chart *, #summary-table, #summary-table * {
      visibility: visible;
    }
    #temperature-chart {
      position: absolute;
      left: 5mm;
      top: 5mm;
      width: calc(100% - 10mm);
      height: calc(50% - 10mm); /* Yüksekliği sayfanın yarısı kadar olacak şekilde ayarlayın */
      page-break-after: always;
    }
    .chart-container {
      width: 100%;
      height: calc(100% - 200px) !important;
    }
    .print-only-logo {
      display: block !important;
      visibility: visible !important;
      position: absolute !important;
      top: 5mm !important;
      right: 5mm !important;
      width: 20mm !important;
      height: auto !important;
      z-index: 1000 !important;
    }
    #summary-table {
      position: absolute;
      left: 5mm;
      top: calc(90% + 80mm); /* Grafik sonrası konumlandırma */
      width: calc(100% - 10mm);
      height: calc(30% - 20mm); /* Bu alanı yeterli büyüklükte yapın */
      margin-bottom: 20px !important;
    }
  }
`
  });

  const handlePrintHumidity = useReactToPrint({
    documentTitle: `Humidity Chart ${Date.now()}`,
    onPrintError: () => toast.error("Humidity chart printing failed"),
    content: () => humidityChartRef.current,
    onBeforeGetContent: () => {
      setIsPrinting(true);
      return new Promise((resolve) => {
        setTimeout(() => {
          resolve();
        }, 0);
      });
    },
    onAfterPrint: async () => {
      try {
        setIsPrinting(false);
        const devicesArray = selectedDevices.map(device => device.trim());
        await saveToHistory(dropdownValue, devicesArray, startDate, endDate);
        toast.success("Humidity chart printed and saved to history successfully", toastOptions);
      } catch (error) {
        toast.error("Failed to save humidity chart to history", toastOptions);
      }
    },
    pageStyle: `
  @page {
    size: landscape;
    margin: 5mm;
  }
  @media print {
    body * {
      visibility: hidden;
    }
    #humidity-chart, #humidity-chart *, #summary-table, #summary-table * {
      visibility: visible;
    }
    #humidity-chart {
      position: absolute;
      left: 5mm;
      top: 5mm;
      width: calc(100% - 10mm);
      height: calc(100% - 10mm);
      page-break-after: always;
    }
    .chart-container {
      width: 100%;
      height: calc(100% - 200px) !important;
    }
    .print-only-logo {
      display: block !important;
      visibility: visible !important;
      position: absolute !important;
      top: 5mm !important;
      right: 5mm !important;
      width: 20mm !important;
      height: auto !important;
      z-index: 1000 !important;
    }
    #summary-table {
      margin-bottom: 20px !important;
    }
  }
`
  });

  const handleDropdownChange = (event) => {
    const selectedId = event.target.value;
    setDropdownValue(selectedId);
    setSelectedDevices([]);
    setSensorData([]);
  };

  const handleDeviceChange = (event) => {
    const { value } = event.target;
    if (value[value.length - 1] === "select-all") {
      handleSelectAll();
    } else {
      setSelectedDevices(typeof value === "string" ? value.split(",") : value);

    }
    if (value.length > 0) {
      setStartDate(minDate);  // Örneğin, minDate'i startDate olarak ayarlayın
      setEndDate(maxDate);    // Örneğin, maxDate'i endDate olarak ayarlayın
    }
  };
  const handleSelectAll = () => {
    if (selectedDevices.length === deviceList.length) {
      setSelectedDevices([]);
    } else {
      setSelectedDevices([]);
      setSelectedDevices(deviceList.map((device) => device.name));
    }
  };

  const handleDeleteClick = () => {
    setOpenDialog(true);
  };

  const handleConfirmDelete = async () => {
    try {
      await deleteFile(dropdownValue);
      setFileList(fileList.filter((file) => file.id !== parseInt(dropdownValue)));
      setDropdownValue("");
      setDeviceList([]);
      setSelectedDevices([]);
      setSensorData([]);
    } catch (error) {
      toast.error("Failed to delete file", toastOptions);
    } finally {
      setOpenDialog(false);
    }
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };
  const handleClearChart = () => {
    setSelectedDevices([]);
    setStartDate();
    setEndDate();

  };
  const processSensorData = useMemo(() => {
    const filteredData = filterSensorData(sensorData);

    const labels = Array.from(
      new Set(
        filteredData.map((item) =>
          format(new Date(item.recordDate), "yyyy-MM-dd HH:mm")
        )
      )
    ).sort((a, b) => new Date(a) - new Date(b));

    const temperatureDatasets = sensorData.reduce(
      (acc, { name, deviceName, value, recordDate }) => {
        if (name.toLowerCase().includes("sıcaklık")) {
          const label = `${name} (${deviceName})`;
          const color = randomColor({
            luminosity: "bright",
            format: "rgb"
          });
          if (!acc[label]) {
            acc[label] = {
              label,
              data: [],
              borderColor: color,
              backgroundColor: color
            };
          }
          acc[label].data.push({
            x: format(new Date(recordDate), "yyyy-MM-dd HH:mm"),
            y: value
          });
        }
        return acc;
      },
      {}
    );

    const humidityDatasets = sensorData.reduce(
      (acc, { name, deviceName, value, recordDate }) => {
        if (name.toLowerCase().includes("nem")) {
          const label = `${name} (${deviceName})`;
          const color = randomColor();
          if (!acc[label]) {
            acc[label] = {
              label,
              data: [],
              borderColor: color,
              backgroundColor: color
            };
          }
          acc[label].data.push({
            x: format(new Date(recordDate), "yyyy-MM-dd HH:mm"),
            y: value
          });
        }
        return acc;
      },
      {}
    );

    return {
      labels,
      temperatureDatasets: Object.values(temperatureDatasets),
      humidityDatasets: Object.values(humidityDatasets)
    };
  }, [sensorData, startDate, endDate]);

  const { labels, temperatureDatasets, humidityDatasets } = processSensorData;

  const createChartData = (datasets) => ({
    labels,
    datasets: datasets.map(({ label, data, borderColor, backgroundColor }) => ({
      label,
      data: data.map(({ x, y }) => ({ x: new Date(x), y })),
      borderColor,
      backgroundColor,
      fill: false
    }))
  });

  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        ticks: {

          maxRotation: 0,
          minRotation: 45,
          font: { size: 10 }
        }
      },
      y: {
        ticks: { font: { size: 10 } }
      }
    },
    plugins: {
      legend: {
        position: "top",
        labels: {
          boxWidth: 8,
          font: { size: 10 },
          padding: 4
        }
      }
    },
    layout: {
      padding: {
        top: 50,
        right: 50,
        bottom: 10,
        left: 10
      }
    }
  };

  const color = "#000000";
  const datePickerTheme = createTheme({
    components: {

      MuiIconButton: {
        styleOverrides: {
          sizeSmall: {
            color
          }
        }
      },
      MuiOutlinedInput: {
        styleOverrides: {
          root: {
            color,
            fontSize: "0.875rem"
          }
        }
      },
      MuiInputLabel: {
        styleOverrides: {
          root: {
            color,
            fontSize: "0.875rem"
          }
        }
      }
    }
  });

  return (
    <DashboardLayout>
      <DashboardNavbar />
      <MDBox py={2}>
        <MDBox display="flex" alignItems="center" gap={2}>
          <FormControl fullWidth sx={{ maxWidth: 250 }}>
            <InputLabel id="dashboard-select-file-label">Select File</InputLabel>
            <Select
              labelId="dashboard-select-file-label"
              value={dropdownValue}
              label="Select File"
              onChange={handleDropdownChange}
              sx={{ height: 50 }}
              disabled={fileList.length === 0}

            >
              {fileList.map((file) => (
                <MenuItem key={file.id} value={file.id}>
                  {file.fileName}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <MDButton
            variant="contained"
            color="error"
            onClick={handleDeleteClick}
            disabled={!dropdownValue}
            size="medium"
            sx={{ height: 25 }}
          >
            Delete File
          </MDButton>
        </MDBox>
        <Dialog open={openDialog} onClose={handleCloseDialog}>
          <DialogTitle>Confirm Delete</DialogTitle>
          <DialogContent>Are you sure you want to delete?</DialogContent>
          <DialogActions>
            <Button onClick={handleCloseDialog} color="primary">Cancel</Button>
            <Button onClick={handleConfirmDelete} color="error">Delete</Button>
          </DialogActions>
        </Dialog>
        <MDBox py={2}>
          <MDBox display="flex" alignItems="center" gap={2}>
            <FormControl fullWidth sx={{ maxWidth: 250 }}>
              <InputLabel id="dashboard-select-device-label">Select Device(s)</InputLabel>
              <Select
                labelId="dashboard-select-device-label"
                multiple
                disabled={!dropdownValue}
                value={selectedDevices}
                onChange={handleDeviceChange}
                input={<OutlinedInput label="Select Device(s)" />}
                renderValue={(selected) => selected.join(",")}
                MenuProps={{
                  PaperProps: {
                    style: {
                      maxHeight: 250,
                      width: 200
                    }
                  }
                }}
                sx={{ height: 50 }}
              >
                <MenuItem
                  key="select-all"
                  value="select-all"
                  sx={{ py: 0.5, minHeight: "35px" }}
                >
                  <Checkbox
                    checked={selectedDevices.length === deviceList.length && deviceList.length > 0}
                    indeterminate={selectedDevices.length > 0 && selectedDevices.length < deviceList.length}
                    size="small"
                  />
                  <ListItemText
                    primary="Select All"
                    primaryTypographyProps={{ fontSize: "0.875rem" }}
                  />
                </MenuItem>
                {deviceList.map((device) => (
                  <MenuItem
                    key={device.id}
                    value={device.name}
                    sx={{ py: 0.5, minHeight: "35px" }}
                  >
                    <Checkbox
                      checked={selectedDevices.includes(device.name)}
                      size="small"
                    />
                    <ListItemText
                      primary={device.name}
                      primaryTypographyProps={{ fontSize: "0.875rem" }}
                    />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <MDButton
              variant="contained"
              color="warning"
              onClick={handleClearChart}
              disabled={selectedDevices.length === 0 || !startDate || !endDate}
              size="medium"
              sx={{ height: 25 }}
            >
              Clear Chart
            </MDButton>
          </MDBox>
          <MDBox py={2}>
            <ThemeProvider theme={datePickerTheme}>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <MDBox display="flex" gap={2}>
                  <DateTimePicker
                    label="Start Date"
                    value={startDate}
                    onChange={(newValue) => {
                      if (newValue && (!endDate || newValue <= endDate)) {
                        setStartDate(newValue);
                      }
                    }}
                    minDate={minDate}
                    maxDate={endDate || maxDate}
                    disabled={!sensorData.length}
                    ampm={false}
                    timezone={"system"}
                  />
                  <DateTimePicker
                    label="End Date"
                    value={endDate}
                    onChange={(newValue) => {
                      if (newValue && (!startDate || newValue >= startDate)) {
                        setEndDate(newValue);
                      }
                    }}
                    minDate={startDate || minDate}
                    maxDate={maxDate}
                    disabled={!sensorData.length}
                    ampm={false}
                    timezone={"system"}
                  />


                </MDBox>
              </LocalizationProvider>
            </ThemeProvider>
          </MDBox>

          {selectedDevices.length > 0 && startDate && endDate && (
            <>
              <MDBox py={2}>
                <MDBox display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                  <MDBox component="h3" fontWeight="medium">Temperature Chart</MDBox>
                  <MDButton onClick={handlePrintTemperature} color="info">
                    <Icon>print</Icon>&nbsp; Print Chart
                  </MDButton>
                </MDBox>
                <MDBox ref={temperatureChartRef} id="temperature-chart"
                       sx={{ position: "relative", width: "100%", height: "80vh" }}>
                  {isPrinting && (
                    <div id="summary-table" style={{ marginBottom: "20px" }}>
                      <TemperatureSensorTable sensorData={summaryData} />
                    </div>
                  )}
                  <div className="chart-container" style={{ width: "100%", height: isPrinting ? "calc(100% - 200px)" : "100%" }}>
                    <Line data={createChartData(temperatureDatasets)} options={chartOptions} />
                  </div>
                  <img src={logoImage} alt="Logo" className="print-only-logo" style={{ display: "none" }} />
                </MDBox>
              </MDBox>

              <MDBox py={2}>
                <MDBox display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                  <MDBox component="h3" fontWeight="medium">Humidity Chart</MDBox>
                  <MDButton onClick={handlePrintHumidity} color="info">
                    <Icon>print</Icon>&nbsp; Print Chart
                  </MDButton>
                </MDBox>
                <MDBox ref={humidityChartRef} id="humidity-chart"
                       sx={{ position: "relative", width: "100%", height: "80vh" }}>
                  {isPrinting && (
                    <div id="summary-table" style={{ marginBottom: "20px" }}>
                      <HumiditySensorTable sensorData={summaryData} />
                    </div>
                  )}
                  <div className="chart-container" style={{ width: "100%", height: isPrinting ? "calc(100% - 200px)" : "100%" }}>
                    <Line data={createChartData(humidityDatasets)} options={chartOptions} />
                  </div>
                  <img src={logoImage} alt="Logo" className="print-only-logo" style={{ display: "none" }} />
                </MDBox>
              </MDBox>
            </>
          )}
        </MDBox>
      </MDBox>
    </DashboardLayout>
  );
}

export default Dashboard;