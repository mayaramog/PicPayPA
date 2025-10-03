using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace PicPayPA.Plugins;

public class AnalyzeSpendingPlugin
{
    private readonly Random _random = new();
    [KernelFunction, Description("Analisa os gastos do cliente baseado em transações PicPay")]
    public string AnalyzeSpending([Description("Período a ser analisado (ex: 'últimos 30 dias')")] string timeframe)
    {
        var random = new Random();
        
        // Simula categorias típicas do PicPay
        var gastoDelivery = random.Next(300, 800); // iFood, Uber Eats
        var gastoTransporte = random.Next(100, 300); // Uber, 99
        var gastoPix = random.Next(500, 1500); // Transferências
        var gastoCompras = random.Next(200, 600); // E-commerce
        var gastoRecarga = random.Next(50, 150); // Celular
        
        var totalGastos = gastoDelivery + gastoTransporte + gastoPix + gastoCompras + gastoRecarga;
        var receitaPix = random.Next(2000, 5000); // Recebimentos
        var saldoMedio = random.Next(100, 1000);
        
        var capacidadeInvestimento = Math.Max(0, receitaPix - totalGastos);
        
        return $"Análise PicPay ({timeframe}): " +
               $"Recebimentos Pix: R$ {receitaPix:N0}, " +
               $"Gastos - Delivery: R$ {gastoDelivery:N0}, Transporte: R$ {gastoTransporte:N0}, " +
               $"Transferências: R$ {gastoPix:N0}, Compras: R$ {gastoCompras:N0}. " +
               $"Saldo médio: R$ {saldoMedio:N0}. " +
               $"Capacidade de investimento: R$ {capacidadeInvestimento:N0}.";
    }
}