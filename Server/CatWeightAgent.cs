using A2A;
using A2A.AspNetCore;

namespace Server;

// https://thecompoundingcuriosity.substack.com/p/agentic-ai-part-1-simple-react-agent

public static class CatWeightAgentSetup
{
    public static IServiceCollection AddCatWeightAgent(this IServiceCollection services)
    {
        services.AddA2AAgent<CatWeightAgent>(CatWeightAgent.Card("http://localhost:5000/echo"));
        return services;
    }

    public static IEndpointRouteBuilder MapCatWeightAgent(this IEndpointRouteBuilder app)
    {
        app.MapA2A("/echo");


        var card = app.ServiceProvider.GetRequiredService<AgentCard>();
        app.MapWellKnownAgentCard(card);
        return app;
    }
}

public class CatWeightAgent : IAgentHandler
{
    public static AgentCard Card(string url) => new()
    {
        Name = "Average Cat Weight Agent",
        Description = "Echoes messages back to the user",
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
            Id = "echo",
            Name = "Echo",
            Description = "Echoes back user messages",
            Tags = ["echo"]
        }],
    };

    public async Task ExecuteAsync(RequestContext context, AgentEventQueue eventQueue, CancellationToken cancellationToken)
    {
        var responder = new MessageResponder(eventQueue, context.ContextId);
        await responder.ReplyAsync($"Echo: {context.UserText}", cancellationToken);
    }
}