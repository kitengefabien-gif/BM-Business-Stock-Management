using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour la gestion financière
    /// Revenus, Coûts, Bénéfices
    /// </summary>
    public class Finance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// ID du camion/voyage associé
        /// </summary>
        public string TruckId { get; set; }
        
        /// <summary>
        /// Montant des revenus (prix de vente)
        /// </summary>
        public decimal Revenue { get; set; }
        
        /// <summary>
        /// Montant des coûts (transport, carburant, etc.)
        /// </summary>
        public decimal Cost { get; set; }
        
        /// <summary>
        /// Bénéfice calculé automatiquement
        /// Formule : Bénéfice = Revenus - Coûts
        /// </summary>
        public decimal Profit => Revenue - Cost;
        
        /// <summary>
        /// Pourcentage de marge bénéficiaire
        /// </summary>
        public decimal ProfitMargin => Revenue > 0 ? (Profit / Revenue) * 100 : 0;
        
        /// <summary>
        /// Type de transaction
        /// </summary>
        public TransactionType TransactionType { get; set; }
        
        /// <summary>
        /// Destination associée
        /// </summary>
        public TruckDestination Destination { get; set; }
        
        /// <summary>
        /// Mois et année de la transaction
        /// </summary>
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Devise locale (ZMW, CDF)
        /// </summary>
        public string Currency { get; set; } = "ZMW";
        
        /// <summary>
        /// Notes/Observations
        /// </summary>
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Model pour les statistiques financières mensuelles/annuelles
    /// </summary>
    public class FinancialReport
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Période du rapport
        /// </summary>
        public ReportPeriod Period { get; set; }
        
        /// <summary>
        /// Année du rapport
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// Mois du rapport (1-12, 0 si année entière)
        /// </summary>
        public int? Month { get; set; }
        
        /// <summary>
        /// Trimestre du rapport (optionnel)
        /// </summary>
        public int? Quarter { get; set; }
        
        /// <summary>
        /// Total des revenus pour la période
        /// </summary>
        public decimal TotalRevenue { get; set; }
        
        /// <summary>
        /// Total des coûts pour la période
        /// </summary>
        public decimal TotalCost { get; set; }
        
        /// <summary>
        /// Bénéfice total = TotalRevenue - TotalCost
        /// </summary>
        public decimal TotalProfit => TotalRevenue - TotalCost;
        
        /// <summary>
        /// Nombre de transactions
        /// </summary>
        public int TransactionCount { get; set; }
        
        /// <summary>
        /// Revenus par destination (JSON ou sérialisé)
        /// </summary>
        public string RevenueByDestination { get; set; }
        
        /// <summary>
        /// Bénéfice par destination
        /// </summary>
        public string ProfitByDestination { get; set; }
        
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Types de transactions
    /// </summary>
    public enum TransactionType
    {
        Vente = 0,      // Vente de ciment
        Transport = 1,  // Frais de transport
        Carburant = 2,  // Carburant
        Maintenance = 3, // Maintenance camion
        Autre = 4       // Autre
    }

    /// <summary>
    /// Périodes de rapport
    /// </summary>
    public enum ReportPeriod
    {
        Hebdomadaire = 0,
        Mensuel = 1,
        Trimestriel = 2,
        Annuel = 3
    }
}
