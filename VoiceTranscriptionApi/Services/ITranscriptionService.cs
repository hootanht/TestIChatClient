namespace VoiceTranscriptionApi.Services;

/// <summary>
/// Defines the contract for a transcription service.
/// </summary>
public interface ITranscriptionService
{
    /// <summary>
    /// Transcribes the audio from the specified file asynchronously.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcribed text.</returns>
    Task<string> TranscribeAudioAsync(IFormFile file);

    /// <summary>
    /// Transcribes the audio from the specified file stream asynchronously.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcribed text.</returns>
    Task<string> TranscribeAudioStreamAsync(IFormFile file);
}
