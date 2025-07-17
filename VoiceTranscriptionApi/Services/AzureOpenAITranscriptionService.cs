using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using VoiceTranscriptionApi.Options;

namespace VoiceTranscriptionApi.Services;

/// <summary>
/// Provides a service for transcribing audio using the Azure OpenAI service.
/// </summary>
public class AzureOpenAITranscriptionService : ITranscriptionService
{
    private readonly AzureOpenAIClient _azureOpenAIClient;
    private readonly ILogger<AzureOpenAITranscriptionService> _logger;
    private readonly string _deploymentName;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureOpenAITranscriptionService"/> class.
    /// </summary>
    /// <param name="options">The configuration options for Azure OpenAI.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    /// <param name="azureOpenAIClient">The client for interacting with the Azure OpenAI service.</param>
    public AzureOpenAITranscriptionService(IOptions<AzureOpenAIOptions> options, ILogger<AzureOpenAITranscriptionService> logger, AzureOpenAIClient azureOpenAIClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _azureOpenAIClient = azureOpenAIClient ?? throw new ArgumentNullException(nameof(azureOpenAIClient));
        _deploymentName = options.Value.DeploymentName ?? throw new InvalidOperationException("Deployment name is not configured.");
    }

    /// <summary>
    /// Transcribes the audio from the specified file asynchronously.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcribed text.</returns>
    public async Task<string> TranscribeAudioAsync(IFormFile file)
    {
        _logger.LogInformation("Starting transcription with Azure OpenAI for file: {FileName}", file.FileName);

        var tempFilePath = Path.GetTempFileName();
        try
        {
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var audioClient = _azureOpenAIClient.GetAudioClient(_deploymentName);

            await using var audioStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);

            var result = await audioClient.TranscribeAudioAsync(audioStream, file.FileName);

            _logger.LogInformation("Transcription completed successfully");

            var transcribedText = result.Value.Text;

            return transcribedText;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transcription for file: {FileName}", file.FileName);
            throw;
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    /// <summary>
    /// Transcribes the audio from the specified file stream asynchronously.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcribed text.</returns>
    public async Task<string> TranscribeAudioStreamAsync(IFormFile file)
    {
        _logger.LogInformation("Starting transcription with Azure OpenAI for file stream: {FileName}", file.FileName);

        try
        {
            await using var audioStream = file.OpenReadStream();
            var audioClient = _azureOpenAIClient.GetAudioClient(_deploymentName);
            var result = await audioClient.TranscribeAudioAsync(audioStream, file.FileName);

            _logger.LogInformation("Transcription completed successfully");

            return result.Value.Text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transcription for file stream: {FileName}", file.FileName);
            throw;
        }
    }
}