using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMBusinessStockManagement.Models;
using SQLite;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Implémentation du service de gestion du stock
    /// </summary>
    public class StockService : IStockService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly ISyncService _syncService;

        public StockService(string databasePath, ISyncService syncService)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _syncService = syncService;
            InitializeDatabaseAsync().Wait();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<Stock>();
            await _database.CreateTableAsync<StockMovement>();
        }

        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            await _database.InsertAsync(stock);
            await _syncService.QueueOperationAsync("CREATE", stock.Id, stock);
            return stock;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _database.Table<Stock>()
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.CementType)
                .ToListAsync();
        }

        public async Task<Stock> GetStockByCementTypeAsync(CementType cementType)
        {
            return await _database.Table<Stock>()
                .FirstOrDefaultAsync(s => s.CementType == cementType && !s.IsDeleted);
        }

        public async Task<StockMovement> AddStockEntryAsync(string stockId, int quantity, string reason, string documentReference)
        {
            var stock = await _database.FindAsync<Stock>(stockId);
            if (stock == null) return null;

            var movement = new StockMovement
            {
                StockId = stockId,
                MovementType = StockMovementType.Entree,
                Quantity = quantity,
                Reason = reason,
                DocumentReference = documentReference,
                QuantityBefore = stock.TotalQuantity,
                QuantityAfter = stock.TotalQuantity + quantity
            };

            stock.TotalQuantity += quantity;
            await _database.InsertAsync(movement);
            await _database.UpdateAsync(stock);
            await _syncService.QueueOperationAsync("STOCK_ENTRY", stockId, movement);

            return movement;
        }

        public async Task<StockMovement> RemoveStockAsync(string stockId, int quantity, string reason, string documentReference)
        {
            var stock = await _database.FindAsync<Stock>(stockId);
            if (stock == null || stock.TotalQuantity < quantity) return null;

            var movement = new StockMovement
            {
                StockId = stockId,
                MovementType = StockMovementType.Sortie,
                Quantity = quantity,
                Reason = reason,
                DocumentReference = documentReference,
                QuantityBefore = stock.TotalQuantity,
                QuantityAfter = stock.TotalQuantity - quantity
            };

            stock.TotalQuantity -= quantity;
            await _database.InsertAsync(movement);
            await _database.UpdateAsync(stock);
            await _syncService.QueueOperationAsync("STOCK_EXIT", stockId, movement);

            return movement;
        }

        public async Task<List<StockMovement>> GetStockMovementsAsync(string stockId, int? limit = null)
        {
            var query = _database.Table<StockMovement>()
                .Where(m => m.StockId == stockId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt);

            if (limit.HasValue)
                query = query.Take(limit.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Stock>> GetLowStockAlertsAsync()
        {
            return await _database.Table<Stock>()
                .Where(s => !s.IsDeleted && s.TotalQuantity <= s.MinimumAlertLevel)
                .OrderBy(s => s.TotalQuantity)
                .ToListAsync();
        }

        public async Task<Stock> UpdateStockAsync(Stock stock)
        {
            stock.UpdatedAt = DateTime.UtcNow;
            await _database.UpdateAsync(stock);
            await _syncService.QueueOperationAsync("UPDATE", stock.Id, stock);
            return stock;
        }

        public async Task<decimal> CalculateTotalStockValueAsync()
        {
            var stocks = await GetAllStocksAsync();
            return stocks.Sum(s => s.TotalQuantity * (s.UnitPrice ?? 0));
        }

        public async Task<Dictionary<CementType, int>> GetStockReportAsync()
        {
            var stocks = await GetAllStocksAsync();
            return stocks.ToDictionary(s => s.CementType, s => s.TotalQuantity);
        }
    }

    /// <summary>
    /// Type de mouvement de stock
    /// </summary>
    public enum StockMovementType
    {
        Entree = 0,  // Entrée
        Sortie = 1   // Sortie
    }
}
