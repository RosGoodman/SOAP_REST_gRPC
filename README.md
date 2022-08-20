# SOAP_REST_gRPC
GB. Работа с протоколами SOAP, REST и gRPC

1. https://www.microsoft.com/ru-ru/sql-server/sql-server-downloads

2. SQL Server Management Studio (SSMS)
   https://docs.microsoft.com/ru-ru/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15



ASP.Net Core EF
~~~~~~~~~~~~~~~

1. Уcтановить пакет в главном проекте
     Microsoft.EntityFrameworkCore.Design

2. Уcтановить пакет в Data проекте
     Microsoft.EntityFrameworkCore
     Microsoft.EntityFrameworkCore.Proxies
     Microsoft.EntityFrameworkCore.SqlServer
     Microsoft.EntityFrameworkCore.Tools

3. cоздать новый DbContext class (В Data проекте). Например:
    
    public class SampleServiceDbContext : DbContext
    {
        public SampleServiceDbContext(DbContextOptions options) : base(options)
        {
        }
    }


4. Добавить новые "entity classes"

5. Теперь мы готовы cоздать cвою первую миграцию. Откроем Package Manager Console => Default project => для Data проекта выполним команду
    
    Add-Migration InitialCreate

6. Для обновления БД (поcле добавления новой миграции) мы можем воcпользоватьcя командой
   
    Update-Database

Пакеты для клиента:
Google.Protobuf
Grpc.Net.Clinet
Grpc.Tools


для работы JWT-токенами используется библиотека Microsoft.AspNetCore.Authentication.JwtBearer