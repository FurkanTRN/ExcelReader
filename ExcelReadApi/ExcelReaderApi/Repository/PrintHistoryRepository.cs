using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class PrintHistoryRepository : IPrintHistoryRepository
{
    private readonly ExcelReaderApiDbContext _context;

    public PrintHistoryRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<PrintHistory?>> GetPrintHistoriesAsync(int userId)
    {
            return await _context.PrintHistories
                .Include(dh=>dh.UploadedFile)
                .Where(ph => ph.UserId == userId)
                .ToListAsync();
    }

    public async Task<PrintHistory> GetPrintHistoryByIdAndFileIdAsync(int id, int userId, int fileId)
    {
        return await _context.PrintHistories.Include(f=>f.UploadedFile).FirstOrDefaultAsync(ph => ph.Id == id && ph.UserId == userId && ph.FileId == fileId);
    }

    public async Task<PrintHistory?> GetPrintHistoryByIdAsync(int id, int userId)
    {
        return await _context.PrintHistories.FirstOrDefaultAsync(ph => ph.Id == id && ph.UserId == userId);
    }
    public async Task AddPrintHistoryAsync(PrintHistory? printHistory)
    {
        _context.PrintHistories.Add(printHistory);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePrintHistoryAsync(int id,int userId)
    {
        var printHistory = await _context.PrintHistories.FirstOrDefaultAsync(ph => ph.Id == id && ph.UserId == userId);
        if (printHistory != null)
        {
            _context.PrintHistories.Remove(printHistory);
            await _context.SaveChangesAsync();
        }    }

    public async Task<bool> PrintHistoryExistsAsync(int id, int userId, int fileId)
    {
        return await _context.PrintHistories.AnyAsync(e => e.Id == id && e.UserId == userId && e.FileId == fileId);
    }
}