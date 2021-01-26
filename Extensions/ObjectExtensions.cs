namespace CafeBazaar.DeveloperApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json.Serialization;
    using Olive;

    public static class ObjectExtensions
    {
        private static readonly NamingStrategy _namingStrategy = new SnakeCaseNamingStrategy();

        public static IDictionary<string, string> ToDictionary(this object @this)
        {
            return @this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .ToDictionary(x => _namingStrategy.GetPropertyName(x.Name, false), x => x.GetValue(@this)?.ToString());
        }
    }
}
