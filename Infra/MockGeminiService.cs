using Microsoft.SemanticKernel;

namespace PicPayPA.Infra;

public class MockGeminiService
{
    private readonly Kernel _kernel;
    private readonly Random _random = new();

    public MockGeminiService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> ProcessUserRequestAsync(string userInput)
    {
        // Simula o comportamento do Gemini sem fazer chamadas reais
        var clientId = "12345678901"; // CPF fictício
        
        try
        {
            // 1. Buscar dados do cliente
            var accountData = await _kernel.InvokeAsync("MockData", "GetAccountBalance", 
                new KernelArguments { ["clientId"] = clientId });
            
            var creditProfile = await _kernel.InvokeAsync("MockData", "GetCreditProfile", 
                new KernelArguments { ["clientId"] = clientId });

            // 2. Analisar gastos
            var spendingAnalysis = await _kernel.InvokeAsync("AnalyzeSpending", "AnalyzeSpending", 
                new KernelArguments { ["timeframe"] = "últimos 30 dias" });

            // 3. Sugerir investimento baseado no input
            decimal amount = ExtractAmountFromInput(userInput);
            string riskTolerance = ExtractRiskFromInput(userInput);
            
            var investmentSuggestion = await _kernel.InvokeAsync("InvestmentSuggestion", "SuggestInvestmentOption", 
                new KernelArguments 
                { 
                    ["amount"] = amount.ToString(),
                    ["riskTolerance"] = riskTolerance 
                });

            // 4. Gerar resposta personalizada (simulando o Gemini)
            return GeneratePersonalizedResponse(userInput, accountData.ToString(), 
                creditProfile.ToString(), spendingAnalysis.ToString(), investmentSuggestion.ToString());
        }
        catch (Exception ex)
        {
            return $"Ops! Tive um problema para analisar sua situação: {ex.Message}";
        }
    }

    private decimal ExtractAmountFromInput(string input)
    {
        if (input.Contains("R$"))
        {
            var parts = input.Split("R$");
            if (parts.Length > 1)
            {
                var numberPart = new string(parts[1].Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
                if (decimal.TryParse(numberPart.Replace(",", ""), out var amount))
                    return amount;
            }
        }
        return 500; // valor padrão
    }

    private string ExtractRiskFromInput(string input)
    {
        var lowerInput = input.ToLower();
        if (lowerInput.Contains("alto risco") || lowerInput.Contains("arrojado") || lowerInput.Contains("agressivo"))
            return "alto";
        if (lowerInput.Contains("médio") || lowerInput.Contains("moderado") || lowerInput.Contains("equilibrado"))
            return "médio";
        return "baixo";
    }

    private string GeneratePersonalizedResponse(string userInput, string accountData, 
        string creditProfile, string spendingAnalysis, string investmentSuggestion)
    {
        var responses = new[]
        {
            $"Oi! Analisei seu perfil no PicPay e tenho boas notícias! 😊\n\n" +
            $"📊 Sua situação atual: {accountData}\n" +
            $"📈 {creditProfile}\n\n" +
            $"💰 {spendingAnalysis}\n\n" +
            $"🎯 Minha recomendação: {investmentSuggestion}\n\n" +
            $"Que tal começarmos devagar? O PicPay tem produtos seguros para você dar os primeiros passos! 🚀",

            $"Perfeito! Vejo que você quer investir! 💪\n\n" +
            $"Primeiro, deixa eu te contar como estão suas finanças:\n" +
            $"• {accountData}\n" +
            $"• {spendingAnalysis}\n\n" +
            $"Com base no seu perfil: {investmentSuggestion}\n\n" +
            $"💡 Dica: No PicPay você pode começar com apenas R$ 1! Que tal testar?",

            $"Que legal que você quer organizar suas finanças! 🎉\n\n" +
            $"Olha só o que descobri sobre você:\n" +
            $"{creditProfile}\n" +
            $"{spendingAnalysis}\n\n" +
            $"Para seu perfil, sugiro: {investmentSuggestion}\n\n" +
            $"📱 No app do PicPay você encontra tudo isso de forma simples e segura!"
        };

        return responses[_random.Next(responses.Length)];
    }
}