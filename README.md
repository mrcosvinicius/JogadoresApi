# JogadoresApi

API REST para gerenciamento de **jogadores, times e ligas** de futebol, desenvolvida com **ASP.NET Core 10**, **Entity Framework Core** e **MySQL**.

A API permite cadastrar, consultar, atualizar (total e parcialmente) e remover registros dos três recursos, com validações, paginação, relacionamentos e persistência em banco de dados.

---

## 🚀 Tecnologias

- [.NET 10](https://dotnet.microsoft.com/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core 10](https://learn.microsoft.com/ef/core)
- [Microting.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Microting.EntityFrameworkCore.MySql) (provider MySQL compatível com EF Core 10)
- [AutoMapper](https://automapper.org/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json) (inclui suporte a JSON Patch)
- [Swagger / Swashbuckle](https://swagger.io/)

---

## 📁 Estrutura do Projeto

```
JogadoresApi/
├── Controllers/                # Endpoints HTTP (um por recurso)
│   ├── JogadorController.cs
│   ├── TimeController.cs
│   └── LigaController.cs
├── Data/                       # DbContext do EF Core
│   └── JogadorContext.cs
├── Dtos/                       # Contratos de entrada e saída
│   ├── JogadorBaseDto.cs       # base abstrata → Criar/Atualizar
│   ├── TimeBaseDto.cs
│   ├── LigaBaseDto.cs
│   ├── ReadJogadorDto.cs       # respostas dos endpoints
│   ├── ReadTimeDto.cs
│   ├── ReadLigaDto.cs
│   ├── JogadorResumoDto.cs     # versões enxutas aninhadas
│   ├── TimeResumoDto.cs
│   └── LigaResumoDto.cs
├── Migrations/                 # Versionamento do esquema (EF Core)
├── Model/                      # Entidades do domínio
│   ├── Jogador.cs
│   ├── Time.cs
│   └── Liga.cs
├── Profiles/                   # Mapeamentos do AutoMapper
│   ├── JogadorProfile.cs
│   ├── TimeProfile.cs
│   └── LigaProfile.cs
├── Properties/
├── appsettings.json
├── appsettings.Development.json.example
├── seed.sql                    # Dados de exemplo (ligas, times e jogadores)
├── ESTRUTURA.md                # Responsabilidade de cada pasta
├── RELACIONAMENTOS.md          # Detalhes dos relacionamentos
├── Program.cs
└── JogadoresApi.csproj
```

> Para uma explicação detalhada da função de cada pasta, veja [`ESTRUTURA.md`](ESTRUTURA.md).

---

## ⚙️ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL](https://www.mysql.com/) instalado e em execução
- [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) (ferramenta de CLI do Entity Framework)

Para instalar o `dotnet-ef`:

```bash
dotnet tool install --global dotnet-ef
```

---

## 🔧 Configuração do Banco de Dados

A connection string fica no arquivo `appsettings.Development.json`, que **não é versionado** por conter dados sensíveis.

### 1. Crie o arquivo de configuração local

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

### 2. Ajuste a connection string

```json
{
  "ConnectionStrings": {
    "JogadoresConnection": "Server=localhost;Database=Jogadores;User=SEU_USUARIO;Password=SUA_SENHA;"
  }
}
```

---

## 🗄️ Migrations

Após configurar o banco, execute as migrations para criar as tabelas:

```bash
dotnet ef database update
```

Se precisar criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration
```

---

## 🌱 Dados de exemplo (seed)

Para povoar o banco com 2 ligas (La Liga e Premier League), 3 times em cada uma e 5 jogadores por time:

```bash
mysql -uSEU_USUARIO -pSUA_SENHA -D Jogadores < seed.sql
```

---

## ▶️ Como Executar

```bash
dotnet run
```

A API estará disponível em:

- HTTP: `http://localhost:5292`
- HTTPS: `https://localhost:7016`

Acesse o Swagger para testar os endpoints:

```
http://localhost:5292/swagger
```

---

## 🧩 Modelagem e Relacionamentos

```
Ligas (Id, Nome)
   ↑↓ N:N (tabela de junção LigaTime)
Times (Id, Nome, Estadio)
   ↓ 1:N
Jogadores (Id, Nome, Posicao, Gols, TimeId)
```

- **Time ↔ Liga:** relação **muitos-para-muitos** — um time pode participar de várias ligas (tabela de junção `LigaTime`, criada pelo EF Core).
- **Time → Jogador:** relação **um-para-muitos** — `Jogador.TimeId` é a FK.
- POST/PUT recebem apenas **FKs** (`timeId`, `ligasIds`); os nomes são resolvidos na leitura via `Include`.

> Mais detalhes em [`RELACIONAMENTOS.md`](RELACIONAMENTOS.md).

---

## 📡 Endpoints

Os três recursos seguem o mesmo padrão. O GET de listagem aceita paginação (`?pula=0&pega=5`).

### Jogador — `/jogador`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/jogador` | Lista jogadores (paginação) |
| GET | `/jogador/{id}` | Busca um jogador pelo ID |
| POST | `/jogador` | Cadastra um jogador |
| PUT | `/jogador/{id}` | Atualiza todos os dados |
| PATCH | `/jogador/{id}` | Atualiza parcialmente |
| DELETE | `/jogador/{id}` | Remove um jogador |

### Time — `/time`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/time` | Lista times (com elenco e ligas) |
| GET | `/time/{id}` | Busca um time pelo ID |
| POST | `/time` | Cadastra um time |
| PUT | `/time/{id}` | Atualiza todos os dados (substitui as ligas) |
| PATCH | `/time/{id}` | Atualiza parcialmente |
| DELETE | `/time/{id}` | Remove um time |

### Liga — `/liga`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/liga` | Lista ligas (com seus times) |
| GET | `/liga/{id}` | Busca uma liga pelo ID |
| POST | `/liga` | Cadastra uma liga |
| PUT | `/liga/{id}` | Atualiza todos os dados |
| PATCH | `/liga/{id}` | Atualiza parcialmente |
| DELETE | `/liga/{id}` | Remove uma liga |

---

## 📦 Formato das respostas

As respostas usam DTOs de leitura para controlar o que é exposto:

- `GET /jogador` → `ReadJogadorDto`: `id`, `nome`, `posicao`, `gols`, `timeId` e `time` (**nome do time**, derivado do relacionamento).
- `GET /time` → `ReadTimeDto`: `id`, `nome`, `estadio`, `ligas` (resumo) e `elenco` (resumo dos jogadores).
- `GET /liga` → `ReadLigaDto`: `id`, `nome` e `times` (resumo).

---

## ✍️ Exemplos de requisições

### POST /jogador

```json
{
  "nome": "Pelé",
  "posicao": "Atacante",
  "gols": 1000,
  "timeId": 1
}
```

**Resposta (201):**

```json
{
  "id": 15,
  "nome": "Pelé",
  "posicao": "Atacante",
  "gols": 1000,
  "timeId": 1,
  "time": "Santos"
}
```

### POST /time

```json
{
  "nome": "Flamengo",
  "estadio": "Maracanã",
  "ligasIds": [1, 2]
}
```

### POST /liga

```json
{
  "nome": "La Liga"
}
```

### PATCH /jogador/1

> Requer o header `Content-Type: application/json-patch+json`.

```json
[
  { "op": "replace", "path": "/gols", "value": 1001 }
]
```

---

## ✅ Validações

As validações usam **Data Annotations** (nos Models e nos DTOs de entrada); o `[ApiController]` retorna `400` automaticamente em caso de erro:

- `Nome`, `Posicao`, `Estadio`: obrigatórios, apenas letras e espaços, com limite de caracteres.
- `Gols`: obrigatório, inteiro maior ou igual a zero (`int?` + `[Range]`).
- FKs (`timeId`, `ligasIds`): opcionais.

---

## 🔒 Segurança

- O arquivo `appsettings.Development.json` está no `.gitignore` para proteger a connection string.
- O arquivo `appsettings.json` versionado não contém dados sensíveis.
- Antes de publicar em produção, use variáveis de ambiente ou um gerenciador de secrets.

---

## 📝 Licença

Este projeto foi desenvolvido para fins de estudo.
