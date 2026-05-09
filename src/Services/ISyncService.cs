using System;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Interface pour la synchronisation offline/online et temps réel
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        /// Indique si l'application est connectée à internet
        /// </summary>
        bool IsOnline { get; }
        
        /// <summary>
        /// Indique si la synchronisation est en cours
        /// </summary>
        bool IsSyncing { get; }
        
        /// <summary>
        /// Synchronise les données locales avec le cloud
        /// </summary>
        Task<bool> SyncAsync();
        
        /// <summary>
        /// Synchronise les données en temps réel
        /// </summary>
        Task<bool> SyncRealtimeAsync();
        
        /// <summary>
        /// Ajoute une opération à la queue d'attente (pour offline)
        /// </summary>
        Task QueueOperationAsync(string operationType, string entityId, object data);
        
        /// <summary>
        /// Récupère les opérations en attente
        /// </summary>
        Task<int> GetPendingOperationsCountAsync();
        
        /// <summary>
        /// Efface la queue d'attente
        /// </summary>
        Task ClearQueueAsync();
        
        /// <summary>
        /// Événement déclenché quand la connexion change
        /// </summary>
        event EventHandler<bool> OnConnectivityChanged;
        
        /// <summary>
        /// Événement déclenché quand la synchronisation commence
        /// </summary>
        event EventHandler OnSyncStarted;
        
        /// <summary>
        /// Événement déclenché quand la synchronisation se termine
        /// </summary>
        event EventHandler<SyncResult> OnSyncCompleted;
    }

    /// <summary>
    /// Résultat de la synchronisation
    /// </summary>
    public class SyncResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ItemsSynced { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}
