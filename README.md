# TestWebhook

Este projeto C# demonstra a integração com um webhook do n8n e um "super agente". Ele envia uma requisição para o n8n, recebe a resposta e, em seguida, encaminha essa resposta para o super agente.

## Pré-requisitos

*   .NET SDK instalado
*   Um ambiente n8n configurado com um webhook ativo
*   Um "super agente" com um endpoint para receber requisições

## Configuração

1.  **Substitua as URLs:**
    *   No arquivo `Program.cs`, substitua `"PELOSEUWEBHOOK"` pela URL do seu webhook do n8n.
    *   Substitua `"http://endereco-do-superagente/api/acao"` pela URL do endpoint do seu super agente.
2.  **Configure as credenciais (se necessário):**
    *   Se o super agente exigir autenticação, adicione o código para realizar o login e obter o token de acesso. Substitua `"seuUsuario"` e `"suaSenha"` pelas credenciais corretas.
3.  **Instale as dependências:**
    *   Rode o comando `dotnet restore` para instalar as dependências do projeto.

## Execução

1.  Abra o terminal na pasta do projeto.
2.  Execute o comando `dotnet run`.

## Explicação do Código

*   O programa envia uma requisição `GET` para o webhook do n8n.
*   Ele recebe a resposta do n8n e imprime o status code e o corpo da resposta no console.
*   Em seguida, ele encaminha a resposta para o super agente, enviando uma requisição `POST` para o endpoint configurado.
*   O programa imprime o status code e o corpo da resposta do super agente no console.
*   O bloco `try-catch` lida com possíveis erros durante a requisição HTTP.

## Integração com o Super Agente

Para integrar com o super agente, você precisa:

1.  Certificar-se de que o super agente tem um endpoint para receber requisições.
2.  Configurar o código C# para enviar os dados corretos para o endpoint do super agente.
3.  Implementar a lógica no super agente para processar os dados recebidos e executar a ação desejada.

## Observações

*   Este é um exemplo básico. Você pode precisar adaptá-lo para atender aos seus requisitos específicos.
*   Certifique-se de que o n8n está configurado corretamente para receber a requisição e processá-la.
*   Mantenha suas credenciais em segurança. Não as compartilhe com ninguém e não as inclua diretamente no seu código.
