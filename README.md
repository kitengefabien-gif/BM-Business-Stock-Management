# 🏢 BM BUSINESS STOCK MANAGEMENT

## 📱 Application ERP Multiplateforme - Gestion Logistique & Finance

**Une solution professionnelle pour la gestion complète de l'activité de vente de ciment (Supaset, Powerbuild, Duracrete)**

---

## 🎯 OBJECTIF GLOBAL

Application ERP moderne, sécurisée et performante pour :
- 🚛 **Gestion des camions** (suivi en temps réel)
- 📦 **Gestion de stock ciment** (entrée/sortie)
- 💰 **Gestion financière** (revenus, coûts, bénéfices)
- 👥 **Gestion des utilisateurs** (rôles & permissions)
- 🔄 **Synchronisation Offline/Online** (temps réel)

---

## 📋 MODULES DE L'APPLICATION

### 🔐 Module 1 : Authentification
- ✅ Login sécurisé (local + cloud Supabase)
- ✅ Gestion des sessions
- ✅ Changement de mot de passe
- ✅ Splash screen professionnel

### 👤 Module 2 : Gestion des Utilisateurs
- ✅ Rôles : Admin, Gestionnaire, Comptable
- ✅ Création/Attribution de rôles
- ✅ Gestion des permissions
- ✅ Sécurité des sessions

### 🚛 Module 3 : Enregistrement des Camions
- ✅ Formulaire moderne avec validation
- ✅ Auto-complétion intelligente (plaques, téléphones)
- ✅ Support multi-destinations (Lubumbashi, Kolwezi, Ndola, Lusaka)
- ✅ Génération automatique du numéro de voyage
- ✅ Support 3 types de ciment (Supaset, Powerbuild, Duracrete)

### 📞 Format Téléphone Obligatoire
- 🇨🇩 RDC : +243 + 9 chiffres
- 🇿🇲 ZAMBIE : +260 + 9 chiffres
- ✅ Sélection pays avec drapeau
- ✅ Validation automatique

### 📦 Module 4 : Stockage de Données
- ✅ SQLite pour offline
- ✅ Supabase pour cloud sync
- ✅ Synchronisation automatique
- ✅ Mode offline complet

### 📡 Module 5 : Synchronisation Temps Réel
- ✅ Mise à jour instantanée multi-appareils
- ✅ Queue offline automatique
- ✅ Sync dès reconnexion

### 📦 Module 6 : Gestion de Stock Ciment
- ✅ Entrée/sortie de stock
- ✅ Alertes stock faible
- ✅ Historique des mouvements

### 📊 Module 7 : Tableau de Bord
- ✅ Affichage centralisé données
- ✅ Actions (modifier, supprimer)
- ✅ Statuts (arrivé/non arrivé, payé/non payé)

### 🔎 Module 8 : Filtrage Avancé
- ✅ Recherche : chauffeur, plaque, téléphone, voyage
- ✅ Filtre statut camion
- ✅ Filtre statut paiement
- ✅ Filtre temporel (semaine, mois, trimestre, année)

### 💰 Module 9 : Finance Avancée
- ✅ Revenus, coûts, bénéfice auto
- ✅ Formule : Bénéfice = Revenus - Coûts
- ✅ Analyse par destination/mois/année
- ✅ Graphiques (barres, lignes, camembert)

### 🔔 Module 10 : Notifications WhatsApp
- ✅ WhatsApp Business API
- ✅ Messages automatiques
- ✅ Alertes arrivée/paiement

### 🧾 Module 11 : Rapports PDF
- ✅ Hebdomadaire, mensuel, trimestriel, annuel
- ✅ Logo & branding BM BUSINESS

### 🔄 Module 12 : Gestion des Statuts
- ✅ Modification en temps réel
- ✅ Historique complet

### 🧠 Module 13 : Logique Mensuelle
- ✅ Reset dashboard chaque mois
- ✅ Archivage automatique données

---

## 🎨 DESIGN & COULEURS

