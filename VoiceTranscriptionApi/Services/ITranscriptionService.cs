using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VoiceTranscriptionApi.Services
{
    public interface ITranscriptionService
    {
        Task<string> TranscribeAudioAsync(IFormFile file);
    }
} 