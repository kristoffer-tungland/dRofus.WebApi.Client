// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json;
using dRofus.WebApi.Client;
using dRofus.WebApi.Client.Extensions;
using dRofus.WebApi.Client.Services;
using Meziantou.Framework.Win32;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Ask for database 
Console.WriteLine("Enter database:");
var database = Console.ReadLine() ?? throw new Exception("Database is required");

// Ask for project
Console.WriteLine("Enter project or press enter to use default:");
var project = Console.ReadLine();

if (string.IsNullOrWhiteSpace(project))
    project = "01";

var options = new dRofusOptions(database, project);

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddDRofusServices(options);

var host = builder.Build();

var userService = host.Services.GetRequiredService<IdRofusUserService>();

// Ask if we should use username/password or api key
Console.WriteLine("Use username/password (y/n), if not api key is used?");
var useUsernamePassword = Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) ?? false;

if (useUsernamePassword)
{
    // Ask for username
    Console.WriteLine("Enter username:");
    var username = Console.ReadLine() ?? throw new Exception("Username is required");

    // Ask for password
    var credential = CredentialManager.PromptForCredentialsConsole($"drofus://{username}@{options.Server}", userName: username);

    await userService.Login(username, credential.Password, CancellationToken.None);
}
else
{
    // Ask for api key
    Console.WriteLine("Enter api key:");
    var apiKey = Console.ReadLine() ?? throw new Exception("Api key is required");

    await userService.Login(apiKey, CancellationToken.None);
}


var occurrenceService = host.Services.GetRequiredService<IdRofusOccurrenceService>();

// Print out properties
var occurrenceProperties = await occurrenceService.GetOptions();

Console.WriteLine("Available properties:");
foreach (var property in occurrenceProperties)
{
    Console.WriteLine($"{property.Name} ({property.PropertyGroup})");
}

// Ask for occurrence id
Console.WriteLine("Enter occurrence id:");
var occurrenceId = int.Parse(Console.ReadLine() ?? throw new Exception("Occurrence id is required"));

// Ask for properties or null if escape
Console.WriteLine("Enter properties (comma separated) or press enter to get all properties:");
var properties = Console.ReadLine()?.Split(',').ToList();

if (properties is null || properties.All(string.IsNullOrWhiteSpace)) 
    properties = null;

var occurrence = await occurrenceService.GetOccurrence(occurrenceId, properties, CancellationToken.None);

Console.WriteLine($"Occurrence {occurrenceId} has the following properties:");

var json = JsonSerializer.Serialize(occurrence, new JsonSerializerOptions { WriteIndented = true });

Console.WriteLine(json);

Console.WriteLine(occurrence.Classification_number);

Console.WriteLine("Press any key to exit");
