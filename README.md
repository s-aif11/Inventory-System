# 🎮 Inventory System

A C# console‑based inventory management system for a game store, implementing the **Memento design pattern** to save and restore inventory states.

---

## 📚 Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Design Pattern](#design-pattern)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## ✨ Features

- **Full CRUD operations** – Add, view, update, and delete game items.
- **State snapshot** – Save the current inventory to a memento.
- **Undo / rollback** – Restore any previous inventory state.
- **Console‑based interface** – Simple and interactive menu.
- **Separation of concerns** – Clear division between data models, business logic, and state management.

---

## 🛠 Technologies

- **Language:** C#  
- **Framework:** .NET Framework 4.x (or .NET Core, adjust as needed)  
- **Design Pattern:** Memento (Originator, Caretaker, Memento)  
- **Storage:** In‑memory collection (can be extended to file/database)

## 📂 Project Structure

Inventory-System/
├── Properties/ # Assembly info and settings
├── App.config # Application configuration
├── CareTaker.cs # Stores and manages mementos (undo history)
├── Class1.cs # (Legacy/placeholder – remove or rename)
├── Class2.cs # (Legacy/placeholder – remove or rename)
├── GameInventoryManager.cs # Core business logic (add/remove/update items)
├── Inventory Manager (Originator).cs # Originator – creates/restores mementos
├── Inventory Models.cs # Data models (Item, InventoryState, etc.)
├── Inventory System.csproj # Project file
├── .gitattributes
└── .gitignore



> **Note:** `Class1.cs` and `Class2.cs` appear to be default files. You can delete them after verifying they are not used.

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 4.x or later)
- Visual Studio / VS Code / any C# IDE (optional)

### Installation

1. **Clone** the repository:
   ```bash
   git clone https://github.com/s-aif11/Inventory-System.git
   cd Inventory-System
---

2. **Restore** dependencies (if any):
   ```bash
   dotnet restore

---

3. **Build** the project:
   ```bash
   dotnet build
---

Running the Application
```bash
dotnet run
---













