namespace ExcelReadApi.DTO;

public class PrintHistoryResponse
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public string FileName { get; set; }
    public List<string> Devices { get; set; } = new List<string>();
    public DateTime PrintedAt { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int UserId { get; set; }
}