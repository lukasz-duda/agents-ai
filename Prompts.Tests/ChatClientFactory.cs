using Microsoft.Extensions.AI;
using OllamaSharp;

namespace Prompts.Tests;

public static class ChatClientFactory
{
    public static IChatClient CreateChatClient()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11435"),
            Timeout = TimeSpan.FromMinutes(10)
        };

        var client = new OllamaApiClient(httpClient, "gemma4:e4b");

        IChatClient chatClient = client;

        return chatClient
            .AsBuilder()
            .UseFunctionInvocation()
            .Build();
    }
}