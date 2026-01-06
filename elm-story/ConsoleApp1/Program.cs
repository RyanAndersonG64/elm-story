// Characters

abstract class Character
{
    public string Name { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int MaxMana { get; protected set; }
    public int CurrentMana { get; protected set; }
    public int PhysicalDefense { get; protected set; }
    public int MagicDefense { get; protected set; }

    protected Character(string CharacterName, int health, int mana, int WD, int MD)
    {
        Name = CharacterName;
        MaxHealth = health;
        CurrentHealth = health;
        MaxMana = mana;
        CurrentMana = mana;
        PhysicalDefense = WD;
        MagicDefense = MD;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}


////////////////////////////////////////
//     Player Characters
////////////////////////////////////////

abstract class PlayerCharacter : Character
{
    public string Job { get; protected set; }
    public int Strength { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Intelligence { get; protected set; }
    public int Luck { get; protected set; }
    public Inventory Bag { get; protected set; }

    public PlayerCharacter(string ClassType, string CharacterName, int health, int mana, int WD, int MD, int strength, int dexterity, int intelligence, int luck)
        : base(CharacterName, health, mana, WD, MD)
    {
        Job = ClassType;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Luck = luck;
        Bag = new Inventory();
    }

}

class Warrior : PlayerCharacter
{
    public Warrior(string name)
        : base("Warrior", name, 150, 20, 20, 5, 20, 10, 5, 5)
    {
        if (name == "Superking")
        {
            Strength = 9000;
        }
    }
}

class Mage : PlayerCharacter
{
    public Mage(string name)
        : base("Mage", name, 50, 100, 5, 20, 1, 5, 24, 10) { }
}

class Archer : PlayerCharacter
{
    public Archer(string name)
        : base("Archer", name, 90, 50, 10, 10, 9, 18, 8, 5) { }
}

class Rogue : PlayerCharacter
{
    public Rogue(string name)
        : base("Rogue", name, 110, 60, 10, 10, 7, 8, 5, 20) { }
}




////////////////////////////////////////
//     Enemies
////////////////////////////////////////

abstract class EnemyCharacter : Character
{
    public int PhysicalAttack;
    public int MagicAttack;
    protected EnemyCharacter(string CharacterName, int health, int mana, int WD, int MD, int WA, int MA)
        : base(CharacterName, health, mana, WD, MD)
    {
        PhysicalAttack = WA;
        MagicAttack = MA;
    }
}

class Slime : EnemyCharacter
{

    public Slime()
        : base("Slime", 50, 0, 1, 1, 5, 0) { }
}





////////////////////////////////////////
//     Items and Inventory
////////////////////////////////////////

abstract class Item
{
    public string Name { get; protected set; }
    public bool UsableInCombat;
    public bool UsableInOverworld;

    protected Item(string name, bool usableInCombat, bool usableInOverworld)
    {
        Name = name;
        UsableInCombat = usableInCombat;
        UsableInOverworld = usableInOverworld;
    }

    public abstract bool Use(PlayerCharacter player);
}

class ItemStack
{
    public Item Item { get; }
    public int Count { get; private set; }

    public ItemStack(Item item, int count)
    {
        Item = item;
        Count = count;
    }

    public void Add(int amount)
    {
        Count += amount;
    }

    public void Subtract(int amount)
    {
        Count -= amount;
    }
}

class Inventory
{
    List<ItemStack> items = new List<ItemStack>();

    public void Add(Item item, int amount)
    {
        var stack = items.FirstOrDefault(s => s.Item.GetType() == item.GetType());

        if (stack != null)
        {
            stack.Add(amount);
        }
        else
        {
            items.Add(new ItemStack(item, amount));
        }
    }

    public void Subtract(Item item, int amount)
    {
        var stack = items.FirstOrDefault(s => s.Item.GetType() == item.GetType());

        stack.Subtract(amount);
        if (stack.Count <= 0)
        {
            items.Remove(stack);
        }

    }

    public void UseItem(int index, PlayerCharacter player)
    {
        if (index < 0 || index >= items.Count)
            return;

        var stack = items[index];

        bool used = stack.Item.Use(player);

        if (used)
        {
            stack.Subtract(1);

            if (stack.Count <= 0)
                items.RemoveAt(index);
        }
    }


    public bool DisplayBagInCombat(PlayerCharacter player)
    {
        Console.WriteLine();
        Console.WriteLine("Inventory:");

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item.UsableInCombat)
            {
                Console.WriteLine($"{i + 1} - {items[i].Count} {items[i].Item.Name}");
            }
        }

        Console.WriteLine($"{items.Count + 1} - Cancel");
        Console.WriteLine();

        string ItemChoice = Console.ReadLine();
        int choice;
        bool ParsedChoice = int.TryParse(ItemChoice, out choice);

        if (choice == items.Count + 1)
        {
            return false;
        }
        else if (choice > 0 && choice < items.Count + 1)
        {
            UseItem(choice - 1, player);
            return true;
        }
        else
        {
            Console.WriteLine("Invalid choice");
            return false;
        }
    }

    public bool DisplayBagInOverworld(PlayerCharacter player)
    {
        Console.WriteLine();
        Console.WriteLine("Inventory:");

        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {items[i].Count} {items[i].Item}");
        }

        Console.WriteLine($"{items.Count + 1} - Cancel");
        Console.WriteLine();

        string ItemChoice = Console.ReadLine();
        int choice;
        bool ParsedChoice = int.TryParse(ItemChoice, out choice);

        if (choice == items.Count + 1)
        {
            return false;
        }
        else if (choice > 0 && choice < items.Count + 1)
        {
            if (items[choice - 1].Item.UsableInOverworld)
            {
                UseItem(choice - 1, player);
                return true;
            }
            else
            {
                Console.WriteLine($"Oak, {player.Name}. This isn't the time to use that!");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid choice");
            return false;
        }



    }

}


// Potions

abstract class HealthPotion : Item
{
    public int HealthRestored;
    public override bool Use(PlayerCharacter player)
    {
        player.Heal(HealthRestored);
        return true;
    }

