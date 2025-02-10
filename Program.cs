using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestWebhook
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string webhookUrl = "https://testeprocesstec.app.n8n.cloud/webhook-test/teste"; // URL do webhook do n8n
            string superAgenteUrl = "http://endereco-do-superagente/api/acao"; // Substitua pelo endpoint do seu super agente

            try
            {
                // Envia a requisição GET para o webhook do n8n
                using (var client = new HttpClient())
                {
                    // Realiza o login e obtém o token
                    var loginUrl = "http://endereco-do-superagente/api/login";
                    var loginData = new { username = "seuUsuario", password = "suaSenha" };
                    var loginContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
                    var loginResponse = await client.PostAsync(loginUrl, loginContent);
                    loginResponse.EnsureSuccessStatusCode();
                    string token = /* extraia o token da resposta, por exemplo, via JSON */;

                    // Define o token para as próximas requisições
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(webhookUrl).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode(); 

                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine($"Resposta do n8n: {responseBody}");

                    // Agora, encaminha a resposta para o super agente
                    var content = new StringContent(responseBody, Encoding.UTF8, "application/json");
                    var responseAgente = await client.PostAsync(superAgenteUrl, content).ConfigureAwait(false);
                    responseAgente.EnsureSuccessStatusCode();

                    Console.WriteLine($"Super Agente acionado com status: {responseAgente.StatusCode}");
                    string responseAgenteBody = await responseAgente.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine($"Resposta do Super Agente: {responseAgenteBody}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro inesperado: {e.Message}");
            }
        }
    }
}