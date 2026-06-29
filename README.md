# B3 CDB Calculator

Simulador de investimento em CDB desenvolvido como projeto de avaliação para a Diretoria de Desenvolvimento de Sistemas de Middle e Back Office da B3.

A solução é composta por uma Web API em .NET 10 e uma interface web em Angular 22, ambos versionados neste repositório em uma única solução do Visual Studio.

---

## Índice

- [Estrutura da solução](#estrutura-da-solução)
- [Pré-requisitos](#pré-requisitos)
- [Executar localmente](#executar-localmente)
- [Testes](#testes)
- [Contrato da API](#contrato-da-api)
- [Regras de negócio](#regras-de-negócio)
- [Decisões técnicas](#decisões-técnicas)
- [Princípios SOLID](#princípios-solid)

---

## Estrutura da solução

```
B3.CdbCalculator.sln
├── src/
│   ├── B3.CdbCalculator.Api/          ← Web API .NET 10
│   │   ├── Controllers/               ← CdbCalculationsController
│   │   ├── DTOs/                      ← Request e Response com validação
│   │   └── Program.cs                 ← Composição e configuração da aplicação
│   ├── B3.CdbCalculator.Core/         ← Lógica de negócio pura
│   │   ├── Constants/                 ← CDI e TB fixos
│   │   ├── Interfaces/                ← ICdbCalculatorService, ITaxRateService
│   │   ├── Models/                    ← CdbCalculationInput, CdbCalculationOutput
│   │   └── Services/                  ← CdbCalculatorService, TaxRateService
│   └── B3.CdbCalculator.Web/          ← Frontend Angular 22
│       ├── src/app/core/              ← Serviço HTTP e modelos TypeScript
│       ├── src/app/features/          ← Componente de simulação
│       └── src/app/shared/            ← Componente de ícone SVG
└── tests/
    └── B3.CdbCalculator.Core.Tests/   ← Testes unitários (100% de cobertura)
```

---

## Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0 |
| [Node.js](https://nodejs.org) | 22.22.3 ou superior |

---

## Executar localmente

Os dois projetos devem ser executados simultaneamente em terminais separados.

### 1. API (.NET 10)

```bash
cd src/B3.CdbCalculator.Api
dotnet run
```

A API sobe em `http://localhost:7001`.
Documentação interativa disponível em `http://localhost:7001/scalar/v1`.

### 2. Frontend (Angular)

```bash
cd src/B3.CdbCalculator.Web
npm install
npm start
```

Acesse `http://localhost:4200`.

O proxy do Angular encaminha automaticamente as chamadas de `/api/*` para a API em `http://localhost:7001`. A API deve estar rodando antes de usar o frontend.

---

## Testes

### API — testes unitários (.NET)

```bash
dotnet test tests/B3.CdbCalculator.Core.Tests
```

### API — com relatório de cobertura

Instale a ferramenta de geração de relatório (necessário apenas uma vez):

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Execute os testes com coleta de cobertura:

```bash
dotnet test tests/B3.CdbCalculator.Core.Tests \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

Gere o relatório HTML:

```bash
reportgenerator \
  "-reports:.\TestResults\**\coverage.cobertura.xml" \
  "-targetdir:.\coverage-report" \
  "-reporttypes:Html;TextSummary"
```

O relatório estará disponível em `coverage-report/index.html`.

Cobertura atual: **100%** de linhas e branches na camada Core.

### Frontend — testes unitários (Angular)

```bash
cd src/B3.CdbCalculator.Web
npm test
```

---

## Contrato da API

### `POST /api/cdb/calculations`

**Requisição**

```json
{
  "initialAmount": 1000.00,
  "months": 12
}
```

| Campo | Tipo | Regra |
|---|---|---|
| `initialAmount` | decimal | Maior que zero |
| `months` | int | Maior que 1 |

**Resposta — 200 OK**

```json
{
  "initialAmount": 1000.00,
  "months": 12,
  "grossAmount": 1123.08,
  "grossEarnings": 123.08,
  "taxRate": 0.20,
  "taxAmount": 24.62,
  "netAmount": 1098.46
}
```

**Resposta — 400 Bad Request**

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Months": ["O prazo deve ser maior que 1 mês."]
  }
}
```

---

## Regras de negócio

### Fórmula de cálculo

```
VF = VI × [1 + (CDI × TB)]
```

Aplicada mês a mês com capitalização composta. O imposto incide somente sobre os rendimentos, nunca sobre o principal.

| Parâmetro | Valor |
|---|---|
| CDI | 0,9% ao mês |
| TB | 108% do CDI |
| Taxa mensal efetiva | 0,972% ao mês |

### Tabela regressiva de IR

| Prazo | Alíquota |
|---|---|
| Até 6 meses | 22,5% |
| Até 12 meses | 20,0% |
| Até 24 meses | 17,5% |
| Acima de 24 meses | 15,0% |

### Arredondamento

Todos os valores monetários são arredondados para duas casas decimais usando `MidpointRounding.AwayFromZero` — padrão comercial, diferente do arredondamento bancário padrão do .NET (`ToEven`).

---

## Decisões técnicas

**Por que `decimal` em vez de `double`?**
`double` usa representação binária de ponto flutuante, o que gera imprecisão em operações financeiras. `decimal` usa representação decimal de 28 dígitos, garantindo exatidão nos cálculos monetários.

**Por que a regra de negócio está no backend?**
Toda lógica de cálculo reside exclusivamente na API. O Angular apenas valida a entrada, chama o endpoint e exibe o resultado. Isso garante fonte única de verdade e impede que clientes diferentes calculem resultados divergentes.

**Por que `InjectionToken` para a URL base no Angular?**
Permite sobrescrever a URL da API no `TestBed` sem depender de variáveis de ambiente, tornando os testes do serviço HTTP completamente isolados.

**Por que sem MediatR ou FluentValidation?**
O projeto tem um endpoint e duas regras de validação simples. Adicionar essas bibliotecas seria complexidade sem benefício. `Data Annotations` resolve a validação; a separação de responsabilidades via interfaces resolve o desacoplamento.

---

## Princípios SOLID

| Princípio | Onde está aplicado |
|---|---|
| **S** — Single Responsibility | `CdbCalculatorService` calcula; `TaxRateService` determina a alíquota; o controller lida com HTTP; o Angular só valida entrada e exibe resultado |
| **O** — Open/Closed | Nova faixa de IR: altera apenas `TaxRateService`, sem tocar no calculador |
| **L** — Liskov Substitution | Qualquer implementação de `ITaxRateService` substitui a padrão sem quebrar o calculador |
| **I** — Interface Segregation | `ICdbCalculatorService` e `ITaxRateService` são contratos enxutos e focados em uma única responsabilidade |
| **D** — Dependency Inversion | `CdbCalculatorService` depende de `ITaxRateService`; o controller depende de `ICdbCalculatorService`; nenhum depende de implementações concretas |