using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics; 

class Program
{
    static void Main()
    {
        int screenWidth = 900;
        int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "Insaniquarium Clone");
        Raylib.SetTargetFPS(60);

        // ---------- BACKGROUND -----------
        Texture2D bg = Raylib.LoadTexture("assets/background.png");

        // ---------- UI Assets -----------
        Texture2D small_fish = Raylib.LoadTexture("assets/smallFish_buy.png");
        Texture2D medium_fish = Raylib.LoadTexture("assets/mediumFish_buy.png");
        Texture2D large_fish = Raylib.LoadTexture("assets/largeFish_buy.png");
        Texture2D massive_fish = Raylib.LoadTexture("assets/massiveFish_buy.png");

        Vector2 buttonPos = new Vector2(130, 10);
        Rectangle buttonRect = new Rectangle(buttonPos.X, buttonPos.Y, small_fish.Width, small_fish.Height);

        // ---------- MONEY SYSTEM -----------
        int money = 0;
        List<Fish> fishes = new List<Fish>();
        List<Coin> coins = new List<Coin>();

        // Onready
        fishes.Add(new SmallFish(400, 300));

        while (!Raylib.WindowShouldClose())
        {
            float x = Raylib.GetRandomValue(0, screenWidth - 50);
            float y = Raylib.GetRandomValue(100, screenHeight - 100);

            // ---------- INPUT: FISH -----------
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

            if (Raylib.IsKeyPressed(KeyboardKey.Three) && money >= 150)
            {
                fishes.Add(new MediumFish(x, y));
                money -= 150;
            }

            // ---------- UPDATE: FISH -----------
            foreach (var fish in fishes) fish.Update(coins);
            foreach (var coin in coins) coin.Update();

            // ---------- INPUT: COIN -----------
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

            // ---------- DRAW -----------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Rectangle src = new Rectangle(0, 0, bg.Width, bg.Height);
            Rectangle dest = new Rectangle(0, 0, 900, 600); 
            Raylib.DrawTexturePro(bg, src, dest, new Vector2(0, 0), 0f, Color.White);

            foreach (var fish in fishes) fish.Draw();
            foreach (var coin in coins) coin.Draw();

            // ---------- UI -----------
            Raylib.DrawText($"Money: {money}", 10, 580, 20, Color.White);
            Raylib.DrawTexture(small_fish, 220, 30, Color.White);
            Raylib.DrawTexture(medium_fish, 290, 30, Color.White);
            Raylib.DrawTexture(large_fish, 410, 10, Color.White);
            Raylib.DrawTexture(massive_fish, 530, 10, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
