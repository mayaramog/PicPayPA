# PicPayPA - Agente Financeiro Pessoal

Agente Financeiro Pessoal (PFA) usando **Microsoft Semantic Kernel** com **Google Gemini API**. O sistema utiliza IA para transformar o atendimento do PicPay do nível Reativo (resolver erros) para o nível Proativo e de Consultoria (ajudar o cliente a planejar e economizar).

## 💡 A Ideia: Agente Financeiro Pessoal (PFA)

O PFA faz a ponte entre os dados transacionais do cliente e metas financeiras, ajudando a economizar e investir melhor através de:

- **Análise proativa de gastos**: Entende o perfil financeiro do cliente
- **Sugestões personalizadas de investimento**: Recomenda produtos adequados ao perfil de risco
- **Consultoria financeira inteligente**: Usa IA para dar conselhos educativos e práticos

## 🏗️ Arquitetura

| Componente | Função |
|------------|--------|
| **Gemini API** | O "Consultor". Recebe a intenção do cliente e decide quais ferramentas usar |
| **Semantic Kernel** | O "Orquestrador". Faz o Function Calling entre a IA e os plugins |
| **Plugins** | O "Backend". Simulam análise de dados e sugestões financeiras |

## 🔧 Plugins Disponíveis

### 1. AnalyzeSpendingPlugin
Simula a análise do histórico financeiro do cliente:
- **Parâmetro**: `timeframe` (período a ser analisado)
- **Retorno**: Detalhes de renda, gastos por categoria e capacidade de investimento

### 2. InvestmentSuggestionPlugin  
Sugere investimentos baseados no perfil do cliente:
- **Parâmetros**: `amount` (valor a investir), `riskTolerance` (baixo/médio/alto)
- **Retorno**: Recomendações personalizadas de produtos financeiros

## 🚀 Como usar

### 1. Obter API Key do Gemini
1. Acesse [Google AI Studio](https://ai.google.dev/)
2. Crie uma conta e gere sua API Key
3. Guarde a chave com segurança

### 2. Configurar e Executar

**Opção 1 - Script Automático (Windows):**
```bash
# Execute o script de configuração
setup.bat
```

**Opção 2 - Manual:**
```bash
# Configure a API Key (substitua pela sua chave)
set GEMINI_API_KEY=sua_chave_aqui

# Compile e execute
dotnet build
dotnet run
```

**⚠️ SEGURANÇA:**
- A API Key fica no backend (variável de ambiente)
- Cliente nunca vê ou insere a API Key
- Em produção, use Azure Key Vault ou similar

### 3. Exemplos de interação
- **"Quero começar a investir R$ 500"**
- **"Como posso economizar mais dinheiro?"**  
- **"Tenho R$ 2000 para investir, o que você sugere?"**
- **"Preciso de ajuda para organizar minhas finanças"**

## 📊 Fluxo de Exemplo

**Cliente**: "Gostaria de começar a investir, mas não sei por onde começar. Tenho cerca de R$ 500 sobrando no fim do mês."

1. **IA analisa** a intenção e decide usar as ferramentas
2. **Plugin 1** analisa os gastos: "Renda: R$ 4000. Capacidade real: R$ 700"
3. **Plugin 2** sugere investimento: "CDB de liquidez diária é ideal para iniciantes"
4. **IA responde**: "Que ótimo que você quer começar! Analisei seus gastos e você tem uma folga confortável. Como é seu primeiro investimento, sugiro o CDB de Liquidez Diária no PicPay..."

## 🛠️ Requisitos

- .NET 8.0 ou superior
- API Key do Google Gemini
- Conexão com internet

## 📁 Estrutura do Projeto

```
PicPayPA/
├── Infra/
│   ├── GeminiChatService.cs     -> Integração com Gemini API
│   ├── ISummarizer.cs           -> Interface para resumos
│   └── JsonMemoryStore.cs       -> Persistência de dados
├── Plugins/
│   ├── AnalyzeSpendingPlugin.cs -> Análise de gastos
│   └── InvestmentSuggestionPlugin.cs -> Sugestões de investimento
├── Program.cs                   -> Interface CLI do PFA
└── README.md                    -> Este arquivo
```

## 🎯 Objetivo

Demonstrar como o **Semantic Kernel** pode orquestrar um agente de IA que:
- Usa **Function Calling** para executar código específico
- Combina **análise de dados** com **inteligência artificial**
- Oferece **consultoria financeira proativa** e personalizada

## 📄 Licença

Projeto educacional/demonstração para o PicPay.