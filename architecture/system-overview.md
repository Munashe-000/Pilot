# System Overview

PilotFlow is a workflow orchestration and approvals platform built on a Clean Architecture foundation.

## Layers
- Domain: core entities and state transitions.
- Application: commands, queries, validation, and orchestration.
- Infrastructure: data access, messaging, and integrations.
- API: HTTP endpoints and transport concerns.
- Frontend: Angular admin console.

## Current V1 Runtime (Local)
- API: ASP.NET Core (minimal API).
- Storage: In-memory repositories.
- Frontend: Angular app served via `ng serve`.

## Target Runtime (Serverless)
- API Gateway + Lambda
- DynamoDB
- SQS for async work
- EventBridge for scheduled SLA scans
- SES for notifications

## Architecture Diagram
```mermaid
flowchart LR
  UI[Angular Admin] -->|HTTP| API[ASP.NET API]
  API --> APP[Application Layer]
  APP --> DOMAIN[Domain]
  APP --> INFRA[Infrastructure]
  INFRA --> STORE[(In-Memory Store)]

  subgraph Target (Serverless)
    API_GW[API Gateway] --> LAMBDA[Lambda Handlers]
    LAMBDA --> DDB[(DynamoDB)]
    LAMBDA --> SQS[SQS]
    EVENT[EventBridge] --> SLA[SLA Scanner]
    SLA --> DDB
  end
```

## Key Rules
- Every record carries `TenantId`.
- Application does not depend on Infrastructure.
- Infrastructure provides data access behind interfaces.
