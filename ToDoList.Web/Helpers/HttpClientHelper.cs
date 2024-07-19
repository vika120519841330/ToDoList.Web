using System.Net.Mime;
using System.Net;
using System.Runtime;
using Serilog;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NewtonSerializer = Newtonsoft.Json.JsonSerializer;
using SystemSerializer = System.Text.Json.JsonSerializer;

namespace ToDoList.Web.Helpers;

public static class HttpClientHelper
{
    private static JsonSerializerOptions SerializersOptions
    => new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = false,
        IncludeFields = false,
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = false,
        MaxDepth = 5,
        DefaultBufferSize = 32 * 1024 * 1024, // 16Kb default - up to 32Mb
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
    };

    private const string contentType = "application/json";

    public static HttpRequestMessage GetRequestMessage(this string uri, HttpMethod httpMethod)
        => new HttpRequestMessage(httpMethod, uri);

    public static HttpRequestMessage GetRequestMessage<TRequest>(this string uri, HttpMethod httpMethod, TRequest content)
    {
        var request = new HttpRequestMessage(httpMethod, uri);

        request.Content = JsonContent.Create(
            inputValue: content,
            inputType: typeof(TRequest),
            mediaType: new MediaTypeHeaderValue(contentType),
            options: SerializersOptions);

        return request;
    }

    public static async Task<HttpResponseMessage> SendRequest(this HttpClient httpClient, HttpRequestMessage request, CancellationToken token = default)
    {
        var details = $"при отправке http-запроса по адресу «{request?.RequestUri?.ToString() ?? string.Empty}» ";
        httpClient.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

        try
        {
            var response = await httpClient.SendAsync(
                request: request,
                completionOption: HttpCompletionOption.ResponseContentRead,
                cancellationToken: token).ConfigureAwait(false);

            if (response == null)
            {
                Log.Error($"Пустой ответ. Ошибка на уровне сервера {details}. ");
            }

            if (!response.IsSuccessStatusCode)
            {
                details += $"{nameof(response.StatusCode)}: {response.StatusCode}, {nameof(response.ReasonPhrase)}: {response.ReasonPhrase}";
                Log.Error($"Ошибка на уровне сервера {details}. ");
                return default;
            }

            return response;
        }
        catch (Exception exc)
        {
            Log.Error($"Исключительная ситуация {details}. " +
                      $"Подробности: {exc.Message ?? exc.InnerException?.Message ?? string.Empty}");
            return default;
        }
    }

    public static async Task<TResponse> DeserializeTResponse<TResponse>(this HttpResponseMessage response)
    {
        var details = $"после отправки http-запроса по адресу «{response?.RequestMessage?.RequestUri?.ToString() ?? string.Empty}». ";

        if ((response?.Content?.Headers?.ContentLength ?? 0) == 0)
        {
            Log.Error($"Пустой контент после запроса по адресу «{response?.RequestMessage?.RequestUri?.ToString() ?? string.Empty}». ");
            return default;
        }

        TResponse result;
        try
        {
            using var data = await response?.Content.ReadAsStreamAsync();
            if ((data?.Length ?? 0) == 0)
            {
                Log.Error($"Ошибка при выполнении операции по чтению контента из ответа сервера (контент пустой) {details}");
                return default;
            }

            using var streamReader = new StreamReader(data);
            using var reader = new JsonTextReader(streamReader);
            reader.SupportMultipleContent = true;
            var serializer = new NewtonSerializer();
            return serializer.Deserialize<TResponse>(reader);
        }
        catch (OutOfMemoryException exc)
        {
            GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Log.Information($"Запуск принудительной сборки мусора по причине: {exc.Message ?? exc.InnerException?.Message ?? string.Empty}");

            // Запуск считывания после сборки мусора на всех поколениях кучи
            var responseBody = await response?.Content?.ReadAsByteArrayAsync();
            if ((responseBody?.Length ?? 0) == 0)
            {
                Log.Error($"Ошибка при выполнении операции по чтению контента из ответа сервера (контент пустой) {details} ");
                throw;
            }

            return responseBody != default ? SystemSerializer.Deserialize<TResponse>(responseBody, SerializersOptions) : default;
        }
        catch (Exception exc)
        {
            Log.Error($"Исключительная ситуация при выполнении операции по чтению и десериализации контента " +
                      $"(длина: {response?.Content?.Headers?.ContentLength ?? 0} байт) {details} " +
                      $"Подробности: {exc.Message ?? exc.InnerException?.Message ?? string.Empty} ");
            throw;
        }
    }
}
