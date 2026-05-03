# Infinite Craft Game

An AI-powered word combination game where you drag and fuse words together to discover new ones. Combine **Seed** + **Spark** and see what Claude invents.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 10, ASP.NET Core, Entity Framework Core, PostgreSQL |
| AI | Anthropic Claude Haiku (via Anthropic SDK) |
| Frontend | Angular 21, TypeScript 5.9, Tailwind CSS 4, PrimeNG |
| Architecture | MediatR (CQRS), Vertical Slice |

## How It Works

1. You start with six seed words: **Seed, Spark, Clay, Song, Coin, Dream**
2. Drag two words close together on the canvas to combine them
3. Claude generates a new word + emoji from the combination
4. The result's dictionary definition is fetched and displayed
5. If you're the first person globally to discover a combination, you get a special sound

Word combinations are cached in PostgreSQL so the same pair never calls the AI twice.

## Project Structure

```
InfiniteCraftGame/          # ASP.NET Core backend
├── Features/
│   ├── CombineWord/        # POST /CombineWord
│   └── GetAllWords/        # GET /GetAllWords
├── Services/
│   ├── AIService/          # Claude API wrapper with prompt caching
│   ├── DictionaryService/  # Fetches definitions from dictionaryapi.dev
│   └── WordGenerationService/
└── Infrastructure/
    ├── Data/               # EF Core DbContext
    └── Entities/           # UserWords, WordCombinations

InfiniteCraftClient/        # Angular frontend
└── src/app/
    ├── game/               # Canvas, drag-drop, combining logic
    ├── word/               # Word chip component
    └── sound.service.ts    # Procedural audio via Web Audio API
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js 20+
- PostgreSQL

### Backend

```bash
cd InfiniteCraftGame

# Set your connection string and Anthropic API key
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=infinitecraft;Username=...;Password=..."
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-..."

dotnet ef database update
dotnet run
```

### Frontend

```bash
cd InfiniteCraftClient
npm install
npm start
```

The app runs at `http://localhost:4200` and expects the API at `http://localhost:5000`.

## API

| Method | Path | Description |
|---|---|---|
| `POST` | `/CombineWord` | Combine two words; returns result + emoji + definition |
| `GET` | `/GetAllWords?userId={guid}` | Get all words discovered by a user |

Each browser session gets a unique `userId` stored in `localStorage` to track individual discoveries.
