using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Dynastic.Infrastructure;
using Dynastic.Application;
using Dynastic.Infrastructure.Persistence;
using Dynastic.Infrastrucutre.Persistence;
using Dynastic.API.Services;
using Dynastic.Domain.Common.Interfaces;
using Microsoft.OpenApi.Models;
using Dynastic.API.Services;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Infrastructure.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authority = builder.Configuration["Auth0:Authority"];
var audience = builder.Configuration["Auth0:Audience"];
var clientId = builder.Configuration["Auth0:ClientId"];

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.Authority = authority;
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = ClaimTypes.NameIdentifier };
});

var cosmosDbConfig = new CosmosDbConfiguration();
builder.Configuration.Bind("CosmosDb", cosmosDbConfig);
builder.Services.AddSingleton(cosmosDbConfig);

var fileStorageConfig = new FileStorageConfiguration(builder.Environment.IsDevelopment());
builder.Configuration.Bind("AzureFileStorage", fileStorageConfig);
builder.Services.AddSingleton<IFileStorageConfiguration>(fileStorageConfig);


builder.Services.Configure<CosmosDbConfiguration>(builder.Configuration.GetSection("CosmosDb"));

builder.Services.AddCloudInfrastructure(cosmosDbConfig);
builder.Services.AddApplication();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows {
                Implicit = new OpenApiOAuthFlow {
                    AuthorizationUrl = new Uri(authority + "/authorize?audience=" + audience),
                    Scopes = new Dictionary<string, string> {
                        { "openid", "Open Id" }, { "email", "Email" }, { "profile", "Profile" },
                    }
                }
            }
        });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();


var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        options.OAuthClientId(clientId);
    });

    // await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
    // await ApplicationDbContextSeed.SeedSampleDataAsync(context);
}
else
{
    await context.Database.EnsureCreatedAsync();
    // TODO: Only run this in some test env. But for now testing in Prod env!
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        options.OAuthClientId(clientId);
    });
}

app.UseCors(options => {
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions() {
    FileProvider = new PhysicalFileProvider(fileStorageConfig.UserCoaEnvironmentPath()),
    RequestPath = new PathString("/user-coa")
});

app.Run();