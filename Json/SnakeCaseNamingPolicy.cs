namespace CafeBazaar.DeveloperApi
{
    using Olive;
    using System.Text.Json;

    class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy SnakeCase { get; } = new SnakeCaseNamingPolicy();

        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}
