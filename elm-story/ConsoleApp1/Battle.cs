class Battle
{
    private PlayerCharacter player;
    private EnemyCharacter enemy;
    private BattleState state;
    private Random RNG = new Random();

    public enum BattleState
    {
        PlayerTurn,
        AbilityMenu,
        ItemMenu,
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
        Console.WriteLine();
        Console.WriteLine($"You encounter a {enemy.Name}!");

        while (state == BattleState.PlayerTurn || state == BattleState.EnemyTurn)
        {
            if (state == BattleState.PlayerTurn)
            {
                DisplayStatus();
                PlayerTurn();
            }
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
        player.TickBuffs();

        Console.WriteLine();
        Console.WriteLine("Choose an action");
        Console.WriteLine("1: Attack");
        Console.WriteLine("2: Ability");
        Console.WriteLine("3: Item");
        Console.WriteLine("4: Run");



        string TurnChoice = Console.ReadLine();

        if (TurnChoice == "1")
        {
            Console.WriteLine();
            int DamageRoll = RNG.Next(80, 121);
            int PlayerDamage = player.Attack(DamageRoll);
            int CritRoll = RNG.Next(1, 101);
            if (CritRoll <= player.CritChance)
            {
                PlayerDamage = (int)(PlayerDamage * (player.CritDamage / 100));
                Console.WriteLine("Critical Hit!");
            }
            enemy.TakeDamage(PlayerDamage);
            Console.WriteLine($"{player.Name} attacks for {PlayerDamage} damage.");



            if (player.Job == "Rogue" && enemy.CurrentHealth > 0 && enemy.CurrentHealth < (enemy.MaxHealth * 0.05) && enemy is EnemyCharacter)
            {
                if (RNG.Next(1, 101) > 50)
                {
                    Console.WriteLine($"{player.Name} spots a weakness and deals a finishing blow!");
                    enemy.TakeDamage(enemy.CurrentHealth);
                }
            }


            state = BattleState.EnemyTurn;
        }

        else if (TurnChoice == "2")
        {
            state = BattleState.AbilityMenu;
            Console.WriteLine();
            Console.WriteLine("1 - Use damage ability");
            Console.WriteLine("2 - Use buff");
            Console.WriteLine("3 - Cancel");

            string AbilityMenuChoice = Console.ReadLine();

            if (AbilityMenuChoice == "1")
            {
                int DamageAbilityChoice = player.DisplayDamageAbilities();
                if (DamageAbilityChoice == -1)
                {
                    state = BattleState.PlayerTurn;
                }
                else
                {
                    player.UseDamageAbility(DamageAbilityChoice, enemy);
                    state = BattleState.EnemyTurn;
                }
            }
            else if (AbilityMenuChoice == "2")
            {
                int BuffAbilityChoice = player.DisplayBuffAbilities();
                if (BuffAbilityChoice == -1)
                {
                    state = BattleState.PlayerTurn;
                }
                else
                {
                    player.UseBuffAbility(BuffAbilityChoice);
                    state = BattleState.EnemyTurn;
                }
            }
            else if (AbilityMenuChoice == "3")
            {
                state = BattleState.PlayerTurn;
            }
            else
            {
                Console.WriteLine("Invalid Choice");
            }
        }

        else if (TurnChoice == "3")
        {
            state = BattleState.ItemMenu;
            bool ItemChoiceResult = player.Bag.DisplayBagInCombat(player);
            if (ItemChoiceResult)
            {
                state = BattleState.EnemyTurn;
            }
            else
            {
                state = BattleState.PlayerTurn;
            }
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
        Console.WriteLine($"{enemy.Name} attacks for {EnemyDamage} damage.");
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