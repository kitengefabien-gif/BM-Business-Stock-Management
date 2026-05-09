using BMBusinessStockManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour le service de gestion financière
    /// </summary>
    public interface IFinanceService
    {
        /// <summary>
        /// Enregistre une nouvelle transaction financière
        /// </summary>
        Task<FinancialTransaction> CreateTransactionAsync(FinancialTransaction transaction);
        
        /// <summary>
        /// Récupère toutes les transactions
        /// </summary>
        Task<List<FinancialTransaction>> GetAllTransactionsAsync();
        
        /// <summary>
        /// Récupère les transactions par période
        /// </summary>
        Task<List<FinancialTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Récupère les transactions par type
        /// </summary>
        Task<List<FinancialTransaction>> GetTransactionsByTypeAsync(TransactionType type);
        
        /// <summary>
        /// Récupère les transactions par destination
        /// </summary>
        Task<List<FinancialTransaction>> GetTransactionsByDestinationAsync(TruckDestination destination);
        
        /// <summary>
        /// Met à jour une transaction
        /// </summary>
        Task<FinancialTransaction> UpdateTransactionAsync(FinancialTransaction transaction);
        
        /// <summary>
        /// Supprime une transaction (soft delete)
        /// </summary>
        Task<bool> DeleteTransactionAsync(string transactionId);
        
        /// <summary>
        /// Calcule le total des revenus pour une période
        /// </summary>
        Task<decimal> CalculateTotalRevenueAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Calcule le total des coûts pour une période
        /// </summary>
        Task<decimal> CalculateTotalCostsAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Calcule le bénéfice (Revenus - Coûts)
        /// </summary>
        Task<decimal> CalculateProfitAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Génère un rapport financier mensuel
        /// </summary>
        Task<FinancialReport> GenerateMonthlyReportAsync(int year, int month);
        
        /// <summary>
        /// Génère une analyse par destination
        /// </summary>
        Task<List<DestinationAnalysis>> GetDestinationAnalysisAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Récupère les données pour les graphiques
        /// </summary>
        Task<ChartData> GetChartDataAsync(DateTime startDate, DateTime endDate, ChartType chartType);
    }

    /// <summary>
    /// Données pour les graphiques
    /// </summary>
    public class ChartData
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Data { get; set; } = new();
        public string Title { get; set; }
        public ChartType Type { get; set; }
    }

    /// <summary>
    /// Types de graphiques
    /// </summary>
    public enum ChartType
    {
        Bar = 0,      // Barres
        Line = 1,     // Lignes
        Pie = 2       // Camembert
    }
}
