using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.SemanticKernel;
using PicPayPA.Infra;
using PicPayPA.Plugins;
using PicPayPA.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure API Key
var apiKey = builder.Configuration["GeminiApiKey"];

if (string.IsNullOrEmpty(apiKey))
{
    try
    {
        var config = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText("appsettings.json"));
        apiKey = config.GetProperty("GeminiApiKey").GetString();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao ler appsettings.json: {ex.Message}");
    }
}

if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("API Key do Gemini não encontrada. Configure no appsettings.json");
}

// Configure Semantic Kernel
var kernel = Kernel.CreateBuilder().Build();

// Register plugins
var analyzeSpending = new AnalyzeSpendingPlugin();
var investmentSuggestion = new InvestmentSuggestionPlugin();
var mockData = new MockDataPlugin();

kernel.ImportPluginFromObject(analyzeSpending, "AnalyzeSpending");
kernel.ImportPluginFromObject(investmentSuggestion, "InvestmentSuggestion");
kernel.ImportPluginFromObject(mockData, "MockData");

// Register services
builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton(provider => new SimpleGeminiService(apiKey, kernel));
builder.Services.AddSingleton<ChatService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Console.WriteLine("🚀 PicPayPA Web Interface iniciado!");
Console.WriteLine("💻 Acesse: https://localhost:5001 ou http://localhost:5000");

app.Run();