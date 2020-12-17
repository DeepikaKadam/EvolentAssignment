using Contact.Api.Constants;
using Contact.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contact.Api.Middleware
{
    public class JwtTokenProvider
    {
        #region Private Fields

        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// The serializer settings
        /// </summary>
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// The application settings/
        /// </summary>
        private readonly AppSettings appSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProviderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="options">The options.</param>
        /// <param name="appSettings"></param>
        public JwtTokenProvider(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            this.next = next;
            this.appSettings = appSettings.Value;

            serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(appSettings.AuthenticationPath, StringComparison.Ordinal))
            {
                return next(context);
            }

            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync(MessageConstants.BAD_REQUEST);
            }

            return GenerateToken(context);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await appSettings.IdentityResolver(username, password, appSettings.AuthorizeUserName, appSettings.AuthorizePassword, "Token");
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(MessageConstants.INVALID_USERNAME_PASSWORD);
                return;
            }

            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                 issuer: appSettings.JwtIssuerUrl,
                 audience: appSettings.JwtAudience,
                claims: claims,
                notBefore: now,
                expires: now.Add(appSettings.Expiration),
                signingCredentials: appSettings.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(5).TotalSeconds

            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }
        #endregion
    }
}
