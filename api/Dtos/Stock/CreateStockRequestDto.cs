using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal Purchase { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Current price must be greater than 0.")]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot exceed 50 characters.")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [MaxLength(20, ErrorMessage = "Market Cap cannot exceed 20 characters.")]
        public string MarketCap { get; set; } = string.Empty;
    }
}