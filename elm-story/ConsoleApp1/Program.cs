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

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }
}

class Warrior : PlayerCharacter
{
    public Warrior(string name)
        :base (name, 150, 20, 20, 5, 20, 10, 5, 5){}
}

class Mage : PlayerCharacter
{
    public Mage(string name)
        :base (name, 50, 100, 5, 20, 1, 5, 24, 10){}
}

class Archer : PlayerCharacter
{
    public Archer(string name)
        :base (name, 90, 50, 10, 10, 9, 18, 8, 5){}
}

class Rogue: PlayerCharacter
{
    public Rogue(string name)
        :base (name, 110, 60, 10, 10, 7, 8, 5, 20){}
}





abstract class EnemyCharacter : Character
{
    public int PhysicalAttack;
    public int MagicAttack;
    protected EnemyCharacter(string CharacterName, int health, int mana, int WD, int MD, int WA, int MA)
        :base(CharacterName, health, mana, WD, MD)
    {
        PhysicalAttack = WA;
        MagicAttack = MA;
    }
}

class Slime : EnemyCharacter
{
    
    public Slime()
        :base("Slime", 50, 0, 1, 1, 5, 0){}
}








class Battle
{
    public PlayerCharacter player;
    public EnemyCharacter enemy;
    public Battle(PlayerCharacter Player, EnemyCharacter Enemy)
    {
        player = Player;
        enemy = Enemy;
    }   
}












public class Program()
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

    static void Main()
    {
        CreatePlayerCharacter();
    }
}