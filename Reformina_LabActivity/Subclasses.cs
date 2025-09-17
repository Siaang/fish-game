using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

class BasicFish : Fish
{
    private static readonly string[] _spritePaths = new string[]
    {
        "assets/regular_fish1.png",
        "assets/regular_fish2.png"
    };

    private float coinTimer = 0;

    public BasicFish(float startX, float startY)
        : base(ChooseRandomSprite(), startX, startY)
    {
    }

    private static string ChooseRandomSprite()
    {
        Random rand = new Random();
        int index = rand.Next(_spritePaths.Length);
        return _spritePaths[index];
    }

    public override void Update(List<Coin> coins)
    {
        base.Update(coins);

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
    public CarnivoreFish(float startX, float startY)
        : base("assets/snapper.png", startX, startY) { }

    public override void Update(List<Coin> coins)
    {
        base.Update(coins);
        // TODO: Eat smaller fish → students implement
    }
}
