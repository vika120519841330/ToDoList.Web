using Microsoft.Extensions.Options;
using ToDoList.Web.Configs;
using ToDoList.Web.Helpers;

namespace ToDoList.Web.Services;

public class HttpService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ConfigBase configs;

    public HttpService(
        IHttpClientFactory httpClientFactory,
        IOptions<ConfigBase> options)
    {
        this.httpClientFactory = httpClientFactory;
        this.configs = options.Value;
    }

    public string NotifyMessage { get; set; } = string.Empty;

    private HttpClient HttpClient => httpClientFactory.CreateClient("todo_list_http_client");

    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        string url,
        HttpMethod httpMethod,
        TRequest content,
        CancellationToken token = default)
    {
        NotifyMessage = string.Empty;
        var request = url.GetRequestMessage(httpMethod, content);
        var response = await HttpClient.SendRequest(request, token);

        if (response == null)
        {
            NotifyMessage = response?.ReasonPhrase ?? "Операция закончилась с ошибкой !";
            return default;
        }

        return await response.DeserializeTResponse<TResponse>();
    }

    public async Task<TResponse> SendRequestAsync<TResponse>(
        string url,
        HttpMethod httpMethod,
        CancellationToken token = default)
    {
        NotifyMessage = string.Empty;
        var request = url.GetRequestMessage(httpMethod);
        var response = await HttpClient.SendRequest(request, token);

        if (response == null)
        {
            NotifyMessage = response?.ReasonPhrase ?? "Операция закончилась с ошибкой !";
            return default;
        }

        return await response.DeserializeTResponse<TResponse>();
    }
}
