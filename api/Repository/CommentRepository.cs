using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Models.StoredProcedures;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            var createdComments = await _context.Comments
                .FromSqlInterpolated($"CALL sp_comments_create({commentModel.StockId}, {commentModel.Title}, {commentModel.Content}, {commentModel.CreatedOn}, {commentModel.AppUserId})")
                .AsNoTracking()
                .ToListAsync();

            return createdComments.First();
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var deletedComments = await _context.Comments
                .FromSqlInterpolated($"CALL sp_comments_delete({id})")
                .AsNoTracking()
                .ToListAsync();

            return deletedComments.FirstOrDefault();
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var rows = await _context.CommentWithUserRows
                .FromSqlInterpolated($"CALL sp_comments_get_all()")
                .AsNoTracking()
                .ToListAsync();

            return rows.Select(MapComment).ToList();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var rows = await _context.CommentWithUserRows
                .FromSqlInterpolated($"CALL sp_comments_get_by_id({id})")
                .AsNoTracking()
                .ToListAsync();

            var row = rows.FirstOrDefault();
            return row == null ? null : MapComment(row);
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment commentModel)
        {
            var updatedComments = await _context.Comments
                .FromSqlInterpolated($"CALL sp_comments_update({id}, {commentModel.Title}, {commentModel.Content})")
                .AsNoTracking()
                .ToListAsync();

            return updatedComments.FirstOrDefault();
        }

        private static Comment MapComment(CommentWithUserRow row)
        {
            return new Comment
            {
                Id = row.Id,
                StockId = row.StockId,
                Title = row.Title,
                Content = row.Content,
                CreatedOn = row.CreatedOn,
                AppUserId = row.AppUserId,
                AppUser = new AppUser
                {
                    Id = row.AppUserId,
                    UserName = row.CreatedBy
                }
            };
        }
    }
}