# Implementacija Redis baze podataka za keširanje podataka

## Pokretanje aplikacije

### Instalacije
- Instaliran Visual Studio 2022 (Workloads koje treba instalirati: ASP.NET and web development, .NET 6 runtime)
- Instaliran redis: 
------------------------------------------------------------------------------------------------------------------------------------------------
Instalirati server (rar datoteka se nalazi u folderu Instalacija), čekirati opciju Add the Redis installation folder to the Path environment variable
Kad se instalira server, u prilogu su jos .conf fajlovi koje treba prekopirati u C:/Program Files/Redis (ili drugi folder gdje je server instaliran, ali trebao bi biti ovaj po defaultu)
Redis server pokrenuti:
Kao servis:
·   Treba otvoriti Services
·   Redis - Start/Stop

Rucno:
·   cmd
·   cd "C:\Program Files\Redis"
·   redis-server.exe "C:\Program Files\Redis\redis.windows.conf"

Provjera:
cd "C:\Program Files\Redis"
redis-cli.exe -h 127.0.0.1
·   PING,odziv od redisa treba biti: PONG

------------------------------------------------------------------------------------------------------------------------------------------------
- Preuzeti SQL Server i instalirati SQL Server Management studio 2019
- Preuzeti StackOverflow2010 bazu podataka (https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent/), otvoriti SSMS, desni klik na Databases -> Attach i selektovati oba filea koja su downloadovana.
- Otvoriti Visual Studio 2022 i kliknuti zeleno play dugme za pokretanje aplikacije.
