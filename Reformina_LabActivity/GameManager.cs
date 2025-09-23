using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;

    private int money = 0;
    public List<Fish> fishes = new List<Fish>();
    public List<Coin> coins = new List<Coin>();
    public List<FoodPellets> pellets = new List<FoodPellets>();

    // Managers
    private UITextureHandler uiTextures;
    private FishTextureHandler fishTextures;
    //private AISystem aiSystem;

    public GameManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;

        // Load UI Texture Manager
        uiTextures = new UITextureHandler();

        // Fish icons
        fishTextures = new FishTextureHandler();
        SmallFish.SetTextureHandler(fishTextures);
        MediumFish.SetTextureHandler(fishTextures);
        LargeFish.SetTextureHandler(fishTextures);

        // Start with one fish
        fishes.Add(new SmallFish(400, 300));
    }

    public void Update()
    {
        float x = Raylib.GetRandomValue(0, screenWidth - 50);
        float y = Raylib.GetRandomValue(100, screenHeight - 100);

        // Input: buy fish
        if (Raylib.IsKeyPressed(KeyboardKey.One) && money >= 50)
        {
            fishes.Add(new SmallFish(x, y));
            money -= 50;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Two) && money >= 150)
        {
            fishes.Add(new MediumFish(x, y));
            money -= 150;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Three) && money >= 250)
        {
            fishes.Add(new LargeFish(x, y));
            money -= 250;
        }

        // Update fish + coins
        foreach (var fish in fishes) fish.Update(coins, pellets, fish.GetType().Name);
        foreach (var coin in coins) coin.Update();

        Vector2 mouse = Raylib.GetMousePosition();

        // Input: collect coins
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (coins[i].IsClicked(mouse))
                {
                    money += coins[i].Value;
                    coins.RemoveAt(i);
                }
            }
        }

        // Pellets
        Rectangle greenPelletRect = new Rectangle(790, 520, uiTextures.greenPelletIcon.Width, uiTextures.greenPelletIcon.Height);
        Rectangle redPelletRect = new Rectangle(840, 520, uiTextures.redPelletIcon.Width, uiTextures.redPelletIcon.Height);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (Raylib.CheckCollisionPointRec(mouse, greenPelletRect) && money >= 3)
            {
                float pelletX = Raylib.GetRandomValue(0, screenWidth - 20);
                float pelletY = 70;
                pellets.Add(new BigFoodPellet(pelletX, pelletY));

                money -= 3;
            }

            if (Raylib.CheckCollisionPointRec(mouse, redPelletRect) && money >= 7)
            {
                float pelletX = Raylib.GetRandomValue(0, screenWidth - 20);
                float pelletY = 90;
                pellets.Add(new SmallFoodPellet(pelletX, pelletY));

                money -= 7;
            }
        }

        // Update pellets
        for (int i = pellets.Count - 1; i >= 0; i--)
        {
            pellets[i].Update();
            if (!pellets[i].isActive)
            {
                pellets.RemoveAt(i);
            }
        }
    }

    public void Draw()
    {
        // Draw bg
        Rectangle src = new Rectangle(0, 0, uiTextures.Bg.Width, uiTextures.Bg.Height);
        Rectangle dest = new Rectangle(0, 0, screenWidth, screenHeight);
        Raylib.DrawTexturePro(uiTextures.Bg, src, dest, new Vector2(0, 0), 0f, Color.White);

        // Draw fishes & coins
        foreach (var fish in fishes) fish.Draw();
        foreach (var coin in coins) coin.Draw();

        // Draw UI
        Raylib.DrawText($"Money: {money}", 10, screenHeight - 20, 20, Color.White);
        Raylib.DrawTexture(uiTextures.SmallFishIcon, 220, 30, Color.White);
        Raylib.DrawTexture(uiTextures.MediumFishIcon, 290, 30, Color.White);
        Raylib.DrawTexture(uiTextures.LargeFishIcon, 410, 10, Color.White);
        Raylib.DrawTexture(uiTextures.MassiveFishIcon, 530, 10, Color.White);

        Raylib.DrawTexture(uiTextures.greenPelletIcon, 790, 520, Color.White);
        Raylib.DrawTexture(uiTextures.redPelletIcon, 840, 520, Color.White);

        // Draw pellets
        foreach (var pellet in pellets)
            pellet.Draw();

    }

    public void Dispose()
    {
        uiTextures.Dispose();
        fishTextures.Dispose();
    }
}
