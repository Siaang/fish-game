using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;

    private float gameTimer = 0f;
    private int money = 0;
    private int aliveFishCount = 0;
    private bool isGameOver = false;
    public List<Fish> fishes = new List<Fish>();
    public List<Coin> coins = new List<Coin>();
    public List<FoodPellets> pellets = new List<FoodPellets>();
    public float poopCount;

    public float cleanliness;
    public HashSet<Fish> deadFishAlreadyCounted = new HashSet<Fish>(); //Store the dead fish + prevents it from counting the same fish twice

    // Managers
    private UITextureHandler uiTextures;
    private FishTextureHandler fishTextures;
    private AISystem aiSystem;
    private SoundManager soundManager;

    public GameManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;

        newGame();
      
    }

    public void newGame()
    {
        // Dispose old maangers
        if (uiTextures != null) uiTextures.Dispose();
        if (fishTextures != null) fishTextures.Dispose();
        if (soundManager != null) soundManager.Dispose();

        money = 100;
        fishes = new List<Fish>();
        coins = new List<Coin>();
        pellets = new List<FoodPellets>();
        cleanliness = 100f;
        gameTimer = 0f;

        // Load UI Texture Manager
        uiTextures = new UITextureHandler();

        // Fish icons
        fishTextures = new FishTextureHandler();
        SmallFish.SetTextureHandler(fishTextures);
        MediumFish.SetTextureHandler(fishTextures);
        CarnivoreFish.SetTextureHandler(fishTextures);
        JanitorFish.SetTextureHandler(fishTextures);
        AlphaFish.SetTextureHandler(fishTextures);

        // Sound
        soundManager = new SoundManager();
        soundManager.LoadAudio();

        // AI 
        aiSystem = new AISystem(fishes, pellets, coins, soundManager, this);

        // Start with one fish
        fishes.Add(new SmallFish(400, 300));
    }

    public void Update()
    {
        // Timer
        float deltaTime = Raylib.GetFrameTime();

        UpdateCleanliness();
        
        if (!isGameOver)
            gameTimer += deltaTime;

        soundManager.Update();

        float x = Raylib.GetRandomValue(0, screenWidth - 50);
        float y = Raylib.GetRandomValue(100, screenHeight - 100);

        // Game over    
        aliveFishCount = 0;
        foreach (var fish in fishes)
        {
            
            if (!fish.isDead) aliveFishCount++;
        }
        if (aliveFishCount == 0 || cleanliness <= 0)
        {
            isGameOver = true;

            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                newGame();
                isGameOver = false;
            }
        }

        // Input: buy fish
        if (Raylib.IsKeyPressed(KeyboardKey.One) && money >= 50)
        {
            fishes.Add(new SmallFish(x, y));
            money -= 50;
            soundManager.PlaySound("buyFish");
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Two) && money >= 150)
        {
            fishes.Add(new MediumFish(x, y));
            money -= 150;
            soundManager.PlaySound("buyFish");
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Three) && money >= 250)
        {
            fishes.Add(new CarnivoreFish(x, y));
            money -= 250;
            soundManager.PlaySound("buyFish");
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Four) && money >= 300)
        {
            fishes.Add(new JanitorFish(x, y));
            money -= 300;
            soundManager.PlaySound("buyFish");
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Five) && money >= 400)
        {
            fishes.Add(new AlphaFish(x, y));
            money -= 400;
            soundManager.PlaySound("buyFish");
        }

        // Update fish + coins
        aiSystem.Update(fishes, coins, pellets);
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
                    soundManager.PlaySound("coin1");
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

                money -= 5;
                soundManager.PlaySound("click");
            }

            if (Raylib.CheckCollisionPointRec(mouse, redPelletRect) && money >= 7)
            {
                float pelletX = Raylib.GetRandomValue(0, screenWidth - 20);
                float pelletY = 90;
                pellets.Add(new SmallFoodPellet(pelletX, pelletY));

                money -= 10;
                soundManager.PlaySound("click");
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

        // Tank cleanliness 
        foreach (var fish in fishes)
        {
            if (fish.isDead && !deadFishAlreadyCounted.Contains(fish))
            {
                cleanliness -= 2f;
                if (cleanliness < 0f) cleanliness = 0f;
                deadFishAlreadyCounted.Add(fish);
            }
        }
        
        // If cleanliness is less than 40% it'll degrade the fish's hp faster 
        if (cleanliness <= 40)
        {
            aiSystem.HealthDrain();
        }
    }

    public void UpdateCleanliness()
    {

        if (poopCount > 0)
        {
            cleanliness -= poopCount * 1f; 
            poopCount = 0; 
        }

        cleanliness = Math.Clamp(cleanliness, 0, 100f); 
    }

    public void Draw()
    {
        // Timer 
        int minutes = (int)(gameTimer / 60);
        int seconds = (int)(gameTimer % 60);
        // Game over 
        if (aliveFishCount == 0 || cleanliness <= 0)
        {
            Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, Color.White);
            Raylib.DrawText($"Game over! Press R To restart\n        Your time was {minutes}:{seconds}",
                130, 250, 40, Color.Red);

            return;
        }

        // Draw bg
        Rectangle src = new Rectangle(0, 0, uiTextures.Bg.Width, uiTextures.Bg.Height);
        Rectangle dest = new Rectangle(0, 0, screenWidth, screenHeight);
        Raylib.DrawTexturePro(uiTextures.Bg, src, dest, new Vector2(0, 0), 0f, Color.White);

        // Draw fishes & coins
        foreach (var fish in fishes) fish.Draw();
        foreach (var coin in coins) coin.Draw();

        // Draw UI
        Raylib.DrawText($"Time: {minutes}:{seconds}", 10, 10, 20, Color.White);
        Raylib.DrawText($"Tank Cleanliness: {cleanliness}%", 10, 30, 20, Color.White);
        Raylib.DrawText($"Money: {money}", screenWidth - 140, screenHeight - 20, 20, Color.White);
        Raylib.DrawTexture(uiTextures.SmallFishIcon, 10, screenHeight - 50, Color.White);
        Raylib.DrawTexture(uiTextures.MediumFishIcon, 50, screenHeight - 50, Color.White);
        Raylib.DrawTexture(uiTextures.CarnivoreFishIcon, 120, screenHeight - 70, Color.White);
        Raylib.DrawTexture(uiTextures.JanitorFishIcon, 200, screenHeight - 60, Color.White);
        Raylib.DrawTexture(uiTextures.AlphaFishIcon, 280, screenHeight - 65, Color.White);

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
