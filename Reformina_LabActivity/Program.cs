using Raylib_cs;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Initialize the window
        Raylib.InitWindow(800, 600, "Insaniquarium Clone");
        Raylib.SetTargetFPS(60);

        // Load background texture
        Texture2D background = Raylib.LoadTexture("Fishking/Fishbg.png");

        // Main game loop
        while (!Raylib.WindowShouldClose())
        {
            // --- Update game logic here ---
            // (handle fish movement, input, coins, etc.)

            // --- Draw ---
            Raylib.BeginDrawing();
            
            // Draw the background texture starting at (0,0)
            Raylib.DrawTexture(background, 0, 0, Color.White);

            // Optional: Draw some text on top
            Raylib.DrawText("Insaniquarium Clone", 10, 10, 20, Color.White);

            Raylib.EndDrawing();
        }

        // Unload texture and close window
        Raylib.UnloadTexture(background);
        Raylib.CloseWindow();
    }
}
