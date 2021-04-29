<h1 align="center" id="title">
 eKasa: Şifre Yönetici <a href="https://github.com/minwka/eKasa"><img title="Version" src="https://img.shields.io/badge/Sürüm-0.10.8.0-blue.svg"></a>
</h1>

<p align="center" id="version">
<a href="https://github.com/minwka/eKasa"><img title="eKasa" src="https://img.shields.io/badge/Uygulama-eKasa-blue.svg"></a>
<a href="https://github.com/minwka/eKasa"><img title="Language" src="https://img.shields.io/badge/Dil-C%23-purple"></a>
<a href="https://github.com/minwka/eKasa"><img title="Framework" src="https://img.shields.io/badge/Framework-.NET%205-purple"></a>
</p>

<p align="center" id="logo">  
<a href="https://github.com/minwka/eKasa"><img title="eKasa" width="300" height="300" src="eKasa.Installer/Controls/ekasa_logo.png"></img></a>
</p>

<p align="center" id="socials">
<a href="https://github.com/minwka"><img title="Github" src="https://img.shields.io/badge/Ferit%20Uzun-%20-red?style=for-the-badge&logo=github"></a>
<a href="https://t.me/anth4"><img title="Telegram" src="https://img.shields.io/badge/Ferit%20Uzun-%20-red?style=for-the-badge&logo=Telegram"></a>
</p>

<p align="center" id="stats">
<a href="https://github.com/minwka?tab=followers"><img title="Followers" src="https://img.shields.io/github/followers/minwka?color=darkblue"></a>
<a href="https://github.com/minwka/eKasa/stargazers"><img title="Stars" src="https://img.shields.io/github/stars/minwka/eKasa?color=darkblue"></a>
<a href="https://github.com/minwka/eKasa/network/members"><img title="Forks" src="https://img.shields.io/github/forks/minwka/eKasa?color=darkblue"></a>
<a href="https://github.com/minwka/eKasa/watchers"><img title="Watching" src="https://img.shields.io/github/watchers/minwka/eKasa?label=Watchers&color=darkblue"></a>
<a href="https://github.com/minwka/eKasa"><img title="Licence" src="https://img.shields.io/badge/License-TBD-red.svg"></a>
</p>

## Uygulama Hakkında
eKasa, şifrelerinizi tek bir yerde güvenle saklayabilmek ve yönetebilmek için kullanabileceğiniz bir şifre yöneticisidir. Kurulum gerektirmez. Veritabanı sunucusuna ihtiyaç duymaz. Bilgi toplamaz. Hafif bir uygulamadır

## Uygulama Özellikleri
- 256-Bit **AES** şifreleme ile korunan veritabanı dosyaları
- Veritabanı içe/dışa aktarma
- Otomatik yazma, otomatik doldurma
- Veritabanı hatırlama
- Kayıt gruplandırma ve sıralama
- Şifre oluşturma yardımcısı
- Taşınabilir `*.fdbx` | `*.json` veritabanı
- Kurulum gerektirmeyen taşınabilir uygulama

## Kurulum
### Kullanıcılar için
- [`Releases`](https://github.com/minwka/eKasa/releases) sekmesini ziyaret ederek güncel sürümün altındaki `"Taşınabilir-*"` veya `"Kurulum-*"` paketlerinden sisteminize uygun olanı seçip indirebilirsiniz.

### Geliştiriciler için
- Proje dizinindeki ```1-Build.ps1``` betiğini çalıştırarak projeyi derleyin.
- Betik bittiğinde ```deploy``` yazarak veya ```3-Deploy.ps1``` betiğini çalıştırarak paketleme sürecini başlatın.
- Paketleme işlemi bittikten sonra proje dizinindeki ```Deployment_Package``` klasörü altında ```Install_Package``` klasörünü bulacaksınız.
- ```Install_Package``` klasörü altındaki ```Installer.exe``` kurulum sihirbazını kullanarak uygulamanın kurulumunu gerçekleştirebilir veya bu klasörü başka kullanıcılara dağıtabilirsiniz
- ```Deployment_Package``` klasörü altındaki ```Executables``` klasöründe, uygulamaların derlenmiş çıktıları bulunmaktadır. Buradan, uygulamalara kurulum gerçekleştirmeden de erişebilirsiniz.

## Kullanım
- Kurulum sonrasında oluşan kısayolları kullanarak veya doğrudan, ```eKasa.exe``` ana uygulamasını çalıştırın.
- Varolan bir veritabanınız yoksa giriş penceresinden yeni bir tane oluşturun.
- Giriş penceresinden veritabanınızı seçip şifrenizi girin. Yeni bir veritabanı oluşturduysanız veritabanınız otomatik olarak açılır.
- Menü çubunğundaki ( `+` ) kayıt ekleme sekmesini kullanarak giriş bilgilerinizi kaydetmeye başlayın.
- Daha fazla bilgi için uygulama penceresinin sol alt köşesindeki ```Yardım ( ? )``` butonunu kullanarak yardım penceresine erişebilir ve uygulamayı nasıl kullanacağınızı öğrenebilirsiniz.

## Proje Yapısı ve Özellikleri

### Projeler
| Proje | Türü | Amacı |
| - | - | - |
| Core | WPF | Ana uygulama |
| Library | Class Library | Ortak özellik kütüphanesi |
| Installer | WPF | Kurulum sihirbazı |
| Uninstaller | WPF | Kaldırma sihirbazı |

### Betikler
| Betik | Amacı |
| - | - |
| 0-Purge.ps1 | Tüm kalıntıları temizler |
| 1-Build.ps1 | Projeyi derler |
| 2-Clean.ps1 | Derleme-sonrası kalıntıları temizler |
| 3-Deploy.ps1 | Proje çıktılarını paketler |
| 9-Version.ps1 | Projelerin sürüm numaralarını günceller |

### Notlar
- PowerShell betikleri projeyi derlemek için ```dotnet``` komutunu kullanmaktadır. Betikletin çalışabilmesi için ```Visual Studio Build Tools``` yüklü olmalıdır.
- ```3-Deploy.ps1``` betiği, proje dizinindeki 7-zip uygulamasını kullanır. Bu betiğin düzgün çalışabilmesi için ```Deployment_Package klasörünü silmeyin```
- Proje çıktılarını ve kurulum paketlerini temizlemek için ```0-Purge.ps1``` betiğini kullanın.