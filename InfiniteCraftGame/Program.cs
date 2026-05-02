using InfiniteCraftGame.Features.CombineWord;
using InfiniteCraftGame.Features.GetAllWords;
using InfiniteCraftGame.Infrastructure.Data;
using InfiniteCraftGame.Services.AIService;
using InfiniteCraftGame.Services.WordGenerationService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddOpenApi();
builder.Services.AddScoped<IWordGenerationService, WordGenerationService>();

builder.Services.AddScoped<IAiService, AiService>();
builder
    .Services.AddOptions<AiServiceOptions>()
    .BindConfiguration("AiService")
    .ValidateDataAnnotations()
    .ValidateOnStart();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAnyOrigin");
app.MapCombineWordEndpoint();
app.MapGetAllWordsEndpoint();
app.UseHttpsRedirection();

app.Run();
