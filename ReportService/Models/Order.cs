namespace ReportService.Models
{
    public class Order
    {
        public string? User { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
