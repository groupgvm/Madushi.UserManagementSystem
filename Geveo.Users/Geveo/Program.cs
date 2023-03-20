using Geveo.Users.Common;
using Geveo.Users.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ILoggerService, LoggerService>();
builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add various middleware to the app pipeline

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseLoggingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Run the app

app.Run();
