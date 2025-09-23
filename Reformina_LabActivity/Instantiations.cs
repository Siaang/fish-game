using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

// ---------- BRONZE COIN -----------
class BronzeCoin : Coin
{
    public BronzeCoin(float x, float y)
        : base("assets/coin_bronze.png", x, y, 10) { }
}

// ---------- SILVER COIN -----------
class SilverCoin : Coin
{
    public SilverCoin(float x, float y)
        : base("assets/coin_silver.png", x, y, 25) { }   
}

// ---------- GOLD COIN -----------
class GoldCoin : Coin
{
    public GoldCoin(float x, float y)
        : base("assets/coin_gold.png", x, y, 50) { }
}

class Poop : Coin
{
    public Poop(float x, float y)
        : base("assets/medium_fish1.png", x, y, -15) { }
}

//====================================================================

//---------- SMALL FISH -----------
class SmallFish : Fish
{
    private static FishTextureHandler? textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    public SmallFish(float startX, float startY)
        : base(textureHandler!.GetRandomTexture("SmallFish"), startX, startY) { }

    public override void Update(List<Coin> coins, List<FoodPellets> pellets, string fishType)
    {
        base.Update(coins, pellets, fishType);

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
    private static FishTextureHandler? textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    public MediumFish(float startX, float startY)
        : base(textureHandler!.GetRandomTexture("MediumFish"), startX, startY) { }

    public override void Update(List<Coin> coins, List<FoodPellets> pellets, string fishType)
    {
        base.Update(coins, pellets, fishType);

        coinTimer -= Raylib.GetFrameTime();
        if (coinTimer <= 0)
        {
            coins.Add(new BronzeCoin(x + 5, y + 5));
            coinTimer = 10f;
        }
    }
}

//---------- LARGE FISH -----------
class LargeFish : Fish
{
    private static FishTextureHandler? textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    public LargeFish(float startX, float startY)
        : base(textureHandler!.GetRandomTexture("largeFish"), startX, startY) { }

    public override void Update(List<Coin> coins, List<FoodPellets> pellets, string fishType)
    {
        base.Update(coins, pellets, fishType);

        coinTimer -= Raylib.GetFrameTime();
        if (coinTimer <= 0)
        {
            coins.Add(new SilverCoin(x + 10, y + 10));
            coinTimer = 15f;
        }
    }
}
