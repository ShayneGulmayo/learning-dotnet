using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
        [MaxLength(280, ErrorMessage = "Title cannot exceed 280 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
        [MaxLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}