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
        Console.WriteLine();
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
        player.Bag.Add(new BasicManaPotion(), 5);

        while (gameState != GameState.GameOver)
        {
            if (gameState == GameState.Overworld)
            {
                NextAction();
            }
        }
    }
}