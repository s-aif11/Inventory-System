# 🎮 Inventory System

A C# console-based inventory management system for a game store, implementing the **Memento design pattern** to save and restore inventory states.

## 📚 Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Design Pattern](#design-pattern)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage](#usage)

## ✨ Features

- **Full CRUD operations** – add, view, and update video games, board games, and accessories.
- **Sales & restocking** – process sales against current stock and restock existing items.
- **Trade operations** – batch multiple item quantity changes into one transaction, with automatic rollback if execution fails partway through.
- **Undo / rollback** – every mutating action snapshots the inventory first, so the last operation can always be undone.
- **Inventory history** – view a chronological log of past snapshots with descriptions and timestamps (last 10 kept).
- **Search** – by name, platform/type, price range, or low-stock threshold (< 10 units).
- **Console-based interface** – simple, menu-driven interaction.

## 🛠 Technologies

- **Language:** C#
- **Framework:** .NET Framework 4.7.2
- **Design Pattern:** Memento (Originator, Caretaker, Memento)
- **Storage:** In-memory collection (no persistence between runs)

## 🧩 Design Pattern

Implements the **Memento** pattern:

- **Originator** — `GameInventoryManager` (`Inventory Manager (Originator).cs`) owns the live inventory and can produce or restore a snapshot of it.
- **Memento** — `IInventoryMemento` implementations (`Memento Implemintation.cs`) hold an immutable copy of inventory state, along with a description and timestamp.
- **Caretaker** — `CareTaker.cs` and the `Stack<IInventoryMemento>` in `Program.cs` keep the last 10 snapshots and drive rollback from the menu.
- **Trade operations** — `Trade servicecs.cs` bundles multiple SKU quantity changes into one named transaction that rolls back automatically on failure.

## 📂 Project Structure

```
Inventory-System/
├── Program.cs                          # Console menu + application flow (entry point)
├── GameInventoryManager.cs             # Core business logic (add/sell/restock/search)
├── Inventory Manager (Originator).cs   # Originator — creates/restores mementos
├── Memento Implemintation.cs           # Memento — snapshot of inventory state
├── CareTaker.cs                        # Stores and manages snapshot history
├── Trade servicecs.cs                  # Trade/transaction logic with rollback
├── Inventory Models.cs                 # Data models (VideoGame, BoardGame, Accessory, etc.)
├── Properties/                         # Assembly info
├── App.config                          # Application configuration
└── Inventory System.csproj             # Project file
```

## 🚀 Getting Started

### Prerequisites

- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) (or the .NET SDK, which can build/run it via `dotnet`)
- Visual Studio / VS Code / any C# IDE (optional)

### Build & run

```bash
git clone https://github.com/s-aif11/Inventory-System.git
cd Inventory-System
dotnet restore
dotnet build
dotnet run
```

Or open `Inventory System.slnx` in Visual Studio and run from there.

On launch, the app preloads 7 sample items (video games, board games, and an accessory) so every menu option can be explored immediately.

## 🕹 Usage

The app runs as an interactive text menu:

```
1. View All Games            6. Trade Games (Risky Operation)
2. Add New Game              7. Rollback Last Operation
3. Update Game Information   8. View Inventory History
4. Process Sale              9. Search Games
5. Restock Games             0. Exit
```

Every add/update/sale/restock/trade first saves a memento, so option 7 always undoes the most recent change.
