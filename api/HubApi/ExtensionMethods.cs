using System.Text.Json;

namespace HubApi;

/**
 * Convenience Extension Methods.
 */
public static class ExtensionMethods
{
    /// <summary>
    /// This is a convenience extension method that takes an instantiated type and "dumps" it by JsonSerializing it,
    /// and then outputting it to the console.
    /// Source: https://github.com/dotnet/MQTTnet/blob/master/Samples/Helpers/ObjectExtensions.cs
    /// </summary>
    public static TObject DumpToConsole<TObject>(this TObject @object)
    {
        var output = "NULL";
        if (@object != null)
        {
            output = JsonSerializer.Serialize(@object, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
        return @object;
    }
}