# BM Business Stock Management

> Système de gestion intégré pour le flux de ciment entre la RDC et la Zambie

## À propos du projet

**BM Business Stock Management** est une application mobile et web moderne conçue pour gérer efficacement :

- Suivi des camions - Localisation GPS, statut d'arrivée, paiements
- Gestion du stock - Quantités, types de ciment, alertes de stock bas
- Finances - Transactions, rapports mensuels, analyses par destination
- Notifications WhatsApp - Alertes en temps réel via WhatsApp
- Mode Offline - Fonctionnement sans internet avec synchronisation automatique
- Temps réel - Synchronisation instantanée entre appareils

### Destinations couvertes
- RDC : Lubumbashi, Kolwezi
- Zambie : Ndola, Lusaka

### Types de ciment
- Supaset
- Powerbuild
- Duracrete

---

## Architecture technique

### Stack technologique

- Frontend : .NET MAUI (iOS, Android, Windows, macOS)
- Backend : Supabase (PostgreSQL + Realtime)
- Base de données locale : SQLite
- Pattern : MVVM avec C# et XAML
- Authentification : JWT (Supabase Auth)
- Intégrations : WhatsApp API, Google Maps, Stripe/MTN

### Structure du projet

```
BM-Business-Stock-Management/
├── src/
│   ├── Models/                 # Entités métier
│   ├── Services/               # Logique métier
│   ├── ViewModels/             # MVVM ViewModels
│   ├── Views/                  # Pages XAML
│   ├── App.xaml(.cs)          # Point d'entrée
│   ├── AppShell.xaml(.cs)     # Navigation
│   ├── MauiProgram.cs         # Configuration DI
│   └── Resources/             # Assets
├── appsettings.json           # Configuration
└── README.md
```

---

## Installation & Démarrage

### Prérequis

- .NET 8.0+
- Visual Studio 2022 ou VS Code
- Supabase Project
- Android SDK / Xcode

### 1. Cloner le projet

```bash
git clone https://github.com/kitengefabien-gif/BM-Business-Stock-Management.git
cd BM-Business-Stock-Management
```

### 2. Configurer Supabase

```json
// appsettings.json
{
  "Supabase": {
    "Url": "https://votre-project.supabase.co",
    "AnonKey": "votre-anon-key"
  }
}
```

### 3. Restaurer les dépendances

```bash
dotnet restore
```

### 4. Lancer l'application

```bash
# Windows
dotnet maui run -f net8.0-windows

# Android
dotnet maui run -f net8.0-android

# iOS
dotnet maui run -f net8.0-ios
```

---

## Utilisation

### Authentification

1. Entrez votre email et mot de passe
2. L'application stocke votre JWT localement
3. Auto-refresh du token toutes les heures

**Rôles disponibles :**
- Admin - Accès complet
- Gestionnaire - Camions + Suivi
- Comptable - Finance uniquement

### Enregistrement d'un voyage

1. Dashboard -> + Ajouter un voyage
2. Remplissez les champs :
   - Nom du chauffeur (auto-majuscules)
   - Plaque du véhicule (auto-complétion)
   - Téléphone (validé: +243/+260)
   - Destination
   - Type de ciment
   - Quantité & Montant

3. Enregistrer -> Sync automatique

### Gestion du stock

- Voir le stock : Stock -> Vue d'ensemble
- Ajouter : Stock -> + Entrée
- Retirer : Stock -> - Sortie
- Alertes : Stock bas -> Auto-notification WhatsApp

### Rapports financiers

- Revenus du jour / du mois
- Coûts par catégorie
- Profit & Marge
- Graphiques par destination
- Export PDF

---

## Mode Offline

L'application fonctionne sans internet :

Disponible offline :
- Voir tous les camions
- Ajouter/Modifier des voyages
- Consulter le stock
- Voir l'historique

Automatiquement synced :
- Retour de connexion -> Sync auto
- Files d'attente en FIFO
- Gestion des conflits de versions

---

## Sécurité

- JWT Authentication (Supabase Auth)
- Stockage sécurisé du token
- HTTPS/TLS obligatoire
- Chiffrement des données sensibles
- Validation côté serveur
- Rate limiting sur API

---

## Support

- Email : kitengefabien@gmail.com
- GitHub Issues : https://github.com/kitengefabien-gif/BM-Business-Stock-Management/issues
- GitHub Discussions : https://github.com/kitengefabien-gif/BM-Business-Stock-Management/discussions

---

## Licence

Ce projet est sous licence MIT - voir LICENSE pour plus de détails.

---

## Roadmap

- [x] Structure MVVM
- [x] Authentication Supabase
- [x] Gestion camions offline
- [ ] Intégration WhatsApp API
- [ ] GPS Tracking (Google Maps)
- [ ] Export PDF/Excel
- [ ] Dashboard analytics avancé
- [ ] Support multi-langues
- [ ] App web (Blazor)

---

Made with love in Congo for Africa
