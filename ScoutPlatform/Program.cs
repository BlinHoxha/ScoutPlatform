using ScoutPlatform.Application;
using ScoutPlatform.Infrastructure;
using ScoutPlatform.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

var connectionString = app.Configuration.GetConnectionString("Default") ?? string.Empty;
if (!connectionString.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase))
{
    await app.Services.ApplyMigrationsAsync();
}

app.Run();
