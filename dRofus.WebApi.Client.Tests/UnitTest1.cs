using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace dRofus.WebApi.Client.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var json =
            """"
            {
              "id": 55,
              "quantity": 1,
              "existing_quantity": 0,
              "net_quantity": 1,
              "comment": null,
              "classification_number": "+215081=433.001:01-UEZ.007T/003",
              "run_no": "003",
              "occurrence_name": null,
              "to_tender": false,
              "category_id": null,
              "parent_occurrence_id": null,
              "equipment_list_type_id": 1,
              "project_id": "01",
              "room_id": 136,
              "article_id": 16137,
              "product_id": null,
              "article_sub_article_id": null,
              "owner": null,
              "priority": "0",
              "article_sub_category_id": null,
              "agreement_id": null,
              "order_id": null,
              "tender_id": null,
              "responsibility": null
            }
            """";

        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Error = Error};
        var occurrence = JsonConvert.DeserializeObject<Occurrence>(json, settings);
       
        Assert.NotNull(occurrence);
        Assert.Equal(0,occurrence.Article_sub_article_id);
    }

    void Error(object? sender, ErrorEventArgs e)
    {
        if (e.ErrorContext.Error.Message.Contains("expects a non-null value", StringComparison.InvariantCultureIgnoreCase))
            e.ErrorContext.Handled = true;
    }

    IList<JsonConverter> CreateConverters()
    {
        var converters = new List<JsonConverter> { new IntConverter() };

        return converters;
    }
}

internal class IntConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            // If the JSON value is null, return a default value depending on the target type
            if (objectType == typeof(int))
                return 0;
            else if (objectType == typeof(int?))
                return null;
            // Add other target types if needed

            throw new NotSupportedException($"The target type '{objectType}' is not supported by this converter.");
        }

        return serializer.Deserialize(reader, objectType);
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}