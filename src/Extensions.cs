using System;

namespace BMBusinessStockManagement
{
    /// <summary>
    /// Extension methods pour les opérations async
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Exécute une tâche sans attendre (fire and forget)
        /// </summary>
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    System.Diagnostics.Debug.WriteLine($"FireAndForget Error: {t.Exception?.InnerException?.Message}");
                }
            });
        }

        /// <summary>
        /// Exécute une tâche générique sans attendre
        /// </summary>
        public static void FireAndForget<T>(this Task<T> task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    System.Diagnostics.Debug.WriteLine($"FireAndForget Error: {t.Exception?.InnerException?.Message}");
                }
            });
        }
    }

    /// <summary>
    /// Extension methods pour les strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convertit un string en majuscules
        /// </summary>
        public static string ToUpperCase(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.ToUpper();
        }

        /// <summary>
        /// Valide un format de téléphone
        /// </summary>
        public static bool IsValidPhoneNumber(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            
            var cleanPhone = System.Text.RegularExpressions.Regex.Replace(phone, @"[^0-9+]", "");
            return cleanPhone.Length >= 10 && (cleanPhone.StartsWith("+243") || cleanPhone.StartsWith("+260"));
        }

        /// <summary>
        /// Valide une plaque minéralogique
        /// </summary>
        public static bool IsValidLicensePlate(this string plate)
        {
            return !string.IsNullOrEmpty(plate) && plate.Length >= 3 && plate.Length <= 20;
        }
    }

    /// <summary>
    /// Extension methods pour les collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Distingue les éléments d'une liste
        /// </summary>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer)
        {
            return source.Distinct(new DynamicEqualityComparer<T>(comparer));
        }

        private class DynamicEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _comparer;

            public DynamicEqualityComparer(Func<T, T, bool> comparer)
            {
                _comparer = comparer;
            }

            public bool Equals(T x, T y) => _comparer(x, y);
            public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// Extension methods pour les décimaux (monnaie)
    /// </summary>
    public static class MoneyExtensions
    {
        /// <summary>
        /// Formate une décimale en devise
        /// </summary>
        public static string ToMoneyCurrency(this decimal amount, string currencyCode = "ZMW")
        {
            var symbol = currencyCode switch
            {
                "ZMW" => "ZK",
                "CDF" => "FC",
                _ => currencyCode
            };
            return $"{symbol} {amount:N2}";
        }

        /// <summary>
        /// Calcule la marge de profit
        /// </summary>
        public static decimal CalculateMargin(this decimal revenue, decimal cost)
        {
            if (revenue == 0)
                return 0;
            return ((revenue - cost) / revenue) * 100;
        }
    }
}
