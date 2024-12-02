namespace Tapanyagok.Server.DTOs
{
    public class DTResult<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T>? Data { get; set; }
        public string? Error { get; set; }
    }
}
