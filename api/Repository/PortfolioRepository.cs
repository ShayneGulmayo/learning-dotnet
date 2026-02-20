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
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            var portfolios = await _context.Portfolios
                .FromSqlInterpolated($"CALL sp_portfolios_create({portfolio.AppUserId}, {portfolio.StockId})")
                .AsNoTracking()
                .ToListAsync();

            return portfolios.First();
        }

        public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
        {
            var deletedPortfolios = await _context.Portfolios
                .FromSqlInterpolated($"CALL sp_portfolios_delete({appUser.Id}, {symbol})")
                .AsNoTracking()
                .ToListAsync();

            return deletedPortfolios.FirstOrDefault();
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            var rows = await _context.PortfolioStockRows
                .FromSqlInterpolated($"CALL sp_portfolios_get_by_user({user.Id})")
                .AsNoTracking()
                .ToListAsync();

            return rows.Select(row => new Stock
            {
                Id = row.Id,
                Symbol = row.Symbol,
                CompanyName = row.CompanyName,
                Purchase = row.Purchase,
                LastDiv = row.LastDiv,
                Industry = row.Industry,
                MarketCap = row.MarketCap
            }).ToList();
        }

        public async Task<bool> ExistsAsync(string appUserId, string symbol)
        {
            var existsRow = await _context.ExistsRows
                .FromSqlInterpolated($"CALL sp_portfolios_exists({appUserId}, {symbol})")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return existsRow?.ExistsFlag ?? false;
        }
    }
}