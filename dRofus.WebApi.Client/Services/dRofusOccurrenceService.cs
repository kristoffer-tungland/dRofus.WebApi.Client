// ReSharper disable InconsistentNaming

using MakeInterface.Contracts.Attributes;

namespace dRofus.WebApi.Client.Services;

[GenerateInterface]
public class dRofusOccurrenceService : IdRofusOccurrenceService
{
    readonly dRofusClient _client;

    public dRofusOccurrenceService(dRofusClient client)
    {
        _client = client;
    }

    public Task<ICollection<PropertyMeta>> GetOptions(int? depth = 1, CancellationToken cancellationToken = default)
    {
        return _client.OccurrencesAllOPTIONSAsync(depth, _client.Options.Db, _client.Options.Pr, cancellationToken);
    }

    public async Task<Occurrence> GetOccurrence(int occurrenceId, IEnumerable<string>? properties = default, CancellationToken cancellationToken = default)
    {
        var select = properties ?? null;

        var response = await _client.OccurrencesGETAsync(occurrenceId, select, _client.Options.Db, _client.Options.Pr, cancellationToken);
        return response;
    }

    public Task<Occurrence> UpdateOccurrence(Occurrence occurrence, CancellationToken cancellationToken = default)
    {
        return _client.OccurrencesPATCHAsync(occurrence.Id, _client.Options.Db, _client.Options.Pr, occurrence, cancellationToken);
    }

    public Task DeleteOccurrence(int occurrenceId, CancellationToken cancellationToken = default)
    {
        return _client.OccurrencesDELETEAsync(occurrenceId, _client.Options.Db, _client.Options.Pr, cancellationToken);
    }
}

[GenerateInterface]
public class dRofusUserService : IdRofusUserService
{
    readonly dRofusClient _client;

    public dRofusUserService(dRofusClient client)
    {
        _client = client;
    }

    public Task Login(string username, string password, CancellationToken cancellationToken = default)
    {
        _client.SetAuthorization(username, password);

        return IsLoggedIn(cancellationToken);
    }

    public Task Login(string apiKey, CancellationToken cancellationToken = default)
    {
        _client.AuthorizeWithReadOnly(apiKey);

        return IsLoggedIn(cancellationToken);
    }

    public async Task<bool> IsLoggedIn(CancellationToken cancellationToken)
    {
        try
        {
            var project = await _client.ProjectsGETAsync(null, _client.Options.Db, _client.Options.Pr, cancellationToken);
            return project is not null;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}