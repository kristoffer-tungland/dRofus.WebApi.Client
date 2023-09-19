// ReSharper disable InconsistentNaming

using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace dRofus.WebApi.Client;

public partial class dRofusClient
{
    [ActivatorUtilitiesConstructor]
    public dRofusClient(HttpClient httpClient, dRofusOptions options) : this(options.BaseUrl, httpClient)
    {
        Options = options;
    }

    partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Error = Error;
    }

    static void Error(object? sender, ErrorEventArgs e)
    {
        if (e.ErrorContext.Error.Message.Contains("expects a non-null value"))
            e.ErrorContext.Handled = true;
    }

    public dRofusOptions Options { get; }

    public void AuthorizeWithReadOnly(string apiKey)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Reference " + apiKey);
    }

    public void SetAuthorization2(string apiKey)
    {
        SetAuthorization("apikey", apiKey);
    }

    public void SetAuthorization(string username, string password)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                       Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
    }
}