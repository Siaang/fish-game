using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

// ---------- BRONZE COIN -----------
class BronzeCoin : Coin
{
    public BronzeCoin(float x, float y)
        : base("assets/coin_bronze.png", x, y, 10) {}
}

// ---------- SILVER COIN -----------
class SilverCoin : Coin
{
    public SilverCoin(float x, float y)
        : base("assets/coin_silver.png", x, y, 25) {}   
}

// ---------- GOLD COIN -----------
class GoldCoin : Coin
{
    public GoldCoin(float x, float y)
        : base("assets/coin_gold.png", x, y, 50) {}
}

//---------------------------------------------------------------------

//---------- SMALL FISH -----------
class SmallFish : Fish
{
    private static FishTextureHandler textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    private float coinTimer = 0;

    public SmallFish(float startX, float startY)
    : base(textureHandler.GetRandomTexture("SmallFish"), startX, startY) {}
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
    private static FishTextureHandler textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }
    
    private float coinTimer = 0;

    public MediumFish(float startX, float startY)
    : base(textureHandler.GetRandomTexture("MediumFish"), startX, startY) {}


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
    private static FishTextureHandler textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    private float coinTimer = 0;

    public LargeFish(float startX, float startY)
        : base(textureHandler.GetRandomTexture("largeFish"), startX, startY) {}


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


