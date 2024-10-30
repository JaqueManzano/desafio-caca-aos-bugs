using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
            : new Response<string>(null, 400, "Não foi possível realizar o login");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/register", request);
        var errorMessages = new List<string>();
        if (!result.IsSuccessStatusCode)
        {
            var bytes = await result.Content.ReadAsByteArrayAsync();
            var errorContent = Encoding.UTF8.GetString(bytes);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var apiErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, options);
            errorMessages = apiErrorResponse?.GetErrorMessages();

        }

        return result.IsSuccessStatusCode
            ? new Response<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!")
            : new Response<string>(null, 400, string.Join(";", string.Join("; ", errorMessages) ?? "Não foi possível realizar o seu cadastro"));

    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }
}