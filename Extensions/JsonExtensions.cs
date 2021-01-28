namespace CafeBazaar.DeveloperApi
{
    using System.Text.Json;

    static class JsonExtensions
    {
        public static string ToJson<T>(this T value)
        {
            return JsonSerializer.Serialize(value, CreateDefaultOptions());
        }

        public static T FromJson<T>(this string value)
        {
            return JsonSerializer.Deserialize<T>(value, CreateDefaultOptions());
        }

        static JsonSerializerOptions CreateDefaultOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase
            };
        }
    }
}
