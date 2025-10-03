using Microsoft.SemanticKernel;
using PicPayPA.Infra;
using PicPayPA.Plugins;
using System.Text.Json;

// API Key configurada no backend (cliente não vê)
var config = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText("appsettings.json"));
var apiKey = config.GetProperty("GeminiApiKey").GetString();

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("❌ Erro: API Key não encontrada no appsettings.json");
    return;
}

Console.WriteLine("🔑 API Key configurada no backend (cliente não tem acesso)");

var kernel = Kernel.CreateBuilder().Build();

// Plugins do Agente Financeiro Pessoal (PFA)
var analyzeSpending = new AnalyzeSpendingPlugin();
var investmentSuggestion = new InvestmentSuggestionPlugin();
var mockData = new MockDataPlugin();

// Registrando plugins no Kernel
kernel.ImportPluginFromObject(analyzeSpending, "AnalyzeSpending");
kernel.ImportPluginFromObject(investmentSuggestion, "InvestmentSuggestion");
kernel.ImportPluginFromObject(mockData, "MockData");

// Serviço de chat com Gemini (usando dados mock)
var geminiChat = new SimpleGeminiService(apiKey, kernel);

Console.WriteLine("=== PicPayPA - Agente Financeiro Pessoal ===");
Console.WriteLine("💰 Seu consultor financeiro pessoal com IA!");
Console.WriteLine("Exemplos:");
Console.WriteLine("- 'Quero começar a investir R$ 500'");
Console.WriteLine("- 'Como posso economizar mais dinheiro?'");
Console.WriteLine("- 'Tenho R$ 2000 para investir, o que você sugere?'");
Console.WriteLine("- Digite 'sair' para encerrar");
Console.WriteLine("----------------------------------------");

while (true)
{
    Console.Write("💬 Você: ");
    var input = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input)) continue;
    if (input.Equals("sair", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    Console.Write("🤖 PFA: ");
    try
    {
        var response = await geminiChat.ProcessUserRequestAsync(input);
        Console.WriteLine(response);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro: {ex.Message}");
    }
    
    Console.WriteLine();
}