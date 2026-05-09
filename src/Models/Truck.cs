using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour la gestion des camions
    /// </summary>
    public class Truck
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Nom du chauffeur (converti automatiquement en MAJUSCULES)
        /// </summary>
        public string DriverName { get; set; }
        
        /// <summary>
        /// Numéro de plaque du véhicule
        /// Format : XXX-XXX ou XXXXX
        /// </summary>
        public string LicensePlate { get; set; }
        
        /// <summary>
        /// Numéro de téléphone du chauffeur
        /// Format : +243 XXXXXXXXX (RDC) ou +260 XXXXXXXXX (ZAMBIE)
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Code pays du téléphone (CD pour RDC, ZM pour Zambie)
        /// </summary>
        public string CountryCode { get; set; } = "CD";
        
        /// <summary>
        /// Destination du voyage : Lubumbashi, Kolwezi, Ndola, Lusaka
        /// </summary>
        public TruckDestination Destination { get; set; }
        
        /// <summary>
        /// Numéro de voyage (auto-généré : YYYYMMDD-XXXX)
        /// </summary>
        public string VoyageNumber { get; set; }
        
        /// <summary>
        /// Type de ciment : Supaset, Powerbuild, Duracrete
        /// </summary>
        public CementType CementType { get; set; }
        
        /// <summary>
        /// Quantité de sacs de ciment
        /// </summary>
        public int CementQuantity { get; set; }
        
        /// <summary>
        /// Statut d'arrivée
        /// </summary>
        public ArrivalStatus ArrivalStatus { get; set; } = ArrivalStatus.NonArrive;
        
        /// <summary>
        /// Statut de paiement
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NonPaye;
        
        /// <summary>
        /// Montant du voyage en devise locale
        /// </summary>
        public decimal TravelAmount { get; set; }
        
        /// <summary>
        /// Frais de transport
        /// </summary>
        public decimal TransportCost { get; set; }
        
        /// <summary>
        /// Données de localisation GPS (optionnel)
        /// </summary>
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ArrivedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Types de destinations
    /// </summary>
    public enum TruckDestination
    {
        Lubumbashi = 0,
        Kolwezi = 1,
        Ndola = 2,
        Lusaka = 3
    }

    /// <summary>
    /// Types de ciment disponibles
    /// </summary>
    public enum CementType
    {
        Supaset = 0,
        Powerbuild = 1,
        Duracrete = 2
    }

    /// <summary>
    /// Statut d'arrivée du camion
    /// </summary>
    public enum ArrivalStatus
    {
        Arrive = 0,
        NonArrive = 1
    }

    /// <summary>
    /// Statut de paiement du voyage
    /// </summary>
    public enum PaymentStatus
    {
        Paye = 0,
        NonPaye = 1
    }
}
