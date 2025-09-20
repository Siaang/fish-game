using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

//---------- SMALL FISH -----------
class SmallFish : Fish
{
    private static readonly string[] _spritePaths = new string[]
    {
        "assets/small_fish1.png",
        "assets/small_fish2.png"
    };

    private float coinTimer = 0;

    public SmallFish(float startX, float startY)
        : base(ChooseRandomSprite(), startX, startY) {}

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
            coins.Add(new BronzeCoin(x + 5, y + 5));
            coinTimer = 5f;
        }
    }
}

//---------- MEDIUM FISH -----------
class MediumFish : Fish
{
    private static readonly string[] _spritePaths = new string[]
    {
        "assets/medium_fish1.png",
        "assets/medium_fish2.png"
    };

    private float coinTimer = 0;

    public MediumFish(float startX, float startY)
        : base(ChooseRandomSprite(), startX, startY) { }

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
            coins.Add(new SilverCoin(x + 5, y + 5));
            coinTimer = 8f;
        }
    }
}

 //---------- LARGE FISH -----------
class LargeFish : Fish
{
    private float coinTimer = 0;

    public LargeFish(float startX, float startY)
        : base("assets/large_fish.png", startX, startY) { }

    public override void Update(List<Coin> coins)
    {
        base.Update(coins);

        coinTimer -= Raylib.GetFrameTime();
        if (coinTimer <= 0)
        {
            coins.Add(new GoldCoin(x + 10, y + 10));
            coinTimer = 12f;
        }
    }
}

