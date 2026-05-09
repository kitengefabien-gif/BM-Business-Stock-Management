using BMBusinessStockManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour le service de gestion des camions
    /// </summary>
    public interface ITruckService
    {
        /// <summary>
        /// Crée un nouveau voyage de camion
        /// </summary>
        Task<Truck> CreateTruckAsync(Truck truck);
        
        /// <summary>
        /// Récupère tous les camions
        /// </summary>
        Task<List<Truck>> GetAllTrucksAsync();
        
        /// <summary>
        /// Récupère un camion par ID
        /// </summary>
        Task<Truck> GetTruckByIdAsync(string truckId);
        
        /// <summary>
        /// Récupère l'historique des plaques minéralogiques pour auto-complétion
        /// </summary>
        Task<List<string>> GetLicensePlateHistoryAsync(string searchTerm = "");
        
        /// <summary>
        /// Récupère l'historique des numéros de téléphone pour auto-complétion
        /// </summary>
        Task<List<string>> GetPhoneNumberHistoryAsync(string searchTerm = "");
        
        /// <summary>
        /// Récupère l'historique des noms de chauffeurs
        /// </summary>
        Task<List<string>> GetDriverNameHistoryAsync(string searchTerm = "");
        
        /// <summary>
        /// Met à jour un camion existant
        /// </summary>
        Task<Truck> UpdateTruckAsync(Truck truck);
        
        /// <summary>
        /// Supprime un camion (soft delete)
        /// </summary>
        Task<bool> DeleteTruckAsync(string truckId);
        
        /// <summary>
        /// Met à jour le statut d'arrivée
        /// </summary>
        Task<bool> UpdateArrivalStatusAsync(string truckId, ArrivalStatus status);
        
        /// <summary>
        /// Met à jour le statut de paiement
        /// </summary>
        Task<bool> UpdatePaymentStatusAsync(string truckId, PaymentStatus status);
        
        /// <summary>
        /// Génère automatiquement le numéro de voyage
        /// Format : YYYYMMDD-XXXX
        /// </summary>
        Task<string> GenerateVoyageNumberAsync();
        
        /// <summary>
        /// Récupère les camions par statut
        /// </summary>
        Task<List<Truck>> GetTrucksByStatusAsync(ArrivalStatus? arrival = null, PaymentStatus? payment = null);
        
        /// <summary>
        /// Vérifie si une plaque minéralogique existe déjà
        /// </summary>
        Task<bool> LicensePlateExistsAsync(string licensePlate, string excludeTruckId = null);
    }
}
