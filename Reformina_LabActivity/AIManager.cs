using Raylib_cs;
using System;
using System.Numerics;
public enum FishState
{
    Idle, Swimming, Dead, Hungry
}

public class AIManager
{
    private List<Fish> fishes;
    private List<Coin> coins;
    private List<FoodPellets> pellets;

    public AIManager(List<Fish> fishes, List<Coin> coins, List<FoodPellets> pellets)
    {
        this.fishes = fishes;
        this.coins = coins;
        this.pellets = pellets;
    }
    
    public void Update()
    {

    }
}
