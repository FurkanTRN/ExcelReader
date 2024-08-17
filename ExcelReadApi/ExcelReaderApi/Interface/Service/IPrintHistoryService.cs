using ExcelReadApi.DTO;
using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IPrintHistoryService
{
    Task<List<PrintHistoryResponse>> GetPrintHistoriesAsync(int userId);
    Task<PrintHistoryResponse> GetPrintHistoryByIdAsync(int id, int userId);
    Task AddPrintHistoryAsync(CreatePrintHistoryDto dto,int userId);
    Task DeletePrintHistoryAsync(int id, int userId);
    
}