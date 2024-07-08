using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeOffice.Data;
using HomeOffice.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;





#if DEBUG
using Microsoft.OpenApi.Models;
#endif

namespace HomeOffice
{
    /// <summary>
    /// Startup class for configuring the server.
    /// </summary>
    public class Startup
    {
        #region Variables
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;
        private readonly string _allowSpecificOrigins = "_allowSpecificOrigins";
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="config">An instance of IConfiguration.</param>
        /// <param name="env">The hosting environment.</param>
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _configuration = config;
            _env = env;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">A collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
			services.AddLogging(options =>
			{
				options.AddSimpleConsole(opts => opts.IncludeScopes = false);
			});
#endif

            // Die Datenbank mithilfe von AddDbContext hinzufügen/registrieren
             services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy(_allowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    }
                );
            });

            services.AddControllers();

            #region Token
            // Mithilfe von Tokens die eingegebenen Daten authentifizieren
        //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidateIssuerSigningKey = true,
        //             ValidIssuer = "yourIssuer",
        //             ValidAudience = "yourAudience",
        //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
        //         };
        //     });
        // services.AddAuthorization();
            #endregion Token

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddOptions();

            services.AddControllers()
                      .AddNewtonsoftJson(options =>
                      {
                          options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                      });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#if DEBUG
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeOffice", Version = "v1" });
			});
#endif
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider provider, ILogger<Startup> logger)
        {
            var webRootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            if (Directory.Exists(webRootPath))
            {
                app.UseDefaultFiles()
                   .UseStaticFiles();
                   
            }

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeOffice"));
            }
            else
            {
                app.UseHsts();
            }

            if (_env.IsProduction())
                app.UseHttpsRedirection();

            app.UseCors(_allowSpecificOrigins)
                .UseRouting()
                .Use(async (context, next) =>
                {
                    const string cacheHeader = "cache-control";
                    if (context.Response.Headers.Any(e => e.Key.ToLower() == cacheHeader))
                    {
                        context.Response.Headers.Remove(cacheHeader);
                    }
                    context.Response.Headers.Append(cacheHeader, "no-cache, no-store, must-revalidate");
                    await next();
                })
                .UseEndpoints(endpoints =>
                {
                    var ep = endpoints.MapControllers();

                    ep = endpoints.MapControllerRoute(
                        name: "Api with action",
                        pattern: "api/{controller}/{action}/{id?}"
                    );

                    ep = endpoints.MapControllerRoute(
                        name: "DefaultApi",
                        pattern: "api/{controller}/{id?}"
                    );
                })
                .Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await next();
                    }
                })
                .UseDefaultFiles()
                .UseStaticFiles();

            

            // 1. SourcePath: gibt den Pfad zur Root der Angular-Anwendung an. In diesem Fall ist dies ../LearnWebApps.Client. Zeile 173
            // 2. UseProxyToSpaDevelopmentServer: leitet Anfragen an den Angular Entwicklungsserver weiter, der standardmäßig auf http://localhost:4200 läuft. Zeile 175
            // Spa = Single Page Application
#if DEBUG
			app.Use(async (ctx, next) =>
			{
				try
				{
					await next();
				}
				catch (System.Net.Http.HttpRequestException)
				{
					// läuft Angular nicht dann wird swagger geöffnet
					ctx.Response.Redirect("/swagger/index.html");
				}
			}).UseSpa(spa =>
			{
                // 1.
				spa.Options.SourcePath = "../LearnWebApps.Client";
                // 2.
				spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
			});
#endif

        }

        #endregion
    }
}