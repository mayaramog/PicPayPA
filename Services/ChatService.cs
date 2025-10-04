using PicPayPA.Infra;

namespace PicPayPA.Services;

public class ChatService
{
    private readonly SimpleGeminiService _geminiService;
    private readonly List<ChatMessage> _messages = new();

    public ChatService(SimpleGeminiService geminiService)
    {
        _geminiService = geminiService;
        
        // Mensagem de boas-vindas
        _messages.Add(new ChatMessage
        {
            IsUser = false,
            Text = "👋 Olá! Sou seu consultor financeiro pessoal do PicPay! Como posso ajudar você hoje?",
            Timestamp = DateTime.Now
        });
    }

    public IReadOnlyList<ChatMessage> Messages => _messages.AsReadOnly();

    public async Task<string> SendMessageAsync(string userMessage)
    {
        // Adiciona mensagem do usuário
        _messages.Add(new ChatMessage
        {
            IsUser = true,
            Text = userMessage,
            Timestamp = DateTime.Now
        });

        try
        {
            // Processa com IA
            var response = await _geminiService.ProcessUserRequestAsync(userMessage);
            
            // Adiciona resposta da IA
            _messages.Add(new ChatMessage
            {
                IsUser = false,
                Text = response,
                Timestamp = DateTime.Now
            });

            return response;
        }
        catch (Exception ex)
        {
            var errorMessage = $"❌ Desculpe, ocorreu um erro: {ex.Message}";
            
            _messages.Add(new ChatMessage
            {
                IsUser = false,
                Text = errorMessage,
                Timestamp = DateTime.Now
            });

            return errorMessage;
        }
    }

    public void ClearChat()
    {
        _messages.Clear();
        _messages.Add(new ChatMessage
        {
            IsUser = false,
            Text = "👋 Olá! Sou seu consultor financeiro pessoal do PicPay! Como posso ajudar você hoje?",
            Timestamp = DateTime.Now
        });
    }
}

public class ChatMessage
{
    public bool IsUser { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}