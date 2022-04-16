using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace ToDoList.Services
{
    public class AccountService
    {
        public const string DefaultIdClaimType = "Id";
        public const string DefaultEmailClaimType = "Email";
        private readonly EmailService _emailService;

        public AccountService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Authenticate(HttpContext httpContext, int id, string email)
        {
            var claims = new List<Claim>
            {
                new Claim(DefaultIdClaimType, id.ToString()),
                new Claim(DefaultEmailClaimType, email),
            };
            ClaimsIdentity claimsIdentityd = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentityd));
        }

        public string GetConfirmatioEmailToken(int id)
        {
            var claims = new List<Claim>
            {
                new Claim(DefaultIdClaimType, id.ToString()),
            };

            DateTime dateTimeNow = DateTime.Now;
            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: Environment.GetEnvironmentVariable("ISSUER"),
                    audience: Environment.GetEnvironmentVariable("AUDIENCE"),
                    notBefore: dateTimeNow,
                    claims: claims,
                    expires: dateTimeNow.Add(TimeSpan.FromMinutes(20)),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECURITY_KEY"))), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task SendConfirmationEmail(string host, int id, string email)
        {
            string subject = "Email confirmation";
            string confirmatioEmailToken = GetConfirmatioEmailToken(id);
            string confirmationEmailLink = $"{host}/Account/ConfirmationEmail/{confirmatioEmailToken}";
            string body = $"To confirm your email follow link: {confirmationEmailLink}";
            await _emailService.SendEmailAsync(subject, body, email);
        }

        public int ValidateConfirmationEmailToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
                ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECURITY_KEY")))
            };

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch
            {
                throw new Exception("Token is not valid");
            }
            JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)validatedToken;
            return int.Parse(jwtSecurityToken.Claims.First(c => c.Type == DefaultIdClaimType).Value);
        }
    }
}
