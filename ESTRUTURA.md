# Estrutura e Responsabilidade das Pastas

Este documento resume a função de cada pasta da **JogadoresApi** e como elas se encaixam no fluxo de uma requisição.

## Fluxo de uma requisição

```
HTTP Request
   ↓
Controllers   → recebe a requisição, valida (via [ApiController] + Data Annotations) e devolve a resposta
   ↓
Profiles      → AutoMapper converte DTO ↔ Model (entrada e saída)
   ↓
Data          → JogadorContext (EF Core) traduz para SQL e conversa com o MySQL
   ↓
Model         → entidades que representam as tabelas
```

As **Migrations** ficam fora do fluxo de execução: elas versionam o esquema do banco.

---

## 📁 `Model/`
**Responsabilidade:** definir as entidades de domínio — as classes que viram tabelas no banco — e suas regras de validação.

**O que tem aqui:** `Jogador.cs`, `Time.cs`, `Liga.cs`.

**Regras adotadas neste projeto:**
- A chave primária usa `[Key]` (convenção `Id`).
- Validações ficam no **Model** via Data Annotations (`[Required]`, `[RegularExpression]`, `[MaxLength]`), não no controller.
- Relacionamentos são declarados por propriedades de navegação; o EF Core infere as FKs automaticamente:
  - `Jogador` → `Time` (N:1) via `TimeId` + `Time`.
  - `Time` ↔ `Liga` (N:N) via `ICollection<Liga>` e `ICollection<Time>` (tabela de junção `LigaTime` criada pelo EF).
  - `Time` → `Jogador` (1:N) via `ICollection<Jogador> Elenco`.
- Campos numéricos obrigatórios usam `int?` (ex.: `Gols`), porque `[Required]` não falha em `int` (valor padrão `0`).

---

## 📁 `Data/`
**Responsabilidade:** concentrar o acesso ao banco via `DbContext` do EF Core.

**O que tem aqui:** `JogadorContext.cs`, que expõe os `DbSet<>` de cada entidade:
```csharp
public DbSet<Jogador> Jogadores { get; set; }
public DbSet<Time> Times { get; set; }
public DbSet<Liga> Ligas { get; set; }
```

**Regras adotadas:**
- É o único ponto que conhece o provedor (MySQL/Microting). A connection string vem de `appsettings.Development.json`.
- Consultas com relacionamentos usam `.Include(...)` para carregar navegações (ex.: `.Include(j => j.Time)`).
- Não contém regra de negócio — só configuração de acesso a dados.

---

## 📁 `Controllers/`
**Responsabilidade:** expor os endpoints HTTP e orquestrar a requisição — receber DTO, chamar o `DbContext`/`AutoMapper` e retornar a resposta adequada.

**O que tem aqui:** `JogadorController.cs`, `TimeController.cs`, `LigaController.cs` (um por recurso).

**Padrão de cada controller:**
- `[ApiController]` + `[Route("[controller]")]` → rota `/jogador`, `/time`, `/liga`.
- GET com paginação: `[FromQuery] int pula = 0, int pega = 5` → `Skip().Take()`.
- POST recebe DTO de criação e retorna `CreatedAtAction` (201).
- PUT (`AtualizarXxxDto`) retorna `NoContent` (204).
- PATCH usa `JsonPatchDocument<T>` (precisa do header `Content-Type: application/json-patch+json`).
- DELETE retorna `NoContent` (204).
- `404 NotFound` quando o recurso não existe.

**Regra importante:** o controller **não** valida manualmente nem monta SQL — ele só coordena `DbContext` + `AutoMapper`.

---

## 📁 `Dtos/`
**Responsabilidade:** definir o "contrato" de dados que entra e sai da API, desacoplado das entidades do banco.

**Dois tipos de DTO neste projeto:**

