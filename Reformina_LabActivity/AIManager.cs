using Raylib_cs;
using System;
using System.Numerics;
public enum FishState
{
    Idle,
    Swim,
    Hungry,
}
public class AISystem
{
    private List<Fish> fishes;
    private List<FoodPellets> pellets;
    private List<Coin> coins;
    private SoundManager soundManager;

    public AISystem(List<Fish> fishes, List<FoodPellets> pellets, List<Coin> coins, SoundManager soundManager)
    {
        this.fishes = fishes;
        this.pellets = pellets;
        this.coins = coins;
        this.soundManager = soundManager;
    }

    public void Update(List<Fish> fishes, List<Coin> coins, List<FoodPellets> pellets)
    {
        float deltaTime = Raylib.GetFrameTime();

        for (int i = fishes.Count - 1; i >= 0; i--)
        {
            Fish fish = fishes[i];

            // Age and HP decay
            fish.age += deltaTime;
            fish.hpTimer -= deltaTime;
            if (fish.hpTimer <= 0)
            {
                fish.hp -= 1;
                fish.hpTimer = 1f;
            }

            if (fish.hp <= 0 || fish.age >= fish.lifespan)
            {
                fish.isDead = true;
                fish.Dead();
                soundManager.PlaySound("die");
            }
            else
            {
                if (fish.hp <= 80)
                    fish.currentState = FishState.Hungry;
                else
                    fish.currentState = FishState.Swim;
            }

            // Growth
         if (!fish.isDead)
        {
            fish.Move(pellets);

            if (fish.isAdult && fish.coinTimer <= 0)
                {
                    coins.Add(new GoldCoin(fish.x, fish.y));
                    soundManager.PlaySound("drop");
                    fish.coinTimer = 12f + (float)new Random().NextDouble() * 8f;
                }
                else if (!fish.isAdult && fish.coinTimer <= 0 && !(fish is SmallFish))
                {
                    coins.Add(new SilverCoin(fish.x, fish.y));
                    soundManager.PlaySound("drop");
                    fish.coinTimer = 8f + (float)new Random().NextDouble() * 10f;
                }
                else if (fish is SmallFish && fish.coinTimer <= 0)
                {
                    coins.Add(new BronzeCoin(fish.x, fish.y));
                    soundManager.PlaySound("drop");
                    fish.coinTimer = 5f + (float)new Random().NextDouble() * 12f;
                }
                else
                {
                    fish.coinTimer -= deltaTime;
                }
        }

            // Poop drop
            if (fish.poopTimer <= 0 && !fish.isDead)
            {
                soundManager.PlaySound("poop");
                coins.Add(new Poop(fish.x, fish.y)); 
                fish.poopTimer = 10f + (float)new Random().NextDouble() * 3f; 
            }
            else
            {
                fish.poopTimer -= deltaTime;
            }

            // AI Behavior
            switch (fish)
            {
                case SmallFish basic:
                    HandleSmallFish(basic);
                    break;

                case MediumFish basic:
                    HandleMediumFish(basic);
                    break;
                
                case CarnivoreFish carnivore:
                    HandleCarnivore(carnivore, deltaTime, fishes);
                    break;

                case JanitorFish janitor:
                    HandleJanitor(janitor);
                    break;
            }
        }
    }

    private void HandleSmallFish(SmallFish fish)
    {
        fish.currentState = fish.hp <= fish.maxHp-(fish.maxHp/4) ? FishState.Hungry : FishState.Swim;

        for (int i = pellets.Count - 1; i >= 0; i--)
        {
            FoodPellets pellet = pellets[i];
            if (fish.IsCollidingWith(pellet) && fish.hp < 70)
            {
                Console.WriteLine("Basic fish eating pellet");
                soundManager.PlaySound("FishEat");
                fish.hp = Math.Clamp(fish.hp + 25, 0,100); 
                pellets.RemoveAt(i);
                break; 
            }
        }
    }

