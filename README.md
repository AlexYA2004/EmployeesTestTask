## Основные возможности
Фильтрация по ФИО, активности, статусу трудоустройства
Серверная пагинация по 20 записей на странице
Сортировка по различным полям
Валидация данных на клиенте и сервере
Логирование в консоль и файлы
Глобальная обработка ошибок
Docker-контейнеризация

## Технологический стек
Backend: ASP.NET Core 8.0, Entity Framework Core 8.0
Database: Microsoft SQL Server 2019
Frontend: Razor Pages, Bootstrap 5, JavaScript
Containerization: Docker, Docker Compose
Logging: Serilog
Architecture: MVC, Repository Pattern

# Быстрый старт
## Предварительные требования
Docker
Docker Compose

## Запуск приложения
### Клонируйте репозиторий
git clone <repository-url>
cd EmployeesTestTask

### Запустите приложение с помощью Docker Compose
docker-compose up --build

### Приложение будет доступно по адресу
http://localhost:5000
