# IMIRank - Platforma za Igre i Recenzije

## Sadržaj

- [Opis Aplikacije](#opis-aplikacije)
- [Tehnologije](#tehnologije)
- [Arhitektura Aplikacije](#arhitektura-aplikacije)
- [Struktura Projekta](#struktura-projekta)
- [Preduslovi](#preduslovi)
- [Postavljanje Aplikacije](#postavljanje-aplikacije)
- [Pokretanje Aplikacije](#pokretanje-aplikacije)
- [APIEndpointi](#api-endpointi)
- [Korisničke Uloge](#korisničke-uloge)
- [Kontakt i Podrška](#kontakt-i-podrška)

---

## Opis Aplikacije

**IMIRank** je web platforma koja omogućava:

- **Pregledavanje igara** - Pretraživanje i otkrivanje novih igara
- **Pisanje recenzija** - Deljenje mišljenja o igrama sa ocenom od 1-10
- **Diskusije** - Komentarisanje recenzija i razgovori sa ostalim igračima
- **Praćenje korisnika** - Pratite omiljene recenzente
- **Statistika** - Pregled adresa igara, prosečnih ocena i trendova
- **Administracija** - Dodavanje novih igara, upravljanje korisnicima i pregledom predloga

### Ključne Karakteristike:

[DA] Autentifikacija i autorizacija korisnika  
[DA] MongoDB baza podataka za čuvanje podataka  
[DA] Real-time notifikacije putem SignalR  
[DA] Upload slika za profile i cover gamea  
[DA] Paginacija i pretrage sa filtriranjem  
[DA] Sistem za praćenje korisnika (Follow/Unfollow)  
[DA] Komentari na recenzije u real-time

---

## Tehnologije

### Backend

| Teknologija      | Verzija  | Opis                   |
| ---------------- | -------- | ---------------------- |
| **C#**           | .NET 8.0 | Jezyk programiranja    |
| **ASP.NET Core** | 8.0      | Web framework          |
| **MongoDB**      | Latest   | NoSQL baza podataka    |
| **SignalR**      | 8.0      | Real-time komunikacija |
| **BCrypt**       | -        | Enkripcija lozinki     |
| **JWT**          | -        | Authentication tokeni  |

### Frontend

| Tehnologija         | Verzija | Opis                 |
| ------------------- | ------- | -------------------- |
| **Angular**         | 18+     | JavaScript framework |
| **TypeScript**      | Latest  | Jezyk programiranja  |
| **Material Design** | Latest  | UI komponente        |
| **RxJS**            | Latest  | Reactive programming |
| **CSS 3**           | -       | Stilizacija          |

### Infrastruktura

- **VS Code / Visual Studio** - Razvojno okruženje
- **Git** - Verzionisanje koda

---

## Arhitektura Aplikacije

```
┌─────────────────────────────────────────────────────────────┐
│                    FRONTEND (Angular)                       │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Presentation Layer                                    │ │
│  │  - Components (profile, review, game, home, etc)      │ │
│  │  - Material Design UI                                  │ │
│  │  - Forms & Modals                                      │ │
│  └────────────────────────────────────────────────────────┘ │
│                            |                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Service Layer                                         │ │
│  │  - Api Service                                         │ │
│  │  - Auth Service                                        │ │
│  │  - Notification Service                                │ │
│  │  - SignalR Services (Real-time)                       │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
              HTTP/WebSocket
┌─────────────────────────────────────────────────────────────┐
│                     BACKEND (C# .NET)                       │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  API Layer (Controllers)                               │ │
│  │  - GameController, ReviewController                   │ │
│  │  - UserController, ProfileController                  │ │
│  │  - AdminController, FollowController                  │ │
│  └────────────────────────────────────────────────────────┘ │
│                            |                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Business Logic Layer (Services)                       │ │
│  │  - AdminService, UserService                           │ │
│  │  - FollowService, HomeService                          │ │
│  └────────────────────────────────────────────────────────┘ │
│                            |                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Data Access Layer (Repositories)                      │ │
│  │  - IGameRepository, IReviewRepository                  │ │
│  │  - IUserRepository, IFollowRepository                  │ │
│  └────────────────────────────────────────────────────────┘ │
│                            |                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Model Layer                                           │ │
│  │  - User, Game, Review, Follow                          │ │
│  │  - GameSuggestion, Notification                        │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                       |
         ┌─────────────┴──────────────┐
         |                            |
    ┌────────────┐          ┌─────────────────┐
    │  MongoDB   │          │  Image Storage  │
    │  Database  │          │  (wwwroot)      │
    └────────────┘          └─────────────────┘
```

---

## Struktura Projekta

```
WEB2 PROJEKTNI/
├── Back/                           # Backend aplikacija
│   ├── Back/
│   │   ├── Controllers/            # API kontroleri
│   │   │   ├── GameController.cs
│   │   │   ├── ReviewController.cs
│   │   │   ├── UserController.cs
│   │   │   ├── ProfileController.cs
│   │   │   ├── AdminController.cs
│   │   │   └── ...
│   │   ├── Services/               # Business logika
│   │   │   ├── Admin/
│   │   │   ├── User/
│   │   │   ├── Home/
│   │   │   └── ...
│   │   ├── Repositories/           # Data access
│   │   │   ├── Game/
│   │   │   ├── Review/
│   │   │   ├── User/
│   │   │   └── ...
│   │   ├── Models/                 # Entity modeli
│   │   │   ├── Game/
│   │   │   ├── Review/
│   │   │   ├── User/
│   │   │   └── ...
│   │   ├── DTO/                    # Data transfer objekti
│   │   ├── Config/                 # Konfiguracija
│   │   ├── Helpers/                # Pomoćne klase
│   │   ├── Hubs/                   # SignalR hubs
│   │   ├── wwwroot/                # Javne datoteke (slike)
│   │   └── Program.cs              # Ulazna tačka
│   ├── Back.Tests/                 # Unit testovi
│   └── Back.sln                    # Solution fajl
│
├── Front/                          # Frontend aplikacija
│   ├── src/
│   │   ├── app/
│   │   │   ├── home/               # Home komponenta
│   │   │   ├── profile/            # Profil korisnika
│   │   │   ├── game-reviews/       # Recenzije igara
│   │   │   ├── review-detail/      # Detalj recenzije
│   │   │   ├── review-comments/    # Komentari na recenzije
│   │   │   ├── login/              # Login stranica
│   │   │   ├── register/           # Registracija
│   │   │   ├── admin/              # Admin panel
│   │   │   ├── notifications/      # Notifikacije
│   │   │   ├── services/           # Angular servisi
│   │   │   │   ├── api.ts
│   │   │   │   ├── auth.service.ts
│   │   │   │   └── ...
│   │   │   ├── dto/                # DTO interfejsi
│   │   │   ├── guards/             # Route guardovi
│   │   │   └── shared/             # Deljene komponente
│   │   ├── styles.css              # Globalni stilovi
│   │   └── main.ts                 # Ulazna tačka
│   ├── package.json                # NPM zavisnosti
│   ├── angular.json                # Angular konfiguracija
│   └── tsconfig.json               # TypeScript konfiguracija
│
└── README.md                       # Ovaj fajl
```

---

## Preduslovi

Pre nego što počnete sa postavljanjem, uverite se da imate instaliran:

### Za Backend:

- [DA] [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [DA] [MongoDB Community Edition](https://www.mongodb.com/try/download/community)
- [DA] Visual Studio 2022 ili VS Code sa C# razvojem
- [DA] Git

### Za Frontend:

- [DA] [Node.js](https://nodejs.org/) (v18+)
- [DA] npm ili yarn
- [DA] VS Code ili bilo koji tekst editor
- [DA] Git

### Proveranje Instalacije:

```bash
# Proverite .NET verziju
dotnet --version

# Proverite Node verziju
node --version

# Proverite npm verziju
npm --version
```

---

## Postavljanje Aplikacije

### Korak 1: Kloniranje Projekta

```bash
git clone https://github.com/vase-username/imirank.git
cd imirank
```

### Korak 2: Postavljanje MongoDB-a

**Windows (sa instalatorom):**

1. Preuzmi [MongoDB Community Edition](https://www.mongodb.com/try/download/community)
2. Pokreni instalator
3. Prihvati podrazumevane postavke
4. MongoDB će se pokrenuti kao Windows servis

**Proverite da je MongoDB pokrenut:**

```bash
mongosh
# Trebalo bi da vidite MongoDB prompt
```

### Korak 3: Postavljanje Backend-a

1. **Otvorite Backend folder:**

```bash
cd Back
```

2. **Otvorite `appsettings.json` i konfigurujte bazu:**

```json
{
  "MongoDBSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "IMIRank"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "IMIRank",
    "Audience": "IMIRankUsers"
  }
}
```

3. **Instalacija zavisnosti i build:**

```bash
# Vrati NuGet pakete
dotnet restore

# Kompajliraj projekt
dotnet build
```

### Korak 4: Postavljanje Frontend-a

1. **Otvorite Frontend folder:**

```bash
cd Front
```

2. **Instalacija NPM zavisnosti:**

```bash
npm install
```

3. **Konfiguracija API URL-a:**
   Otvorite `src/app/services/api.ts` i uverite se da je backend URL:

```typescript
private url : string = 'http://localhost:5062/api/'
```

---

## Pokretanje Aplikacije

### Pokretanje Backend-a

```bash
cd Back/Back

# Opcija 1: Sa dotnet CLI
dotnet run

# Opcija 2: Sa Visual Studio
# - Desni klik na projekt → Set as Startup Project
# - Pritisnite F5 ili Debug → Start Debugging
```

Backend će biti dostupan na: **http://localhost:5062**

### Pokretanje Frontend-a

```bash
cd Front

# Development server
ng serve

# Ili
npm start
```

Frontend će biti dostupan na: **http://localhost:4200**

### Kompletno okruženje

**Terminal 1 - Backend:**

```bash
cd Back/Back
dotnet run
```

**Terminal 2 - Frontend:**

```bash
cd Front
ng serve
```

**Terminal 3 - MongoDB (ako nije kao servis):**

```bash
mongod
```

---

## API Endpointi

### Autentifikacija

```
POST   /api/auth/register          - Registracija korisnika
POST   /api/auth/login             - Login korisnika
POST   /api/auth/forgot-password   - Zaboravljena lozinka
```

### Igre

```
GET    /api/game/{id}              - Dohvati igru po ID-u
GET    /api/game?query=...         - Pretraga igara
```

### Recenzije

```
GET    /api/review                 - Sve recenzije sa filtriranjem
GET    /api/review/{id}            - Detalj recenzije
GET    /api/review/byGame/{gameId} - Recenzije za igru
GET    /api/review/byUser/{userId} - Recenzije korisnika
POST   /api/review                 - Kreiraj recenziju
```

### Korisnici

```
GET    /api/user                   - Pretraga korisnika
GET    /api/user/{id}              - Profil korisnika
GET    /api/profile                - Moj profil
PUT    /api/profile/username       - Promeni korisničko ime
PUT    /api/profile/password       - Promeni lozinku
PUT    /api/profile/picture        - Promeni sliku profila
```

### Praćenje

```
POST   /api/follow/follow/{userId}     - Zaprati korisnika
POST   /api/follow/unfollow/{userId}   - Otprati korisnika
GET    /api/follow/status/{userId}     - Status praćenja
```

### Admin

```
GET    /api/admin/stats            - Statistika
POST   /api/admin/games            - Dodaj igru
```

---

## Korisničke Uloge

Aplikacija ima tri tipa korisnika:

### 1. **Obični Korisnik (RegularUser)**

- [NE] Pisanje recenzija
- [DA] Komentarisanje recenzija
- [DA] Praćenje drugih korisnika
- [DA] Pregled igara i recenzija
- [NE] Dodavanje igara
- [NE] Pristup admin panelu

### 2. **Editor (Editor)**

- [DA] Sve što obični korisnik
- [DA] Dodavanje novih igara
- [DA] Uređivanje recenzija
- [NE] Pristup admin panelu

### 3. **Administrator (Admin)**

- [DA] Pristup admin panelu
- [DA] Pregled statistike
- [DA] Pregled predloga igara
- [DA] Dodavanje novih igara

---

## Baza Podataka - MongoDB Kolekcije

```
IMIRank (baza)
├── User              - Korisnici (id, username, email, role, profilePicture)
├── Game              - Igre (id, title, genre, developer, coverImage)
├── Review            - Recenzije (id, gameId, userId, rating, title, content)
├── ReviewComment     - Komentari (id, reviewId, userId, content)
├── Follow            - Praćenja (id, followerId, followingId)
├── Notification      - Notifikacije (id, userId, type, actor)
├── GameSuggestion    - Predlozi igara (id, title, suggestedBy, status)
└── Index             - Indeksi za brže pretrage
```

---

## Rešavanje Čestih Problema

### Problem: "Cannot connect to MongoDB"

**Rešenje:**

```bash
# Proverite da li je MongoDB pokrenut
mongosh

# Ako nije, pokrenite ga
mongod
```

### Problem: "Port 5062 je već u upotrebi"

**Rešenje:**
Promenite port u `launchSettings.json`:

```json
"applicationUrl": "http://localhost:5063"
```

### Problem: "404 Not Found" na frontu

**Rešenje:**
Uverite se da je backend pokrenut na `http://localhost:5062`

### Problem: "CORS error"

**Rešenje:**
Backend ima konfigurisan CORS. Uverite se da je frontend na `http://localhost:4200`

---

## Sljedeće Korake - Deployment

Za production okruženje:

1. **Build Frontend:**

```bash
cd Front
ng build --configuration production
```

2. **Publish Backend:**

```bash
cd Back/Back
dotnet publish -c Release -o ./publish
```

3. **Deploy na:**
   - Azure App Service
   - AWS EC2
   - DigitalOcean
   - Local server

---
