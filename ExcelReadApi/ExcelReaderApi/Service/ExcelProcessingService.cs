using System.Globalization;
using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using OfficeOpenXml;

namespace ExcelReadApi.Service;

public class ExcelProcessingService : IExcelProcessingService
{
    private readonly ExcelReaderApiDbContext _context;

    public ExcelProcessingService(ExcelReaderApiDbContext context)
    {
        _context = context;
    }

   public async Task ProcessExcelFileAsync(Stream stream,int userId,int fileId)
    {

        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets[0]; // İlk sayfayı al
            var rowCount = worksheet.Dimension.Rows;
            var columnCount = worksheet.Dimension.Columns;

            for (int row = 2; row <= rowCount; row++) // Başlık satırını atla
            {
                var deviceId = worksheet.Cells[row, 1].Text; // Cihaz Id
                var deviceName = worksheet.Cells[row, 2].Text; // Cihaz İsmi
                var recordDate = DateTime.Parse(worksheet.Cells[row, 3].Text, CultureInfo.InvariantCulture); // Kayıt Tarihi
                
                var device = new Device
                {
                    DeviceIdentity = deviceId,
                    Name = deviceName,
                    UserId = userId,
                    UploadedFileId = fileId,
                    DeviceSensors = new List<DeviceSensor>()
                };

                for (int col = 4; col <= columnCount; col++)
                {
                    var sensorName = worksheet.Cells[1, col].Text; // Sensör Adı (başlık)
                    var sensorValueText = worksheet.Cells[row, col].Text; // Sensör Değeri

                    if (string.IsNullOrEmpty(sensorName) || string.IsNullOrEmpty(sensorValueText))
                        continue;
                    
                    if (sensorName.StartsWith("Maks.") || sensorName.StartsWith("Min."))
                        continue;

                    if (float.TryParse(sensorValueText, NumberStyles.Float, CultureInfo.InvariantCulture, out var sensorValue))
                    {
                        var deviceSensor = new DeviceSensor
                        {
                            DeviceIdentity = deviceId,
                            
                            Sensor = new Sensor
                            {
                                Name = sensorName,
                                Value = sensorValue,
                                RecordDate = recordDate
                            },
                        };

                        device.DeviceSensors.Add(deviceSensor);
                    }
                }

                _context.Devices.Add(device);
            }

            await _context.SaveChangesAsync();
        }
    }
}