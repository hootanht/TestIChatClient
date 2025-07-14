using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.AI;

namespace VoiceTranscriptionApi.Services
{
    public class OllamaTranscriptionService : ITranscriptionService
    {
        private readonly IChatClient _chatClient;
        private readonly ILogger<OllamaTranscriptionService> _logger;

        public OllamaTranscriptionService(IChatClient chatClient, ILogger<OllamaTranscriptionService> logger)
        {
            _chatClient = chatClient;
            _logger = logger;
        }

        public async Task<string> TranscribeAudioAsync(IFormFile file)
        {
            _logger.LogInformation("Starting audio transcription with Ollama IChatClient for file: {FileName}", file.FileName);

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.User, [
                    new TextContent("این یک فایل صوتی است، لطفا آن را به دقت به متن فارسی تبدیل کن."),
                    new DataContent(fileBytes, file.ContentType)
                ])
            };

            var response = await _chatClient.GetResponseAsync(chatMessages);
            var result = response.Text ?? string.Empty;

            _logger.LogInformation("Successfully transcribed audio file with Ollama: {FileName}", file.FileName);
            return result;
        }
    }
}