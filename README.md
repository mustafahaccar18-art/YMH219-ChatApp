# 💬 YMH219 — İstemci-Sunucu Chat Uygulaması

C# ve .NET ile geliştirilmiş, **Windows Forms** tabanlı gerçek zamanlı bir istemci-sunucu sohbet uygulaması.

---

## 📸 Ekran Görüntüleri

| Chat Server | Chat Client |
|---|---|
| Sunucu, bağlı istemcileri listeler ve tüm mesajları loglar | İstemci kullanıcı adıyla bağlanır ve mesaj gönderir |

---

## 🚀 Özellikler

- ✅ TCP/IP üzerinden gerçek zamanlı mesajlaşma
- ✅ Kullanıcı adı ile bağlanma
- ✅ Tüm mesajlarda `[HH:mm:ss]` zaman damgası
- ✅ **Bağlan / Mesaj Gönder / Bağlantıyı Kes** butonları
- ✅ Bağlantı hataları kullanıcıya bildirilir, uygulama çökmez
- ✅ Sunucu panelinde bağlanan istemcilerin listesi (Kullanıcı adı | IP | Bağlantı saati)
- ✅ Sunucudan tüm istemcilere yayın (broadcast) mesajı gönderme
- ✅ Birden fazla istemci aynı anda bağlanabilir

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Kullanım Yeri |
|---|---|
| `TcpListener` / `TcpClient` | Sunucu bağlantı dinleme / İstemci bağlantısı |
| `StreamReader` / `StreamWriter` | Satır tabanlı mesaj alışverişi |
| `async` / `await` | Asenkron ağ I/O, UI donmaz |
| `event` + OOP | Sınıflar arası haberleşme |
| `Invoke()` | Arka plan thread'inden UI güncellemesi |
| Windows Forms | Grafiksel arayüz |

---

## 📁 Proje Yapısı

```
YMH219-ChatApp/
├── ChatApp.slnx
├── ChatServer/
│   ├── Program.cs              # Giriş noktası
│   ├── ServerForm.cs           # Sunucu UI mantığı
│   ├── ServerForm.Designer.cs  # Form kontrolleri
│   ├── ChatServer.cs           # TcpListener + broadcast motoru
│   └── ClientHandler.cs        # Her bağlı istemci için yönetici
└── ChatClient/
    ├── Program.cs              # Giriş noktası
    ├── ClientForm.cs           # İstemci UI mantığı
    ├── ClientForm.Designer.cs  # Form kontrolleri
    └── ChatClientCore.cs       # TcpClient + okuma döngüsü
```

---

## ⚙️ Kurulum ve Çalıştırma

### Gereksinimler
- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 veya üzeri)
- Windows işletim sistemi

### 1. Repoyu klonla
```bash
git clone https://github.com/mustafahaccar18-art/YMH219-ChatApp.git
cd YMH219-ChatApp
```

### 2. Build al
```bash
dotnet build ChatApp.slnx
```

### 3. Sunucuyu başlat (Terminal 1)
```bash
dotnet run --project ChatServer
```
- Port alanına `5000` yaz → **▶ Start** butonuna tıkla
- **● Running** göründüğünde sunucu hazırdır

### 4. İstemciyi bağla (Terminal 2)
```bash
dotnet run --project ChatClient
```
- `Server: 127.0.0.1` | `Port: 5000` | `Username: Alice`
- **🔗 Connect** butonuna tıkla

> Aynı anda birden fazla istemci penceresi açılabilir.

---

## 🌐 Farklı Bilgisayardan Bağlanma

Sunucu bilgisayarının **LAN IP adresini** öğren:
```bash
ipconfig
```
İstemci `Server` alanına bu IP'yi yaz (örn. `192.168.1.10`).

---

## 📐 Mimari

```
[ChatClient] ──── TCP ────► [ChatServerCore]
                                    │
                            [ClientHandler x N]
                                    │
                            Broadcast ──► tüm istemciler
```

---

## 👤 Geliştirici

**Mustafa Haccar** — YMH219 Bilgisayar Ağları Dersi, Ödev 5
