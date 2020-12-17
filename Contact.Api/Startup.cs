using Contact.Api.Constants;
using Contact.Api.Helper;
using Contact.Api.Middleware;
using Contact.Api.Repository.Helper;
using Contact.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = Configuration.GetSection<AppSettings>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =
                             new TokenValidationParameters
                             {
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,

                                 ValidIssuer = appSettings.JwtIssuerUrl,
                                 ValidAudience = appSettings.JwtAudience,
                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecretKey))
                             };
                    });

            services.Configure<AppSettings>(Configuration.GetSection(typeof(AppSettings).Name));
            services.AddControllers();

            var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = GlobalConstants.SWAGGER_UI_AUTHORISE_TOKEN_MESSAGE,
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Reference = new Microsoft.OpenApi.Models.OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        appSettings.SwaggerPageVersion,
                        new OpenApiInfo
                        {
                            Title = appSettings.SwaggerPageTitle,
                            Version = appSettings.SwaggerPageVersion,
                            Description = appSettings.SwaggerPageDescription
                        });
                    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {securityScheme,new string[]{ } }
                        });
                });
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(appSettings.SqlDbConnectionString);

            });

            services.AddTransient<IContactRepository, ContactRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            AppSettings appSettings = Configuration.GetSection<AppSettings>();

            appSettings.IdentityResolver = GetIdentity;
            appSettings.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecretKey)), SecurityAlgorithms.HmacSha256);
            app.UseMiddleware<JwtTokenProvider>(Options.Create(appSettings));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            
            //.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseAuthentication();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(GlobalConstants.SWAGGER_ENDPOINT, GlobalConstants.SWAGGER_DOC_NAME);
            });

        }


        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="inputUserName">Name of the input user.</param>
        /// <param name="inputPassword">The input password.</param>
        /// <param name="genericIdentityType"></param>
        /// <returns></returns>
        private Task<ClaimsIdentity> GetIdentity(string username, string password, string inputUserName, string inputPassword, string genericIdentityType)
        {
            if (username == inputUserName && password == inputPassword)
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, genericIdentityType), new Claim[] { }));
            }

            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, genericIdentityType), new Claim[] { }));
        }
    }
}
