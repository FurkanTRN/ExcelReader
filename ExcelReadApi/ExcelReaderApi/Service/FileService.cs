using ExcelReadApi.DTO;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using ExcelReadApi.Repository;

namespace ExcelReadApi.Service;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<FileDto> GetFileByIdAsync(int fileId, int userId)
    {
        var file = await _fileRepository.GetUploadedFileByIdAsync(fileId);

        if (file == null || file.UserId != userId)
        {
            throw new ArgumentException("File not found or not accessible by the user");
        }

        return new FileDto
        {
            Id = file.Id,
            UserId = file.UserId,
            FileName = file.FileName,
            UploadDate = file.UploadDate,
        };
    }
    

    public async Task AddUploadedFileAsync(FileDto uploadedFileDto, int userId)
    {
        var uploadedFile = new UploadedFile
        {
            UserId = userId,
            FileName = uploadedFileDto.FileName,
            UploadDate = DateTime.Now,
        };

        await _fileRepository.AddUploadedFileAsync(uploadedFile);
    }

    public async Task<List<FileDto>> GetAllUploadedFilesAsync()
    {
        var uploadedFiles = await _fileRepository.GetAllUploadedFilesAsync();
        return uploadedFiles.Select(uf => new FileDto
        {
            Id = uf.Id,
            UserId = uf.UserId,
            FileName = uf.FileName,
            UploadDate = uf.UploadDate,
        }).ToList();
    }

    public async Task<List<UserFileDto>> GetUserFilesAsync(int userId)
    {
        var files = await _fileRepository.GetUserFilesAsync(userId);
        return files.Select(f => new UserFileDto()
        {
            Id = f.Id,
            UploadDate = f.UploadDate,
            FileName = f.FileName,
        }).ToList();
        
    }

    public async Task DeleteFileAsync(int fileId,int userId)
    {
        await _fileRepository.DeleteFileAsync(fileId,userId);
    }
}