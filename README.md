ASimpleSingleR Client
This is the client application for ASimpleSingleR, a simple real-time messaging application using SignalR.

Key Features
Chat Functionality:

Allows users to join chat rooms and send messages in real-time.
SignalR Integration:

Utilizes SignalR for establishing WebSocket connections to the server.
User Login:

Users can log in to join specific chat rooms.
How to Run
Make sure the backend server is running.
Run the React application with npm start.
Open the application in your web browser at http://localhost:3000.
Feel free to ask if you need anything else!

RU

ASimpleSingleR
ASimpleSingleR — это простое приложение для обмена сообщениями в реальном времени, использующее ASP.NET Core с SignalR и Redis для кэширования.

Основные компоненты
ChatHub:

Позволяет пользователям подключаться к чатам и обмениваться сообщениями.
Использует распределенный кэш для хранения информации о подключениях пользователей.
Обрабатывает события подключения и отключения, уведомляя группу пользователей.
Redis:

Используется для хранения информации о пользователях и их состояниях.
Настроен с помощью StackExchange.Redis.
CORS:

Разрешает запросы только с определенного источника (например, клиента React).
Как запустить
Убедитесь, что у вас установлен Docker.
Запустите docker-compose up для поднятия приложения и сервиса Redis.
Откройте клиентскую часть на http://localhost:3000.
Теперь у вас есть полное описание кода и его работы. Если нужно что-то дополнительно или уточнить, дайте знать!




