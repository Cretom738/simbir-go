# SimbirGo

## Инструкция по запуску приложения
1. Перейти в папку ./SimbirGo/WebApi
2. В файле appsettings.json изменить пароль Password в строке подключения DefaultConnection на пароль пользователя postgres на вашем ПК.
3. Запустить консоль в той же папке.
4. Скомпилировать и запустить приложение утилитой: dotnet run --environment Development --urls=http://localhost:8080
   (Если порт занят, его можно изменить на любой другой в параметре --urls утилиты dotnet run)
5. Приложение будет доступно по адресу http://localhost:8080, адрес swagger http://localhost:8080/swagger/index.html

## URL: http://localhost:8080/swagger/index.html

## Аккаунт админа
Логин: Admin
Пароль: Admin
