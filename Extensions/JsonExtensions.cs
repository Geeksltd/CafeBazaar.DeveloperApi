namespace CafeBazaar.DeveloperApi
{
    using System.Text.Json;

    static class JsonExtensions
    {
        public static string ToJson<T>(this T value) => JsonSerializer.Serialize(value);

        public static T FromJson<T>(this string value) => JsonSerializer.Deserialize<T>(value);
    }
}
