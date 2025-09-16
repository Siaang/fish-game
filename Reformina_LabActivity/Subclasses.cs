using System;
using System.Numerics;
using Raylib_cs;

class BasicFish : Fish
{
    private float coinTimer = 0;

    public BasicFish(float startX, float startY) : base("assets/fish_basic.png", startX, startY) {}

    public override void Update(List<Coin> coins)
    {
        base.Update(coins);

        // Drops a coin every 5 sec
        coinTimer -= Raylib.GetFrameTime();
        if (coinTimer <= 0)
        {
            coins.Add(new Coin(x + 20, y + 20, 25));
            coinTimer = 5f;
        }
    }
}

class CarnivoreFish : Fish
{
    public CarnivoreFish(float startX, float startY) : base("assets/fish_carnivore.png", startX, startY) {}

    public override void Update(List<Coin> coins)
    {
        base.Update(coins);
        // TODO: Eat smaller fish → students implement
    }
}

