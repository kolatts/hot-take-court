# ⚖️ Verdict — Hot Takes on Trial

A browser-based multiplayer party game where players argue hot takes they may not even believe, then vote on who was most convincing.

**Play at:** https://kolatts.github.io/verdict/

---

## How to play

### Setup

1. One player creates a room and picks 1–5 hot takes (provocative statements like *"Open offices were a war crime"*).
2. They share the 4-letter room code. Everyone joins — minimum 3 players, no maximum.
3. The room locks when at least 3 players are in. The host starts the trial.

### Each round

**Argument phase**

Each player is secretly assigned a side — **Prosecution** (argue *for* the take) or **Defense** (argue *against*). You don't know who got which side. Write a 2–3 sentence argument making your assigned position as convincing as possible. You submit privately; no one sees what others wrote until everyone is done.

**Vote phase**

All arguments are shown anonymously — you can see PROSECUTION or DEFENSE labels but not who wrote what. Cast two votes:
- **Best argument** — whose argument was most convincing, regardless of whether you agree with it?
- **Your real stance** — do you personally Agree or Disagree with the take?

**Reveal phase**

Authors are unmasked. You see who wrote what, which side they argued, and what they actually believe. Scores update:
- **+1 point** for each best-arg vote you receive.

### Scoring

| Outcome | Points |
|---|---|
| Each best-arg vote received | +1 |
| Getting zero best-arg votes | 0, plus 🔨 Held in Contempt |

**Held in Contempt** means nobody found your argument convincing enough to vote for it that round. Zero best-arg votes = contempt of court.

After all rounds the final leaderboard shows cumulative points and contempt counts.

---

## Tech stack

| Layer | Tech |
|---|---|
| Frontend | Vanilla HTML + JS, GitHub Pages (`/docs`) |
| Backend | C# .NET 10, Azure Functions v4, isolated worker, Consumption plan |
| Storage | Azure Table Storage (`Azure.Data.Tables`) |
| Identity | GUID in URL hash — no accounts, no auth |
| Infra | Bicep (`infra/main.bicep`), deployed via GitHub Actions + OIDC |

---

## Local dev

```bash
# 1. Start Azurite (storage emulator)
azurite

# 2. Start Functions
cd src/Verdict.Functions && func start

# 3. Serve frontend
npx http-server docs -p 8080
```

Open http://localhost:8080.

## Running tests

```bash
# Unit tests (none yet beyond E2E)
dotnet test

# E2E (Playwright / Reqnroll — requires local stack running)
dotnet test tests/Verdict.E2E/Verdict.E2E.csproj
```

CI runs the full stack (Azurite + Functions + http-server) automatically before deploying to Azure.
