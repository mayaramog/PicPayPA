window.initScrollToBottom = () => {
    // Função para rolar para o final das mensagens
};

window.scrollToBottom = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};