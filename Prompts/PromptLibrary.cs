using Microsoft.Extensions.AI;

namespace Prompts;

public class PromptLibrary(IChatClient chatClient)
{
    public async Task<string> SayHello(CancellationToken cancellationToken = default)
    {
        ChatMessage chatMessage = new(ChatRole.User, "Hello");
        ChatResponse response = await chatClient.GetResponseAsync(
            [chatMessage],
            cancellationToken: cancellationToken);

        return response.Text;
    }

    public async Task<string> GetAverageWeight(string catSpecies, CancellationToken cancellationToken = default)
    {
        ChatMessage chatMessage = new(ChatRole.User, $"What is the average weight of {catSpecies}?");
        ChatResponse response = await chatClient.GetResponseAsync(
            [chatMessage],
            options: new ChatOptions
            {
                Tools = [AIFunctionFactory.Create(CatAverageWeight.ByWeight)]
            },
            cancellationToken: cancellationToken);

        return response.Text;
    }

    public async Task<Plan?> CreatePlan(string taskDescription, CancellationToken cancellationToken = default)
    {
        ChatMessage systemPrompt = new(ChatRole.System, @"Plan the first steps to start with to resolve the user's problem. The actions will be executed and we will continue in next iteration.

Available actions and required arguments:
- get_avg_weight(species): Request for an average weight for a given cat species.
- divide(numerator, denominator): Divide two values.
- multiply(factor_a, factor_b): Multiply two values.
- sum(value_a, value_b): Add values together.

Return the response in valid JSON format only, using this exact structure:

{
  ""actions"": [
    {
      ""name"": ""get_avg_weight"",
      ""comment"": ""Get average weight of Siamese cat""
    },
    { 
      ""name"": ""divide"", 
      ""comment"": ""Divide maximum load by Siamese cat weight"" 
    }
  ]
}");
        ChatMessage userMessage = new(ChatRole.User, taskDescription);
        ChatResponse<Plan> response = await chatClient.GetResponseAsync<Plan>(
            [systemPrompt, userMessage],
            cancellationToken: cancellationToken);
        return response.Result;
    }
}

public record Plan(Action[] Actions);

public record Action(string Name, string Comment);