```
🔵 Bleu primaire      : #1E3A8A
⚪ Blanc             : #FFFFFF
⚫ Gris              : #6B7280
🟠 Orange ciment     : #FF8C00
```

---

## 🏗️ ARCHITECTURE

```
BM-Business-Stock-Management/
├── src/
│   ├── Models/               # Entités métier
│   ├── Views/                # Pages XAML
│   ├── ViewModels/           # MVVM Logic
│   ├── Services/             # Logique métier
│   │   ├── Auth/
│   │   ├── Truck/
│   │   ├── Stock/
│   │   ├── Finance/
│   │   ├── Sync/
│   │   └── Notification/
│   ├── Data/
│   │   ├── SQLite/
│   │   └── Cloud/
│   ├── Resources/            # Styles, Couleurs
│   ├── Converters/
│   ├── Behaviors/
│   └── Utils/
├── Tests/
├── Assets/                   # Images, Logos
└── MauiProgram.cs
```

---

## 💻 TECHNOLOGIES

- **Framework** : .NET MAUI 8+
- **Langage** : C# 12+
- **Pattern** : MVVM (Model-View-ViewModel)
- **Base de données** : SQLite (local) + Supabase (cloud)
- **UI** : XAML avec animations fluides
- **API** : Supabase REST API + Real-time
- **Notifications** : WhatsApp Business API
- **Rapports** : PDF generation

---

## 📱 PLATEFORMES SUPPORTÉES

- ✅ **Android** (API 21+)
- ✅ **Windows** (10/11)
- ✅ **macOS** (11+)
- ✅ **iOS** (15+) - support optionnel

---

## 🚀 DÉMARRAGE RAPIDE

### Prérequis
- .NET 8 SDK ou supérieur
- Visual Studio 2022 / VS Code
- Compte Supabase

### Installation

```bash
# Cloner le repo
git clone https://github.com/kitengefabien-gif/BM-Business-Stock-Management.git
cd BM-Business-Stock-Management

# Restaurer les packages
dotnet restore

# Configurer Supabase
# Éditer appsettings.json avec vos credentials

# Exécuter
dotnet maui run
```

---

## 📊 STRUCTURE DE DONNÉES

### Tables Principales
- **users** - Utilisateurs & authentification
- **trucks** - Camions & chauffeurs
- **cement_stock** - Mouvements de stock
- **trips** - Voyages & destinations
- **payments** - Transactions financières
- **reports** - Données archivées

---

## 🔒 SÉCURITÉ

- ✅ Authentification Supabase JWT
- ✅ Row Level Security (RLS) PostgreSQL
- ✅ Chiffrement données sensibles
- ✅ Validation côté client & serveur
- ✅ Gestion des permissions par rôle

---

## 📞 SUPPORT MULTI-PAYS

- 🇨🇩 **RDC** (Kasumbalesa, Lubumbashi, Kolwezi)
- 🇿🇲 **ZAMBIE** (Ndola, Lusaka)
- 📞 Format téléphone adapté par pays
- 💱 Support devise locale

---

## 👨‍💼 GESTION DES RÔLES

### Admin
- Accès complet à tous les modules
- Gestion des utilisateurs
- Configuration système

### Gestionnaire
- Enregistrement camions
- Suivi en temps réel
- Gestion stock

### Comptable
- Accès finance uniquement
- Rapports & analyses
- Paiements

---

## 📈 ROADMAP

- [x] Architecture MVVM
- [x] Models & Services
- [ ] Pages d'authentification
- [ ] Dashboard principal
- [ ] Formulaire camions
- [ ] Gestion stock
- [ ] Finance & rapports
- [ ] Synchronisation temps réel
- [ ] WhatsApp integration
- [ ] Tests unitaires

---

## 📝 LICENSE

MIT License - Voir LICENSE.md

---

## 👤 AUTEUR

**Fabien Kitenge** - BM BUSINESS STOCK MANAGEMENT

---

## 📧 CONTACT

Pour toute question ou suggestion, ouvrir une issue.

---

**Made with ❤️ in DRC - Powering Cement Business Excellence**
