namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json;

    static class JsonExtensions
    {
        public static string ToJson<T>(this T value, NamingPolicy policy = NamingPolicy.CamelCase)
        {
            return JsonSerializer.Serialize(value, CreateDefaultOptions(policy));
        }

        public static T FromJson<T>(this string value, NamingPolicy policy = NamingPolicy.CamelCase)
        {
            return JsonSerializer.Deserialize<T>(value, CreateDefaultOptions(policy));
        }

        static JsonSerializerOptions CreateDefaultOptions(NamingPolicy policy)
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = GetNamingPolicy(policy)
            };
        }

        static JsonNamingPolicy GetNamingPolicy(NamingPolicy policy)
        {
            if (policy == NamingPolicy.SnakeCase)
                return SnakeCaseNamingPolicy.SnakeCase;

            return JsonNamingPolicy.CamelCase;
        }
    }
}
