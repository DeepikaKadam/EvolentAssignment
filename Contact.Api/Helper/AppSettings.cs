using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contact.Api.Helper
{
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the authentication path.
        /// </summary>
        /// <value>
        /// The authentication path.
        /// </value>
        public string AuthenticationPath { get; set; }

        /// <summary>
        /// Gets or sets the JWT issuer URL.
        /// </summary>
        /// <value>
        /// The JWT issuer URL.
        /// </value>
        public string JwtIssuerUrl { get; set; }

        /// <summary>
        /// Gets or sets the JWT audience.
        /// </summary>
        /// <value>
        /// The JWT audience.
        /// </value>
        public string JwtAudience { get; set; }

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Gets or sets the name of the authorize user.
        /// </summary>
        /// <value>
        /// The name of the authorize user.
        /// </value>
        public string AuthorizeUserName { get; set; }

        /// <summary>
        /// Gets or sets the authorize password.
        /// </summary>
        /// <value>
        /// The authorize password.
        /// </value>
        public string AuthorizePassword { get; set; }

        /// <summary>
        /// Resolves a user identity given a username and password.
        /// </summary>
        public Func<string, string, string, string, string, Task<ClaimsIdentity>> IdentityResolver { get; set; }


        /// <summary>
        /// Gets or sets the swagger page version.
        /// </summary>
        /// <value>
        /// The swagger page version.
        /// </value>
        public string SwaggerPageVersion { get; set; }

        /// <summary>
        /// Gets or sets the swagger page title.
        /// </summary>
        /// <value>
        /// The swagger page title.
        /// </value>
        public string SwaggerPageTitle { get; set; }

        /// <summary>
        /// Gets or sets the swagger page description.
        /// </summary>
        /// <value>
        /// The swagger page description.
        /// </value>
        public string SwaggerPageDescription { get; set; }

        /// <summary>
        /// Gets or sets the SQL database connection string.
        /// </summary>
        /// <value>
        /// The SQL database connection string.
        /// </value>
        public string SqlDbConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
    }
}
