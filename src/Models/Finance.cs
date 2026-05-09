using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour les transactions financières et rapports
    /// </summary>
    public class FinancialTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Type de transaction : Revenu, Coût, etc.
        /// </summary>
        public TransactionType Type { get; set; }
        
        /// <summary>
        /// Description de la transaction
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Montant en devise locale
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Devise (ZMW pour Zambie, CDF pour RDC)
        /// </summary>
        public string Currency { get; set; } = "ZMW";
        
        /// <summary>
        /// Catégorie de transaction
        /// </summary>
        public TransactionCategory Category { get; set; }
        
        /// <summary>
        /// Destination associée (optionnel)
        /// </summary>
        public TruckDestination? Destination { get; set; }
        
        /// <summary>
        /// ID du camion associé (optionnel)
        /// </summary>
        public string TruckId { get; set; }
        
        /// <summary>
        /// Utilisateur qui a enregistré la transaction
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Numéro de reçu/facture (optionnel)
        /// </summary>
        public string ReceiptNumber { get; set; }
        
        /// <summary>
        /// Méthode de paiement
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
        
        /// <summary>
        /// Statut de la transaction
        /// </summary>
        public TransactionStatus Status { get; set; } = TransactionStatus.Completed;
        
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Model pour les rapports financiers mensuels
    /// </summary>
    public class FinancialReport
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Mois du rapport (format: YYYY-MM)
        /// </summary>
        public string Month { get; set; }
        
        /// <summary>
        /// Total des revenus du mois
        /// </summary>
        public decimal TotalRevenue { get; set; }
        
        /// <summary>
        /// Total des coûts du mois
        /// </summary>
        public decimal TotalCosts { get; set; }
        
        /// <summary>
        /// Bénéfice = Revenus - Coûts
        /// </summary>
        public decimal Profit => TotalRevenue - TotalCosts;
        
        /// <summary>
        /// Marge bénéficiaire en %
        /// </summary>
        public decimal ProfitMargin => TotalRevenue > 0 ? (Profit / TotalRevenue) * 100 : 0;
        
        /// <summary>
        /// Nombre de voyages effectués
        /// </summary>
        public int TripCount { get; set; }
        
        /// <summary>
        /// Montant moyen par voyage
        /// </summary>
        public decimal AverageTripAmount => TripCount > 0 ? TotalRevenue / TripCount : 0;
        
        /// <summary>
        /// Devises utilisées (ZMW, CDF)
        /// </summary>
        public string Currency { get; set; } = "ZMW";
        
        /// <summary>
        /// Analyse par destination
        /// </summary>
        public List<DestinationAnalysis> DestinationBreakdown { get; set; } = new();
        
        /// <summary>
        /// Notes/Observations
        /// </summary>
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Analyse des revenus par destination
    /// </summary>
    public class DestinationAnalysis
    {
        public TruckDestination Destination { get; set; }
        public int TripCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal Profit => TotalRevenue - TotalCosts;
        public decimal AverageRevenue => TripCount > 0 ? TotalRevenue / TripCount : 0;
    }

    /// <summary>
    /// Types de transactions
    /// </summary>
    public enum TransactionType
    {
        Revenu = 0,      // Revenu/Vente
        Cout = 1,        // Coût/Dépense
        Remboursement = 2,
        Bonus = 3,
        Deduction = 4
    }

    /// <summary>
    /// Catégories de transactions
    /// </summary>
    public enum TransactionCategory
    {
        VenteCiment = 0,         // Vente de ciment
        FraisTransport = 1,      // Frais de transport
        CoutCarburant = 2,       // Coût du carburant
        MaintenanceCamion = 3,   // Maintenance véhicule
        SalaireChauffeur = 4,    // Salaire chauffeur
        FraisAdministratifs = 5, // Frais administratifs
        AutreRevenu = 6,
        AutreDepense = 7
    }

    /// <summary>
    /// Méthodes de paiement
    /// </summary>
    public enum PaymentMethod
    {
        Especes = 0,        // Espèces
        Virement = 1,       // Virement bancaire
        Cheque = 2,
        MobileWallet = 3,   // Porte-monnaie mobile (MTN, Airtel, etc.)
        Crypto = 4,         // Crypto-monnaie
        Credit = 5          // Crédit/À crédit
    }

    /// <summary>
    /// Statut de la transaction
    /// </summary>
    public enum TransactionStatus
    {
        Pending = 0,      // En attente
        Completed = 1,    // Complétée
        Cancelled = 2,    // Annulée
        Failed = 3        // Échouée
    }
}
