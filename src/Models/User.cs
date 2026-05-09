using System;

namespace BMBusinessStockManagement.Models
{
    /// <summary>
    /// Model pour la gestion des utilisateurs
    /// Rôles : Admin, Gestionnaire, Comptable
    /// </summary>
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        
        /// <summary>
        /// Rôles disponibles : Admin, Gestionnaire, Comptable
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Gestionnaire;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Permissions dynamiques basées sur le rôle
        /// </summary>
        public List<string> Permissions { get; set; } = new();
    }

    /// <summary>
    /// Énumération des rôles utilisateur
    /// </summary>
    public enum UserRole
    {
        Admin = 0,           // Accès total
        Gestionnaire = 1,    // Camions + suivi
        Comptable = 2        // Finance uniquement
    }
}
