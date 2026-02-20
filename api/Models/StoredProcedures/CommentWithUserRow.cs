using System;

namespace api.Models.StoredProcedures
{
    public class CommentWithUserRow
    {
        public int Id { get; set; }
        public int? StockId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