    protected HealthPotion(string Item, bool UsableInCombat, bool UsableInOverworld, int HealValue)
        : base(Item, UsableInCombat, UsableInOverworld)
    {
        HealthRestored = HealValue;
    }
}

class BasicHealthPotion : HealthPotion
{
    public BasicHealthPotion()
        : base("Basic Health Potion", true, true, 50) { }
}

class StandardHealthPotion : HealthPotion
{
    public StandardHealthPotion()
        : base("Standard Health Potion", true, true, 100) { }
}

class StrongHealthPotion : HealthPotion
{
    public StrongHealthPotion()
        : base("Strong Health Potion", true, true, 150) { }
}










////////////////////////////////////////
//     Battle System
////////////////////////////////////////

class Battle
{
    private PlayerCharacter player;
    private EnemyCharacter enemy;
    private BattleState state;
    private Random RNG = new Random();

    public enum BattleState
    {
        PlayerTurn,
        ItemMenu,
        EnemyTurn,
        Victory,
        Defeat,
        Fled
    }

    public Battle(PlayerCharacter Player, EnemyCharacter Enemy)
    {
        player = Player;
        enemy = Enemy;
        state = BattleState.PlayerTurn;
    }

    public BattleState Run()
    {

        Console.WriteLine($"You encounter a {enemy.Name}!");

        while (state == BattleState.PlayerTurn || state == BattleState.EnemyTurn)
        {
            DisplayStatus();

            if (state == BattleState.PlayerTurn)
                PlayerTurn();
            else
                EnemyTurn();

            CheckEndConditions();
        }

        DisplayOutcome();
        return state;

    }

    private void DisplayStatus()
    {
        Console.WriteLine();
        Console.WriteLine($"{player.Name}: {player.CurrentHealth}/{player.MaxHealth} HP, {player.CurrentMana}/{player.MaxMana} MP");
        Console.WriteLine($"{enemy.Name}: {enemy.CurrentHealth}/{enemy.MaxHealth} HP, {enemy.CurrentMana}/{enemy.MaxMana} MP");
        Console.WriteLine();
    }

