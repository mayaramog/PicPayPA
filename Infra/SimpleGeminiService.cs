using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;

namespace PicPayPA.Infra;

public class SimpleGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Kernel _kernel;

    public SimpleGeminiService(string apiKey, Kernel kernel)
    {
        _apiKey = apiKey;
        _kernel = kernel;
        _httpClient = new HttpClient();
    }

    public async Task<string> ProcessUserRequestAsync(string userInput)
    {
        try
        {
            // Usar a IA para classificar se é relacionado a finanças ou ajuda geral
            var classificationPrompt = $@"
Classifique esta pergunta do usuário em uma dessas categorias:
1. FINANCEIRO - APENAS sobre: dinheiro, investimentos, gastos, economia, produtos bancários, PIX, cartão, saldo, conta, empréstimo, financiamento, juros, renda, salário
2. AJUDA - APENAS: cumprimentos (oi, olá), perguntando especificamente o que o consultor financeiro pode fazer, como o sistema funciona
3. OUTRO - QUALQUER outro assunto: relacionamentos, saúde, esportes, política, receitas, trabalho, etc.

Pergunta: '{userInput}'

Responda apenas com: FINANCEIRO, AJUDA ou OUTRO";

            var classificationRequest = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = classificationPrompt }
                        }
                    }
                }
            };

            var classificationJson = JsonSerializer.Serialize(classificationRequest);
            var classificationContent = new StringContent(classificationJson, Encoding.UTF8, "application/json");

            var classificationResponse = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}", 
                classificationContent);

            var classificationResult = await classificationResponse.Content.ReadAsStringAsync();
            var classification = "FINANCEIRO"; // Default

            if (classificationResponse.IsSuccessStatusCode)
            {
                var classificationResultData = JsonSerializer.Deserialize<JsonElement>(classificationResult);
                if (classificationResultData.TryGetProperty("candidates", out var classificationCandidates) && classificationCandidates.GetArrayLength() > 0)
                {
                    var classificationCandidate = classificationCandidates[0];
                    if (classificationCandidate.TryGetProperty("content", out var contentProp) &&
                        contentProp.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        var part = parts[0];
                        if (part.TryGetProperty("text", out var textElement))
                        {
                            classification = textElement.GetString()?.Trim().ToUpper() ?? "FINANCEIRO";
                        }
                    }
                }
            }

            // Se não for financeiro nem ajuda, rejeitar educadamente
            if (classification == "OUTRO")
            {
                return "Desculpe, sou um consultor financeiro do PicPay e só posso ajudar com questões relacionadas a finanças, investimentos, gastos e produtos financeiros. Como posso ajudá-lo com suas finanças hoje? 💰";
            }

            // 1. Executar plugins automaticamente
            var clientId = "12345678901";
            var accountData = await _kernel.InvokeAsync("MockData", "GetAccountBalance", 
                new KernelArguments { ["clientId"] = clientId });
            
            var spendingAnalysis = await _kernel.InvokeAsync("AnalyzeSpending", "AnalyzeSpending", 
                new KernelArguments { ["timeframe"] = "últimos 30 dias" });

            // 2. Extrair valor do input
            decimal amount = 500;
            if (userInput.Contains("R$"))
            {
                var parts = userInput.Split("R$");
                if (parts.Length > 1)
                {
                    var numberPart = new string(parts[1].Where(c => char.IsDigit(c)).ToArray());
                    if (decimal.TryParse(numberPart, out var parsedAmount))
                        amount = parsedAmount;
                }
            }

            var investmentSuggestion = await _kernel.InvokeAsync("InvestmentSuggestion", "SuggestInvestmentOption", 
                new KernelArguments 
                { 
                    ["amount"] = amount.ToString(),
                    ["riskTolerance"] = "baixo" 
                });

            // 3. Prompt para o Gemini
            var prompt = $@"
Você é um consultor financeiro do PicPay chamado PoupAí. IMPORTANTE: Responda APENAS sobre tópicos financeiros (investimentos, gastos, economia, produtos bancários).

Cliente perguntou: {userInput}

Dados do cliente:
- Conta: {accountData}
- Análise de gastos: {spendingAnalysis}  
- Sugestão de investimento: {investmentSuggestion}

Responda como um consultor experiente, usando esses dados para dar conselhos personalizados sobre finanças.";

            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}", 
                content);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                return $"Erro da API: {response.StatusCode} - {responseContent}";
            }
            
            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
            
            if (result.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
            {
                var candidate = candidates[0];
                if (candidate.TryGetProperty("content", out var contentProp) &&
                    contentProp.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                {
                    var part = parts[0];
                    if (part.TryGetProperty("text", out var textElement))
                    {
                        return textElement.GetString() ?? "Resposta vazia.";
                    }
                }
            }
            
            return $"Resposta inesperada da API: {responseContent}";
        }
        catch (Exception ex)
        {
            return $"Erro detalhado: {ex.GetType().Name} - {ex.Message}";
        }
    }
}