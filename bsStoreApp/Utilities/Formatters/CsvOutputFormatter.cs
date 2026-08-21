using Microsoft.Net.Http.Headers;
using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace bsStoreApp.Utilities.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (type is null)
            return false;

        return typeof(ExpandoObject).IsAssignableFrom(type)
               || typeof(IEnumerable<ExpandoObject>).IsAssignableFrom(type);
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<ExpandoObject> entities)
        {
            var entitiesList = entities.ToList();

            if (entitiesList.Count > 0)
            {
                var firstEntity = (IDictionary<string, object?>)entitiesList[0];

                // Header
                buffer.AppendLine(
                    string.Join(",", firstEntity.Keys)
                );

                // Rows
                foreach (var entity in entitiesList)
                {
                    var dictionary = (IDictionary<string, object?>)entity;

                    var values = dictionary.Values.Select(value =>
                        EscapeCsvValue(value)
                    );

                    buffer.AppendLine(
                        string.Join(",", values)
                    );
                }
            }
        }
        else if (context.Object is ExpandoObject entity)
        {
            var dictionary = (IDictionary<string, object?>)entity;

            // Header
            buffer.AppendLine(
                string.Join(",", dictionary.Keys)
            );

            // Row
            var values = dictionary.Values.Select(value =>
                EscapeCsvValue(value)
            );

            buffer.AppendLine(
                string.Join(",", values)
            );
        }

        await response.WriteAsync(
            buffer.ToString(),
            selectedEncoding
        );
    }

    private static string EscapeCsvValue(object? value)
    {
        if (value is null)
            return string.Empty;

        var stringValue = value.ToString() ?? string.Empty;

        // CSV'de virgül, çift tırnak veya satır sonu varsa
        // değer çift tırnak içine alınmalıdır.
        if (stringValue.Contains(',') ||
            stringValue.Contains('"') ||
            stringValue.Contains('\n') ||
            stringValue.Contains('\r'))
        {
            stringValue = stringValue.Replace("\"", "\"\"");
            return $"\"{stringValue}\"";
        }

        return stringValue;
    }
}