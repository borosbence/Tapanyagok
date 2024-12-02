namespace Tapanyagok.Server.DTOs
{
    public class DTPostModel
    {
        public int Draw { get; set; }
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 10;
        public Search? Search { get; set; }
        public List<Column>? Columns { get; set; }
        public List<Order>? Order { get; set; }
    }

    public class Column
    {
        public string? Data { get; set; }
        public string? Name { get; set; }
        public bool? Searchable { get; set; }
        public bool? Orderable { get; set; }
        public Search? Search { get; set; }
    }

    public class Search
    {
        public string? Value { get; set; } = null;
        public string? Regex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string? Dir { get; set; }
    }
}
