using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour la gestion du stock de ciment
    /// </summary>
    public class Stock
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Type de ciment en stock
        /// </summary>
        public CementType CementType { get; set; }
        
        /// <summary>
        /// Quantité totale en sacs
        /// </summary>
        public int TotalQuantity { get; set; }
        
        /// <summary>
        /// Quantité minimum avant alerte
        /// </summary>
        public int MinimumAlertLevel { get; set; } = 50;
        
        /// <summary>
        /// Emplacement du stock
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// Prix unitaire par sac (en devise locale)
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Valeur totale du stock
        /// </summary>
        public decimal TotalValue => TotalQuantity * UnitPrice;
        
        /// <summary>
        /// Indique si le stock est en alerte
        /// </summary>
        public bool IsLowStock => TotalQuantity <= MinimumAlertLevel;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Model pour les mouvements de stock (entrées/sorties)
    /// </summary>
    public class StockMovement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StockId { get; set; }
        
        /// <summary>
        /// Type de mouvement : Entrée, Sortie
        /// </summary>
        public MovementType MovementType { get; set; }
        
        /// <summary>
        /// Quantité modifiée
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Raison du mouvement
        /// </summary>
        public string Reason { get; set; }
        
        /// <summary>
        /// Référence du document (Bon de livraison, Facture, etc.)
        /// </summary>
        public string DocumentReference { get; set; }
        
        /// <summary>
        /// Utilisateur qui a effectué le mouvement
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Quantité avant le mouvement
        /// </summary>
        public int QuantityBefore { get; set; }
        
        /// <summary>
        /// Quantité après le mouvement
        /// </summary>
        public int QuantityAfter { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Types de mouvements de stock
    /// </summary>
    public enum MovementType
    {
        Entree = 0,    // Entrée/Réception
        Sortie = 1     // Sortie/Vente
    }
}
