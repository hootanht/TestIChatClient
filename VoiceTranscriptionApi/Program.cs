#pragma warning disable SKEXP0010
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using VoiceTranscriptionApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceType = builder.Configuration["TranscriptionServiceType"];

if (serviceType == "AzureOpenAI")
{
    builder.Services.AddKernel();
    builder.Services.AddAzureOpenAIChatClient(
        deploymentName: builder.Configuration["AzureOpenAI:DeploymentName"]!,
        endpoint: builder.Configuration["AzureOpenAI:Endpoint"]!,
        apiKey: builder.Configuration["AzureOpenAI:ApiKey"]!
    );
    builder.Services.AddScoped<ITranscriptionService, AzureOpenAITranscriptionService>();
}
else // Default to Ollama
{
    builder.Services.AddOllamaChatClient(
        endpoint: new Uri(builder.Configuration["Ollama:Endpoint"]!),
        modelId: builder.Configuration["Ollama:ModelId"]!
    );
    builder.Services.AddScoped<ITranscriptionService, OllamaTranscriptionService>();
}


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VoiceTranscriptionApi", Version = "v1" });

    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter your API Key",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
