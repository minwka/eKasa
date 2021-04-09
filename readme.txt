Notlar:
• Proje kümesini bütünüyle derleyip dağıtılabilir hale getirmek için <Deploy.ps1> dosyasına sağ tıklayıp "PowerShell ile Çalıştır" diyin.
• <Deploy.ps1> script'i <dotnet> komutunu kullanmaktadır. Script'in düzgün çalışabilmesi için <Visual Studio Build Tools>'un veya Visual Studio'nun yüklü olması gerekmektedir.
• <Deployment_Package> klasörünü silmeyin.
• Uygulama dosyalarını <Deployment_Package\Executables> altında bulabilirsiniz.
• <Installer> ve <Uninstaller> uygulamalarının dosyalarını <Deployment_Package\Install_Package> altında bulabilirsiniz.
• <Install_Package> klasörünü kullanıcılara dağıtabilirsiniz.
• Proje, yerel Git repo'su kullanmaktadır. Proje kod geçmişini orada bulabilirsiniz.
• Bunlar dışında projeyi, diğer projeler gibi, aynı şekilde Visual Studio üzerinden kullanabilirsiniz.
• <Cleanup.ps1> script'i, derleme sonrası uygulama dosyalarını <Deployment_Package> dizini altındaki paketleri temizler.