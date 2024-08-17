namespace ExcelReadApi.DTO;

public class FileDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
}