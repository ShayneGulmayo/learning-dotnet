using System;

namespace api.Models.StoredProcedures
{
    public class StockWithCommentsRow
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public string MarketCap { get; set; } = string.Empty;

        public int? CommentId { get; set; }
        public int? CommentStockId { get; set; }
        public string? CommentTitle { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? CommentCreatedOn { get; set; }
        public string? CommentAppUserId { get; set; }
        public string? CommentCreatedBy { get; set; }
    }
}
