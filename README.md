# Запуск приложения
SimbirGO Загрузка исходных файлов Скачайте исходные файлы проекта с репозитория. Запуск приложения Откройте командную строку.

Перейдите в каталог с проектом. Например, если проект находится по пути 
```sh
C:\Users[Имя пользователя]\source\repos\SimbirGO\SimbirGO_API, выполните следующую команду:
```
cd C:\Users[Имя пользователя]\source\repos\SimbirGO\SimbirGO_API
Убедитесь, что в этом каталоге есть файл с расширением .csproj.

Запустите приложение с помощью следующей команды:

dotnet run Вы увидите сообщение о запуске приложения и информацию о том, на каком адресе приложение слушает.

Откройте веб-браузер и перейдите по следующему URL, указав порт, предоставленный приложением:
```sh
http://localhost:5125/swagger
```
Вы будете перенаправлены на страницу Swagger UI, где можно просматривать и тестировать функциональность приложения.

Использование JWT токена Для использования защищенных ресурсов приложения, необходимо создать JWT токен. Обратите внимание, что доступ к созданию токена доступен только для пользователя с именем "Admin" и паролем "1234". Можно будет добавить свои аккаунты.

Создание JWT токена Для создания JWT токена выполните следующие действия:

Отправьте POST-запрос на следующий эндпоинт приложения, указав имя пользователя и пароль:
```sh
POST /security/createToken 
```
Пример запроса:
```sh
"UserName": "Admin",
"Password": "1234"
```
В ответ на запрос вы получите JWT токен.

Использование JWT токена Чтобы использовать JWT токен для доступа к защищенным ресурсам, укажите токен в заголовке запроса Authorization следующим образом:

Bearer [ваш_токен] После этого вы получите доступ ко всем HTTP запросам WebAPI SimbirGO.

# база данных

В репозитории будет дан скрипт БД SimbirGO

Его нужно будет активировать и выполнить в ново созданной базе данных: SimbirGO

Тогда приложение должно успешно подключить БД к WebAPI

Вот код
```sh
public static NpgsqlConnection connecting = new NpgsqlConnection(@"Host=localhost;Port=5432;Database=SimbirGO;Username=postgres;Password=1122334455");
```
Пароль будет зависить от вашего пользователя в PGAdmin или в другой СУБД

Автор: Давид Иус
