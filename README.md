# PicPayPA - Agente Financeiro Pessoal

Agente Financeiro Pessoal (PFA) usando **Microsoft Semantic Kernel** com **Google Gemini API**. O sistema utiliza IA para transformar o atendimento do PicPay do nível Reativo (resolver erros) para o nível Proativo e de Consultoria (ajudar o cliente a planejar e economizar).

🌐 **Agora com Interface Web!** - Chat interativo estilo ChatGPT com design PicPay

## 📸 Preview da Interface

```
┌─────────────────────────────────────────┐
│  🏦 PicPay Agente Financeiro    [🗑️]   │  <- Header verde PicPay
├─────────────────────────────────────────┤
│                                         │
│  🤖 Olá! Sou seu consultor financeiro  │  <- Mensagem da IA
│      Como posso ajudar você hoje?      │
│                                         │
│              Quero investir R$ 500  👤 │  <- Mensagem do usuário
│                                         │
│  🤖 Perfeito! Vou analisar seu perfil. │
│      Baseado nos seus gastos...         │
│      Recomendo o CDB de liquidez...     │
│                                         │
├─────────────────────────────────────────┤
│ [💬 Digite sua mensagem...]     [📤]   │  <- Input área
└─────────────────────────────────────────┘
```

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
| **Blazor Server** | A "Interface". Aplicação web moderna com chat em tempo real |
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

**Configure a API Key no arquivo `appsettings.json`:**
```json
{
  "GeminiApiKey": "sua_api_key_aqui"
}
```

**Execute a aplicação web:**
```bash
# Compile e execute
dotnet build
dotnet run

# Acesse no navegador:
# http://localhost:5000 ou https://localhost:5001
```

**⚠️ SEGURANÇA:**
- A API Key fica no backend (arquivo de configuração)
- Cliente nunca vê ou insere a API Key
- Em produção, use Azure Key Vault ou similar

### 3. Interface Web
- 💬 **Chat interativo** estilo ChatGPT
- 🎨 **Design PicPay** com cores da marca
- 📱 **Responsivo** para qualquer dispositivo
- ⚡ **Tempo real** com Blazor Server
- 🤖 **Indicador de digitação** durante processamento

### 4. Exemplos de interação
- **"Quero começar a investir R$ 500"**
- **"Como posso economizar mais dinheiro?"**  
- **"Tenho R$ 2000 para investir, o que você sugere?"**
- **"Preciso de ajuda para organizar minhas finanças"**

## 📊 Fluxo de Exemplo

**Cliente** (na interface web): "Gostaria de começar a investir, mas não sei por onde começar. Tenho cerca de R$ 500 sobrando no fim do mês."

1. **IA analisa** a intenção e decide usar as ferramentas
2. **Plugin 1** analisa os gastos: "Renda: R$ 4000. Capacidade real: R$ 700"
3. **Plugin 2** sugere investimento: "CDB de liquidez diária é ideal para iniciantes"
4. **IA responde** na interface: "Que ótimo que você quer começar! Analisei seus gastos e você tem uma folga confortável. Como é seu primeiro investimento, sugiro o CDB de Liquidez Diária no PicPay..."

## 🛠️ Requisitos

- .NET 8.0 ou superior
- API Key do Google Gemini
- Navegador web moderno
- Conexão com internet

## 📁 Estrutura do Projeto

```
PicPayPA/
├── Components/                  -> Componentes Blazor
│   ├── Layout/MainLayout.razor -> Layout principal
│   ├── Pages/Home.razor        -> Página de chat
│   └── App.razor               -> Componente raiz
├── Infra/                      -> Infraestrutura
│   ├── SimpleGeminiService.cs  -> Integração com Gemini API
│   ├── ISummarizer.cs          -> Interface para resumos
│   └── JsonMemoryStore.cs      -> Persistência de dados
├── Plugins/                    -> Plugins de negócio
│   ├── AnalyzeSpendingPlugin.cs -> Análise de gastos
│   ├── InvestmentSuggestionPlugin.cs -> Sugestões de investimento
│   └── MockDataPlugin.cs       -> Dados simulados
├── Services/                   -> Serviços da aplicação
│   └── ChatService.cs          -> Gerenciamento do chat
├── Pages/                      -> Páginas Razor
│   └── _Host.cshtml           -> Página host
├── wwwroot/                    -> Arquivos estáticos
│   ├── css/app.css            -> Estilos PicPay
│   └── js/app.js              -> JavaScript auxiliar
├── Program.cs                  -> Configuração da aplicação web
├── appsettings.json           -> Configurações (API Key)
└── README.md                  -> Este arquivo
```

## 🎯 Objetivo

Demonstrar como o **Semantic Kernel** pode orquestrar um agente de IA que:
- Usa **Function Calling** para executar código específico
- Combina **análise de dados** com **inteligência artificial**
- Oferece **consultoria financeira proativa** e personalizada
- Apresenta uma **interface web moderna** para melhor experiência do usuário

## 🌟 Funcionalidades

- ✅ **Chat em tempo real** com IA financeira
- ✅ **Interface responsiva** com design PicPay
- ✅ **Análise inteligente** de gastos e investimentos
- ✅ **Sugestões personalizadas** baseadas no perfil
- ✅ **Dados simulados** para demonstração
- ✅ **Integração completa** com Semantic Kernel

## 🚀 Tecnologias

- **Backend**: .NET 8, ASP.NET Core, Blazor Server
- **Frontend**: HTML5, CSS3, Bootstrap, Font Awesome
- **IA**: Microsoft Semantic Kernel, Google Gemini API
- **Arquitetura**: Function Calling, Plugin System

## 📄 Licença

Projeto educacional/demonstração para o PicPay.