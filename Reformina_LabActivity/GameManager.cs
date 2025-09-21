using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;

    private int money = 0;
    private List<Fish> fishes = new List<Fish>();
    private List<Coin> coins = new List<Coin>();

    private UITextureHandler uiTextures;
    private FishTextureHandler fishTextures;

    public GameManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;

        // Load UI Textures
        uiTextures = new UITextureHandler();

        // Load fish textures 
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
        foreach (var fish in fishes) fish.Update(coins);
        foreach (var coin in coins) coin.Update();

        // Input: collect coins
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mouse = Raylib.GetMousePosition();
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (coins[i].IsClicked(mouse))
                {
                    money += coins[i].Value;
                    coins.RemoveAt(i);
                }
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
    }

    public void Dispose()
    {
        uiTextures.Dispose();
        fishTextures.Dispose();
    }
}
