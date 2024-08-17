import React from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
import { styled } from '@mui/material/styles';
import PropTypes from 'prop-types';

const theme = createTheme({
  components: {
    MuiTableCell: {
      styleOverrides: {
        root: {
          padding: '4px 8px',
          textAlign: 'left',
          borderRight: '0.5px solid rgba(224, 224, 224, 0.3)',
          '&:last-child': {
            borderRight: 'none',
          },
        },
        head: {
          backgroundColor: '#f5f5f5',
          fontWeight: 'bold',
          borderBottom: '1px solid rgba(224, 224, 224, 1)',
        },
        body: {
          fontSize: '0.5em',
        },
      },
    },
    MuiTableRow: {
      styleOverrides: {
        root: {
          '&:nth-of-type(odd)': {
            backgroundColor: '#fafafa',
          },
        },
      },
    },
  },
});

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  padding: '4px 8px',
  borderRight: '0.5px solid rgba(224, 224, 224, 0.3)',
  '&:last-child': {
    borderRight: 'none',
  },
}));

const HeaderCell = styled(StyledTableCell)(({ theme }) => ({
  backgroundColor: '#f5f5f5',
  fontWeight: 'bold',
  borderBottom: '1px solid rgba(224, 224, 224, 1)',
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
  '&:nth-of-type(odd)': {
    backgroundColor: '#fafafa',
  },
}));

const SensorSummaryTable = ({ sensorData }) => {
  const calculateSensorStats = (sensorData) => {
    const filteredData = sensorData.filter(entry => entry.name.startsWith("Nem"));

    const groupedData = filteredData.reduce((acc, entry) => {
      const { deviceName, value } = entry;
      if (!acc[deviceName]) {
        acc[deviceName] = [];
      }
      acc[deviceName].push(value);
      return acc;
    }, {});

    // Gruplandırılmış verilere göre istatistikleri hesaplar
    const stats = Object.keys(groupedData).map(deviceName => {
      const values = groupedData[deviceName];
      const sum = values.reduce((a, b) => a + b, 0);
      const avg = sum / values.length;
      const min = Math.min(...values);
      const max = Math.max(...values);

      return {
        deviceName,
        sum,
        avg,
        min,
        max
      };
    });

    return stats;
  };

  const stats = calculateSensorStats(sensorData);

  const minAvg = stats.reduce((minAvg, stat) => (stat.avg < minAvg.avg ? stat : minAvg), stats[0]);
  const avgOfAvgs = stats.reduce((sum, stat) => sum + stat.avg, 0) / stats.length;

  return (
    <>
      <TableContainer component={Paper} sx={{ boxShadow: 'none', border: '1px solid rgba(224, 224, 224, 1)', maxWidth: '400px' }}>
        <Table>
          <TableHead>
            <TableRow>
              <HeaderCell>Device</HeaderCell>
              <HeaderCell align="right">Min</HeaderCell>
              <HeaderCell align="right">Max</HeaderCell>
              <HeaderCell align="right">Average</HeaderCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stats.map((row) => (
              <StyledTableRow key={row.deviceName}>
                <StyledTableCell component="th" scope="row">{row.deviceName}</StyledTableCell>
                <StyledTableCell align="right">%{row.min.toFixed(2)}</StyledTableCell>
                <StyledTableCell align="right">%{row.max.toFixed(2)}</StyledTableCell>
                <StyledTableCell align="right">%{row.avg.toFixed(2)}</StyledTableCell>
              </StyledTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <TableContainer component={Paper} sx={{ boxShadow: 'none', border: '1px solid rgba(224, 224, 224, 1)', marginTop: '16px', maxWidth: '400px' }}>
        <Table>
          <TableHead>
            <TableRow>
              <HeaderCell>Metric</HeaderCell>
              <HeaderCell align="right">Value</HeaderCell>
            </TableRow>
          </TableHead>
          <TableBody>
            <StyledTableRow>
              <StyledTableCell component="th" scope="row">Lowest Sensor</StyledTableCell>
              <StyledTableCell align="right">{minAvg.deviceName}</StyledTableCell>
            </StyledTableRow>
            <StyledTableRow>
              <StyledTableCell component="th" scope="row">Average Humidity</StyledTableCell>
              <StyledTableCell align="right">%{avgOfAvgs.toFixed(2)}</StyledTableCell>
            </StyledTableRow>
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
};

SensorSummaryTable.propTypes = {
  sensorData: PropTypes.arrayOf(PropTypes.shape({
    deviceName: PropTypes.string.isRequired,
    value: PropTypes.number.isRequired,
  })).isRequired,
};

const HumiditySensorTable = (props) => (
  <ThemeProvider theme={theme}>
    <SensorSummaryTable {...props} />
  </ThemeProvider>
);

export default HumiditySensorTable;
