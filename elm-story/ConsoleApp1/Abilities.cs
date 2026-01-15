public abstract class PlayerDamageAbility
{
    public string Name;
    public int HealthCost { get; protected set; }
    public int ManaCost { get; protected set; }
    public string Description;
    public void Use(PlayerCharacter user, Character target)
    {
        if (user.CurrentHealth <= HealthCost)
        {
            Console.WriteLine($"Not enough HP to use {Name}.");
            return;
        }

        if (user.CurrentMana < ManaCost)
        {
            Console.WriteLine($"Not enough MP to use {Name}.");
            return;
        }

        user.TakeDamage(HealthCost);
        user.UseMana(ManaCost);

        Apply(user, target);
    }

    protected abstract void Apply(PlayerCharacter user, Character target);

    protected PlayerDamageAbility(string AbilityName, int AbilityHealthCost, int AbilityManaCost, string AbilityDescription)
    {
        Name = AbilityName;
        HealthCost = AbilityHealthCost;
        ManaCost = AbilityManaCost;
        Description = AbilityDescription;
    }
}

// Warrior damage abilities
public class RecklessSwing : PlayerDamageAbility
{
    private Random RecklessSwingRNG = new Random();
    public RecklessSwing()
        : base("Reckless Swing", 0, 10, "Consume 5% of your HP to deal high damage") { }

    protected override void Apply(PlayerCharacter user, Character target)
    {
        Console.WriteLine(user.CurrentHealth);
        Console.WriteLine(user.MaxHealth * .05);
        user.TakeDamage((int)(user.MaxHealth * .05));


        int damage = user.Strength * 2;
        if (RecklessSwingRNG.Next(1, 101) <= user.CritChance)
        {
            damage = (int)(damage * user.CritDamage / 100);
            Console.WriteLine("Critical hit!");
        }
        Console.WriteLine($"{user.Name} swings recklessly for {damage} damage.");
        target.TakeDamage(damage);
    }
}


// mage damage abilities
public class MagicBolt : PlayerDamageAbility
{
    private Random MagicBoltRNG = new Random();
    public MagicBolt()
        : base("Magic Bolt", 0, 10, "Shoots a basic blast of magic energy to deal damage") { }

    protected override void Apply(PlayerCharacter user, Character target)
    {
        int damage = (int)(MagicBoltRNG.Next(90, 111) * user.Intelligence / 200);
        if (MagicBoltRNG.Next(1, 101) <= user.CritChance)
        {
            damage = (int)(damage * user.CritDamage / 100);
            Console.WriteLine("Critical hit!");
        }
        Console.WriteLine($"{user.Name} casts a magic bolt for {damage} damage.");
        target.TakeDamage(damage);
    }
}


// Archer damage abilities


// Rogue damage abilities
public class Backstab : PlayerDamageAbility
{
    private Random BackstabRNG = new Random();

    public Backstab()
        : base("Backstab", 0, 0, "If the target is at full health, hits them with a massive attack that is more likely to crit") { }

    protected override void Apply(PlayerCharacter user, Character target)
    {
        if (target.CurrentHealth < target.MaxHealth)
        {
            int FailedBackstabDamage = user.Attack(BackstabRNG.Next(70, 111));
            if (BackstabRNG.Next(1, 101) <= user.CritChance)
            {
                FailedBackstabDamage *= user.CritDamage;
                Console.WriteLine("Critical Hit!");
            }
            Console.WriteLine($"Failed to take {target.Name} by surprise. {user.Name} make a weak attack for {FailedBackstabDamage} damage.");
            target.TakeDamage(FailedBackstabDamage);
        }
        else
        {
            int BackstabDamage = user.Attack(BackstabRNG.Next(100, 141));
            if (BackstabRNG.Next(1, 101) <= (user.CritChance + 50))
            {
                BackstabDamage *= user.CritDamage / 50;
                Console.WriteLine("Critical Hit!");
            }
            Console.WriteLine($"{user.Name} takes {target.Name} by surprise with a backstab for {BackstabDamage} damage");
            target.TakeDamage(BackstabDamage);
        }
    }
}


public abstract class PlayerBuffAbility
{
    public string Name;
    public int HealthCost { get; protected set; }
    public int ManaCost { get; protected set; }
    public string Description;
    public virtual void Use(PlayerCharacter user)
    {
        if (user.CurrentHealth <= HealthCost)
        {
            Console.WriteLine($"Not enough HP to use {Name}.");
            return;
        }

        if (user.CurrentMana < ManaCost)
        {
            Console.WriteLine($"Not enough MP to use {Name}.");
            return;
        }

        user.TakeDamage(HealthCost);
        user.UseMana(ManaCost);

        Apply(user);
    }

    protected abstract void Apply(PlayerCharacter user);

    protected PlayerBuffAbility(string AbilityName, int AbilityHealthCost, int AbilityManaCost, string AbilityDescription)
    {
        Name = AbilityName;
        HealthCost = AbilityHealthCost;
        ManaCost = AbilityManaCost;
        Description = AbilityDescription;
    }
}


public class BlessRNG : PlayerBuffAbility
{
    public BlessRNG()
        : base("BlessRNG", 0, 10, "Increases critical chance by 30% for 5 turns") { }

    protected override void Apply(PlayerCharacter user)
    {
        user.AddOrRefreshBuff(new CritChanceBuff(30, 6));
    }
}


// Enemy Abilities
public abstract class EnemyAbility
{
    public string Name;
    public int HealthCost { get; protected set; }
    public int ManaCost { get; protected set; }
    public string Description;
    public virtual void Use(Character user, PlayerCharacter target)
    {
        if (user.CurrentHealth <= HealthCost)
        {
            Console.WriteLine($"Not enough HP to use {Name}.");
            return;
        }

        if (user.CurrentMana < ManaCost)
        {
            Console.WriteLine($"Not enough MP to use {Name}.");
            return;
        }

        user.TakeDamage(HealthCost);
        user.UseMana(ManaCost);

        Apply(user, target);
    }

    protected abstract void Apply(Character user, PlayerCharacter target);

    protected EnemyAbility(string AbilityName, int AbilityHealthCost, int AbilityManaCost, string AbilityDescription)
    {
        Name = AbilityName;
        HealthCost = AbilityHealthCost;
        ManaCost = AbilityManaCost;
        Description = AbilityDescription;
    }
}