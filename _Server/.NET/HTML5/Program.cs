using HTML5.Controllers;
using HTML5.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//註冊 WebSocket 的服務
builder.Services.AddSingleton<Ws1Controller>();
builder.Services.AddSingleton<Ws2Controller>();

//註冊 SignalR 的服務
builder.Services.AddSignalR();

//允許跨域請求
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5500") // 指定允許的前端來源
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // 允許憑證
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(); //使用跨域請求
app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(60) });
app.MapHub<ChatHub>("/chatSr");    //設定SignalR的程式進入點
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.Run();
