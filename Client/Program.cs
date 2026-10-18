using A2A;

var cardResolver = new A2ACardResolver(new Uri("http://localhost:5000/"));
var agentCard = await cardResolver.GetAgentCardAsync();

var client = new A2AClient(new Uri(agentCard.SupportedInterfaces[0].Url));

var response = await client.SendMessageAsync(new SendMessageRequest
{
    Message = new Message
    {
        MessageId = Guid.NewGuid().ToString("N"),
        Role = Role.User,
        Parts = [Part.FromText("I have 3 leopards and 5 European cats. How many of them can fit in a vehicle with a 100 kg payload capacity?")]
    }
});

switch (response.PayloadCase)
{
    case SendMessageResponseCase.Message:
        Console.WriteLine(response.Message!.Parts[0].Text);
        break;
    case SendMessageResponseCase.Task:
        Console.WriteLine($"Task created: {response.Task!.Id}");
        break;
}