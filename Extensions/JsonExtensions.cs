namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json;

    static class JsonExtensions
    {
        public static string ToJson<T>(this T value)
        {
            return JsonSerializer.Serialize(value);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
