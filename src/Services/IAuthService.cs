using BMBusinessStockManagement.Models;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour le service d'authentification
    /// Gère la connexion, déconnexion, enregistrement et gestion des sessions
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authentifie un utilisateur avec email et mot de passe
        /// </summary>
        Task<AuthResult> LoginAsync(string email, string password);
        
        /// <summary>
        /// Enregistre un nouvel utilisateur (Admin uniquement)
        /// </summary>
        Task<AuthResult> RegisterAsync(User user, string password);
        
        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        Task LogoutAsync();
        
        /// <summary>
        /// Récupère l'utilisateur actuellement connecté
        /// </summary>
        Task<User> GetCurrentUserAsync();
        
        /// <summary>
        /// Change le mot de passe de l'utilisateur
        /// </summary>
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
        
        /// <summary>
        /// Valide le token actuel
        /// </summary>
        Task<bool> ValidateTokenAsync(string token);
        
        /// <summary>
        /// Rafraîchit le token JWT
        /// </summary>
        Task<string> RefreshTokenAsync();
        
        /// <summary>
        /// Obtient le token JWT courant
        /// </summary>
        string GetCurrentToken();
        
        /// <summary>
        /// Indique si un utilisateur est actuellement connecté
        /// </summary>
        bool IsLoggedIn { get; }
    }

    /// <summary>
    /// Résultat d'une opération d'authentification
    /// </summary>
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
