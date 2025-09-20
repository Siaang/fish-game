using System;
using System.Numerics;
using Raylib_cs;

// ---------- BRONZE COIN -----------
class BronzeCoin : Coin
{
    public BronzeCoin(float x, float y)
        : base("assets/coin_bronze.png", x, y, 10) 
    {
    }
}

// ---------- SILVER COIN -----------
class SilverCoin : Coin
{
    public SilverCoin(float x, float y)
        : base("assets/coin_silver.png", x, y, 25)
    {
    }
}

// ---------- GOLD COIN -----------
class GoldCoin : Coin
{
    public GoldCoin(float x, float y)
        : base("assets/coin_gold.png", x, y, 50)
    {
    }
}

