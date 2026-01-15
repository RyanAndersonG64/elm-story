// Player buffs

public abstract class PlayerBuff
{
    public string Name;
    public int Duration;
    public abstract void Effect(PlayerCharacter player);
    public abstract void Remove(PlayerCharacter player);
    

    protected PlayerBuff(string name, int duration)
    {
        Name = name;
        Duration = duration;
    }
}

public class ActivePlayerBuff
{
    public PlayerBuff Buff {get;}
    public int TurnsRemaining {get; protected set;}

    public ActivePlayerBuff(PlayerBuff buff)
    {
        Buff = buff;
        TurnsRemaining = buff.Duration;
    }

    public void Refresh()
    {
        TurnsRemaining = Buff.Duration;
    }

    public void TickDown()
    {
        TurnsRemaining--;
    }

}

public class CritChanceBuff : PlayerBuff
{
    private readonly int amount;

    public CritChanceBuff(int BuffAmount, int duration)
        : base("Increase Critical Chance", duration)
    {
        amount = BuffAmount;
    }

    public override void Effect(PlayerCharacter player)
    {
        player.ModifyCritChance(amount);
    }

    public override void Remove(PlayerCharacter player)
    {
            player.ModifyCritChance(-amount); 
    }
}


// Enemy buffs

public abstract class EnemyBuff
{
    public string Name;
    public int Duration;
    public abstract void Effect(Character enemy);
}