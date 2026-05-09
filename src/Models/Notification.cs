using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour les notifications et alertes
    /// </summary>
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Type de notification
        /// </summary>
        public NotificationType Type { get; set; }
        
        /// <summary>
        /// Titre de la notification
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Message/Contenu
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Utilisateur destinataire
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Lien associé (optionnel)
        /// </summary>
        public string ActionUrl { get; set; }
        
        /// <summary>
        /// Paramètres supplémentaires (JSON)
        /// </summary>
        public string Metadata { get; set; }
        
        /// <summary>
        /// Indique si la notification a été lue
        /// </summary>
        public bool IsRead { get; set; } = false;
        
        /// <summary>
        /// Indique si envoyée via WhatsApp
        /// </summary>
        public bool SentViaWhatsApp { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// Model pour les messages WhatsApp
    /// </summary>
    public class WhatsAppMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Numéro de téléphone destinataire (+243... ou +260...)
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Message à envoyer
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Type de message
        /// </summary>
        public WhatsAppMessageType MessageType { get; set; }
        
        /// <summary>
        /// Référence au camion (optionnel)
        /// </summary>
        public string TruckId { get; set; }
        
        /// <summary>
        /// ID de l'entreprise/organisation
        /// </summary>
        public string OrganizationId { get; set; }
        
        /// <summary>
        /// Statut d'envoi
        /// </summary>
        public WhatsAppStatus Status { get; set; } = WhatsAppStatus.Pending;
        
        /// <summary>
        /// Identifiant du message WhatsApp
        /// </summary>
        public string WhatsAppMessageId { get; set; }
        
        /// <summary>
        /// Raison de l'échec (si applicable)
        /// </summary>
        public string ErrorMessage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Types de notifications
    /// </summary>
    public enum NotificationType
    {
        CamionArrive = 0,      // Camion arrivé à destination
        Paiement = 1,          // Paiement effectué
        StockBas = 2,          // Stock bas/alerte
        NouveauVoyage = 3,     // Nouveau voyage créé
        AlerteFinance = 4,     // Alerte financière
        MaintenanceCamion = 5, // Maintenance camion
        Autre = 6
    }

    /// <summary>
    /// Types de messages WhatsApp
    /// </summary>
    public enum WhatsAppMessageType
    {
        ArriveeNotification = 0,    // Notification d'arrivée
        PaymentReminder = 1,         // Rappel de paiement
        LowStockAlert = 2,          // Alerte stock bas
        ManualMessage = 3           // Message manuel
    }

    /// <summary>
    /// Statut d'envoi WhatsApp
    /// </summary>
    public enum WhatsAppStatus
    {
        Pending = 0,      // En attente
        Sent = 1,         // Envoyé
        Delivered = 2,    // Livré
        Read = 3,         // Lu
        Failed = 4        // Échoué
    }
}
