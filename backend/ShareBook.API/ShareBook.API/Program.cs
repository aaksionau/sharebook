using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShareBook.API.Contracts;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Endpoints;
using ShareBook.API.Persistence;
using ShareBook.API.Persistence.Repositories;
using ShareBook.API.Services.Abstractions.Helpers;
using ShareBook.API.Services.Abstractions.Validators;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "oauth2",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        }
    );
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddBearerToken();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOfLibraryOnly", policy => policy.RequireClaim("AdminForLibraryId"));
});

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not configured")
    )
);

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

// Add Repositories
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookInstanceRepository, BookInstanceRepository>();
builder.Services.AddScoped<IClaimsHelper, ClaimsHelper>();

// Add services.
builder.Services.AddHttpClient<IISBNdbService, ISBNdbService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ISBNdb:BaseUrl"]
            ?? throw new InvalidOperationException("Base URL not configured")
    );
    client.DefaultRequestHeaders.Add("Authorization", builder.Configuration["ISBNdb:ApiKey"]);
});

// Add FluentValidation
builder.Services.AddScoped<IValidator<BookDto>, BookValidator>();
builder.Services.AddScoped<IValidator<LibraryDto>, LibraryValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShareBook API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapSearchEndpoints();
app.MapBookEndpoints();
app.MapLibraryEndpoints();
app.MapIdentityApi<IdentityUser>();
app.Run();