    private void HandleMediumFish(MediumFish fish)
    {
        fish.currentState = fish.hp <= fish.maxHp-(fish.maxHp/4) ? FishState.Hungry : FishState.Swim;

        if (!fish.isAdult && fish.age >= 60f)
        {
            fish.isAdult = true;
            Console.WriteLine("MediumFish became an adult!");
        }

        for (int i = pellets.Count - 1; i >= 0; i--)
        {
            FoodPellets pellet = pellets[i];
            if (fish.IsCollidingWith(pellet) && fish.hp < 70)
            {
                Console.WriteLine("Basic fish eating pellet");
                soundManager.PlaySound("FishEat");
                fish.hp = Math.Clamp(fish.hp + 25, 0, 100);
                pellets.RemoveAt(i);
                break;
            }
        }
    }
    

    private void HandleCarnivore(CarnivoreFish fish, float deltaTime, List<Fish> fishes)
    {
        fish.hungerTimer -= deltaTime;

        if (!fish.isAdult && fish.age >= 100f)
        {
            fish.isAdult = true;
            Console.WriteLine("CarnivoreFish became an adult!");
        }

        if (fish.hungerTimer <= 0)
        {
            fish.currentState = FishState.Hungry;

            Fish prey = Fish.FindNearestPrey(fish, fishes);

            if (prey != null)
            {
                fish.MoveTowards(prey.x, prey.y, 80f);

                if (fish.IsCollidingWith(prey))
                {
                    soundManager.PlaySound("FishEat");
                    fish.hp = Math.Clamp(fish.hp + 50, 0, fish.maxHp);
                    fishes.Remove(prey);
                    fish.hungerTimer = 15f;
                }
            }
        }
        else
        {
            fish.currentState = FishState.Swim;
        }
    }

    private void HandleJanitor(JanitorFish fish)
    {
        if (fish.currentTargetPoop == null || !coins.Contains(fish.currentTargetPoop))
        {
            fish.currentTargetPoop = FindNearestPoop(fish);
        }

        if (fish.currentTargetPoop == null)
        {
            fish.currentState = FishState.Swim;
            return;
        }

        Poop targetPoop = fish.currentTargetPoop;
        fish.currentState = FishState.Hungry;
        fish.MoveTowards(targetPoop.X, targetPoop.Y, 60f);

        if (fish.IsCollidingWith(targetPoop))
        {
            soundManager.PlaySound("FishEat");
            Console.WriteLine("JanitorFish ate poop!");
            coins.Add(new SilverCoin(fish.x, fish.y));
            coins.Remove(targetPoop);
            fish.hp = Math.Clamp(fish.hp + 10, 0, fish.maxHp);

            fish.currentTargetPoop = null;
        }
    }

    private T FindNearestPellet<T>(Fish fish) where T : FoodPellets
    {
        T closest = null;
        float minDist = float.MaxValue;

        foreach (var pellet in pellets)
        {
            if (pellet is T typedPellet)
            {
                float dist = Vector2.Distance(new Vector2(fish.x, fish.y), new Vector2(pellet.x, pellet.y));
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = typedPellet;
                }
            }
        }

        return closest;
    }

    private Fish FindNearestPrey(CarnivoreFish predator)
    {
        Fish closest = null;
        float minDist = float.MaxValue;

        foreach (var fish in fishes)
        {
            if (fish is SmallFish basic && !basic.isAdult)
            {
                float dist = Vector2.Distance(new Vector2(predator.x, predator.y), new Vector2(basic.x, basic.y));
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = basic;
                }
            }
        }

        return closest;
    }

    private Poop FindNearestPoop(Fish fish)
    {
        Poop closest = null;
        float minDist = float.MaxValue;

        foreach (var coin in coins)
        {
            if (coin is Poop poop)
            {
                float dist = Vector2.Distance(new Vector2(fish.x, fish.y), new Vector2(poop.X, poop.Y));
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = poop;
                }
            }
        }

        return closest;
    }
}
