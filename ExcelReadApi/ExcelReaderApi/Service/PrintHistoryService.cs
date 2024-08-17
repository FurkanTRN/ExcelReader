using ExcelReadApi.DTO;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;

namespace ExcelReadApi.Service;

public class PrintHistoryService : IPrintHistoryService
{
    private readonly IPrintHistoryRepository _historyRepository;
    private readonly IFileService _fileService;

    public PrintHistoryService(IPrintHistoryRepository historyRepository,IFileService fileService)
    {
        _historyRepository = historyRepository;
        _fileService = fileService;
    }
    public async Task<List<PrintHistoryResponse>> GetPrintHistoriesAsync(int userId)
    {
        var histories= await _historyRepository.GetPrintHistoriesAsync(userId);
        var historyResponse = histories.Select(ds => new PrintHistoryResponse()
        {
            UserId = ds.UserId,
            PrintedAt = ds.PrintedAt,
            Devices = ds.Devices,
            FileId = ds.FileId,
            FileName = ds.UploadedFile.FileName,
            Id = ds.Id,
            EndDateTime = ds.EndDateTime,
            StartDateTime = ds.StartDateTime
        }).ToList();

        return historyResponse;
    }

    public async Task<PrintHistoryResponse> GetPrintHistoryByIdAsync(int id, int userId)
    {
        var history = await _historyRepository.GetPrintHistoryByIdAsync(id, userId);
        
        return new PrintHistoryResponse()
        {
            Id = history.Id,
            Devices = history.Devices,
            FileId = history.FileId,
            PrintedAt = history.PrintedAt,
            UserId = history.UserId,
            EndDateTime = history.EndDateTime,
            StartDateTime = history.StartDateTime
        };
    }

    public async Task AddPrintHistoryAsync(CreatePrintHistoryDto dto,int userId)
    {
        var printHistory = new PrintHistory()
        {
            FileId = dto.FileId,
            Devices = dto.Devices,
            PrintedAt = DateTime.Now,
            UserId = userId,
            StartDateTime = DateTime.Parse(dto.StartDate),
            EndDateTime = DateTime.Parse(dto.EndDate)
        };
        await _historyRepository.AddPrintHistoryAsync(printHistory);
    }

    public async Task DeletePrintHistoryAsync(int printHistoryId,int userId )
    {
        var printHistory = await _historyRepository.GetPrintHistoryByIdAsync(printHistoryId, userId);
    
        if (printHistory != null)
        {
            await _historyRepository.DeletePrintHistoryAsync(printHistoryId,userId);
        }
        else
        {
            throw new KeyNotFoundException("Print history not found.");
        }
    }
}