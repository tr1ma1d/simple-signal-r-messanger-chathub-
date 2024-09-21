using ASimpleSingleR.API.Hubs;

var builder = WebApplication.CreateBuilder(args); // Create a new web application instance / ������� ����� ��������� ���-����������

builder.Services.AddSignalR(); // Add SignalR services / ��������� ������ SignalR

// Setting up Redis cache / ��������� ���� Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("Redis"); // Retrieve the connection string / �������� ������ �����������
    options.Configuration = connection; // Set the connection string / ������������� ������ �����������
});

// Setting up CORS to allow requests from the React client / ��������� CORS ��� ���������� �������� � ������� React
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow only this origin / ��������� ������ ���� ��������
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build(); // Build the application / ������� ����������

app.MapHub<ChatHub>("/chat"); // Set up routing for ChatHub / ����������� ������������� ��� ChatHub

app.UseCors(); // Enable CORS / �������� CORS

app.Run(); // Run the application / ��������� ����������
