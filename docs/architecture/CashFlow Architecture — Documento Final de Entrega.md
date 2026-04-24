# CashFlow Architecture — Documento Final de Entrega

## 1. Visão Geral

Este projeto implementa uma solução de gestão de fluxo de caixa baseada em arquitetura orientada a eventos, com foco em:

- Consistência eventual
- Desacoplamento entre serviços
- Escalabilidade e resiliência
- Clareza arquitetural

A solução é composta por dois serviços principais:

- **Ledger Service** — responsável pelo registro de transações
- **Daily Consolidation Service** — responsável pela consolidação diária de saldo

---

## 2. Arquitetura

### Componentes

- API Gateway (YARP)
- Ledger API
- Consolidation API
- PostgreSQL (schemas separados)
- RabbitMQ (mensageria)
- Outbox Pattern (garantia de entrega)

### Diagrama lógico

```
Client
   ↓
API Gateway
   ↓
Ledger Service ───→ PostgreSQL (ledger + integration)
   ↓
Outbox Publisher
   ↓
RabbitMQ (Exchange: cashflow.events)
   ↓
Consolidation Consumer
   ↓
Consolidation Service ───→ PostgreSQL (consolidation)
```

---

## 3. Fluxo de Dados

### Criação de transação

```
POST /transactions
→ Ledger persiste transação
→ Outbox registra evento
→ Publisher envia para RabbitMQ
→ Consumer recebe evento
→ Consolidation atualiza saldo diário
```

### Consulta de saldo

```
GET /daily-balances/{date}
→ Consolidation consulta banco
→ Retorna saldo consolidado
```

---

## 4. Decisões Arquiteturais (ADRs)

### ADR-001 — Arquitetura baseada em microserviços
Permite desacoplamento e evolução independente dos serviços.

### ADR-002 — Uso de RabbitMQ
Escolhido pela simplicidade operacional e adequação ao cenário.

### ADR-003 — Outbox Pattern
Garante consistência entre banco e mensageria.

### ADR-004 — PostgreSQL compartilhado com schemas separados
Reduz complexidade operacional mantendo isolamento lógico.

### ADR-005 — API Gateway com YARP
Solução leve e adequada ao escopo.

### ADR-006 — Segurança via API Key
Implementação simplificada, com recomendação futura de Zero Trust.

### ADR-007 — Observabilidade
Uso de logs estruturados (Serilog) e preparação para OpenTelemetry.

---

## 5. Modelo de Dados

### Ledger

**transactions**
- id
- type
- amount
- transaction_date
- description
- created_at_utc

**outbox_messages**
- id
- event_type
- payload
- occurred_at_utc
- processed_at_utc
- status
- retry_count

---

### Consolidation

**daily_balances**
- id (date)
- balance_date
- total_credits
- total_debits
- updated_at_utc

**processed_messages**
- message_id
- processed_at_utc

---

## 6. Requisitos Não Funcionais

### Escalabilidade
- Serviços independentes
- Preparado para containerização e Kubernetes

### Resiliência
- Retry em mensagens
- Idempotência via `processed_messages`
- Outbox Pattern

### Consistência
- Eventual consistency entre serviços

### Observabilidade
- Logs estruturados
- Health checks
- Preparação para tracing distribuído

### Segurança
- API Key (simplificado)
- Evolução recomendada para OAuth2 / Zero Trust

---

## 7. Estratégia de Testes

- Testes unitários com xUnit
- Assertions com FluentAssertions
- Testes de integração com Testcontainers
- Testes de fluxo end-to-end recomendados

---

## 8. Execução do Projeto

### Subir ambiente

```bash
docker compose --profile full up --build
```

### Criar transação

```bash
curl -X POST http://localhost:5080/transactions \
  -H "x-api-key: local-dev-api-key" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "credit",
    "amount": 100,
    "date": "2026-01-01",
    "description": "teste"
  }'
```

### Consultar saldo

```bash
curl http://localhost:5080/daily-balances/2026-01-01 \
  -H "x-api-key: local-dev-api-key"
```

---

## 9. Trade-offs

| Decisão | Justificativa |
|--------|-------------|
| Sem Kubernetes | Fora do escopo |
| API Key simples | Simplificação para o teste |
| Banco compartilhado | Redução de complexidade |
| RabbitMQ vs Kafka | Menor overhead operacional |

---

## 10. Evoluções Futuras

- Implementar autenticação robusta (OAuth2)
- Migrar para Kubernetes
- Separar banco por serviço
- Implementar Dead Letter Queue
- Adicionar OpenTelemetry completo
- Substituir API Gateway por solução cloud

---

## 11. Conclusão

A solução demonstra:

- Aplicação de boas práticas de arquitetura
- Uso correto de mensageria e consistência eventual
- Clareza nas decisões técnicas
- Preparação para evolução

O foco não foi apenas entregar código funcional, mas uma base arquitetural sólida e extensível.

---