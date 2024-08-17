using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IFileRepository
{
    Task<UploadedFile> GetUploadedFileByIdAsync(int uploadedFileId);
    Task AddUploadedFileAsync(UploadedFile uploadedFile);
    Task<List<UploadedFile>> GetAllUploadedFilesAsync();
    Task<List<UploadedFile>> GetUserFilesAsync(int userId);

    Task DeleteFileAsync(int fileId, int userId);
}