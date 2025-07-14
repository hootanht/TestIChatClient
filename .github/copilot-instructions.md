# Copilot Instructions for Voice Transcription API

## Architecture Overview

This is a .NET 9 Web API that provides Persian voice transcription using a **Strategy Pattern** for AI provider selection. The core architecture uses Microsoft.Extensions.AI abstractions to support both Ollama and Azure OpenAI through a unified `IChatClient` interface.

### Key Components

- **Services Layer**: `ITranscriptionService` with provider-specific implementations (`OllamaTranscriptionService`, `AzureOpenAITranscriptionService`)
- **Configuration-Based DI**: Provider selection via `TranscriptionServiceType` in `appsettings.json`
- **Custom Authentication**: `TokenAuthorizeAttribute` for header-based token validation
- **Persian Language Support**: All user-facing messages and prompts in Persian

## Critical Patterns

### Provider Registration Pattern (Program.cs)

```csharp
var serviceType = builder.Configuration["TranscriptionServiceType"];
if (serviceType == "AzureOpenAI") {
    builder.Services.AddAzureOpenAIChatClient(/* config */);
    builder.Services.AddScoped<ITranscriptionService, AzureOpenAITranscriptionService>();
} else {
    builder.Services.AddOllamaChatClient(/* config */);
    builder.Services.AddScoped<ITranscriptionService, OllamaTranscriptionService>();
}
```

### Chat Message Pattern for Transcription

Both services use identical message structure:

```csharp
var chatMessages = new List<ChatMessage> {
    new ChatMessage(ChatRole.User, [
        new TextContent("این یک فایل صوتی است، لطفا آن را به دقت به متن فارسی تبدیل کن."),
        new DataContent(fileBytes, file.ContentType)
    ])
};
```

### Error Handling Convention

- Persian error messages in all user responses
- Structured logging with file names and operations
- Consistent exception handling in controller with 500 status codes

## Development Workflows

### Configuration Setup

1. Set `TranscriptionServiceType` to "Ollama" or "AzureOpenAI"
2. Configure provider-specific settings in corresponding config sections
3. Update `ApiSettings:Token` for authentication

### Testing with HTTP File

Use `VoiceTranscriptionApi.http`:

1. Update `@ApiToken` variable with your token
2. Place audio files in project root
3. Test different formats: WAV, MP3, M4A

### Adding New Providers

1. Implement `ITranscriptionService`
2. Add provider-specific configuration section
3. Register in `Program.cs` following the existing pattern
4. Use same Persian prompt structure

## Key Dependencies

- **Microsoft.Extensions.AI**: Core abstraction layer
- **Microsoft.Extensions.AI.Ollama**: Local AI provider
- **Microsoft.SemanticKernel.Connectors.AzureOpenAI**: Cloud AI provider
- **Swashbuckle.AspNetCore**: API documentation with custom auth scheme

## File Upload Handling

Controllers expect `IFormFile` parameter named "file". Both services:

1. Copy to `MemoryStream`
2. Convert to byte array
3. Include file bytes and content type in chat message
4. Return transcription text directly

## Authentication Requirements

All endpoints use `[TokenAuthorize]` attribute that:

- Checks `Authorization` header
- Validates against `ApiSettings:Token` configuration
- Returns Persian error messages for auth failures
- No Bearer prefix required in configuration, but expected in requests

## Swagger Configuration

Custom security scheme configured for "ApiKey" in header with Authorization field. All endpoints require authentication in Swagger UI.
