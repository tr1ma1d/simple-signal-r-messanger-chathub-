using ASimpleSingleR.API.Hubs;

var builder = WebApplication.CreateBuilder(args); // Create a new web application instance / Создаем новый экземпляр веб-приложения

builder.Services.AddSignalR(); // Add SignalR services / Добавляем службы SignalR

// Setting up Redis cache / Настройка кэша Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("Redis"); // Retrieve the connection string / Получаем строку подключения
    options.Configuration = connection; // Set the connection string / Устанавливаем строку подключения
});

// Setting up CORS to allow requests from the React client / Настройка CORS для разрешения запросов с клиента React
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow only this origin / Разрешаем только этот источник
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build(); // Build the application / Создаем приложение

app.MapHub<ChatHub>("/chat"); // Set up routing for ChatHub / Настраиваем маршрутизацию для ChatHub

app.UseCors(); // Enable CORS / Включаем CORS

app.Run(); // Run the application / Запускаем приложение
