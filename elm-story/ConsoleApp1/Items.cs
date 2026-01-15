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

// Health Potions
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


// Mana Potions
abstract class ManaPotion : Item
{
    public int ManaRestored;
    public override bool Use(PlayerCharacter player)
    {
        player.RestoreMana(ManaRestored);
        return true;
    }

    protected ManaPotion(string Item, bool UsableInCombat, bool UsableInOverworld, int HealValue)
        : base(Item, UsableInCombat, UsableInOverworld)
    {
        ManaRestored = HealValue;
    }
}

class BasicManaPotion : ManaPotion
{
    public BasicManaPotion()
        : base("Basic Mana Potion", true, true, 50) { }
}

class StandardManaPotion : ManaPotion
{
    public StandardManaPotion()
        : base("Standard Mana Potion", true, true, 100) { }
}

class StrongManaPotion : ManaPotion
{
    public StrongManaPotion()
        : base("Strong Mana Potion", true, true, 150) { }
}