using Microsoft.Extensions.AI;

namespace CatWeight.Tests;

public class CatWeightCalculatorTests
{
    private IChatClient _chatClient;
    private CatWeightCalculator _sut;

    public CatWeightCalculatorTests()
    {
        _chatClient = ChatClientFactory.CreateChatClient();
        _sut = new CatWeightCalculator(_chatClient);
    }

    [Fact]
    public async Task UsesModel()
    {
        string response = await _sut.SayHello();
        Assert.Contains("hello", response, StringComparison.OrdinalIgnoreCase);
        Assert.True(response.Length > 5);
    }

    [Theory]
    [InlineData("European cat", "5 kg")]
    [InlineData("leopard", "50 kg")]
    public async Task UsesTool(string catSpecies, string expectedAverageWeight)
    {
        string response = await _sut.GetAverageWeight(catSpecies);
        Assert.Contains(expectedAverageWeight, response, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreatePlan_ReturnsPlannedSteps()
    {
        Plan? plan = await _sut.CreatePlan("I have 3 leopards and 5 European cats. How many of them can fit in a vehicle with a 100 kg payload capacity?");
        Assert.NotNull(plan);
        Assert.NotEmpty(plan.Actions);
        var firstAction = plan.Actions[0];
        Assert.NotEmpty(firstAction.Name);
        Assert.NotEmpty(firstAction.Comment);
    }

    [Fact]
    public async Task ExecuteAction_ReturnsResult()
    {
        Action action = new("get_avg_weight", "Get average weight of leopard");
        string result = await _sut.ExecuteAction(action);
        Assert.Contains("50", result);
    }
}