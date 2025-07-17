using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using VoiceTranscriptionApi.Options;
using VoiceTranscriptionApi.Services;

namespace VoiceTranscriptionApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureOpenAIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureOpenAIOptions>(configuration.GetSection(AzureOpenAIOptions.SectionName));

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
            return new AzureOpenAIClient(new Uri(options.Endpoint), new AzureKeyCredential(options.ApiKey));
        });
        services.AddScoped<ITranscriptionService, AzureOpenAITranscriptionService>();

        return services;
    }
}
