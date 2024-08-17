using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IPrintHistoryRepository
{
    Task<List<PrintHistory?>> GetPrintHistoriesAsync(int userId);
    Task<PrintHistory> GetPrintHistoryByIdAndFileIdAsync(int id, int userId, int fileId);
    Task<PrintHistory?> GetPrintHistoryByIdAsync(int id, int userId);
    Task AddPrintHistoryAsync(PrintHistory? printHistory);
    Task DeletePrintHistoryAsync(int id,int userId);
    Task<bool> PrintHistoryExistsAsync(int id, int userId, int fileId);
}