using dRofus.Api.Client;
// ReSharper disable InconsistentNaming

namespace dRofus.WebApi.Client
{
    public class dRofusService
    {
        readonly dRofusClient _client;
        readonly dRofusOptions _options;

        public dRofusService(dRofusClient client, dRofusOptions options)
        {
            _client = client;
            _client.BaseUrl = options.BaseUrl;
            _options = options;
        }

        public dRofusClient GetConfiguredClient()
        {
            return _client;
        }

        public async Task<Occurrence> UpdateOccurrence(Occurrence occurrence, CancellationToken cancellationToken = default)
        {
            var response = await _client.OccurrencesPATCHAsync(occurrence.Id, _options.Db, _options.Pr, occurrence, cancellationToken);
            return response;
        }

        public async Task DeleteOccurrence(int occurrenceId, CancellationToken cancellationToken = default)
        {
            await _client.OccurrencesDELETEAsync(occurrenceId, _options.Db, _options.Pr, cancellationToken);
        }
    }

    public class dRofusOptions
    {
        public dRofusOptions(string database, string project)
        {
            Db = database;
            Pr = project;
        }

        public string BaseUrl { get; set; } = "https://api.drofus.no";

        public string Db { get; }
        public string Pr { get; }
    }
}
