namespace ExcelReadApi.DTO;

public class CreatePrintHistoryDto
{
    public int FileId { get; set; }
    public List<string> Devices { get; set; } = new List<string>();
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}