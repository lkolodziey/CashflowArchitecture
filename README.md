# CashFlow

Solução em .NET 10 para controle de lançamentos financeiros e saldo diário consolidado.

## Decisões principais

- Microsserviços enxutos
- .NET 10 com ASP.NET Core Controllers
- API Gateway simples com YARP
- API Key no gateway apenas para o escopo do desafio; em produção, priorizar Zero Trust com OAuth2/OIDC e API Management
- RabbitMQ para comunicação assíncrona
- Transactional Outbox no Ledger
- PostgreSQL único com schemas separados: `ledger`, `integration`, `consolidation`
- EF Core para escrita e Dapper para leitura
- Serilog, health checks e preparação para OpenTelemetry
- Docker Compose com perfis

## Como rodar infraestrutura

```bash
cd deploy
docker compose --profile infra up -d
```

## Como rodar tudo via Docker Compose

```bash
cd deploy
docker compose --profile full up --build
```

## Endpoints via Gateway

Header obrigatório:

```http
X-API-Key: local-dev-api-key
```

Criar lançamento:

```bash
curl -X POST http://localhost:5080/transactions \
  -H "Content-Type: application/json" \
  -H "X-API-Key: local-dev-api-key" \
  -d '{"type":"credit","amount":100.50,"transactionDate":"2026-01-10","description":"Venda"}'
```

Consultar consolidado:

```bash
curl http://localhost:5080/daily-balances/2026-01-10 -H "X-API-Key: local-dev-api-key"
```

## Documentação

- `docs/architecture/target-architecture.md`
- `docs/adr/*`
