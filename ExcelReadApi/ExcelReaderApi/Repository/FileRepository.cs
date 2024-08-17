using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class FileRepository : IFileRepository
{
    private readonly ExcelReaderApiDbContext _context;

    public FileRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }
    public async Task<UploadedFile> GetUploadedFileByIdAsync(int uploadedFileId)
    {
        return await _context.UploadedFiles.FindAsync(uploadedFileId);
    }

    public async Task AddUploadedFileAsync(UploadedFile uploadedFile)
    {
        _context.UploadedFiles.Add(uploadedFile);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UploadedFile>> GetAllUploadedFilesAsync()
    {
        return await _context.UploadedFiles.ToListAsync();
    }

    public async Task<List<UploadedFile>> GetUserFilesAsync(int userId)
    {
        return await _context.UploadedFiles.Where(s=>s.UserId==userId).ToListAsync();
    }

    public async Task DeleteFileAsync(int fileId,int userId)
    {
        var file = await _context.UploadedFiles.Include(f=>f.PrintHistories).Where(s=>s.Id==fileId && s.UserId==userId).FirstOrDefaultAsync();
        if (file==null)
        {
            return;
        }

        _context.UploadedFiles.Remove(file);
        await _context.SaveChangesAsync();
    }
}