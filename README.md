# NET-9-ShortURL
Приложение для генерации коротких ссылок. Без учёта доменного имени, длинна ссылки в диапазоне от 7 до 10 символов. Генерация короткой ссылки через обычный HTTP запрос.

Пример запроса для получения короткой ссылки: http://localhost:5045/home?FullURL=www.google.com , где www.google.com - ссылка для сокращения. <\br>
Пример запроса для перехода по короткой ссылке: http://localhost:5045/3q75I7n , где 3q75I7n - короткая ссылка.
