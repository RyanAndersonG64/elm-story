public abstract class PlayerAbility
{
    public string Name;
    public int HealthCost { get; protected set; }
    public int ManaCost { get; protected set; }
    public string Description;
    public virtual void Use(PlayerCharacter user, Character target)
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

    protected PlayerAbility(string AbilityName, int AbilityHealthCost, int AbilityManaCost, string AbilityDescription)
    {
        Name = AbilityName;
        HealthCost = AbilityHealthCost;
        ManaCost = AbilityManaCost;
        Description = AbilityDescription;
    }
}

public class RecklessSwing : PlayerAbility
{
    public RecklessSwing(string name, int HCost, int MCost, string desc)
        : base("Reckless Swing", 5, 10, "Consume 5% of your HP to deal extreme damage") { }

    protected override void Apply(PlayerCharacter user, Character target)
    {
        if (user.CurrentHealth <= user.MaxHealth * .05)
        {
            user.TakeDamage(user.CurrentHealth - 1);
        }
        else
        {
            user.TakeDamage((int)(user.MaxHealth * .05));
        }
        target.TakeDamage(user.Strength * 2);
    }
}
public class MagicBolt : PlayerAbility
{
    private Random MagicBoltRNG = new Random();
    public MagicBolt(string name, int HCost, int MCost, string desc)
        : base("Magic Bolt", 0, 20, "Shoots a basic blast of magic energy to deal damage") { }

    protected override void Apply(PlayerCharacter user, Character target)
    {
        int damage = (int)(MagicBoltRNG.Next(90, 111) * user.Intelligence / 200);
        target.TakeDamage(user.Intelligence);
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