using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestWebhook
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        static async Task Main(string[] args)
        {
            // Configura timeout de 30 segundos para todas as requisições
            client.Timeout = TimeSpan.FromSeconds(30);
            
            // Carrega credenciais de variáveis de ambiente
            var username = Environment.GetEnvironmentVariable("SUPERAGENT_USERNAME") ?? throw new ArgumentNullException("SUPERAGENT_USERNAME");
            var password = Environment.GetEnvironmentVariable("SUPERAGENT_PASSWORD") ?? throw new ArgumentNullException("SUPERAGENT_PASSWORD");

            try
            {
                var token = await Authenticate(username, password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var webhookResponse = await ProcessWebhook("https://testeprocesstec.app.n8n.cloud/webhook-test/teste");
                await SendToSuperAgent(webhookResponse, "http://endereco-do-superagente/api/acao");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Erro de autenticação: Credenciais inválidas ou token expirado");
                // Lógica adicional para renovação de token pode ser adicionada aqui
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro HTTP [{ex.StatusCode}]: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }

        private static async Task<string> Authenticate(string username, string password)
        {
            const string loginUrl = "http://endereco-do-superagente/api/login";
            
            var loginData = new { username, password };
            using var loginContent = new StringContent(
                JsonSerializer.Serialize(loginData, jsonOptions), 
                Encoding.UTF8, 
                "application/json"
            );

            using var response = await client.PostAsync(loginUrl, loginContent);
            
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Falha no login: {response.StatusCode}");

            using var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            
            if (!jsonDocument.RootElement.TryGetProperty("token", out var tokenProp))
                throw new InvalidOperationException("Resposta de login inválida: propriedade 'token' não encontrada");

            return tokenProp.GetString() ?? throw new InvalidOperationException("Token nulo recebido");
        }

        private static async Task<string> ProcessWebhook(string webhookUrl)
        {
            using var response = await client.GetAsync(webhookUrl);
            
            response.EnsureSuccessStatusCode();
            
            var responseBody = await response.Content.ReadAsStringAsync();

            // Validação mais robusta do JSON
            try
            {
                JsonDocument.Parse(responseBody);
                return responseBody;
            }
            catch (JsonException)
            {
                throw new InvalidOperationException("Resposta do webhook não é um JSON válido");
            }
        }

        private static async Task SendToSuperAgent(string jsonContent, string superAgentUrl)
        {
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            using var response = await client.PostAsync(superAgentUrl, content);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpRequestException("Não autorizado", null, HttpStatusCode.Unauthorized);

            response.EnsureSuccessStatusCode();
            
            var responseBody = await response.Content.ReadAsStringAsync();
            
// Valida resposta do Super Agente se necessário
// Pode ser adicionada lógica adicional de processamento aqui

Console.WriteLine($"Resposta do Super Agente: {responseBody}");
}
}
}