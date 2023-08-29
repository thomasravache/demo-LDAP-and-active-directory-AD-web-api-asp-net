using System.DirectoryServices;
using System.Dynamic;
using DemoLDAPApi.Extensions;

namespace DemoLDAPApi.Services
{
    public class LDAPService
    {
        public string GetCurrentDomainPath()
        {
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE");

            var connectionString = $"LDAP://{directoryEntry.Properties["defaultNamingContext"][0]?.ToString()}";

            return connectionString;
        }

        public List<string> GetAllUsers()
        {
            DirectoryEntry directoryEntry = new(GetCurrentDomainPath());
            DirectorySearcher directorySearcher = new(directoryEntry);

            directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person))";
            
            SearchResultCollection? results = directorySearcher.FindAll();

            List<string> users = new();

            foreach(SearchResult searchResult in results)
            {
                var userName = searchResult.GetPropertyValue("name");

                if (userName is null) continue;

                users.Add(userName);
            }

            return users.OrderBy(x => x).ToList();
        }

        public List<dynamic> GetAdditionalUserInfo()
        {
            DirectoryEntry directoryEntry = new(GetCurrentDomainPath());
            DirectorySearcher directorySearcher = _BuildUserSearcher(directoryEntry);

            directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person))";

            SearchResultCollection? results = directorySearcher.FindAll();
            var users = _BuildUsersList(results);

            return users.OrderBy(x => x.Name).ToList();
        }

        public List<dynamic> SearchForUsers(string userName)
        {
            DirectoryEntry directoryEntry = new(GetCurrentDomainPath());
            DirectorySearcher directorySearcher = _BuildUserSearcher(directoryEntry);

            directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person)(name=" + userName + "*))"; // asterisco deixa o filtro como se fosse o LIKE do SQL Server

            var results = directorySearcher.FindAll();
            var users = _BuildUsersList(results);

            return users.OrderBy(x => x.Name).ToList();
        }

        public dynamic? GetAUser(string userName)
        {
            DirectoryEntry directoryEntry = new(GetCurrentDomainPath());
            DirectorySearcher directorySearcher = _BuildUserSearcher(directoryEntry);

            directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person)(name=" + userName + "))";

            var searchResult = directorySearcher.FindOne();

            if (searchResult is null) return new { Message = "Não encontrado" };

            var user = new
            {
                Name = searchResult.GetPropertyValue("name"),
                Mail = searchResult.GetPropertyValue("mail"),
                Givenname = searchResult.GetPropertyValue("givenname"),
                Surname = searchResult.GetPropertyValue("sn"),
                UserPrincipalName = searchResult.GetPropertyValue("userPrincipalName"),
                DistinguishedName = searchResult.GetPropertyValue("distinguishedName"),
            };

            return user;
        }

        private DirectorySearcher _BuildUserSearcher(DirectoryEntry directoryEntry)
        {
            DirectorySearcher directorySearcher = new(directoryEntry);

            directorySearcher.PropertiesToLoad.Add("name"); // Nome Completo
            directorySearcher.PropertiesToLoad.Add("mail"); // Email
            directorySearcher.PropertiesToLoad.Add("givenname"); // Primeiro nome
            directorySearcher.PropertiesToLoad.Add("sn"); // Surname -> Sobrenome
            directorySearcher.PropertiesToLoad.Add("userPrincipalName"); // nome Login
            directorySearcher.PropertiesToLoad.Add("distinguishedName");

            return directorySearcher;
        }

        private List<dynamic> _BuildUsersList(SearchResultCollection resultsCollection)
        {
            List<dynamic> users = new();

            foreach(SearchResult searchResult in resultsCollection)
            {
                var user = new
                {
                    Name = searchResult.GetPropertyValue("name"),
                    Mail = searchResult.GetPropertyValue("mail"),
                    Givenname = searchResult.GetPropertyValue("givenname"),
                    Surname = searchResult.GetPropertyValue("sn"),
                    UserPrincipalName = searchResult.GetPropertyValue("userPrincipalName"),
                    DistinguishedName = searchResult.GetPropertyValue("distinguishedName"),
                };

                users.Add(user);
            }

            return users;
        }
    }
}