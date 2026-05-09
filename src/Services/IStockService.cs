using BMBusinessStockManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour le service de gestion du stock de ciment
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Crée un nouveau stock
        /// </summary>
        Task<Stock> CreateStockAsync(Stock stock);
        
        /// <summary>
        /// Récupère tous les stocks
        /// </summary>
        Task<List<Stock>> GetAllStocksAsync();
        
        /// <summary>
        /// Récupère le stock d'un type de ciment
        /// </summary>
        Task<Stock> GetStockByCementTypeAsync(CementType cementType);
        
        /// <summary>
        /// Enregistre une entrée de stock
        /// </summary>
        Task<StockMovement> AddStockEntryAsync(string stockId, int quantity, string reason, string documentReference);
        
        /// <summary>
        /// Enregistre une sortie de stock
        /// </summary>
        Task<StockMovement> RemoveStockAsync(string stockId, int quantity, string reason, string documentReference);
        
        /// <summary>
        /// Récupère l'historique des mouvements de stock
        /// </summary>
        Task<List<StockMovement>> GetStockMovementsAsync(string stockId, int? limit = null);
        
        /// <summary>
        /// Récupère les stocks en alerte (stock bas)
        /// </summary>
        Task<List<Stock>> GetLowStockAlertsAsync();
        
        /// <summary>
        /// Met à jour un stock
        /// </summary>
        Task<Stock> UpdateStockAsync(Stock stock);
        
        /// <summary>
        /// Calcule la valeur totale du stock
        /// </summary>
        Task<decimal> CalculateTotalStockValueAsync();
        
        /// <summary>
        /// Récupère le rapport de stock (quantité par type)
        /// </summary>
        Task<Dictionary<CementType, int>> GetStockReportAsync();
    }
}
