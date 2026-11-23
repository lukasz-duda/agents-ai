# Agents AI

- [A2A protocol](https://a2a-protocol.org/)
- [ReAct: Synergizing Reasoning and Acting in Language Models](https://arxiv.org/abs/2210.03629)
- https://react-lm.github.io/

Requirements:

- [Docker](https://docs.docker.com/engine/install/ubuntu/)
- [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Start Ollama:

```bash
docker compose up -d
```

Pull models:

```bash
docker exec -it ollama ollama pull gemma4:e4b
```

Test prompts:

```bash
dotnet test
```

Start server:

```bash
cd Server
dotnet run
```

Start client:

```bash
cd Client
dotnet run
```
