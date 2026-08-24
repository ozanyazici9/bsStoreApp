# bsStoreApp

**bsStoreApp**, N-Katmanlı Mimari (N-Layer Architecture) prensipleriyle geliştirilen bir **ASP.NET Core Web API** projesidir. Proje; kimlik doğrulama, önbellekleme, hız sınırlama, sayfalama ve dinamik sorgulama gibi production-grade bir Web API'de bulunması beklenen pek çok özelliği uçtan uca uygulamalı olarak barındırır.

📖 **API Dokümantasyonu:** [ozanyazici9.github.io/bsStoreApp](https://ozanyazici9.github.io/bsStoreApp/)

---

## İçindekiler

- [Proje Hakkında](#proje-hakkında)
- [Mimari](#mimari)
- [Proje Yapısı](#proje-yapısı)
- [Özellikler](#özellikler)
- [Kullanılan Teknolojiler](#kullanılan-teknolojiler)
- [Başlarken](#başlarken)
- [API Kullanımı](#api-kullanımı)
- [Yol Haritası](#yol-haritası)

## Proje Hakkında

bsStoreApp, bir e-ticaret / mağaza senaryosu üzerinden **temiz mimari (clean architecture)** ve **iyi Web API pratiklerini** bir araya getiren bir portföy projesidir. Katmanlar arası sorumluluk ayrımı, tekrar kullanılabilir servis/repository yapıları ve ölçeklenebilir bir API tasarımı hedeflenmiştir.

> Proje, BTK Akademi müfredatı kapsamında geliştirilen bir Web API kursunun uygulamalı çıktısıdır ve zamanla yeni katmanlar/özelliklerle genişletilmektedir.

## Mimari

Proje, sorumlulukların net biçimde ayrıldığı bir **N-Katmanlı Mimari** üzerine kuruludur:

```
┌─────────────────────┐
│     Presentation     │  → Controller'lar, filtre'ler, API uç noktaları
├─────────────────────┤
│       Services       │  → İş kuralları, Service Manager (Lazy<T>), Facade
├─────────────────────┤
│     Repositories      │  → Repository + Unit of Work, EF Core sorguları
├─────────────────────┤
│       Entities        │  → Domain modelleri, DTO'lar
├─────────────────────┤
│  bsStoreApp (Host)    │  → Startup/Program, appsettings, DI kayıtları
└─────────────────────┘
```

Uygulanan başlıca tasarım desenleri:

- **Repository + Unit of Work** – veri erişim katmanının soyutlanması
- **Service Manager (`Lazy<T>`)** – servislerin ihtiyaç anında (lazy) oluşturulması
- **Facade Pattern** – birden fazla servisin tek bir arayüz üzerinden yönetilmesi
- **AOP (Aspect-Oriented Programming)** – action filter'lar aracılığıyla loglama ve doğrulama gibi çapraz kesen ilgilerin ayrıştırılması

## Proje Yapısı

| Klasör / Dosya  | Açıklama                                                           |
| --------------- | ------------------------------------------------------------------ |
| `Entities/`     | Domain modelleri ve veri transfer nesneleri (DTO)                  |
| `Repositories/` | Repository ve Unit of Work implementasyonları, EF Core `DbContext` |
| `Services/`     | İş mantığı katmanı, servis arayüzleri ve implementasyonları        |
| `Presentation/` | API controller'ları, action filter'lar                             |
| `bsStoreApp/`   | Host projesi — `Program.cs`, `appsettings.json`, DI konfigürasyonu |
| `docs/`         | GitHub Pages üzerinden yayınlanan API dokümantasyonu               |

## Özellikler

- 🔐 **Kimlik Doğrulama & Yetkilendirme** — ASP.NET Core Identity (`IdentityDbContext<User>`) ve JWT tabanlı authentication, statik GUID'lerle rol seed işlemleri
- 🧩 **API Versiyonlama** — birden fazla API sürümünün bir arada yönetilmesi
- 📄 **Swagger / OpenAPI** — Swashbuckle ile interaktif API dokümantasyonu
- 🔄 **İçerik Pazarlığı (Content Negotiation)** — JSON'un yanı sıra XML formatter desteği
- ⚡ **HTTP Önbellekleme** — Marvin.Cache.Headers ile ETag/Cache-Control yönetimi
- 🚦 **Hız Sınırlama (Rate Limiting)** — AspNetCoreRateLimit ile istemci başına istek sınırlandırma
- 🗺️ **AutoMapper** — Entity ↔ DTO dönüşümleri
- ✏️ **JSON Patch** — Newtonsoft.Json tabanlı kısmi güncelleme (`PATCH`) desteği
- 📑 **Sayfalama** — özel `PagedList<T>` implementasyonu ile sayfalanmış sonuç kümeleri
- 🧵 **Dinamik Sıralama** — `System.Linq.Dynamic.Core` ile çalışma zamanında (runtime) alan bazlı sıralama
- 🛡️ **Global Hata Yönetimi** — merkezi exception handling middleware
- 🌐 **CORS** yapılandırması
- 🧱 **Action Filter'lar** — loglama ve model doğrulama için özel filtreler
- 🗄️ **EF Core Migrations** — kod-öncelikli (code-first) veritabanı yönetimi

## Kullanılan Teknolojiler

- **ASP.NET Core Web API**
- **Entity Framework Core** & ASP.NET Core Identity
- **JWT Bearer Authentication**
- **AutoMapper**
- **Swashbuckle (Swagger / OpenAPI)**
- **Marvin.Cache.Headers**
- **AspNetCoreRateLimit**
- **System.Linq.Dynamic.Core**
- **Newtonsoft.Json** (JSON Patch desteği için)

## Başlarken

### Gereksinimler

- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server (yerel veya uzak bir instance)

### Kurulum

```bash
# Depoyu klonlayın
git clone https://github.com/ozanyazici9/bsStoreApp.git
cd bsStoreApp
```

`bsStoreApp/appsettings.json` dosyasında bağlantı dizesini ve JWT ayarlarını kendi ortamınıza göre güncelleyin:

```json
{
  "ConnectionStrings": {
    "sqlConnection": "Server=.;Database=bsStoreApp;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "validIssuer": "...",
    "validAudience": "...",
    "expires": 5
  }
}
```

Veritabanı migration'larını uygulayın (migration'lar host projesinde tutulur, bu nedenle hem `--project` hem `--startup-project` bayrakları `bsStoreApp`'i gösterir):

```bash
dotnet ef database update --project bsStoreApp --startup-project bsStoreApp
```

Uygulamayı çalıştırın:

```bash
dotnet run --project bsStoreApp
```

Uygulama ayağa kalktıktan sonra Swagger arayüzüne `https://localhost:<port>/swagger` üzerinden erişebilirsiniz.

## API Kullanımı

Uç noktaların tam listesi, istek/yanıt şemaları ve örnekler için canlı API dokümantasyonuna göz atabilirsiniz:

👉 **[https://ozanyazici9.github.io/bsStoreApp/](https://ozanyazici9.github.io/bsStoreApp/)**

Korumalı uç noktalar için önce `/authentication/login` (veya ilgili giriş uç noktası) üzerinden bir JWT token alıp isteklerinize `Authorization: Bearer <token>` header'ı ile devam etmeniz gerekir.
