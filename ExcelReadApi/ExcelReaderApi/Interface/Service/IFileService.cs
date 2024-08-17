using ExcelReadApi.DTO;

namespace ExcelReadApi.Interface;

public interface IFileService
{
    Task<FileDto> GetFileByIdAsync(int fileId, int userId);
    Task AddUploadedFileAsync(FileDto uploadedFileDto, int userId);
    Task<List<FileDto>> GetAllUploadedFilesAsync();
    Task<List<UserFileDto>> GetUserFilesAsync(int userId);
    Task DeleteFileAsync(int fileId, int userId);
}