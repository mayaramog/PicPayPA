using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace PicPayPA.Plugins;

public class MockDataPlugin
{
    private readonly Random _random = new();
    private static readonly string[] ClientNames = { "Ana Silva", "Carlos Santos", "Maria Oliveira", "João Pereira", "Fernanda Costa" };
    private static string? _selectedClientName;
    
    private string GetClientName()
    {
        if (_selectedClientName == null)
        {
            _selectedClientName = ClientNames[_random.Next(ClientNames.Length)];
        }
        return _selectedClientName;
    }
    
    [KernelFunction, Description("Simula dados de transações do cliente PicPay")]
    public string GetTransactionHistory([Description("CPF ou ID do cliente")] string clientId)
    {
        var clientName = GetClientName();
        var transactions = new[]
        {
            "Pix enviado - Padaria do João: R$ 15,50",
            "Compra - iFood: R$ 45,80", 
            "Pix recebido - Maria Silva: R$ 200,00",
            "Compra - Uber: R$ 12,30",
            "Pix enviado - Conta de luz: R$ 180,00",
            "Compra - Mercado Livre: R$ 89,90"
        };
        
        var selectedTransactions = transactions.OrderBy(x => _random.Next()).Take(4);
        return $"Últimas transações de {clientName}: {string.Join(", ", selectedTransactions)}";
    }

    [KernelFunction, Description("Simula score de crédito e perfil financeiro do cliente")]
    public string GetCreditProfile([Description("CPF ou ID do cliente")] string clientId)
    {
        var clientName = GetClientName();
        var scores = new[] { 650, 720, 580, 800, 690, 750 };
        var score = scores[_random.Next(scores.Length)];
        
        var profile = score switch
        {
            >= 750 => "Excelente - Acesso a todos os produtos",
            >= 650 => "Bom - Elegível para a maioria dos investimentos", 
            >= 600 => "Regular - Produtos básicos disponíveis",
            _ => "Baixo - Foque em organizar as finanças primeiro"
        };
        
        return $"{clientName}: Score {score} - {profile}";
    }

    [KernelFunction, Description("Simula saldo e produtos PicPay do cliente")]
    public string GetAccountBalance([Description("CPF ou ID do cliente")] string clientId)
    {
        var clientName = GetClientName();
        var balance = _random.Next(50, 5000);
        var hasCard = _random.Next(0, 2) == 1;
        var hasCashback = _random.Next(0, 2) == 1;
        
        return $"{clientName}: Saldo R$ {balance:N2}, " +
               $"Cartão PicPay: {(hasCard ? "Ativo" : "Não possui")}, " +
               $"Cashback acumulado: {(hasCashback ? $"R$ {_random.Next(5, 50)}" : "R$ 0")}";
    }
}