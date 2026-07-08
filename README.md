# JogadoresApi

API REST para gerenciamento de jogadores de futebol, desenvolvida com **ASP.NET Core 10**, **Entity Framework Core** e **MySQL**.

A API permite cadastrar, consultar, atualizar, atualizar parcialmente e remover jogadores, com validações, paginação e persistência em banco de dados.

---

## 🚀 Tecnologias

- [.NET 10](https://dotnet.microsoft.com/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core 10](https://learn.microsoft.com/ef/core)
- [Microting.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Microting.EntityFrameworkCore.MySql) (provider MySQL compatível com EF Core 10)
- [AutoMapper](https://automapper.org/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [Swagger / Swashbuckle](https://swagger.io/)

---

## 📁 Estrutura do Projeto

```
JogadoresApi/
├── Controllers/            # Controllers da API
│   └── JogadorController.cs
├── Data/                   # DbContext do EF Core
│   └── JogadorContext.cs
├── Dtos/                   # Data Transfer Objects
│   ├── CriarJogadorDto.cs
│   └── AtualizarJogadorDto.cs
├── Migrations/             # Migrations do EF Core
├── Model/                  # Entidades do domínio
│   └── Jogador.cs
├── Profiles/               # Perfis do AutoMapper
│   └── JogadorProfile.cs
├── Properties/
├── appsettings.json
├── appsettings.Development.json.example
├── Program.cs
└── JogadoresApi.csproj
```

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

Copie o arquivo de exemplo:

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

### 2. Ajuste a connection string

Edite o `appsettings.Development.json` com os dados do seu MySQL:

```json
{
  "ConnectionStrings": {
    "JogadoresConnection": "Server=localhost;Database=Jogadores;User=SEU_USUARIO;Password=SUA_SENHA;"
  }
}
```

---

## 🗄️ Migrations

Após configurar o banco de dados, execute as migrations para criar as tabelas:

```bash
dotnet ef database update
```

Se precisar criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration
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
https://localhost:7016/swagger
```

---

## 📡 Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/jogador?pula=0&pega=5` | Lista jogadores com paginação |
| GET | `/jogador/{id}` | Busca um jogador pelo ID |
| POST | `/jogador` | Cadastra um novo jogador |
| PUT | `/jogador/{id}` | Atualiza todos os dados de um jogador |
| PATCH | `/jogador/{id}` | Atualiza parcialmente um jogador |
| DELETE | `/jogador/{id}` | Remove um jogador |

### Exemplos de requisições

#### POST /jogador

```json
{
  "nome": "Pelé",
  "posicao": "Atacante",
  "gols": 1000,
  "time": "Santos"
}
```

#### PATCH /jogador/1

```json
[
  { "op": "replace", "path": "/nome", "value": "Edson Arantes" }
]
```

---

## ✅ Validações

Os DTOs possuem validações com Data Annotations:

- `Nome`, `Posicao` e `Time`: obrigatórios, apenas letras e espaços, limite de caracteres
- `Gols`: obrigatório, valor maior ou igual a zero

---

## 🔒 Segurança

- O arquivo `appsettings.Development.json` está no `.gitignore` para proteger a connection string.
- O arquivo `appsettings.json` versionado não contém dados sensíveis.
- Antes de publicar em produção, use variáveis de ambiente ou um gerenciador de secrets.

---

## 📝 Licença

Este projeto foi desenvolvido para fins de estudo.
