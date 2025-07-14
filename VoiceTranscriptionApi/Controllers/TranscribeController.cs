using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VoiceTranscriptionApi.Attributes;
using VoiceTranscriptionApi.Services;

namespace VoiceTranscriptionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [TokenAuthorize]
    public class TranscribeController : ControllerBase
    {
        private readonly ITranscriptionService _transcriptionService;
        private readonly ILogger<TranscribeController> _logger;

        public TranscribeController(ITranscriptionService transcriptionService, ILogger<TranscribeController> logger)
        {
            _transcriptionService = transcriptionService;
            _logger = logger;
        }

        [HttpPost]
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
    }
} 