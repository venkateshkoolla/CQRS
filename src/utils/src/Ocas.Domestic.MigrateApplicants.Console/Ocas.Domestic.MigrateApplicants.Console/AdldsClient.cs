using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Ocas.Identity.Core.Configuration;

namespace Ocas.Identity.Auth.Clients
{
    public class AdldsClient : IAdldsClient
    {
        private const string OrganizationUnit = "OU=Applicants,DC=OntarioColleges,DC=CA";

        private static readonly byte[] _key =
        {
            0x3D, 0x65, 0x06, 0xD8, 0x84, 0xD9, 0x1C, 0xF3,
            0x7B, 0xBD, 0x27, 0x94, 0x73, 0x3A, 0xA6, 0xF8,
            0xF1, 0xCD, 0xBF, 0xF9, 0xB0, 0xC6, 0xD6, 0xC3,
            0xC3, 0x05, 0x88, 0x50, 0x13, 0xDA, 0x85, 0xF9
        };

        private static object GetSearchResultPropertyValue(SearchResult rearchResult, string propertyName)
        {
            var values = rearchResult.Properties[propertyName];

            return values?.Count > 0 ? values[0] : null;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }

        private readonly IAdldsConfiguration _appSettings;

        public AdldsClient(IAdldsConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public Dictionary<string, AdldsUser> FindAll()
        {
            var result = new Dictionary<string, AdldsUser>();
            var connectionString = $"LDAP://{_appSettings.Domain}/{OrganizationUnit}";

            using (var containerEntry = new DirectoryEntry(connectionString))
            using (var directorySearcher = new DirectorySearcher(containerEntry)
            {
                SearchScope = SearchScope.OneLevel,
                Filter = "(&(objectCategory=person)(objectClass=user))"
            })
            {
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.ObjectGuid);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.Email);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.FirstName);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.LastName);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.PasswordQuestions);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.PasswordAnswers);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.AccountDisabled);
                directorySearcher.PropertiesToLoad.Add(ActiveDirectoryAttributes.UserName);

                // Required to get more than 1000 results
                // https://docs.microsoft.com/en-us/previous-versions/ms180880(v=vs.90)?redirectedfrom=MSDN
                directorySearcher.PageSize = 1000;

                var searchResults = directorySearcher.FindAll();

                foreach (SearchResult searchResult in searchResults)
                {
                    var objectGUID = GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.ObjectGuid);
                    var email = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.Email);
                    var firstName = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.FirstName);
                    var lastName = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.LastName);
                    var passwordQuestions = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.PasswordQuestions);
                    var passwordAnswers = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.PasswordAnswers);
                    var emailConfirmed = !(bool)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.AccountDisabled);
                    var username = (string)GetSearchResultPropertyValue(searchResult, ActiveDirectoryAttributes.UserName);

                    // skip test data
                    if (firstName == null || lastName == null || passwordQuestions == null || passwordAnswers == null || username == null || email == null)
                    {
                        Console.Out.WriteLine($"Missing data for user {email ?? username}");
                        continue;
                    }

                    try
                    {
                        var adldsUser = new AdldsUser
                        {
                            Id = objectGUID != null ? new Guid((byte[])objectGUID) : Guid.Empty,
                            Email = email,
                            Username = username,
                            FirstName = firstName,
                            LastName = lastName,
                            PasswordQuestions = passwordQuestions.Split('|'),
                            PasswordAnswers = GetPasswordAnswers(passwordAnswers).Split('|'),
                            EmailConfirmed = emailConfirmed
                        };

                        result.Add(username, adldsUser);

                        if (result.Count % 1000 == 0)
                        {
                            Console.Out.WriteLine($"{result.Count} users retrieved");
                        }
                    }
                    catch
                    {
                        Console.Out.WriteLine($"Failed to convert user {email}");
                    }
                }
            }

            return result;
        }

        public void CreateAspNetIdentityUsers(Dictionary<string, AdldsUser> adldsUsers)
        {
            var guids = new HashSet<Guid>();
            using (var conn = new SqlConnection(_appSettings.IdentityConnectionString))
            {
                conn.Open();

                var currentUsers = conn.Query<string>("SELECT DISTINCT UPPER(UserName) FROM [dbo].[Users]").ToHashSet();

                if (adldsUsers.Count == 0)
                {
                    throw new Exception("No new users to add");
                }

                var customQuestionId = conn.QueryFirst<Guid>("SELECT Id FROM SecurityQuestions WHERE Code = 'custom'");
                var securityQuestions = conn.Query("SELECT Content, Id FROM I18NSecurityQuestions").ToDictionary(x => x.Content, x => x.Id);

                var utcNow = DateTime.UtcNow;

                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.BulkCopyTimeout = 300;
                    bulkCopy.DestinationTableName = "dbo.Users";

                    var dt = new DataTable("Users");
                    dt.Columns.Add("Id", typeof(Guid));
                    dt.Columns.Add("UserName", typeof(string));
                    dt.Columns.Add("NormalizedUserName", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("NormalizedEmail", typeof(string));
                    dt.Columns.Add("EmailConfirmed", typeof(bool));
                    dt.Columns.Add("PasswordHash", typeof(string));
                    dt.Columns.Add("SecurityStamp", typeof(string));
                    dt.Columns.Add("ConcurrencyStamp", typeof(string));
                    dt.Columns.Add("PhoneNumber", typeof(string));
                    dt.Columns.Add("PhoneNumberConfirmed", typeof(bool));
                    dt.Columns.Add("TwoFactorEnabled", typeof(bool));
                    dt.Columns.Add("LockoutEnd", typeof(DateTime));
                    dt.Columns.Add("LockoutEnabled", typeof(bool));
                    dt.Columns.Add("AccessFailedCount", typeof(int));
                    dt.Columns.Add("FullName", typeof(string));
                    dt.Columns.Add("FirstName", typeof(string));
                    dt.Columns.Add("LastName", typeof(string));
                    dt.Columns.Add("IsActive", typeof(bool));
                    dt.Columns.Add("LoginFlowInitiatorReturnUrl", typeof(string));
                    dt.Columns.Add("UnconfirmedNewEmail", typeof(string));
                    dt.Columns.Add("AutoProvisionAuthScheme", typeof(string));
                    dt.Columns.Add("AllowLocalLogin", typeof(bool));
                    dt.Columns.Add("LastLoginDate", typeof(DateTime));
                    dt.Columns.Add("CreatedDate", typeof(DateTime));

                    foreach (var adldsUser in adldsUsers.Values)
                    {
                        // user already exists, so skip
                        if (currentUsers.Contains(adldsUser.Username.ToUpperInvariant()))
                            continue;

                        // duplicate Guid
                        if (guids.Contains(adldsUser.Id))
                        {
                            Console.WriteLine($"Duplicate Guid detected. Username: {adldsUser.Username} objectGUID: {adldsUser.Id}");
                            continue;
                        }
                        guids.Add(adldsUser.Id);
                        adldsUser.Migrate = true;

                        adldsUser.FirstName = Truncate(adldsUser.FirstName, 50);
                        adldsUser.LastName = Truncate(adldsUser.LastName, 50);

                        dt.Rows.Add(
                            adldsUser.Id,
                            adldsUser.Username,
                            adldsUser.Username.Normalize().ToUpperInvariant(),
                            adldsUser.Email,
                            adldsUser.Email.Normalize().ToUpperInvariant(),
                            adldsUser.EmailConfirmed,
                            null, // PasswordHash
                            Guid.NewGuid().ToString(), // SecurityStamp
                            Guid.NewGuid().ToString(), // ConcurrencyStamp
                            null, // PhoneNumber
                            false, // PhoneNumberConfirmed
                            false, // TwoFactorEnabled
                            null, // LockoutEnd
                            true, // LockoutEnabled
                            0, // AccessFailedCount
                            Truncate($"{adldsUser.FirstName} {adldsUser.LastName}".Trim(), 256),
                            adldsUser.FirstName,
                            adldsUser.LastName,
                            true, // IsActive
                            null, // LoginFlowInitiatorReturnUrl
                            null, // UnconfirmedNewEmail
                            null, // AutoProvisionAuthScheme
                            true, // AllowLocalLogin
                            null, // LastLoginDate
                            utcNow); // CreatedDate
                    }

                    bulkCopy.WriteToServer(dt);
                }

                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "dbo.UserSecurityQuestions";

                    var dt = new DataTable("UserSecurityQuestions");
                    dt.Columns.Add("Id", typeof(Guid));
                    dt.Columns.Add("UserId", typeof(Guid));
                    dt.Columns.Add("SecurityQuestionId", typeof(Guid));
                    dt.Columns.Add("CustomQuestion", typeof(string));
                    dt.Columns.Add("Answer", typeof(string));

                    foreach (var adldsUser in adldsUsers.Values)
                    {
                        if (!adldsUser.Migrate)
                            continue;

                        if (adldsUser.PasswordQuestions == null || adldsUser.PasswordAnswers == null)
                        {
                            Console.WriteLine($"No security questions or answers. Username: {adldsUser.Username}");
                            continue;
                        }

                        if (adldsUser.PasswordQuestions.Length != adldsUser.PasswordAnswers.Length)
                        {
                            Console.WriteLine($"Bad security question data. Username: {adldsUser.Username}");
                            continue;
                        }

                        for (var i = 0; i < adldsUser.PasswordQuestions.Length; i++)
                        {
                            if (securityQuestions.TryGetValue(adldsUser.PasswordQuestions[i],
                                out var securityQuestionId))
                            {
                                dt.Rows.Add(
                                    Guid.NewGuid(),
                                    adldsUser.Id,
                                    securityQuestionId,
                                    null,
                                    Truncate(adldsUser.PasswordAnswers[i], 60));
                            }
                            else
                            {
                                dt.Rows.Add(
                                    Guid.NewGuid(),
                                    adldsUser.Id,
                                    customQuestionId,
                                    Truncate(adldsUser.PasswordQuestions[i], 60),
                                    Truncate(adldsUser.PasswordAnswers[i], 60));
                            }
                        }
                    }

                    bulkCopy.WriteToServer(dt);
                }
            }
        }

        private string GetPasswordAnswers(string encryptedString)
        {
            var data = Convert.FromBase64String(encryptedString);

            using (var aes = new AesManaged())
            {
                aes.Key = _key;
                var bytes = Decrypt(data, aes.Key, aes.IV);

                // throw away salt and decode string
                var result = Encoding.Unicode.GetString(bytes, 16, bytes.Length - 16);

                return result;
            }
        }

        private byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iV)
        {
            using (var aes = new AesManaged())
            using (var ms = new MemoryStream(cipherText))
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(key, iV), CryptoStreamMode.Read))
            using (var reader = new MemoryStream())
            {
                cs.CopyTo(reader);

                return reader.ToArray();
            }
        }

        private static class ActiveDirectoryAttributes
        {
            public const string AccountDisabled = "msDS-UserAccountDisabled";
            public const string Email = "mail";
            public const string FirstName = "givenName";
            public const string LastName = "sn";
            public const string ObjectGuid = "objectGUID";
            public const string PasswordAnswers = "passwordAnswer";
            public const string PasswordQuestions = "passwordQuestion";
            public const string UserName = "userPrincipalName";
        }
    }
}
