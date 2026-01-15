public abstract class Character
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

    public void UseMana(int amount)
    {
        CurrentMana -= amount;
    }

    public void RestoreMana(int amount)
    {
        CurrentMana += amount;
        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }


}


////////////////////////////////////////
//     Player Characters
////////////////////////////////////////

public abstract class PlayerCharacter : Character
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
        : base("Mage", name, 50, 100, 5, 20, 1, 5, 24, 10)
    {

    }
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