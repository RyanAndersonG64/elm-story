public abstract class Character
{
    public string Name { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int MaxMana { get; protected set; }
    public int CurrentMana { get; protected set; }
    public int PhysicalDefense { get; protected set; }
    public int MagicDefense { get; protected set; }
    public string StatusCondition { get; protected set; }

    protected Character(string CharacterName, int health, int mana, int WD, int MD)
    {
        Name = CharacterName;
        MaxHealth = health;
        CurrentHealth = health;
        MaxMana = mana;
        CurrentMana = mana;
        PhysicalDefense = WD;
        MagicDefense = MD;
        StatusCondition = "No Status";
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
    public int Level { get; protected set; }
    public int Strength { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Intelligence { get; protected set; }
    public int Luck { get; protected set; }
    public int CritChance { get; protected set; }
    public int CritDamage { get; protected set; }
    public Inventory Bag { get; protected set; }

    public PlayerCharacter(string ClassType, string CharacterName, int health, int mana, int WD, int MD, int strength, int dexterity, int intelligence, int luck, int critDamage)
        : base(CharacterName, health, mana, WD, MD)
    {
        Job = ClassType;
        Level = 1;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Luck = luck;
        CritChance = (int)(Dexterity / 4 + Luck / 2);
        CritDamage = critDamage;
        Bag = new Inventory();
    }

    public virtual int Attack(double AttackRoll)
    {
        return 5;
    }

    public void ModifyStrength(int amount)
    {
        Strength += amount;
    }
    public void ModifyDexterity(int amount)
    {
        Dexterity += amount;
    }
    public void ModifyIntelligence(int amount)
    {
        Intelligence += amount;
    }
    public void ModifyLuck(int amount)
    {
        Luck += amount;
    }
    public void ModifyCritChance(int amount)
    {
        CritChance += amount;
    }

    // public void BuffPlayer(string stat, int amount)
    // {
    //     switch (stat)
    //     {
    //         case "Strength":
    //             Strength += amount;
    //             break;
    //         case "Dexterity":
    //             Dexterity += amount;
    //             break;
    //         case "Intelligence":
    //             Intelligence += amount;
    //             break;
    //         case "Luck":
    //             Luck += amount;
    //             break;
    //         case "Crit Chance":
    //             CritChance += amount;
    //             break;
    //         default:
    //             break;
    //     }
    // }

    public List<ActivePlayerBuff> ActiveBuffs { get; } = new();

    public void AddOrRefreshBuff(PlayerBuff buff)
    {
        var existing = ActiveBuffs.FirstOrDefault(b => b.Buff.GetType() == buff.GetType());
        if (existing != null)
        {
            existing.Refresh(); // reuse the ACTIVE instance (not the definition)
            Console.WriteLine($"{buff.Name} refreshed!");
            return;
        }

        var active = new ActivePlayerBuff(buff);
        ActiveBuffs.Add(active);
        buff.Effect(this);
        Console.WriteLine($"{buff.Name} applied for {buff.Duration} turns!");
    }

    public void TickBuffs()
    {
        for (int i = ActiveBuffs.Count - 1; i >= 0; i--)
        {
            var active = ActiveBuffs[i];
            active.TickDown();

            if (active.TurnsRemaining <= 0)
            {
                active.Buff.Remove(this);
                ActiveBuffs.RemoveAt(i);
            }
        }
    }



}

class Warrior : PlayerCharacter
{
    public Warrior(string name)
        : base("Warrior", name, 150, 20, 20, 5, 20, 10, 5, 5, 125)
    {
        if (name == "Superking")
        {
            Strength = 9000;
        }
    }

    public override int Attack(double AttackRoll)
    {
        double Roll = AttackRoll / 200;
        return (int)Math.Round((Strength + Dexterity * 0.25) * Roll);
    }
}

class Mage : PlayerCharacter
{
    public Mage(string name)
        : base("Mage", name, 50, 100, 5, 20, 1, 5, 24, 10, 125)
    {
        if (name == "doomasater")
        {
            Intelligence = 9000;
        }
    }

    public override int Attack(double AttackRoll)
    {
        double Roll = AttackRoll / 200;
        return (int)Math.Round((Strength + Dexterity * 0.25) * Roll);
    }
}

class Archer : PlayerCharacter
{
    public Archer(string name)
        : base("Archer", name, 90, 50, 10, 10, 9, 18, 8, 5, 150)
    {
        if (name == "thewisepark")
        {
            Dexterity = 9000;
        }
    }

    public override int Attack(double AttackRoll)
    {
        double Roll = AttackRoll / 200;
        return (int)Math.Round((Dexterity * 0.9 + Strength * 0.5) * Roll);
    }
}

class Rogue : PlayerCharacter
{
    public Rogue(string name)
        : base("Rogue", name, 110, 60, 10, 10, 7, 8, 5, 20, 200)
    {
        if (name == "themrhobo")
        {
            Luck = 9000;
        }
    }

    public override int Attack(double AttackRoll)
    {
        double Roll = AttackRoll / 200;
        return (int)Math.Round((Luck * 0.6 + Dexterity * 0.3 + Strength * 0.1) * Roll);
    }
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