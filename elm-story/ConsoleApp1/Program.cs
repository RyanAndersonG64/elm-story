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
    public int Strength { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Intelligence { get; protected set; }
    public int Luck { get; protected set; }

    protected PlayerCharacter(string CharacterName, int health, int mana, int WD, int MD, int strength, int dexterity, int intelligence, int luck)
        : base(CharacterName, health, mana, WD, MD)
    {
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Luck = luck;
    }

}

class Warrior : PlayerCharacter
{
    public Warrior(string name)
        : base(name, 150, 20, 20, 5, 20, 10, 5, 5)
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
        : base(name, 50, 100, 5, 20, 1, 5, 24, 10) { }
}

class Archer : PlayerCharacter
{
    public Archer(string name)
        : base(name, 90, 50, 10, 10, 9, 18, 8, 5) { }
}

class Rogue : PlayerCharacter
{
    public Rogue(string name)
        : base(name, 110, 60, 10, 10, 7, 8, 5, 20) { }
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
        Console.WriteLine($"{player.Name}: {player.CurrentHealth}/{player.MaxHealth} HP");
        Console.WriteLine($"{enemy.Name}: {enemy.CurrentHealth}/{enemy.MaxHealth} HP");
        Console.WriteLine();
    }

    private void PlayerTurn()
    {
        Console.WriteLine("Choose an action");
        Console.WriteLine("1: Attack");
        // Console.WriteLine("2: Ability");
        // Console.WriteLine("3: Item");
        // Console.WriteLine("4: Run");

        Random PlayerRng = new Random();

        string TurnChoice = Console.ReadLine();
        
        if (TurnChoice == "1")
        {
            int PlayerDamage = PlayerRng.Next(1,6);
            enemy.TakeDamage(PlayerDamage);
            state = BattleState.EnemyTurn;
        }
    }

    private void EnemyTurn()
    {
        Random EnemyDamageRng = new Random();
        int EnemyDamage = EnemyDamageRng.Next(1,4);
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
        if (player.CurrentHealth <= 0)
        {
            Console.WriteLine($"You have died! Game over.");
        }
        else if (enemy.CurrentHealth <= 0)
        {
            Console.WriteLine($"{enemy.Name} was defeated!");
        }
    }
}

public class Game ()
{
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

    public void Run()
    {
        var player = CreatePlayerCharacter();
        var enemy = new Slime();

        var battle = new Battle(player, enemy);
        battle.Run();
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