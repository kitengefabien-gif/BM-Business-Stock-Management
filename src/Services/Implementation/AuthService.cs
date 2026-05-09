using BMBusinessStockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Implémentation du service d'authentification avec Supabase
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly Client _supabaseClient;
        private User _currentUser;
        private string _currentToken;

        public bool IsLoggedIn => _currentUser != null && !string.IsNullOrEmpty(_currentToken);

        public AuthService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _supabaseClient.Auth.SignIn(email, password);
                
                if (response?.User == null)
                    return new AuthResult 
                    { 
                        Success = false, 
                        Message = "Identifiants invalides" 
                    };

                _currentUser = MapAuthUserToUser(response.User);
                _currentToken = response.Session?.AccessToken;

                return new AuthResult
                {
                    Success = true,
                    Message = "Connexion réussie",
                    User = _currentUser,
                    Token = _currentToken,
                    RefreshToken = response.Session?.RefreshToken
                };
            }
            catch (Exception ex)
            {
                return new AuthResult 
                { 
                    Success = false, 
                    Message = $"Erreur: {ex.Message}" 
                };
            }
        }

        public async Task<AuthResult> RegisterAsync(User user, string password)
        {
            try
            {
                var response = await _supabaseClient.Auth.SignUp(user.Email, password);
                
                if (response?.User == null)
                    return new AuthResult 
                    { 
                        Success = false, 
                        Message = "Erreur lors de l'enregistrement" 
                    };

                // Stocker l'utilisateur dans la table users de Supabase
                await _supabaseClient
                    .From("users")
                    .Insert(new List<User> { user });

                return new AuthResult
                {
                    Success = true,
                    Message = "Utilisateur enregistré avec succès",
                    User = user
                };
            }
            catch (Exception ex)
            {
                return new AuthResult 
                { 
                    Success = false, 
                    Message = $"Erreur: {ex.Message}" 
                };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();
                _currentUser = null;
                _currentToken = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur de déconnexion: {ex.Message}");
            }
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return _currentUser ?? await Task.FromResult<User>(null);
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                if (_currentUser == null)
                    return false;

                await _supabaseClient.Auth.Update(new UserAttributes { Password = newPassword });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var user = await _supabaseClient.Auth.GetUser(token);
                return user != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> RefreshTokenAsync()
        {
            try
            {
                // Implémentation basée sur la session Supabase
                var session = _supabaseClient.Auth.CurrentSession;
                if (session != null)
                {
                    var newSession = await _supabaseClient.Auth.RefreshSession();
                    _currentToken = newSession?.AccessToken;
                    return _currentToken;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public string GetCurrentToken()
        {
            return _currentToken;
        }

        private User MapAuthUserToUser(Supabase.Gotrue.User authUser)
        {
            return new User
            {
                Id = authUser.Id,
                Email = authUser.Email,
                FullName = authUser.UserMetadata?["full_name"]?.ToString() ?? "",
                PhoneNumber = authUser.Phone ?? "",
                LastLogin = DateTime.UtcNow
            };
        }
    }
}
