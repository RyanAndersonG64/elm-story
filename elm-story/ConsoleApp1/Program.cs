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
}

abstract class PlayerCharacter : Character
{
    public string Class { get; protected set; }
    public int Strength { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Intelligence { get; protected set; }
    public int Luck { get; protected set; }

    protected PlayerCharacter(string ClassType, string CharacterName, int health, int mana, int WD, int MD, int strength, int dexterity, int intelligence, int luck)
        : base(CharacterName, health, mana, WD, MD)
    {
        Class = ClassType;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Luck = luck;
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








class Battle
{
    private PlayerCharacter player;
    private EnemyCharacter enemy;
    private BattleState state;
    private Random RNG = new Random();

    public enum BattleState
    {
        PlayerTurn,
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
            Console.WriteLine("Items not implemented yet");
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
            Console.WriteLine("Coming soon");
        }
        else if (NextActionChoice == "3")
        {
            Console.WriteLine("Coming soon");
        }

    }
    public void Run()
    {
        player = CreatePlayerCharacter();
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