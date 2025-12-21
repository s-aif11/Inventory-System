using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStoreInventory
{
    // ==================== MEMENTO PATTERN ====================
    public interface IInventoryMemento
    {
        DateTime GetSnapshotDate();
        string GetDescription();
        void Restore();
    }

    public class GameInventoryMemento : IInventoryMemento
    {
        public DateTime SnapshotDate { get; }
        public string Description { get; }
        private readonly GameInventoryManager _originator;
        private readonly List<Game> _state;

        public GameInventoryMemento(GameInventoryManager originator, List<Game> state, string description)
        {
            SnapshotDate = DateTime.UtcNow;
            Description = description;
            _originator = originator;
            _state = state.Select(game => (Game)game.Clone()).ToList();
        }

        public DateTime GetSnapshotDate() => SnapshotDate;
        public string GetDescription() => Description;

        public void Restore()
        {
            _originator.SetState(_state);
            Console.WriteLine($"\n Inventory restored from: {Description}");
        }
    }

    // ==================== INVENTORY MANAGER ====================
    public class GameInventoryManager
    {
        private List<Game> _games = new List<Game>();
        private readonly Stack<IInventoryMemento> _history = new Stack<IInventoryMemento>();

        // Add a game to inventory
        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        // Get all games
        public List<Game> GetAllGames()
        {
            return new List<Game>(_games);
        }

        // Get game by Game_id
        public Game GetGame(string g_id)
        {
            return _games.FirstOrDefault(g => g.Game_id == g_id);
        }

        // Create memento
        public IInventoryMemento CreateMemento(string description)
        {
            return new GameInventoryMemento(this, _games, description);
        }

        // Internal method to restore state
        internal void SetState(List<Game> state)
        {
            _games = state.Select(game => (Game)game.Clone()).ToList();
        }

        // Execute risky trade with automatic rollback
        public bool ExecuteRiskyTrade(TradeOperation trade)
        {
            Console.WriteLine($"\n=== Executing Trade: {trade.Description} ===");

            // Save snapshot before risky operation
            var memento = CreateMemento($"Before trade: {trade.TradeId}");
            _history.Push(memento);

            try
            {
                // Apply all changes
                foreach (var change in trade.ItemChanges)
                {
                    var game = GetGame(change.Key);
                    if (game == null)
                    {
                        throw new InvalidOperationException($"Game with Game_id {change.Key} not found!");
                    }

                    int newQuantity = game.Quantity + change.Value;
                    if (newQuantity < 0)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient quantity for {game.Name}. Available: {game.Quantity}, Requested: {-change.Value}");
                    }

                    Console.WriteLine($"  {game.Name}: {game.Quantity} → {newQuantity}");
                    game.UpdateQuantity(newQuantity);
                }

                Console.WriteLine("\n Trade completed successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Trade failed: {ex.Message}");
                Console.WriteLine("Rolling back to previous state...");

                // Rollback using memento
                Rollback();
                return false;
            }
        }

        // Manual rollback
        public void Rollback()
        {
            if (_history.Count > 0)
            {
                var memento = _history.Pop();
                memento.Restore();
            }
            else
            {
                Console.WriteLine(" No history available for rollback!");
            }
        }

        // Check if rollback is possible
        public bool CanRollback()
        {
            return _history.Count > 0;
        }

        // Display current inventory
        public void DisplayInventory()
        {
            Console.WriteLine("\n=== CURRENT INVENTORY ===");

            if (_games.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
                return;
            }

            decimal totalValue = 0;
            int totalItems = 0;

            // Video Games
            var videoGames = _games.OfType<VideoGame>().ToList();
            if (videoGames.Any())
            {
                Console.WriteLine("\n--- VIDEO GAMES ---");
                foreach (var game in videoGames)
                {
                    Console.WriteLine($"  {game.DisplayInfo()}");
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            // Board Games
            var boardGames = _games.OfType<BoardGame>().ToList();
            if (boardGames.Any())
            {
                Console.WriteLine("\n--- BOARD GAMES ---");
                foreach (var game in boardGames)
                {
                    Console.WriteLine($"  {game.DisplayInfo()}");
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            // Accessories
            var accessories = _games.OfType<Accessory>().ToList();
            if (accessories.Any())
            {
                Console.WriteLine("\n--- ACCESSORIES ---");
                foreach (var game in accessories)
                {
                    Console.WriteLine($"  {game.DisplayInfo()}");
                    totalValue += game.Quantity * game.Price;
                    totalItems += game.Quantity;
                }
            }

            Console.WriteLine("\n══════════════════════════════════════════");
            Console.WriteLine($"Total Games/Accessories: {_games.Count}");
            Console.WriteLine($"Total Units in Stock: {totalItems}");
            Console.WriteLine($"Total Inventory Value: L.E.{totalValue:F2}");
        }

        // Display history
        public void DisplayHistory()
        {
            Console.WriteLine("\n=== INVENTORY HISTORY ===");

            if (_history.Count == 0)
            {
                Console.WriteLine("No history available.");
                return;
            }

            var historyArray = _history.ToArray();
            Array.Reverse(historyArray);

            int index = 1;
            foreach (var memento in historyArray)
            {
                Console.WriteLine($"{index++}. {memento.GetDescription()}");
                Console.WriteLine($"   Time: {memento.GetSnapshotDate():g}");
                Console.WriteLine();
            }
        }

        // Search methods - FIXED VERSION
        public List<Game> SearchByName(string searchTerm)
        {
            return _games.Where(g => g.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
        }

        public List<Game> SearchByPlatform(string platform)
        {
            var results = new List<Game>();

            foreach (var game in _games)
            {
                if (game is VideoGame vg &&
                    vg.Platform.IndexOf(platform, StringComparison.OrdinalIgnoreCase) >= 0)
                    results.Add(game);
                else if (game is BoardGame bg &&
                         bg.Category.IndexOf(platform, StringComparison.OrdinalIgnoreCase) >= 0)
                    results.Add(game);
                else if (game is Accessory acc &&
                         acc.CompatibleWith.IndexOf(platform, StringComparison.OrdinalIgnoreCase) >= 0)
                    results.Add(game);
            }

            return results;
        }

        public List<Game> SearchByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _games.Where(g => g.Price >= minPrice && g.Price <= maxPrice)
                        .OrderBy(g => g.Price)
                        .ToList();
        }

        public List<Game> GetLowStockGames(int threshold = 10)
        {
            return _games.Where(g => g.Quantity < threshold)
                        .OrderBy(g => g.Quantity)
                        .ToList();
        }

        // Get history count
        public int GetHistoryCount()
        {
            return _history.Count;
        }

        // Clear inventory (for testing)
        public void ClearInventory()
        {
            _games.Clear();
            _history.Clear();
        }

        // Remove game
        public bool RemoveGame(string game_id)
        {
            var game = GetGame(game_id);
            if (game != null)
            {
                _games.Remove(game);
                return true;
            }
            return false;
        }

        // Get inventory statistics
        public (int totalGames, int totalUnits, decimal totalValue) GetStatistics()
        {
            int totalUnits = _games.Sum(g => g.Quantity);
            decimal totalValue = _games.Sum(g => g.Quantity * g.Price);

            return (_games.Count, totalUnits, totalValue);
        }

        // Get games by type
        public List<VideoGame> GetVideoGames()
        {
            return _games.OfType<VideoGame>().ToList();
        }

        public List<BoardGame> GetBoardGames()
        {
            return _games.OfType<BoardGame>().ToList();
        }

        public List<Accessory> GetAccessories()
        {
            return _games.OfType<Accessory>().ToList();
        }
    }

    // ==================== GAME CLASSES (LSP Principle) ====================
    public abstract class Game : ICloneable
    {
        public string Game_id { get; protected set; }
        public string Name { get; set; }
        public int Quantity { get; protected set; }
        public decimal Price { get; set; }

        public abstract string GetGameType();
        public abstract string DisplayInfo();

        public void UpdateQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class VideoGame : Game
    {
        public string Developer { get; set; }
        public string Genre { get; set; }
        public string Platform { get; set; }

        public VideoGame(string g_id, string name, int quantity, decimal price,
                         string developer, string genre, string platform)
        {
            Game_id = g_id;
            Name = name;
            Quantity = quantity;
            Price = price;
            Developer = developer;
            Genre = genre;
            Platform = platform;
        }

        public override string GetGameType() => "Video Game";

        public override string DisplayInfo()
        {
            return $"[{Game_id}] {Name} | Platform: {Platform} | Genre: {Genre} | " +
                   $"Qty: {Quantity} | Price: L.E.{Price:F2} | Developer: {Developer}";
        }
    }

    public class BoardGame : Game
    {
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Category { get; set; }

        public BoardGame(string sku, string name, int quantity, decimal price,
                         int minPlayers, int maxPlayers, string category)
        {
            Game_id = sku;
            Name = name;
            Quantity = quantity;
            Price = price;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            Category = category;
        }

        public override string GetGameType() => "Board Game";

        public override string DisplayInfo()
        {
            return $"[{Game_id}] {Name} | Players: {MinPlayers}-{MaxPlayers} | " +
                   $"Category: {Category} | Qty: {Quantity} | Price: L.E.{Price:F2}";
        }
    }

    public class Accessory : Game
    {
        public string CompatibleWith { get; set; }
        public string AccessoryType { get; set; }

        public Accessory(string sku, string name, int quantity, decimal price,
                         string compatibleWith, string accessoryType)
        {
            Game_id = sku;
            Name = name;
            Quantity = quantity;
            Price = price;
            CompatibleWith = compatibleWith;
            AccessoryType = accessoryType;
        }

        public override string GetGameType() => "Accessory";

        public override string DisplayInfo()
        {
            return $"[{Game_id}] {Name} | Type: {AccessoryType} | " +
                   $"Compatible: {CompatibleWith} | Qty: {Quantity} | Price: L.E.{Price:F2}";
        }
    }

    // ==================== TRADE OPERATION ====================
    public class TradeOperation
    {
        public string TradeId { get; }
        public string Description { get; }
        public Dictionary<string, int> ItemChanges { get; }

        public TradeOperation(string tradeId, string description)
        {
            TradeId = tradeId;
            Description = description;
            ItemChanges = new Dictionary<string, int>();
        }

        public void AddItemChange(string sku, int quantityChange)
        {
            ItemChanges[sku] = quantityChange;
        }
    }
}