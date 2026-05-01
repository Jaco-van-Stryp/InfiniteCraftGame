using InfiniteCraftGame.Infrastructure.Data;
using InfiniteCraftGame.Services.AIService;
using InfiniteCraftGame.Services.WordGenerationService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<IWordGenerationService, WordGenerationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
