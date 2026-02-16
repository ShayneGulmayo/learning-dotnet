using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                StockId = comment.StockId,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn
            };
        }
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
        {
            return new Comment
            {
                StockId = stockId,
                Title = commentDto.Title,
                Content = commentDto.Content,
                CreatedOn = DateTime.UtcNow
            };
        }
        
    }

    
}