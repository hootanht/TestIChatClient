using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.AI;
using System.Collections.Generic;

namespace VoiceTranscriptionApi.Services
{
    public class AzureOpenAITranscriptionService : ITranscriptionService
    {
        private readonly IChatClient _chatClient;
        private readonly ILogger<AzureOpenAITranscriptionService> _logger;

        public AzureOpenAITranscriptionService(IChatClient chatClient, ILogger<AzureOpenAITranscriptionService> logger)
        {
            _chatClient = chatClient;
            _logger = logger;
        }

        public async Task<string> TranscribeAudioAsync(IFormFile file)
        {
            _logger.LogInformation("Starting transcription with Azure OpenAI IChatClient for file: {FileName}", file.FileName);

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.User, [
                    new Microsoft.Extensions.AI.TextContent("این یک فایل صوتی است، لطفا آن را به دقت به متن فارسی تبدیل کن."),
                    new DataContent(fileBytes, file.ContentType)
                ])
            };

            var response = await _chatClient.GetResponseAsync(chatMessages);
            var result = response.Text ?? string.Empty;

            _logger.LogInformation("Transcription result: {Result}", result);
            return result;
        }
    }
}