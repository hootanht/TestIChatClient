using Microsoft.AspNetCore.Mvc;
using VoiceTranscriptionApi.Attributes;
using VoiceTranscriptionApi.Services;

namespace VoiceTranscriptionApi.Controllers;

/// <summary>
/// Handles the transcription of audio files.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TranscribeController : ControllerBase
{
    private readonly ITranscriptionService _transcriptionService;
    private readonly ILogger<TranscribeController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranscribeController"/> class.
    /// </summary>
    /// <param name="transcriptionService">The transcription service.</param>
    /// <param name="logger">The logger.</param>
    public TranscribeController(ITranscriptionService transcriptionService, ILogger<TranscribeController> logger)
    {
        _transcriptionService = transcriptionService ?? throw new ArgumentNullException(nameof(transcriptionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Transcribes an audio file.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcription of the audio file.</returns>
    [HttpPost]
    [TokenAuthorize]
    public async Task<IActionResult> Post(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Upload attempt with no file.");
            return BadRequest("هیچ فایلی آپلود نشده است.");
        }

        try
        {
            var transcription = await _transcriptionService.TranscribeAudioAsync(file);
            return Ok(new { transcription });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during transcription for file {FileName}", file.FileName);
            return StatusCode(500, "خطایی در پردازش درخواست شما رخ داد.");
        }
    }

    /// <summary>
    /// Transcribes an audio file from a stream.
    /// </summary>
    /// <param name="file">The audio file to transcribe.</param>
    /// <returns>The transcription of the audio file.</returns>
    [HttpPost("stream")]
    [TokenAuthorize]
    public async Task<IActionResult> PostStream(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Upload attempt with no file.");
            return BadRequest("هیچ فایلی آپلود نشده است.");
        }

        try
        {
            var transcription = await _transcriptionService.TranscribeAudioStreamAsync(file);
            return Ok(new { transcription });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during transcription for file {FileName}", file.FileName);
            return StatusCode(500, "خطایی در پردازش درخواست شما رخ داد.");
        }
    }
}
