# TestWebhook - Integração C# com n8n e Super Agente

Este projeto C# demonstra a integração entre um aplicativo, um webhook do n8n e um "Super Agente". Ele envia uma requisição para o n8n, recebe a resposta processada e a encaminha para o Super Agente para executar ações específicas.

## 🚀 Pré-requisitos

*   .NET SDK instalado
*   Ambiente n8n configurado com um webhook ativo
*   "Super Agente" com endpoint para receber requisições

## ⚙️ Configuração

1.  **Clone o repositório**
2.  **Substitua as URLs** no `Program.cs`:
    *   `webhookUrl`: URL do webhook do n8n.
    *   `superAgenteUrl`: URL do endpoint do Super Agente.
3.  **Configure a autenticação (se necessário)** no método `AutenticarSuperAgente`.
4.  **Rode `dotnet restore`** para instalar as dependências.

## ▶️ Execução

1.  Abra o terminal na pasta do projeto.
2.  Execute `dotnet run`.
3.  Verifique a saída no console.

## 🧩 Explicação do Código

O código:

*   Autentica no Super Agente (opcional).
*   Envia um `GET` para o webhook do n8n.
*   Recebe e imprime a resposta do n8n.
*   Encaminha a resposta para o Super Agente via `POST`.
*   Imprime a resposta do Super Agente.
*   Lida com erros de requisição.

## 🤝 Integração com o Super Agente

*   Certifique-se de que o Super Agente tem um endpoint para receber requisições.
*   Configure o código C# para enviar os dados corretos.
*   Implemente a lógica no Super Agente para processar os dados.

## ⚠️ Observações

*   Adapte o código para seus requisitos.
*   Configure o n8n corretamente.
*   Mantenha as credenciais em segurança.
