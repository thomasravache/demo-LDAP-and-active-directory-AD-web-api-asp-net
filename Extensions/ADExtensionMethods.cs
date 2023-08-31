using System.DirectoryServices;
using System.Runtime.Versioning;

namespace DemoLDAPApi.Extensions
{
    public static class ADExtensionMethods
    {
        [SupportedOSPlatform("windows")]
        public static string? GetPropertyValue(this SearchResult searchResult, string propertyName)
        {
            var propertyValueCollection = searchResult.Properties[propertyName];

            if (propertyValueCollection.Count <= 0) return null;

            return propertyValueCollection[0]?.ToString();
        }
    }
}