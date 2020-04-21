using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using SwaggerAspCoreOData.Authorization;
using SwaggerAspCoreOData.Controllers;
using SwaggerAspCoreOData.DBContext;
using SwaggerAspCoreOData.Repositories;
using SwaggerAspCoreOData.RequestValidation;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerAspCoreOData
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
      readonly IApiVersionDescriptionProvider provider;

      public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
        this.provider = provider;

      public void Configure(SwaggerGenOptions options)
      {
        foreach (var description in provider.ApiVersionDescriptions)
        {
          options.SwaggerDoc(
            description.GroupName,
              new OpenApiInfo()
              {
                Title = $"Sample API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
              });
        }
      }
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers(options => options.EnableEndpointRouting = false);

      services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"), x => x.CommandTimeout(120).EnableRetryOnFailure()).EnableSensitiveDataLogging().EnableDetailedErrors());
      services.AddScoped<IUserRepository, UserRepository>();

      services.AddScoped<ModelValidationFilterAttribute>();

      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
             // base-address of identityserver
             options.Authority = "https://localhost:5021";
             // name of the API resource
             options.Audience = "https://localhost:5021/resources";
             //options.SaveToken = true;
           });

      services.AddODataApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;       
      
        options.DefaultApiVersion = new ApiVersion(1, 0);
      });

      services.AddApiVersioning(options => options.ReportApiVersions = true);
      services.AddOData().EnableApiVersioning();
   
      services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
      services.AddSwaggerGen(c =>
      {
        c.EnableAnnotations();

        // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.OAuth2,
          Flows = new OpenApiOAuthFlows
          {
            Implicit = new OpenApiOAuthFlow
            {
              // this your authorisation server endpoint
              AuthorizationUrl = new Uri("https://localhost:5021/connect/authorize", UriKind.Absolute),
              Scopes = new Dictionary<string, string>
                {
                    { "read", "Access identity information" },
                    { "roles", "Access API roles" }
                }
            }
          }
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "read", "roles" }
         }
       });
     });

      // This policy registers a requirement that the user profile must be Admin and that the user must have a sub claim.
     services.AddAuthorization(options =>
        options.AddPolicy("AuthorizePolicy",
        requirement => requirement.AddRequirements(new AuthorizeRequirement("Admin"))
                                  .RequireClaim(JwtClaimNames.Sub)
     ));

     services.AddScoped<IAuthorizationHandler, AuthorizeHandler>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VersionedODataModelBuilder modelBuilder,
    IApiVersionDescriptionProvider provider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      // token validation
      app.UseAuthentication();

      var models = modelBuilder.GetEdmModels();

      app.UseMvc(routes =>
      {
        routes.EnableDependencyInjection();
        routes.MapVersionedODataRoutes("api", "v{version:apiVersion}", models);
        routes.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
      });

      app.UseSwagger();
      app.UseSwaggerUI(
          options =>
          {
            options.DisplayRequestDuration();
            options.ShowExtensions();
            options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);

            options.OAuthClientId("sampleapi");
            options.OAuthClientSecret("");
            options.OAuthRealm("Sample API");
            options.OAuthAppName("Sample API");

            foreach (var description in provider.ApiVersionDescriptions)
            {
              options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            }
            
          });

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
