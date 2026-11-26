using A2A;
using A2A.AspNetCore;
using Microsoft.Extensions.AI;
using OllamaSharp;
using CatWeight;


namespace Server;

// https://thecompoundingcuriosity.substack.com/p/agentic-ai-part-1-simple-react-agent

public static class CatWeightAgentSetup
{
    public static IServiceCollection AddCatWeightAgent(this IServiceCollection services)
    {
        services.AddSingleton<CatWeightCalculator>();
        services.AddSingleton<IChatClient>(sp =>
        {
            var uri = new Uri("http://localhost:11434");
            var client = new OllamaApiClient(uri)
            {
                SelectedModel = "gemma4:e4b"
            };
            return client;
        });

        services.AddA2AAgent<CatWeightAgent>(CatWeightAgent.Card("http://localhost:5000/cat-weight"));
        return services;
    }

    public static IEndpointRouteBuilder MapCatWeightAgent(this IEndpointRouteBuilder app)
    {
        app.MapA2A("/cat-weight");


        var card = app.ServiceProvider.GetRequiredService<AgentCard>();
        app.MapWellKnownAgentCard(card);
        return app;
    }
}

#pragma warning disable CS9113 // Parameter is unread.
public class CatWeightAgent(CatWeightCalculator prompt) : IAgentHandler
#pragma warning restore CS9113 // Parameter is unread.
{
    public static AgentCard Card(string url) => new()
    {
        Name = "Cat Weight Agent",
        Description = "Calculates the total weight of different cat species.",
        Version = "1.0.0",
        SupportedInterfaces = [new AgentInterface
        {
            Url = url,
            ProtocolBinding = "JSONRPC",
            ProtocolVersion = "1.0"
        }],
        DefaultInputModes = ["text/plain"],
        DefaultOutputModes = ["text/plain"],
        Capabilities = new AgentCapabilities { Streaming = false },
        Skills = [new AgentSkill
        {
            Id = "cat-weight",
            Name = "Cat weight",
            Description = "Use this to calculate the total weight of different cat species.",
            Tags = ["total", "cat", "weight"]
        }],
    };

    public async Task ExecuteAsync(RequestContext context, AgentEventQueue eventQueue, CancellationToken cancellationToken)
    {
        var responder = new MessageResponder(eventQueue, context.ContextId);

        //string response = await prompt.SayHello(cancellationToken);

        await responder.ReplyAsync("ok", cancellationToken);
    }
}