using System.Diagnostics;
using System.Text.Json;

namespace Ticketing.Core.Observability.OpenTelemetry.Helpers;

public static class ActivityExtensions
{
  public static void SetActivityCustomProperties(this Activity activity, string serializedContent, List<string> propertiesToTrace)
  {
    if (!string.IsNullOrEmpty(serializedContent) && propertiesToTrace?.Count > 0)
    {
      Dictionary<string, List<string>> values = GetPropertiesToTraceValues(serializedContent, propertiesToTrace);
      foreach (string property in values.Keys)
      {
        string propertyValue = string.Join(",", values[property]);

        activity.SetBaggage(property, propertyValue);
        activity.AddTag(property, propertyValue);
        activity.SetCustomProperty(property, propertyValue);
      }
    }
  }

  public static void SetActivityCustomProperties(this Activity activity, Dictionary<string, string> properties)
  {
    foreach (var property in properties)
    {
      activity.SetBaggage(property.Key, property.Value);
      activity.AddTag(property.Key, property.Value);
      activity.SetCustomProperty(property.Key, property.Value);
    }
  }

  public static void SetActivityCustomProperties(this Activity activity, string property, string value)
  {
    activity.SetBaggage(property, value);
    activity.AddTag(property, value);
    activity.SetCustomProperty(property, value);
  }
  private static Dictionary<string, List<string>> GetPropertiesToTraceValues(string serializedContent, List<string> propertiesToTrace)
  {
    Dictionary<string, List<string>> values = [];

    try
    {
      void GetProperty(JsonElement jDoc)
      {
        var e = jDoc.EnumerateObject();
        foreach (var item in e)
        {
          if (item.Value.ValueKind == JsonValueKind.Object)
          {
            GetProperty(item.Value);
          }
          else if (propertiesToTrace.Any(prop => item.NameEquals(prop)))
          {
            if (!values.TryGetValue(item.Name, out var _))
            {
              values.Add(item.Name, []);
            }
            values[item.Name].Add(item.Value.ToString());
          }
        }
      }

      JsonDocument jDoc = JsonDocument.Parse(serializedContent);
      GetProperty(jDoc.RootElement);
    }
    catch { }
    return values;
  }
}