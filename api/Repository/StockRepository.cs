using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using api.Models.StoredProcedures;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var rows = await _context.StockWithCommentsRows
                .FromSqlInterpolated($"CALL sp_stocks_get_all({query.Symbol}, {query.CompanyName}, {query.SortBy}, {query.IsDescending}, {skipNumber}, {query.PageSize})")
                .AsNoTracking()
                .ToListAsync();

            return MapStocksWithComments(rows);
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            var rows = await _context.StockWithCommentsRows
                .FromSqlInterpolated($"CALL sp_stocks_get_by_id({id})")
                .AsNoTracking()
                .ToListAsync();

            return MapStocksWithComments(rows).FirstOrDefault();
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            var createdStocks = await _context.Stocks
                .FromSqlInterpolated($"CALL sp_stocks_create({stockModel.Symbol}, {stockModel.CompanyName}, {stockModel.Purchase}, {stockModel.LastDiv}, {stockModel.Industry}, {stockModel.MarketCap})")
                .AsNoTracking()
                .ToListAsync();

            return createdStocks.First();
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
        {
            var updatedStocks = await _context.Stocks
                .FromSqlInterpolated($"CALL sp_stocks_update({id}, {stockDto.Symbol}, {stockDto.CompanyName}, {stockDto.Purchase}, {stockDto.LastDiv}, {stockDto.Industry}, {stockDto.MarketCap})")
                .AsNoTracking()
                .ToListAsync();

            return updatedStocks.FirstOrDefault();
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var deletedStocks = await _context.Stocks
                .FromSqlInterpolated($"CALL sp_stocks_delete({id})")
                .AsNoTracking()
                .ToListAsync();

            return deletedStocks.FirstOrDefault();
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            var existsRow = await _context.ExistsRows
                .FromSqlInterpolated($"CALL sp_stocks_exists({id})")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return existsRow?.ExistsFlag ?? false;
        }

        public async Task<Stock?> GetStockBySymbolAsync(string symbol)
        {
            var stocks = await _context.Stocks
                .FromSqlInterpolated($"CALL sp_stocks_get_by_symbol({symbol})")
                .AsNoTracking()
                .ToListAsync();

            return stocks.FirstOrDefault();
        }

        private static List<Stock> MapStocksWithComments(List<StockWithCommentsRow> rows)
        {
            return rows
                .GroupBy(row => row.Id)
                .Select(stockGroup =>
                {
                    var first = stockGroup.First();
                    var stock = new Stock
                    {
                        Id = first.Id,
                        Symbol = first.Symbol,
                        CompanyName = first.CompanyName,
                        Purchase = first.Purchase,
                        LastDiv = first.LastDiv,
                        Industry = first.Industry,
                        MarketCap = first.MarketCap,
                        Comments = stockGroup
                            .Where(row => row.CommentId.HasValue)
                            .GroupBy(row => row.CommentId)
                            .Select(commentGroup =>
                            {
                                var commentRow = commentGroup.First();
                                return new Comment
                                {
                                    Id = commentRow.CommentId!.Value,
                                    StockId = commentRow.CommentStockId,
                                    Title = commentRow.CommentTitle ?? string.Empty,
                                    Content = commentRow.CommentContent ?? string.Empty,
                                    CreatedOn = commentRow.CommentCreatedOn ?? DateTime.UtcNow,
                                    AppUserId = commentRow.CommentAppUserId ?? string.Empty,
                                    AppUser = new AppUser
                                    {
                                        Id = commentRow.CommentAppUserId ?? string.Empty,
                                        UserName = commentRow.CommentCreatedBy ?? "Unknown"
                                    }
                                };
                            })
                            .ToList()
                    };

                    return stock;
                })
                .ToList();
        }
    }
}