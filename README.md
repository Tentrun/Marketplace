
# Marketplace

Краткий список фреймов с версиями и ссылок, во избежании потери


## Установка

Устанавливаем менеджер версий NodeJS (NVM)

[NVM под винду](https://github.com/coreybutler/nvm-windows/releases/download/1.1.12/nvm-setup.exe)

**Открываем CMD/PowerShell** и прописываем 
```
nvm install 20.15.0
```

Установка фронта с директории ClientApp через **NPM**:

```
npm i
```

Развертка БД в докере через CMD

```
docker run --name marketplace -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=Qwerty123@! -e POSTGRES_HOST_AUTH_METHOD=trust -v pg_data:/var/lib/postgresql/data -d postgres
```

Через pgAdmin подключаемся к БД на localhost:5432

Там создаем базы данных **LoggerDb  ProductsDb SettingsDb**

**Миграции применятся автоматически**    
## Tech Stack

**Client:** Angular 18.0.5, .Net 8.0,

**WebDep:** NodeJS 20.15, TaigaUI 3.84.0

**Server:** .Net 8.0


## Ссылки

[Trello доска](https://trello.com/invite/b/oxx5FA0R/ATTIc19c3499bcf48de8f1a71221c7d6da9345C20AD0/marketplace)

[NVM под винду](https://github.com/coreybutler/nvm-windows/releases/download/1.1.12/nvm-setup.exe)

[Дока TaigaUI](https://taiga-ui.dev/getting-started)

[Docker Desktop](https://www.docker.com/products/docker-desktop)


## Подключение различных сервисов в Program.cs
Подключение DbContext'a
```
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("название бд из appsettings"));
});
```

Подключение UnitOfWork для работы с репозиториями
``` 
builder.Services.AddUnitOfWork<ApplicationDbContext>();
```

Подключение HealthChecker'a
``` 
builder.Services.AddDatabaseHealthReporter(ServicesConst.НазваниеСервиса, "Описание сервиса");
```

``` 
app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<IServer>();
    var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

    if (serverAddressesFeature == null) return;

    HealthCheckService.ApplicationAddress =
        serverAddressesFeature.Addresses.FirstOrDefault(x => x.Contains("http"));
});
```
