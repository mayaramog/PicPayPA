using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace PicPayPA.Plugins;

public class InvestmentSuggestionPlugin
{
    [KernelFunction, Description("Sugere opções de investimento baseado no valor e perfil de risco")]
    public string SuggestInvestmentOption(
        [Description("Valor que o cliente deseja investir")] decimal amount,
        [Description("Nível de risco: baixo, médio ou alto")] string riskTolerance = "baixo")
    {
        return riskTolerance.ToLower() switch
        {
            "baixo" when amount <= 1000 => 
                "Recomendo o CDB de liquidez diária do PicPay. É seguro, tem garantia do FGC até R$ 250.000 e você pode resgatar a qualquer momento. Rendimento atual: 100% do CDI.",
            
            "baixo" when amount > 1000 => 
                "Para esse valor, sugiro dividir: 70% no CDB de liquidez diária e 30% no Tesouro Selic. Ambos são de baixo risco e oferecem boa liquidez.",
            
            "médio" when amount <= 2000 => 
                "Considere o CDB com prazo de 1 ano (120% do CDI) ou fundos de renda fixa. Maior rentabilidade com risco controlado.",
            
            "médio" when amount > 2000 => 
                "Diversifique: 50% em CDB prefixado, 30% em fundos multimercado e 20% em ações via ETF. Equilibra risco e retorno.",
            
            "alto" => 
                "Para perfil arrojado: 40% em ações individuais, 30% em fundos de ações, 20% em criptomoedas e 10% em reserva (CDB). Potencial de maior retorno com volatilidade.",
            
            _ => "Recomendo começar com o CDB de liquidez diária para conhecer o mercado de investimentos."
        };
    }
}