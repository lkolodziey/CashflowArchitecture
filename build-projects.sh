#!/usr/bin/env bash
set -euo pipefail
dotnet restore src/ApiGateway/CashFlow.ApiGateway/CashFlow.ApiGateway.csproj
dotnet restore src/Services/Ledger/CashFlow.Ledger.Api/CashFlow.Ledger.Api.csproj
dotnet restore src/Services/DailyConsolidation/CashFlow.DailyConsolidation.Api/CashFlow.DailyConsolidation.Api.csproj
dotnet test tests/CashFlow.Ledger.Tests/CashFlow.Ledger.Tests.csproj
dotnet test tests/CashFlow.DailyConsolidation.Tests/CashFlow.DailyConsolidation.Tests.csproj