| Tipo | Função | Exemplos |
|------|--------|----------|
| **Entrada** (`Base` + `Criar`/`Atualizar`) | dados que o cliente envia no POST/PUT | `JogadorBaseDto` → `CriarJogadorDto` / `AtualizarJogadorDto` |
| **Leitura** (`Read` / `Resumo`) | formato da resposta, controlando o que aparece | `ReadJogadorDto`, `ReadTimeDto`, `ReadLigaDto`, `JogadorResumoDto`, `TimeResumoDto`, `LigaResumoDto` |

**Convenções adotadas:**
- DTOs de entrada usam **classe base abstrata** para não duplicar campos/validações entre criação e atualização.
- `Read{Entity}Dto` é a resposta do **próprio endpoint**; `{Entity}ResumoDto` é a versão **enxuta aninhada** dentro de outro DTO (evita recursão e payload grande):
  - `ReadTimeDto` → tem `List<JogadorResumoDto> Elenco` e `List<LigaResumoDto> Ligas`.
  - `ReadLigaDto` → tem `List<TimeResumoDto> Times`.
  - `ReadJogadorDto` → mostra `timeId` e o nome do time em `Time` (derivado da navegação).
- POST/PUT recebem apenas **FKs** (ex.: `timeId`, `ligasIds`); os nomes vêm via `Include` na leitura.

---

## 📁 `Profiles/`
**Responsabilidade:** centralizar os mapeamentos do **AutoMapper** entre DTOs e entidades.

**O que tem aqui:** `JogadorProfile.cs`, `TimeProfile.cs`, `LigaProfile.cs` (um `Profile` por entidade), registrados no `Program.cs` com `AddAutoMapper(typeof(Program))`.

**Padrão:**
- `CreateMap<Origem, Destino>()` mapeia automaticamente propriedades de **mesmo nome**.
- Quando o nome ou o tipo difere, usa `.ForMember(...).MapFrom(...)` — exemplo:
  ```csharp
  CreateMap<Jogador, ReadJogadorDto>()
      .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time != null ? src.Time.Nome : null));
  ```
  (a origem `Time` é um objeto `Time`; o destino `Time` é uma `string` com o nome).

---

## 📁 `Migrations/`
**Responsabilidade:** versionar o esquema do banco de forma incremental e reproduzível.

**O que tem aqui:** pares de arquivos por migration (`<timestamp>_<Nome>.cs` + `.Designer.cs`) e o `JogadorContextModelSnapshot.cs` (estado atual do modelo).

**Histórico deste projeto:**
1. `InitialCreate` — cria `Jogadores`.
2. `AdicionarTabelaTimes` — cria `Times`.
3. `AdicionarTabelaLigas` — cria `Ligas`.
4. `RemoverColunaTimeDeJogador` — remove a string `Time` de `Jogadores`.
5. `AdicionarLigaEmTime` — adiciona `LigaId` em `Times` (1:N).
6. `TimeVariasLigas` — transforma em N:N: remove `LigaId`, cria a junção `LigaTime` e **migra os dados existentes** via SQL antes de dropar a coluna.

**Comandos frequentes:**
```bash
dotnet ef migrations add <Nome>   # gera uma nova migration a partir do Model atual
dotnet ef database update         # aplica as migrations pendentes no banco
dotnet ef migrations remove       # desfaz a última migration (antes de aplicar)
```
> Migrations podem ser editadas manualmente (ex.: reordenar operações ou adicionar `migrationBuilder.Sql(...)`) quando necessário.

---

## Tabela-resumo

| Pasta | Responsabilidade | Conhece o banco? | Vai para o cliente? |
|-------|------------------|:----------------:|:-------------------:|
| `Model` | Entidades + validações (viram tabelas) | define o esquema | não (só via DTO) |
| `Data` | `DbContext` e acesso a dados | ✅ | não |
| `Controllers` | Endpoints HTTP e orquestração | usa o `DbContext` | retorna DTO |
| `Dtos` | Contrato de entrada/saída | não | ✅ |
| `Profiles` | Mapeamentos DTO ↔ Model (AutoMapper) | não | não |
| `Migrations` | Versionamento do esquema | ✅ (DDL) | não |
