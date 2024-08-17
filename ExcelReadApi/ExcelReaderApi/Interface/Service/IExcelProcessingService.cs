namespace ExcelReadApi.Interface;

public interface IExcelProcessingService
{
    Task ProcessExcelFileAsync(Stream excelFileStream, int userId,int fileId);
}