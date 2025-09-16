using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics; 

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Insaniquarium Clone");
        Raylib.SetTargetFPS(60);

        // Background
        Texture2D bg = Raylib.LoadTexture("Fishking/Fishbg.png");

        // Money system
        int money = 0;
        List<Fish> fishes = new List<Fish>();
        List<Coin> coins = new List<Coin>();

        // Start with one fish
        fishes.Add(new BasicFish(400, 300));

        while (!Raylib.WindowShouldClose())
        {
            // --- Update ---
            // Check keypress to buy fish (inside loop so it reacts each frame)
            if (Raylib.IsKeyPressed(KeyboardKey.One) && money >= 50)
            {
                fishes.Add(new BasicFish(100, 200));
                money -= 50;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Two) && money >= 150)
            {
                fishes.Add(new CarnivoreFish(200, 300));
                money -= 150;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Three) && money >= 150)
            {
                fishes.Add(new CarnivoreFish(200, 300));
                money -= 150;
            }

            // Update fish and coins
            foreach (var fish in fishes) fish.Update(coins);
            foreach (var coin in coins) coin.Update();

            // Mouse click: check for coins
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

            // --- Draw ---
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawTexture(bg, 0, 0, Color.White);

            foreach (var fish in fishes) fish.Draw();
            foreach (var coin in coins) coin.Draw();

            // HUD
            Raylib.DrawText($"Money: {money}", 10, 10, 20, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
