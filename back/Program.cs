using back.Services;
using back.Services.External;
using back.Utilities;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]?.Split(",") ?? new[] { "http://localhost:3000" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add HTTP Client
builder.Services.AddHttpClient();

// Register Application Services
builder.Services.AddSingleton<ICacheManager, CacheManager>();
builder.Services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

// Register External API Clients
builder.Services.AddScoped<ICoinMarketCapClient, CoinMarketCapClient>();
builder.Services.AddScoped<IOpenAiClient, OpenAiClient>();
builder.Services.AddScoped<IGeminiClient, GeminiClient>();

// Register Core Services
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IMarketService, MarketService>();
builder.Services.AddScoped<IAnalysisService, AnalysisService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/api/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

// Use response compression
app.UseResponseCompression();

// Use CORS
app.UseCors("AllowFrontend");

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use routing and endpoints
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () =>
{
    return new
    {
        status = "healthy",
        timestamp = DateTime.UtcNow
    };
});

app.Run();

// Leave this empty, we don't need the WeatherForecast record
