# 📝 FUNKCIONANOST KOMENTARA NA RECENZIJAMA

## ✅ KOMPLETAN FLOW - BACKEND + FRONTEND + REAL-TIME (SignalR)

---

## 🔧 BACKEND IMPLEMENTACIJA

### 1️⃣ **Model** - `ReviewComment.cs`

- `Id` - Jedinstveni ID komentara
- `ReviewId` - ID recenzije na koju se odnosi komentar
- `UserId` - ID korisnika koji je ostavio komentar
- `UserName` - Ime korisnika
- `Content` - Tekst komentara
- `CreatedAt` - Vrijeme kreiranja
- `UpdatedAt` - Vrijeme zadnje izmjene

**Lokacija**: `Back/Models/Review/ReviewComment.cs`

---

### 2️⃣ **Repository Layer**

**Fajlovi**:

- `IReviewCommentRepository.cs` - Interface sa metodama
- `ReviewCommentRepository.cs` - Implementacija sa MongoDB

**Metode**:

- `CreateAsync()` - Kreira novi komentar
- `GetByIdAsync()` - Dobija komentar po ID-u
- `GetByReviewIdAsync()` - Dobija sve komentare za recenziju (sa paginacijom)
- `GetCommentCountByReviewIdAsync()` - Broji sve komentare za recenziju
- `UpdateAsync()` - Ažurira komentar
- `DeleteAsync()` - Briše komentar

**Lokacija**: `Back/Repositories/Review/`

---

### 3️⃣ **Service Layer**

**Fajlovi**:

- `IReviewCommentService.cs` - Interface
- `ReviewCommentService.cs` - Implementacija sa business logikom

**Metode**:

- `CreateCommentAsync()` - Kreiranja komentara sa validacijom
- `GetCommentByIdAsync()` - Dobija jedan komentar
- `GetCommentsByReviewIdAsync()` - Dobija sve komentare sa paginacijom (stranica, veličina)
- `UpdateCommentAsync()` - Ažurira samo vlastite komentare
- `DeleteCommentAsync()` - Briše samo vlastite komentare

**Sigurnost**:

- ✅ Samo vlasnik može edituovati/brisati
- ✅ Minimalna validacija (komentar ne može biti prazan)

**Lokacija**: `Back/Services/Review/`

---

### 4️⃣ **Controller** - `ReviewCommentController.cs`

**Endpointovi**:

```
POST   /api/reviewcomment/create        - Kreiraj komentar (JWT Authorization)
GET    /api/reviewcomment/{commentId}   - Dobij jedan komentar
GET    /api/reviewcomment/review/{reviewId}?page=1&pageSize=10  - Dobij sve komentare
PUT    /api/reviewcomment/{commentId}   - Ažuriraj komentar (JWT Authorization)
DELETE /api/reviewcomment/{commentId}   - Obriši komentar (JWT Authorization)
```

**Sigurnost**: Svi write operacije zahtijevaju `[Authorize]` atribut

**Lokacija**: `Back/Controllers/ReviewCommentController.cs`

---

### 5️⃣ **SignalR Hub** - `ReviewCommentsHub.cs`

**Real-time metode**:

- `JoinReviewGroup(reviewId)` - Korisnik se uključuje u grupu za recenziju
- `LeaveReviewGroup(reviewId)` - Korisnik napušta grupu
- `BroadcastNewComment()` - Emitira novi komentar svim u grupi
- `BroadcastUpdateComment()` - Emitira ažurirani komentar
- `BroadcastDeleteComment()` - Emitira obrisani komentar

**Lokacija**: `Back/Hubs/ReviewCommentsHub.cs`

---

### 6️⃣ **DTOs** - `ReviewCommentDTO.cs`

- `CreateReviewCommentRequest` - Za slanje novog komentara
- `UpdateReviewCommentRequest` - Za ažuriranje komentara
- `ReviewCommentResponse` - Odgovor sa detaljima
- `ReviewCommentsListResponse` - Lista sa paginacijom i brojanjem

---

### 7️⃣ **Program.cs Konfiguracija**

```csharp
// Dodano:
builder.Services.AddSingleton<IReviewCommentRepository, ReviewCommentRepository>();
builder.Services.AddScoped<IReviewCommentService, ReviewCommentService>();
builder.Services.AddSignalR();

// U app konfiguraciji:
app.MapHub<ReviewCommentsHub>("/reviewCommentsHub");
```

---

## 🎯 FRONTEND IMPLEMENTACIJA

### 1️⃣ **DTOs** - `reviewComment.ts`

```typescript
-ReviewCommentResponse -
  ReviewCommentsListResponse -
  CreateReviewCommentRequest -
  UpdateReviewCommentRequest;
```

**Lokacija**: `Front/src/app/dto/reviewComment.ts`

---

### 2️⃣ **SignalR Servis** - `review-comments-signalr.service.ts`

**Funkcionanost**:

- ✅ Automatska konekcija sa hub-om
- ✅ Grupirane poruke po recenziji
- ✅ Observables za: `commentReceived$`, `commentUpdated$`, `commentDeleted$`
- ✅ `joinReviewGroup()` - Uključi se u grupu
- ✅ `leaveReviewGroup()` - Napusti grupu

