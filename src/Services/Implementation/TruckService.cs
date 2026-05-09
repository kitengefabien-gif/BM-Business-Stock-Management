using BMBusinessStockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace BMBusinessStockManagement.Services
{
    /// <summary>
    /// Implémentation du service de gestion des camions
    /// Gère les opérations CRUD et l'auto-complétion
    /// </summary>
    public class TruckService : ITruckService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly ISyncService _syncService;

        public TruckService(string databasePath, ISyncService syncService)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _syncService = syncService;
            InitializeDatabaseAsync().Wait();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<Truck>();
        }

        public async Task<Truck> CreateTruckAsync(Truck truck)
        {
            // Convertir le nom du chauffeur en MAJUSCULES
            truck.DriverName = truck.DriverName?.ToUpper();
            
            // Générer le numéro de voyage s'il n'existe pas
            if (string.IsNullOrEmpty(truck.VoyageNumber))
                truck.VoyageNumber = await GenerateVoyageNumberAsync();

            await _database.InsertAsync(truck);
            
            // Synchroniser avec le cloud
            await _syncService.QueueOperationAsync("CREATE", truck.Id, truck);
            
            return truck;
        }

        public async Task<List<Truck>> GetAllTrucksAsync()
        {
            return await _database.Table<Truck>()
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Truck> GetTruckByIdAsync(string truckId)
        {
            return await _database.Table<Truck>()
                .FirstOrDefaultAsync(t => t.Id == truckId && !t.IsDeleted);
        }

        public async Task<List<string>> GetLicensePlateHistoryAsync(string searchTerm = "")
        {
            var plates = await _database.Table<Truck>()
                .Where(t => !t.IsDeleted && t.LicensePlate != null)
                .Distinct()
                .ToListAsync();

            return plates
                .Select(t => t.LicensePlate)
                .Where(p => p.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        public async Task<List<string>> GetPhoneNumberHistoryAsync(string searchTerm = "")
        {
            var phones = await _database.Table<Truck>()
                .Where(t => !t.IsDeleted && t.PhoneNumber != null)
                .Distinct()
                .ToListAsync();

            return phones
                .Select(t => t.PhoneNumber)
                .Where(p => p.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        public async Task<List<string>> GetDriverNameHistoryAsync(string searchTerm = "")
        {
            var names = await _database.Table<Truck>()
                .Where(t => !t.IsDeleted && t.DriverName != null)
                .Distinct()
                .ToListAsync();

            return names
                .Select(t => t.DriverName)
                .Where(n => n.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        public async Task<Truck> UpdateTruckAsync(Truck truck)
        {
            truck.DriverName = truck.DriverName?.ToUpper();
            truck.UpdatedAt = DateTime.UtcNow;
            
            await _database.UpdateAsync(truck);
            await _syncService.QueueOperationAsync("UPDATE", truck.Id, truck);
            
            return truck;
        }

        public async Task<bool> DeleteTruckAsync(string truckId)
        {
            var truck = await GetTruckByIdAsync(truckId);
            if (truck == null) return false;

            truck.IsDeleted = true;
            truck.UpdatedAt = DateTime.UtcNow;
            
            await _database.UpdateAsync(truck);
            await _syncService.QueueOperationAsync("DELETE", truckId, truck);
            
            return true;
        }

        public async Task<bool> UpdateArrivalStatusAsync(string truckId, ArrivalStatus status)
        {
            var truck = await GetTruckByIdAsync(truckId);
            if (truck == null) return false;

            truck.ArrivalStatus = status;
            truck.ArrivedAt = status == ArrivalStatus.Arrive ? DateTime.UtcNow : null;
            truck.UpdatedAt = DateTime.UtcNow;
            
            await _database.UpdateAsync(truck);
            await _syncService.QueueOperationAsync("UPDATE_STATUS", truckId, truck);
            
            return true;
        }

        public async Task<bool> UpdatePaymentStatusAsync(string truckId, PaymentStatus status)
        {
            var truck = await GetTruckByIdAsync(truckId);
            if (truck == null) return false;

            truck.PaymentStatus = status;
            truck.UpdatedAt = DateTime.UtcNow;
            
            await _database.UpdateAsync(truck);
            await _syncService.QueueOperationAsync("UPDATE_STATUS", truckId, truck);
            
            return true;
        }

        public async Task<string> GenerateVoyageNumberAsync()
        {
            // Format : YYYYMMDD-XXXX (ex: 20260509-0001)
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var count = await _database.Table<Truck>()
                .Where(t => !t.IsDeleted && t.CreatedAt.Date == DateTime.UtcNow.Date)
                .CountAsync();
            
            return $"{today}-{(count + 1):D4}";
        }

        public async Task<List<Truck>> GetTrucksByStatusAsync(ArrivalStatus? arrival = null, PaymentStatus? payment = null)
        {
            var query = _database.Table<Truck>().Where(t => !t.IsDeleted);

            if (arrival.HasValue)
                query = query.Where(t => t.ArrivalStatus == arrival.Value);

            if (payment.HasValue)
                query = query.Where(t => t.PaymentStatus == payment.Value);

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate, string excludeTruckId = null)
        {
            var query = _database.Table<Truck>()
                .Where(t => !t.IsDeleted && t.LicensePlate == licensePlate);

            if (!string.IsNullOrEmpty(excludeTruckId))
                query = query.Where(t => t.Id != excludeTruckId);

            return await query.CountAsync() > 0;
        }
    }
}
