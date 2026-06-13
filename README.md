# BankApi

REST API банковской системы, разработанный на ASP.NET Core. Поддерживает управление пользователями, счетами, картами, транзакциями и автоматическую конвертацию валют по курсам Национального Банка Кыргызской Республики.

## Технологии

- **ASP.NET Core** — веб-фреймворк
- **Entity Framework Core** — ORM
- **PostgreSQL** — база данных
- **Swagger / Swashbuckle** — документация API
- **IMemoryCache** — кэширование курсов валют
- **BackgroundService** — фоновое обновление курсов валют (НБКР)

## Возможности

- Регистрация, обновление и деактивация пользователей
- Создание и закрытие банковских счетов (KGS, USD, EUR, RUB)
- Выпуск, блокировка и перевыпуск банковских карт
- Переводы между счетами с автоматической конвертацией валют
- Управление телефонными номерами пользователя
- Многоязычные сообщения об ошибках (ru / en / ky)
- Автоматическое обновление курсов валют раз в сутки (XML-фид НБКР)

## Запуск проекта

### Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### 1. Клонировать репозиторий

```bash
git clone https://github.com/salyamovss/BankApi.git
cd BankApi
```

### 2. Настроить строку подключения к БД

В файле `appsettings.json` укажи свои параметры подключения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=bankapi;Username=postgres;Password=yourpassword"
  }
}
```

### 3. Запустить проект

```bash
dotnet run --project BankApi
```

База данных создастся автоматически при первом запуске (`EnsureCreatedAsync`).

### 4. Открыть Swagger

```
https://localhost:{port}/swagger
```

## Структура проекта

```
BankApi/
├── Controllers/         # HTTP контроллеры
├── Services/            # Бизнес-логика
│   └── job/             # Фоновые задачи
├── dal/
│   ├── Models/          # Сущности БД
│   │   └── Enums/       # Перечисления
│   ├── DTOs/            # Объекты запросов и ответов
│   └── Repositories/    # Интерфейсы и реализации репозиториев
│       └── impl/
├── Data/                # AppDbContext
├── Mappers/             # Маппинг между моделями и DTO
└── Common/              # Вспомогательные классы, коды ошибок, сообщения
```

## API Эндпоинты

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/users` | Регистрация пользователя |
| GET | `/api/users/{id}` | Получить профиль пользователя |
| PUT | `/api/users/{id}` | Обновить данные пользователя |
| PATCH | `/api/users/{id}/deactivate` | Деактивировать пользователя |
| POST | `/api/users/restore` | Восстановить пользователя |
| GET | `/api/users` | Список пользователей с фильтрацией |
| POST | `/api/accounts` | Создать счёт |
| GET | `/api/accounts/{id}` | Получить счёт |
| GET | `/api/accounts/user` | Счета текущего пользователя |
| DELETE | `/api/accounts/{id}` | Закрыть счёт |
| POST | `/api/accounts/{id}/restore` | Восстановить счёт |
| POST | `/api/cards` | Выпустить карту |
| GET | `/api/cards/{id}` | Получить карту |
| GET | `/api/cards/account/{accountId}` | Карты счёта |
| DELETE | `/api/cards/{id}/block` | Заблокировать карту |
| PUT | `/api/cards/{id}/unblock` | Разблокировать карту |
| PUT | `/api/cards/{id}/reissue` | Перевыпустить карту |
| POST | `/api/transactions/transfer` | Перевод между счетами |
| GET | `/api/transactions/account/{accountId}` | История транзакций |
| POST | `/api/phones` | Добавить телефон |
| DELETE | `/api/phones/{id}` | Удалить телефон |

## Аутентификация

Текущая версия использует упрощённую идентификацию через хедер `X-User-Id`. так как это тезтовая ТЗ 

## Валюты

Поддерживаемые валюты: `KGS`, `USD`, `EUR`, `RUB`

Курсы обновляются автоматически каждые 24 часа с официального XML-фида НБКР.

