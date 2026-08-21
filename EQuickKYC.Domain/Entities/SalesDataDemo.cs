using System.ComponentModel.DataAnnotations;

namespace EQuickKYC.Domain.Entities
{
    public class SalesDataDemo
    {
        [Key]
        public long Id { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string SalesChannel { get; set; } = string.Empty;
        public string OrderPriority { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public long OrderId { get; set; }
        public DateTime ShipDate { get; set; }

        public int UnitsSold { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
