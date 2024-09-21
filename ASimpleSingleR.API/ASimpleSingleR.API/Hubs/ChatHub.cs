using ASimpleSingleR.API.Models; // Import models used in the application / Импорт моделей, используемых в приложении
using Microsoft.AspNetCore.SignalR; // Import SignalR for WebSocket communication / Импорт SignalR для работы с веб-сокетами
using Microsoft.Extensions.Caching.Distributed; // Import for distributed caching / Импорт для распределенного кэша
using System.Text.Json; // Import for JSON handling / Импорт для работы с JSON

namespace ASimpleSingleR.API.Hubs
{
    // Interface for chat clients defining a method to receive messages / Интерфейс для клиентов чата, определяющий метод получения сообщений
    public interface IChatClient
    {
        public Task ReceiveMessage(string username, string message); // Method to receive a message / Метод для получения сообщения
    }

    // ChatHub class inherited from Hub with IChatClient interface / Класс ChatHub, унаследованный от Hub с интерфейсом IChatClient
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IDistributedCache cache; // Field for distributed cache / Поле для кэша

        // Constructor accepting IDistributedCache / Конструктор, принимающий IDistributedCache
        public ChatHub(IDistributedCache cache)
        {
            this.cache = cache; // Initialize cache / Инициализация кэша
        }

        // Method for user to join a chat / Метод для подключения пользователя к чату
        public async Task JoinChat(UserConnection userConnection)
        {
            // Add user to the group based on their chat room / Добавляем пользователя в группу по его чат-комнате
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.chatRoom);

            // Serialize user connection info to a string / Сериализация информации о подключении пользователя в строку
            var stringConnection = JsonSerializer.Serialize(userConnection);
            // Store the string in the cache using the connection ID / Сохранение строки в кэше по ID подключения
            await cache.SetStringAsync(Context.ConnectionId, stringConnection);

            // Notify all users in the group about the new connection / Уведомление всех пользователей в группе о новом подключении
            await Clients.Group(userConnection.chatRoom)
                .ReceiveMessage("Admin", $"{userConnection.userName} присоединился к чату");
        }

        // Method to send a message in the chat / Метод для отправки сообщения в чат
        public async Task SendMessage(string message)
        {
            // Retrieve user connection info from the cache / Получаем информацию о подключении из кэша
            var stringConnection = await cache.GetStringAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            // If the connection is found, send the message to the group / Если соединение найдено, отправляем сообщение в группу
            if (connection != null)
            {
                await Clients.Group(connection.chatRoom)
                              .ReceiveMessage(connection.userName, message);
            }
        }

        // Method called when the user disconnects / Метод, вызываемый при отключении пользователя
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Retrieve user connection info from the cache / Получаем информацию о подключении из кэша
            var stringConnection = await cache.GetStringAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            // If the connection is found, notify about the user leaving / Если соединение найдено, уведомляем о выходе пользователя
            if (connection != null)
            {
                await Clients.Group(connection.chatRoom)
                    .ReceiveMessage("Admin", $"{connection.userName} вышел из чата");

                // Remove connection from the cache and group / Удаляем соединение из кэша и группы
                await cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.chatRoom);
            }

            await base.OnDisconnectedAsync(exception); // Call base method / Вызов базового метода
        }
    }
}
