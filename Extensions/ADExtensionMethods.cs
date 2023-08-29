using System.DirectoryServices;

namespace DemoLDAPApi.Extensions
{
    public static class ADExtensionMethods
    {
        public static string? GetPropertyValue(this SearchResult searchResult, string propertyName)
        {
            var propertyValueCollection = searchResult.Properties[propertyName];

            if (propertyValueCollection.Count <= 0) return null;

            return propertyValueCollection[0]?.ToString();
        }
    }
}