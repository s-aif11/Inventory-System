using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStoreInventory
{
    class Program
    {
        static GameInventoryManager inventory = new GameInventoryManager();
        static Stack<IInventoryMemento> history = new Stack<IInventoryMemento>();

        public static void Main(string[] args)
        {
            InitializeSampleGames();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                DisplayHeader();
                DisplayMainMenu();

                Console.Write("\nEnter your choice (0-9): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllGames();
                        break;

                    case "2":
                        AddNewGame();
                        break;

                    case "3":
                        UpdateGame();
                        break;

                    case "4":
                        ProcessSale();
                        break;

                    case "5":
                        ProcessRestock();
                        break;

                    case "6":
                        TradeGames();
                        break;

                    case "7":
                        RollbackLastOperation();
                        break;

                    case "8":
                        ViewInventoryHistory();
                        break;

                    case "9":
                        SearchGames();
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("\nThank you for using Saif 's GameStore Inventory System!");
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("\nInvalid choice! Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayHeader()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("       Saif STORE INVENTORY SYSTEM");
            Console.WriteLine(@"  Applied (Memento Pattern + LSP Principle)");
            Console.WriteLine("==============================================\n");
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("MAIN MENU:");
            Console.WriteLine("1. View All Games");
            Console.WriteLine("2. Add New Game");
            Console.WriteLine("3. Update Game Information");
            Console.WriteLine("4. Process Sale");
            Console.WriteLine("5. Restock Games");
            Console.WriteLine("6. Trade Games (Risky Operation)");
            Console.WriteLine("7. Rollback Last Operation");
            Console.WriteLine("8. View Inventory History");
            Console.WriteLine("9. Search Games");
            Console.WriteLine("0. Exit");
            Console.WriteLine("==============================================");
        }

        static void InitializeSampleGames()
        {
            // Adding sample games to inventory
            inventory.AddGame(new VideoGame("VG001", "Cyberpunk 2077", 25, 699.99m, "CD Projekt Red", "RPG", "PS5"));
            inventory.AddGame(new VideoGame("VG002", "Fifa 26", 30, 849.99m, "EAsports", "Action RPG", "PS5"));
            inventory.AddGame(new VideoGame("VG003", "Call of Duty III", 35, 1199.99m, "Infinity Ward", "FPS", "Xbox Series X"));
            inventory.AddGame(new BoardGame("BG001", "Catan", 15, 549.99m, 3, 4, "Strategy"));
            inventory.AddGame(new BoardGame("BG002", "Ticket to Ride", 18, 449.99m, 2, 5, "Family"));
            inventory.AddGame(new BoardGame("BG003", "Pandemic", 14, 1199.99m, 2, 4, "Cooperative"));
            inventory.AddGame(new Accessory("ACC001", "Xbox Wireless Headset", 25, 2399.99m, "Xbox Series X", "Audio"));

            Console.WriteLine("Sample games loaded successfully!\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ViewAllGames()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("INVENTORY OVERVIEW:\n");

            var allGames = inventory.GetAllGames();
            if (!allGames.Any())
            {
                Console.WriteLine("No games in inventory.");
                WaitForUser();
                return;
            }

            decimal totalValue = 0;
            int totalItems = 0;

            // Video Games
            var videoGames = allGames.OfType<VideoGame>().ToList();
            if (videoGames.Any())
            {
                Console.WriteLine("=== VIDEO GAMES ===");
                foreach (var game in videoGames)
                {
                    Console.WriteLine(game.DisplayInfo());
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            // Board Games
            var boardGames = allGames.OfType<BoardGame>().ToList();
            if (boardGames.Any())
            {
                Console.WriteLine("\n=== BOARD GAMES ===");
                foreach (var game in boardGames)
                {
                    Console.WriteLine(game.DisplayInfo());
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            // Accessories
            var accessories = allGames.OfType<Accessory>().ToList();
            if (accessories.Any())
            {
                Console.WriteLine("\n=== ACCESSORIES ===");
                foreach (var game in accessories)
                {
                    Console.WriteLine(game.DisplayInfo());
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            Console.WriteLine("\n==============================================");
            Console.WriteLine($"Total Games/Accessories: {allGames.Count}");
            Console.WriteLine($"Total Units in Stock: {totalItems}");
            Console.WriteLine($"Total Inventory Value: ${totalValue:F2}");

            WaitForUser();
        }

        static void AddNewGame()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("ADD NEW GAME:\n");

            Console.WriteLine("Select game type:");
            Console.WriteLine("1. Video Game");
            Console.WriteLine("2. Board Game");
            Console.WriteLine("3. Accessory");
            Console.Write("\nChoice: ");

            string typeChoice = Console.ReadLine();

            try
            {
                Console.Write("\nEnter Game_id: ");
                string sku = Console.ReadLine();

                // Check if Game_id already exists
                if (inventory.GetGame(sku) != null)
                {
                    Console.WriteLine($"\n Game_id {sku} already exists!");
                    WaitForUser();
                    return;
                }

                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                Console.Write("Enter Price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Game newGame = null;

                switch (typeChoice)
                {
                    case "1": // Video Game
                        Console.Write("Developer: ");
                        string developer = Console.ReadLine();

                        Console.Write("Genre: ");
                        string genre = Console.ReadLine();

                        Console.Write("Platform (PS5/Xbox/Switch/PC): ");
                        string platform = Console.ReadLine();

                        newGame = new VideoGame(sku, name, quantity, price, developer, genre, platform);
                        break;

                    case "2": // Board Game
                        Console.Write("Min Players: ");
                        int minPlayers = int.Parse(Console.ReadLine());

                        Console.Write("Max Players: ");
                        int maxPlayers = int.Parse(Console.ReadLine());

                        Console.Write("Category: ");
                        string category = Console.ReadLine();

                        newGame = new BoardGame(sku, name, quantity, price, minPlayers, maxPlayers, category);
                        break;

                    case "3": // Accessory
                        Console.Write("Compatible With: ");
                        string compatibleWith = Console.ReadLine();

                        Console.Write("Type (Controller/Headset/Charger/etc): ");
                        string accessoryType = Console.ReadLine();

                        newGame = new Accessory(sku, name, quantity, price, compatibleWith, accessoryType);
                        break;

                    default:
                        Console.WriteLine("Invalid type selected.");
                        WaitForUser();
                        return;
                }

                // Save state before adding
                SaveInventoryState($"Before adding new game: {name}");

                // Add the game
                inventory.AddGame(newGame);

                Console.WriteLine($"\n Successfully added {name} to inventory!");
                Console.WriteLine(newGame.DisplayInfo());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }

            WaitForUser();
        }

        static void UpdateGame()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("UPDATE GAME INFORMATION:\n");

            Console.Write("Enter Game Game_id to update: ");
            string sku = Console.ReadLine();

            var game = inventory.GetGame(sku);
            if (game == null)
            {
                Console.WriteLine($"\n Game with Game_id {sku} not found!");
                WaitForUser();
                return;
            }

            Console.WriteLine($"\nCurrent Information:");
            Console.WriteLine(game.DisplayInfo());

            try
            {
                // Save memento before update
                SaveInventoryState($"Before updating {game.Name}");

                Console.Write("\nEnter new quantity (press Enter to keep current): ");
                string qtyInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(qtyInput))
                {
                    int newQty = int.Parse(qtyInput);
                    if (newQty < 0)
                    {
                        Console.WriteLine(" Quantity cannot be negative!");
                        WaitForUser();
                        return;
                    }
                    game.UpdateQuantity(newQty);
                }

                Console.Write("Enter new price (press Enter to keep current): ");
                string priceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(priceInput))
                {
                    decimal newPrice = decimal.Parse(priceInput);
                    if (newPrice < 0)
                    {
                        Console.WriteLine(" Price cannot be negative!");
                        WaitForUser();
                        return;
                    }

                    // Update price based on game type
                    if (game is VideoGame vg)
                        vg.Price = newPrice;
                    else if (game is BoardGame bg)
                        bg.Price = newPrice;
                    else if (game is Accessory acc)
                        acc.Price = newPrice;
                }

                Console.WriteLine($"\n Successfully updated {game.Name}!");
                Console.WriteLine("\nUpdated Information:");
                Console.WriteLine(game.DisplayInfo());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }

            WaitForUser();
        }

        static void ProcessSale()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("PROCESS SALE:\n");

            Console.Write("Enter Game Game_id to sell: ");
            string sku = Console.ReadLine();

            var game = inventory.GetGame(sku);
            if (game == null)
            {
                Console.WriteLine($"\n Game with Game_id {sku} not found!");
                WaitForUser();
                return;
            }

            Console.WriteLine($"\nGame: {game.Name}");
            Console.WriteLine($"Available Quantity: {game.Quantity}");
            Console.WriteLine($"Price: ${game.Price:F2}");

            Console.Write("\nEnter quantity to sell: ");
            string qtyInput = Console.ReadLine();

            if (string.IsNullOrEmpty(qtyInput))
            {
                Console.WriteLine("\n Quantity is required!");
                WaitForUser();
                return;
            }

            try
            {
                int saleQty = int.Parse(qtyInput);

                if (saleQty <= 0)
                {
                    Console.WriteLine("\n Quantity must be positive!");
                    WaitForUser();
                    return;
                }

                if (saleQty > game.Quantity)
                {
                    Console.WriteLine($"\n Not enough stock! Only {game.Quantity} available.");
                    WaitForUser();
                    return;
                }

                // Save memento before sale
                SaveInventoryState($"Before selling {saleQty} of {game.Name}");

                int newQty = game.Quantity - saleQty;
                game.UpdateQuantity(newQty);

                decimal totalSale = saleQty * game.Price;

                Console.WriteLine($"\n Sale processed successfully!");
                Console.WriteLine($"Sold: {saleQty} x {game.Name}");
                Console.WriteLine($"Unit Price: ${game.Price:F2}");
                Console.WriteLine($"Total: ${totalSale:F2}");
                Console.WriteLine($"Remaining stock: {newQty}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Invalid quantity format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }

            WaitForUser();
        }

        static void ProcessRestock()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("RESTOCK GAMES:\n");

            Console.Write("Enter Game Game_id to restock: ");
            string sku = Console.ReadLine();

            var game = inventory.GetGame(sku);
            if (game == null)
            {
                Console.WriteLine($"\n Game with Game_id {sku} not found!");
                WaitForUser();
                return;
            }

            Console.WriteLine($"\nGame: {game.Name}");
            Console.WriteLine($"Current Quantity: {game.Quantity}");

            Console.Write("\nEnter quantity to add: ");
            string qtyInput = Console.ReadLine();

            if (string.IsNullOrEmpty(qtyInput))
            {
                Console.WriteLine("\n Quantity is required!");
                WaitForUser();
                return;
            }

            try
            {
                int restockQty = int.Parse(qtyInput);

                if (restockQty <= 0)
                {
                    Console.WriteLine("\n Quantity must be positive!");
                    WaitForUser();
                    return;
                }

                // Save memento before restock
                SaveInventoryState($"Before restocking {restockQty} of {game.Name}");

                int newQty = game.Quantity + restockQty;
                game.UpdateQuantity(newQty);

                Console.WriteLine($"\n Restock successful!");
                Console.WriteLine($"Added: {restockQty} x {game.Name}");
                Console.WriteLine($"Old quantity: {game.Quantity - restockQty}");
                Console.WriteLine($"New total: {newQty}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Invalid quantity format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }

            WaitForUser();
        }

        static void TradeGames()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("TRADE GAMES (Risky Operation):\n");

            Console.WriteLine("This operation allows trading multiple games at once.");
            Console.WriteLine("The system will automatically rollback if any trade fails.\n");

            Console.Write("Enter trade description: ");
            string description = Console.ReadLine();

            if (string.IsNullOrEmpty(description))
            {
                Console.WriteLine("\n Description is required!");
                WaitForUser();
                return;
            }

            var tradeOperation = new TradeOperation($"TRADE{DateTime.Now:HHmmss}", description);

            bool addingItems = true;
            while (addingItems)
            {
                Console.Write("\nEnter Game Game_id (or 'done' to finish): ");
                string sku = Console.ReadLine();

                if (sku.ToLower() == "done")
                    break;

                var game = inventory.GetGame(sku);
                if (game == null)
                {
                    Console.WriteLine(" Game not found! Try again.");
                    continue;
                }

                Console.WriteLine($"Game: {game.Name}, Available: {game.Quantity}");
                Console.Write("Quantity (+ to add, - to remove from inventory): ");
                string qtyInput = Console.ReadLine();

                try
                {
                    int qtyChange = int.Parse(qtyInput);
                    tradeOperation.AddItemChange(sku, qtyChange);
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Invalid quantity format! Try again.");
                    continue;
                }

                Console.Write("Add another item? (y/n): ");
                addingItems = Console.ReadLine().ToLower() == "y";
            }

            if (tradeOperation.ItemChanges.Count == 0)
            {
                Console.WriteLine("\n No items added to trade!");
                WaitForUser();
                return;
            }

            // Display trade summary
            Console.WriteLine("\n══════════════════════════════════════════");
            Console.WriteLine("TRADE SUMMARY:");
            Console.WriteLine($"ID: {tradeOperation.TradeId}");
            Console.WriteLine($"Description: {tradeOperation.Description}");
            Console.WriteLine($"Items: {tradeOperation.ItemChanges.Count}");
            Console.WriteLine("------------------------------------------");

            foreach (var change in tradeOperation.ItemChanges)
            {
                var game = inventory.GetGame(change.Key);
                string action = change.Value > 0 ? "Adding" : "Removing";
                Console.WriteLine($"  {action} {Math.Abs(change.Value)} x {game.Name}");
            }

            Console.Write("\nExecute this trade? (y/n): ");
            if (Console.ReadLine().ToLower() != "y")
            {
                Console.WriteLine("\nTrade cancelled.");
                WaitForUser();
                return;
            }

            // Execute the risky trade
            Console.WriteLine("\nExecuting trade...");
            bool success = inventory.ExecuteRiskyTrade(tradeOperation);

            if (success)
            {
                Console.WriteLine("\n Trade completed successfully!");
            }
            else
            {
                Console.WriteLine("\n Trade failed and was rolled back.");
            }

            WaitForUser();
        }

        static void RollbackLastOperation()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("ROLLBACK LAST OPERATION:\n");

            if (history.Count == 0)
            {
                Console.WriteLine(" No operations to rollback!");
                WaitForUser();
                return;
            }

            Console.WriteLine($"Last operation: {history.Peek().GetDescription()}");
            Console.WriteLine($"Saved at: {history.Peek().GetSnapshotDate():g}");

            Console.Write("\nAre you sure you want to rollback? (y/n): ");
            if (Console.ReadLine().ToLower() != "y")
            {
                Console.WriteLine("Rollback cancelled.");
                WaitForUser();
                return;
            }

            var memento = history.Pop();
            memento.Restore();

            Console.WriteLine("\n Successfully rolled back to previous state!");

            WaitForUser();
        }

        static void ViewInventoryHistory()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("INVENTORY HISTORY:\n");

            if (history.Count == 0)
            {
                Console.WriteLine("No history available.");
                WaitForUser();
                return;
            }

            Console.WriteLine($"Total snapshots: {history.Count}\n");

            // Convert stack to array and reverse for chronological order
            var historyArray = history.ToArray();
            Array.Reverse(historyArray);

            int index = 1;
            foreach (var memento in historyArray)
            {
                Console.WriteLine($"{index++}. {memento.GetDescription()}");
                Console.WriteLine($"   Time: {memento.GetSnapshotDate():g}");
                Console.WriteLine();
            }

            WaitForUser();
        }

        static void SearchGames()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine("SEARCH GAMES:\n");

            Console.WriteLine("Search by:");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Platform/Type");
            Console.WriteLine("3. Price Range");
            Console.WriteLine("4. Low Stock (less than 10)");
            Console.Write("\nChoice: ");

            string choice = Console.ReadLine();
            List<Game> results = new List<Game>();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter search term: ");
                        string term = Console.ReadLine();
                        results = inventory.SearchByName(term);
                        break;

                    case "2":
                        Console.Write("Enter platform/type (PS5/Xbox/Switch/Board/Accessory): ");
                        string platform = Console.ReadLine();
                        results = inventory.SearchByPlatform(platform);
                        break;

                    case "3":
                        Console.Write("Minimum price: ");
                        decimal min = decimal.Parse(Console.ReadLine());
                        Console.Write("Maximum price: ");
                        decimal max = decimal.Parse(Console.ReadLine());
                        results = inventory.SearchByPriceRange(min, max);
                        break;

                    case "4":
                        results = inventory.GetLowStockGames(10);
                        break;

                    default:
                        Console.WriteLine(" Invalid choice!");
                        WaitForUser();
                        return;
                }

                Console.WriteLine($"\nFound {results.Count} game(s):\n");

                if (results.Count == 0)
                {
                    Console.WriteLine("No games found.");
                }
                else
                {
                    foreach (var game in results)
                    {
                        Console.WriteLine(game.DisplayInfo());
                        Console.WriteLine();
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Invalid input format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }

            WaitForUser();
        }

        static void SaveInventoryState(string description)
        {
            var memento = inventory.CreateMemento(description);
            history.Push(memento);

            // Keep only last 10 states
            if (history.Count > 10)
            {
                var temp = new Stack<IInventoryMemento>();
                for (int i = 0; i < 10; i++)
                {
                    temp.Push(history.Pop());
                }
                history = temp;
            }
        }

        static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}