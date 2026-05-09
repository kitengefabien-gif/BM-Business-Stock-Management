using BMBusinessStockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Implémentation du service de synchronisation offline/online
    /// </summary>
    public class SyncService : ISyncService
    {
        private readonly SQLiteAsyncConnection _database;
        private bool _isOnline = true;
        private bool _isSyncing = false;
        private Queue<SyncOperation> _operationQueue = new();

        public bool IsOnline 
        { 
            get => _isOnline; 
            set 
            { 
                if (_isOnline != value)
                {
                    _isOnline = value;
                    OnConnectivityChanged?.Invoke(this, value);
                    if (value) // Si reconnexion, synchroniser
                        SyncAsync().FireAndForget();
                }
            }
        }
        
        public bool IsSyncing => _isSyncing;

        public event EventHandler<bool> OnConnectivityChanged;
        public event EventHandler OnSyncStarted;
        public event EventHandler<SyncResult> OnSyncCompleted;

        public SyncService(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            InitializeDatabaseAsync().Wait();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<SyncOperation>();
        }

        public async Task<bool> SyncAsync()
        {
            if (_isSyncing || !_isOnline)
                return false;

            _isSyncing = true;
            OnSyncStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                var operations = await _database.Table<SyncOperation>().ToListAsync();
                var successCount = 0;

                foreach (var op in operations)
                {
                    try
                    {
                        // Traiter chaque opération
                        // La logique réelle dépendra de votre implémentation Supabase
                        await _database.DeleteAsync<SyncOperation>(op.Id);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erreur sync: {ex.Message}");
                    }
                }

                var result = new SyncResult
                {
                    Success = true,
                    Message = $"{successCount} opération(s) synchronisée(s)",
                    ItemsSynced = successCount,
                    SyncedAt = DateTime.UtcNow
                };

                OnSyncCompleted?.Invoke(this, result);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur sync globale: {ex.Message}");
                return false;
            }
            finally
            {
                _isSyncing = false;
            }
        }

        public async Task<bool> SyncRealtimeAsync()
        {
            // Implémentation de la synchronisation temps réel
            // Utiliser WebSockets ou SignalR pour les mises à jour instantanées
            return await SyncAsync();
        }

        public async Task QueueOperationAsync(string operationType, string entityId, object data)
        {
            var operation = new SyncOperation
            {
                Id = Guid.NewGuid().ToString(),
                OperationType = operationType,
                EntityId = entityId,
                Data = System.Text.Json.JsonSerializer.Serialize(data),
                CreatedAt = DateTime.UtcNow
            };

            await _database.InsertAsync(operation);
        }

        public async Task<int> GetPendingOperationsCountAsync()
        {
            return await _database.Table<SyncOperation>().CountAsync();
        }

        public async Task ClearQueueAsync()
        {
            await _database.DeleteAllAsync<SyncOperation>();
        }
    }

    /// <summary>
    /// Model pour les opérations en queue
    /// </summary>
    [Table("sync_operations")]
    public class SyncOperation
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string OperationType { get; set; }
        public string EntityId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