**Lokacija**: `Front/src/app/services/review-comments-signalr.service.ts`

---

### 3️⃣ **API Servis Ekstenzija** - `api.ts`

**Dodane metode**:

```typescript
createReviewComment(payload); // POST
getReviewComments(reviewId, page); // GET
updateReviewComment(id, payload); // PUT
deleteReviewComment(id); // DELETE
```

---

### 4️⃣ **AuthService Ekstenzija** - `auth.service.ts`

**Dodane metode** (JWT dekodiranje):

```typescript
isAuthenticated(); // Provjeri je li korisnik prijavljen
getCurrentUserId(); // Izvuci ID iz JWT tokena
getCurrentUserName(); // Izvuči korisničko ime iz JWT tokena
decodeToken(); // Interna metoda za dekodiranje tokena
```

---

### 5️⃣ **Komponenta za Komentare** - `ReviewCommentsComponent`

**Fajlovi**:

- `review-comments.ts` - Logika
- `review-comments.html` - Šabloni
- `review-comments.css` - Stilovi

**Funkcije**:

- ✅ Prikaz svih komentara sa paginacijom
- ✅ Forma za nove komentare (samo za prijavljene)
- ✅ Real-time učitavanje novih komentara (SignalR)
- ✅ Editovanje samo svojih komentara
- ✅ Brisanje samo svojih komentara
- ✅ Prikaz ko je napisao šta
- ✅ Datum i vrijeme komentara
- ✅ Paginacija (10 komentara po stranici)

**Lokacija**: `Front/src/app/review-comments/`

---

### 6️⃣ **Integracija u Review Detail** - `review-detail.ts`

```typescript
// Import com komponente
import { ReviewCommentsComponent } from "../review-comments/review-comments";

// Dodaj u imports:
imports: [CommonModule, ReviewCommentsComponent];
```

**U HTML**:

```html
<app-review-comments [reviewId]="review.id" [reviewUserId]="review.userId">
</app-review-comments>
```

---

### 7️⃣ **Package.json Update**

```json
"@microsoft/signalr": "^8.0.0"  // Za real-time komunikaciju
```

---

## 🔄 KOMPLETАН FLOW - KAKO RADI

### 📤 **Slanje Komentara**:

1. Korisnik unese tekst u formu
2. Komponenta pozove `api.createReviewComment()`
3. Backend kreira komentar i sprema u MongoDB
4. SignalR emituje novi komentar svim korisnicima u grupi
5. Sve otvorene React komponente dobivaju real-time update
6. Novi komentar se pojavljuje na sve stranice bez refresh-a

### 🔄 **Real-time Ažuriranje**:

- Kada korisnik editira komentar → SignalR -> Ažuriranje na svim ekranima
- Kada korisnik obriše komentar → SignalR -> Uklanjanje sa svih ekrana
- Paginacija se automatski upravlja

### 🔐 **Sigurnost**:

- ✅ JWT authentication na svim POST/PUT/DELETE zahtjevima
- ✅ Samo vlasnik može edituovati/brisati
- ✅ Validacija na backend-u

---

## 📊 DATABASE STRUKTURA

**Kolekcija**: `review_comments`

```json
{
  "_id": ObjectId,
  "reviewId": ObjectId,
  "userId": ObjectId,
  "userName": "string",
  "content": "string",
  "createdAt": Date,
  "updatedAt": Date
}
```

---

## 🚀 KAKO KORISTITI

### **Za Korisnika**:

1. Otiđi na detalje recenzije
2. Skrolaj dolje do sekcije "Komentari"
3. Ako si prijavljen, unesi svoj komentar
4. Klikni "Pošalji"
5. Vidiš svoj komentar odmah (bez osvježavanja)
6. Vidiš tuđe komentare u real-time

### **Za Developer-a**:

1. Backend: `dotnet run` (SignalR će biti dostupan na `/reviewCommentsHub`)
2. Frontend: `npm install` (da instalira `@microsoft/signalr`)
3. `ng serve` - Aplikacija je gotova

---

## 📝 VALIDACIJA I OGRANIČENJA

- ✅ Komentar mora biti minimalno 1 karakter
- ✅ Max stranica: bez limita (ali 10 po stranici)
- ✅ Samo prijavljeni korisnici mogu pisati
- ✅ Samo vlastiti komentari se mogu edituovati/brisati
- ✅ Автоматска paginacija nakon 10 komentara

---

## 🎨 UI/UX

**Komponenta prikazuje**:

- 📝 Tekst komentara
- 👤 Korisničko ime
- 📅 Datum i vrijeme komentara
- ✏️ Dugme za edit (samo vlastitih)
- 🗑️ Dugme za brisanje (samo vlastitih)
- 📄 Paginaciju
- ⚙️ Loading stanja

---

## ✨ FUTURE ENHANCEMENTS

Opcije za proširenje:

- 🔔 Notifikacije kada neko odgovori na tvoj komentar
- 👍 Like/unlike na komentarima
- 🏷️ Mentionovanje @username
- 🔍 Pretraga komentara
- ⭐ Rejtanje komentara (helpful/unhelpful)

---

**GOTOVO! Svi komentari sada rade sa real-time updates! 🎉**
