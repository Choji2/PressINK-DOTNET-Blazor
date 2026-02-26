using PressINK_Server_App.Data.Auth;
using PressINK_Server_App.Model.Auth._Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.CRUD;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PressINK_Server_App.Services.Auth.Services
{
    public class LdapAuthenticationService
    {
        ILogger<LdapAuthenticationService> _logger;
        IDbContextFactory<AdminDBContext> _dbFactory;
        public LdapAuthenticationService(ILogger<LdapAuthenticationService> logger, IDbContextFactory<AdminDBContext> dbFactory) 
        {
            _logger = logger;
            _dbFactory = dbFactory;


        }



        public async Task<LDAP_Obj>? LDAP_API_POST(string username, string password)
        {
            var userDat = new LDAP_Obj();
            SD sD = new SD();

            using (HttpClientHandler handler = new HttpClientHandler())
            {

                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {

                    return true;   //Is valid

                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Add(SD.REQ_Header, SD.VALID);

                    string requestBody = $"{{\"username\":\"{username}\",\"password\":\"{System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password))}\",\"tag\":\"{SD.PlantTag}\"}}";
                    // Compute HMAC-SHA1 signature
                    var signature = ComputeHmacSha1(SD.X_HUB_SECRET, requestBody);
                    client.DefaultRequestHeaders.Add(SD.X_HUB_HEAD, $"sha1={signature}");

                    var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                   

                    try
                    {
                        var rtn_result = new LDAP_Obj();
                        for (int i= 0; i<sD.LDAP.Count();i++)
                        {
                            HttpResponseMessage response = await client.PostAsync(sD.LDAP[i], requestContent);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseStream = await response.Content.ReadAsStreamAsync();
                                var user = await JsonSerializer.DeserializeAsync<LDAP_Obj>(responseStream);
                                rtn_result = user;
                                return rtn_result;
                            }                       
                        }                         
                        return null;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        return null;
                    }

                }
            }

        }


        private string ComputeHmacSha1(string secret, string body)
        {
            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            return Convert.ToHexString(hash).ToLower(); // GitHub expects lowercase hex
        }

        public async Task<bool> GetUserIsAdmin(string username)
        {
            var context = _dbFactory.CreateDbContext();

            AdminDB data = await context.AdminDB.FirstOrDefaultAsync(x => x.USER_NAME == username);

            if (data == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }




    }
}