    private void PlayerTurn()
    {
        Console.WriteLine("Choose an action");
        Console.WriteLine("1: Attack");
        Console.WriteLine("2: Ability");
        Console.WriteLine("3: Item");
        Console.WriteLine("4: Run");



        string TurnChoice = Console.ReadLine();

        if (TurnChoice == "1")
        {
            int PlayerDamage = RNG.Next(1, 6);
            enemy.TakeDamage(PlayerDamage);
            state = BattleState.EnemyTurn;
        }

        else if (TurnChoice == "2")
        {
            Console.WriteLine("Abilities not implemented yet");
        }

        else if (TurnChoice == "3")
        {
            state = BattleState.ItemMenu;
            bool ItemChoiceResult = player.Bag.DisplayBagInCombat(player);
            if (ItemChoiceResult)
            {
                state = BattleState.EnemyTurn;
            }
            else
            {
                state = BattleState.PlayerTurn;
            }
        }

        else if (TurnChoice == "4")
        {

            int FleeRoll = RNG.Next(1, player.Dexterity + player.Luck / 2 + 1);

            if (FleeRoll >= 10)
            {
                state = BattleState.Fled;
            }
            else
            {
                Console.WriteLine("Failed to flee!");
                state = BattleState.EnemyTurn;
            }
        }
        else
        {
            Console.WriteLine("Invalid Choice");
        }
    }

    private void EnemyTurn()
    {

        int EnemyDamage = RNG.Next(1, 4);
        player.TakeDamage(EnemyDamage);
        state = BattleState.PlayerTurn;
    }

    private void CheckEndConditions()
    {
        if (player.CurrentHealth <= 0)
        {
            state = BattleState.Defeat;
        }
        else if (enemy.CurrentHealth <= 0)
        {
            state = BattleState.Victory;
        }
    }
    private void DisplayOutcome()
    {
        if (state == BattleState.Defeat)
        {
            Console.WriteLine($"{player.Name} has died! Game over.");
        }
        else if (state == BattleState.Victory)
        {
            Console.WriteLine($"{enemy.Name} was defeated!");
        }
        else if (state == BattleState.Fled)
        {
            Console.WriteLine($"{player.Name} fled from {enemy.Name}!");
        }
    }
}


////////////////////////////////////////
//     Game Flow
////////////////////////////////////////

class Game
{

    private PlayerCharacter player;
    private GameState gameState;
    static PlayerCharacter CreatePlayerCharacter()
    {
        Console.WriteLine("Choose a class:");
        Console.WriteLine("1 - Warrior");
        Console.WriteLine("2 - Mage");
        Console.WriteLine("3 - Archer");
        Console.WriteLine("4 - Rogue");

        string choice = Console.ReadLine();

        Console.Write("Enter your character name: ");
        string name = Console.ReadLine();

        return choice switch
        {
            "1" => new Warrior(name),
            "2" => new Mage(name),
            "3" => new Archer(name),
            "4" => new Rogue(name),
            _ => throw new Exception("Invalid choice")
        };

    }

    public enum GameState
    {
        Overworld,
        InInventory,
        Combat,
        GameOver,
    }

    public Game()
    {
        gameState = GameState.Overworld;
    }

    private void NextAction()
    {
        Console.WriteLine("What would you like to do next?");
        Console.WriteLine("1 - Proceed to next encounter");
        Console.WriteLine("2 - Check inventory");
        Console.WriteLine("3 - Go to town");
        Console.WriteLine("4 - Quit");

        string NextActionChoice = Console.ReadLine();

        if (NextActionChoice == "1")
        {
            var enemy = new Slime();
            var battle = new Battle(player, enemy);
            gameState = GameState.Combat;
            var BattleResult = battle.Run();
            gameState = BattleResult switch
            {
                Battle.BattleState.Victory => GameState.Overworld,
                Battle.BattleState.Fled => GameState.Overworld,
                Battle.BattleState.Defeat => GameState.GameOver,
                _ => gameState
            };
        }
        else if (NextActionChoice == "2")
        {
            gameState = GameState.InInventory;
            Console.WriteLine($"{player.Name}: {player.CurrentHealth}/{player.MaxHealth} HP, {player.CurrentMana}/{player.MaxMana} MP");
            Console.WriteLine();
            player.Bag.DisplayBagInOverworld(player);
            gameState = GameState.Overworld;
        }
        else if (NextActionChoice == "3")
        {
            Console.WriteLine("Coming soon");
        }
        else if (NextActionChoice == "4")
        {
            gameState = GameState.GameOver;
        }

    }
    public void Run()
    {
        player = CreatePlayerCharacter();
        player.Bag.Add(new BasicHealthPotion(), 5);

        while (gameState != GameState.GameOver)
        {
            if (gameState == GameState.Overworld)
            {
                NextAction();
            }
        }
    }
}










public class Program()
{


    static void Main()
    {

        var game = new Game();
        game.Run();
    }
}