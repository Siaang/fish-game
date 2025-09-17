using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics; 

class Program
{
    static void Main()
    {
        Raylib.InitWindow(900, 600, "Insaniquarium Clone");
        Raylib.SetTargetFPS(60);

        // ---------- BACKGROUND -----------
        Texture2D bg = Raylib.LoadTexture("assets/background.png");

        // ---------- UI Assets -----------
        Texture2D regular_fish = Raylib.LoadTexture("assets/regularFish_buy.png");

        Vector2 buttonPos = new Vector2(130, 10);
        Rectangle buttonRect = new Rectangle(buttonPos.X, buttonPos.Y, regular_fish.Width, regular_fish.Height);

        // ---------- MONEY SYSTEM -----------
        int money = 0;
        List<Fish> fishes = new List<Fish>();
        List<Coin> coins = new List<Coin>();

        // Onready
        fishes.Add(new BasicFish(400, 300));

        while (!Raylib.WindowShouldClose())
        {
            // ---------- INPUT: FISH -----------
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
            Raylib.DrawText($"Money: {money}", 10, 10, 20, Color.White);
            Raylib.DrawTexture(regular_fish, 130, 10, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
