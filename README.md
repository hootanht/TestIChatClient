# Voice Transcription API

A .NET 9 Web API that provides voice file transcription services using AI models. The API supports both Ollama and Azure OpenAI for transcription processing with token-based authentication.

## Features

- 🎙️ **Audio Transcription**: Convert voice files to Persian text
- 🔐 **Token Authentication**: Secure API access with configurable tokens
- 🤖 **Multiple AI Providers**: Support for both Ollama and Azure OpenAI
- 📚 **Swagger Documentation**: Interactive API documentation
- 🔍 **Comprehensive Logging**: Persian language logging support
- 🏗️ **SOLID Architecture**: Clean, maintainable code structure

## Supported Audio Formats

- WAV
- MP3
- M4A
- Other formats supported by the AI models

## Quick Start

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Either:
  - [Ollama](https://ollama.ai/) installed locally, OR
  - Azure OpenAI service subscription

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd VoiceTranscriptionApi
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the application**
   
   Update `appsettings.json` with your settings:

   ```json
   {
     "ApiSettings": {
       "Token": "your-secure-api-token"
     },
     "TranscriptionServiceType": "Ollama", // or "AzureOpenAI"
     "Ollama": {
       "Endpoint": "http://localhost:11434",
       "ModelId": "llama3.2-vision"
     },
     "AzureOpenAI": {
       "Endpoint": "https://your-resource.openai.azure.com/",
       "ApiKey": "your-azure-openai-api-key",
       "DeploymentName": "gpt-4o-mini"
     }
   }
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**
   
   Navigate to: `http://localhost:5153/swagger`

## Configuration

### Service Selection

Set `TranscriptionServiceType` in `appsettings.json`:
- `"Ollama"` - Use local Ollama instance
- `"AzureOpenAI"` - Use Azure OpenAI service

### Ollama Setup

1. Install Ollama from [ollama.ai](https://ollama.ai/)
2. Pull a vision-capable model:
   ```bash
   ollama pull llama3.2-vision
   ```
3. Ensure Ollama is running on `http://localhost:11434`

### Azure OpenAI Setup

1. Create an Azure OpenAI resource
2. Deploy a vision-capable model (e.g., `gpt-4o-mini`)
3. Configure endpoint, API key, and deployment name

## API Usage

### Authentication

All API endpoints require authentication using a Bearer token:

```http
Authorization: Bearer your-api-token
```

### Transcribe Audio

**Endpoint:** `POST /api/transcribe`

**Request:**
```http
POST /api/transcribe
Content-Type: multipart/form-data
Authorization: Bearer your-api-token

file: [audio file]
```

**Response:**
```json
{
  "transcription": "متن فارسی تبدیل شده از فایل صوتی"
}
```

**Error Responses:**
- `400 Bad Request` - No file provided or invalid file
- `401 Unauthorized` - Missing or invalid token
- `500 Internal Server Error` - Transcription failed

## Testing

### Using HTTP File

The project includes `VoiceTranscriptionApi.http` with sample requests:

1. Update the `@ApiToken` variable with your token
2. Place sample audio files in the project directory
3. Execute requests using your IDE or HTTP client

### Using cURL

```bash
curl -X POST "http://localhost:5153/api/transcribe" \
  -H "Authorization: Bearer your-api-token" \
  -F "file=@sample-audio.wav"
```

### Using Swagger UI

1. Navigate to `http://localhost:5153/swagger`
2. Click "Authorize" and enter your token
3. Use the interactive interface to test endpoints

## Architecture

### Project Structure

```
VoiceTranscriptionApi/
├── Controllers/
│   └── TranscribeController.cs     # API endpoints
├── Services/
│   ├── ITranscriptionService.cs    # Service interface
│   ├── OllamaTranscriptionService.cs
│   └── AzureOpenAITranscriptionService.cs
├── Auth/
│   └── TokenAuthorizeAttribute.cs  # Authentication attribute
├── Program.cs                      # Application startup
├── appsettings.json               # Configuration
└── README.md
```

### Design Patterns

- **Dependency Injection**: Service registration and resolution
- **Strategy Pattern**: Configurable transcription service selection
- **SOLID Principles**: Clean, maintainable architecture
- **Attribute-based Authentication**: Secure API access

## Dependencies

- **Microsoft.Extensions.AI**: AI abstractions
- **Microsoft.Extensions.AI.Ollama**: Ollama integration
- **Microsoft.SemanticKernel.Connectors.AzureOpenAI**: Azure OpenAI integration
- **Swashbuckle.AspNetCore**: API documentation

## Configuration Options

### API Settings
```json
"ApiSettings": {
  "Token": "string"  // Authentication token
}
```

### Ollama Settings
```json
"Ollama": {
  "Endpoint": "string",  // Ollama server URL
  "ModelId": "string"    // Model identifier
}
```

### Azure OpenAI Settings
```json
"AzureOpenAI": {
  "Endpoint": "string",      // Azure OpenAI endpoint
  "ApiKey": "string",        // API key
  "DeploymentName": "string" // Model deployment name
}
```

## Logging

The application provides comprehensive logging in Persian:

- Request/response logging
- Authentication events
- Transcription process tracking
- Error details

Logs are output to the console and can be configured for other providers.

## Error Handling

The API includes comprehensive error handling:

- **File Validation**: Ensures files are provided and valid
- **Authentication**: Validates tokens and returns appropriate errors
- **AI Service Errors**: Handles transcription failures gracefully
- **Persian Error Messages**: User-friendly error messages in Persian

## Security Considerations

- **Token Authentication**: Secure API access
- **File Type Validation**: Prevents malicious file uploads
- **Error Message Sanitization**: Prevents information leakage
- **HTTPS Support**: Secure communication (configure in production)

## Performance

- **Streaming Support**: Efficient handling of large audio files
- **Async Operations**: Non-blocking request processing
- **Memory Management**: Proper disposal of resources

## Deployment

### Development
```bash
dotnet run
```

### Production
```bash
dotnet publish -c Release
```

Configure production settings:
- Use secure tokens
- Enable HTTPS
- Configure logging providers
- Set up monitoring

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues and questions:
- Create an issue in the repository
- Check the Swagger documentation
- Review the logs for error details

## Changelog

### v1.0.0
- Initial release
- Ollama and Azure OpenAI support
- Token authentication
- Persian language support
- Swagger documentation 