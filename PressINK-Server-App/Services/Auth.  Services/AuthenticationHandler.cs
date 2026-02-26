using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PressINK_Server_App.Services.Auth.Services
{
    public class AuthenticationHandler : AuthenticationStateProvider

    {
        private readonly LdapAuthenticationService _ldapServcie;
        private readonly ILogger<AuthenticationHandler> _logger;
        private readonly NavigationManager _navigationManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationHandler(ILocalStorageService localStorageService, IOptions<JwtSettings> jwtSettings, LdapAuthenticationService ldapServcie, ILogger<AuthenticationHandler> logger)
        {
            _ldapServcie = ldapServcie;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _localStorageService = localStorageService;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>(_jwtSettings.TokenName);

                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return new AuthenticationState(claimsPrincipal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

        }



        public async Task<string> Login(string username, string password)
        {

            var userAuth = await _ldapServcie.LDAP_API_POST(username, password);

            if (userAuth != null && userAuth.role.Contains(SD.USERMODE))
            {
                _logger.LogInformation("setting claims");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userAuth.displayName),
                    new Claim("DisplayName", userAuth.displayName),
                    new Claim("UserName", userAuth.cn),
                    new Claim(ClaimTypes.Authentication, "Authenticated"),
                    new Claim("PhysicalDeliveryOfficeName", userAuth.physicalDeliveryOfficeName),
                    new Claim("Department", userAuth.department),
                    new Claim("Company",userAuth.company),
                    new Claim("Mail",userAuth.mail)

                };

                if (userAuth.role.Contains(SD.USERMODE))
                {
                    claims.Add(new Claim(ClaimTypes.Role, SD.USERMODE));
                }
                if (await _ldapServcie.GetUserIsAdmin(username))
                {

                    claims.Add(new Claim(ClaimTypes.Role, SD.GODEMODE));

                }

                var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                var user = new ClaimsPrincipal(identity);


                // Sign in the user with the authentication cookie
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,  // This makes the authentication session persistent across multiple requests
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3)  // Set the cookie to expire in 3 Days
                };




                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)); // Replace with your secret key
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(2);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,  // Replace with your issuer
                    audience: _jwtSettings.Audience,  // Replace with your audience
                    claims: claims,
                    expires: expiry,
                    signingCredentials: creds
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(token);

                return stringToken;

            }
            else
            {
                _logger.LogInformation("No claims set, No user Auth.");
                return null;

            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _localStorageService.RemoveItemAsync(_jwtSettings.TokenName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }


    }
}

