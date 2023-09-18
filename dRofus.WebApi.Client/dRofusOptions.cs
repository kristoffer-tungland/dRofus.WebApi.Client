// ReSharper disable InconsistentNaming
namespace dRofus.WebApi.Client;

public record dRofusOptions
{
    public dRofusOptions(string database, string project)
    {
        Db = database;
        Pr = project;
    }

    public string BaseUrl { get; set; } = "https://api-no.drofus.com/";
    public string Server { get; set; } = "db2.nosyko.no";

    public string Db { get; }
    public string Pr { get; }
}