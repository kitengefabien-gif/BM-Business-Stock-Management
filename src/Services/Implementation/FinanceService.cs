using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMBusinessStockManagement.Models;
using SQLite;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour les opérations financières
    /// </summary>
    public interface IFinanceService
    {
        Task<FinancialTransaction> CreateTransactionAsync(FinancialTransaction transaction);
        Task<List<FinancialTransaction>> GetAllTransactionsAsync();
        Task<List<FinancialTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<FinancialTransaction>> GetTransactionsByDestinationAsync(string destination);
        Task<List<FinancialTransaction>> GetTransactionsByTypeAsync(string transactionType);
        Task<decimal> CalculateTotalRevenueAsync(DateTime startDate, DateTime endDate);
        Task<decimal> CalculateTotalCostsAsync(DateTime startDate, DateTime endDate);
        Task<decimal> CalculateProfitAsync(DateTime startDate, DateTime endDate);
        Task<FinancialReport> GenerateMonthlyReportAsync(string month);
        Task<Dictionary<string, decimal>> GetRevenueByDestinationAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, decimal>> GetCostsByCategoryAsync(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Implémentation du service financier
    /// </summary>
    public class FinanceService : IFinanceService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly ISyncService _syncService;

        public FinanceService(string databasePath, ISyncService syncService)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _syncService = syncService;
            InitializeDatabaseAsync().Wait();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<FinancialTransaction>();
            await _database.CreateTableAsync<FinancialReport>();
        }

        public async Task<FinancialTransaction> CreateTransactionAsync(FinancialTransaction transaction)
        {
            transaction.Id = Guid.NewGuid().ToString();
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.TransactionDate = transaction.TransactionDate ?? DateTime.UtcNow;

            await _database.InsertAsync(transaction);
            await _syncService.QueueOperationAsync("CREATE_TRANSACTION", transaction.Id, transaction);

            return transaction;
        }

        public async Task<List<FinancialTransaction>> GetAllTransactionsAsync()
        {
            return await _database.Table<FinancialTransaction>()
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<FinancialTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var start = startDate.Date;
            var end = endDate.Date.AddDays(1);

            return await _database.Table<FinancialTransaction>()
                .Where(t => !t.IsDeleted 
                    && t.TransactionDate >= start 
                    && t.TransactionDate < end)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<FinancialTransaction>> GetTransactionsByDestinationAsync(string destination)
        {
            return await _database.Table<FinancialTransaction>()
                .Where(t => !t.IsDeleted && t.Destination == destination)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<FinancialTransaction>> GetTransactionsByTypeAsync(string transactionType)
        {
            return await _database.Table<FinancialTransaction>()
                .Where(t => !t.IsDeleted && t.TransactionType == transactionType)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> CalculateTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByDateRangeAsync(startDate, endDate);
            return transactions
                .Where(t => t.TransactionType == "revenu")
                .Sum(t => t.Amount);
        }

        public async Task<decimal> CalculateTotalCostsAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByDateRangeAsync(startDate, endDate);
            return transactions
                .Where(t => t.TransactionType == "cout")
                .Sum(t => t.Amount);
        }

        public async Task<decimal> CalculateProfitAsync(DateTime startDate, DateTime endDate)
        {
            var revenue = await CalculateTotalRevenueAsync(startDate, endDate);
            var costs = await CalculateTotalCostsAsync(startDate, endDate);
            return revenue - costs;
        }

        public async Task<FinancialReport> GenerateMonthlyReportAsync(string month)
        {
            // Format: YYYY-MM
            var startDate = DateTime.ParseExact(month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
            var endDate = startDate.AddMonths(1);

            var revenue = await CalculateTotalRevenueAsync(startDate, endDate);
            var costs = await CalculateTotalCostsAsync(startDate, endDate);
            var tripCount = await _database.Table<FinancialTransaction>()
                .CountAsync(t => !t.IsDeleted 
                    && t.TransactionType == "revenu"
                    && t.TransactionDate >= startDate 
                    && t.TransactionDate < endDate);

            var report = new FinancialReport
            {
                Id = Guid.NewGuid().ToString(),
                Month = month,
                TotalRevenue = revenue,
                TotalCosts = costs,
                TripCount = tripCount,
                Currency = "ZMW",
                CreatedAt = DateTime.UtcNow
            };

            // Vérifier si le rapport existe déjà
            var existingReport = await _database.Table<FinancialReport>()
                .FirstOrDefaultAsync(r => r.Month == month);

            if (existingReport != null)
            {
                report.Id = existingReport.Id;
                await _database.UpdateAsync(report);
            }
            else
            {
                await _database.InsertAsync(report);
            }

            await _syncService.QueueOperationAsync("UPDATE_REPORT", report.Id, report);

            return report;
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByDestinationAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByDateRangeAsync(startDate, endDate);
            
            return transactions
                .Where(t => t.TransactionType == "revenu" && !string.IsNullOrEmpty(t.Destination))
                .GroupBy(t => t.Destination)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        public async Task<Dictionary<string, decimal>> GetCostsByategoryAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByDateRangeAsync(startDate, endDate);
            
            return transactions
                .Where(t => t.TransactionType == "cout" && !string.IsNullOrEmpty(t.Category))
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }
    }
}
