using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

// ---------- BRONZE COIN -----------
class BronzeCoin : Coin
{
    public BronzeCoin(float x, float y)
        : base("assets/coin_bronze.png", x, y, 20) { }
}

// ---------- SILVER COIN -----------
class SilverCoin : Coin
{
    public SilverCoin(float x, float y)
        : base("assets/coin_silver.png", x, y, 30) { }   
}

// ---------- GOLD COIN -----------
class GoldCoin : Coin
{
    public GoldCoin(float x, float y)
        : base("assets/coin_gold.png", x, y, 50) { }
}

public class Poop : Coin
{
    public Poop(float x, float y)
        : base("assets/poop.png", x, y, -15) { }

    public override void Update()
    {
        y -= 1f;

        if (y < 20f)
        {
            y = 20f; 
        }

        timer += Raylib.GetFrameTime();
        if (timer >= frameTime)
        {
            currentFrame++;
            if (currentFrame >= maxFrames) currentFrame = 0;
            timer = 0f;
        }
    }
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
        : base(textureHandler!.GetRandomTexture("SmallFish"), startX, startY)
    {
        maxHp = 50;
        hp = maxHp;
        lifespan = 80f;
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
        : base(textureHandler!.GetRandomTexture("MediumFish"), startX, startY)
    { 
        maxHp = 80; 
        hp = maxHp;
        lifespan = 120f; 
    }
}

//---------- CARNIVORE FISH -----------
public class CarnivoreFish : Fish
{
    private static FishTextureHandler? textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    public CarnivoreFish(float startX, float startY)
        : base(textureHandler!.GetRandomTexture("CarnivoreFish"), startX, startY)
    {
        maxHp = 110;
        hp = maxHp;
        lifespan = 140f;
    }
}

// ---------- JANITOR FISH ---------- 
public class JanitorFish : Fish
{
    public Poop currentTargetPoop;

    private static FishTextureHandler? textureHandler;

    public static void SetTextureHandler(FishTextureHandler handler)
    {
        textureHandler = handler;
    }

    public JanitorFish(float startX, float startY)
       : base(textureHandler!.GetRandomTexture("JanitorFish"), startX, startY)
    {
        maxHp = 90;
        hp = maxHp;
        lifespan = 160f;
    }
}


