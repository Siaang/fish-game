using System;
using System.Numerics;
using Raylib_cs;

abstract class Fish
{
    protected Texture2D sprite;
    protected float x, y;
    protected float hp = 100;
    protected float speed = 2f;
    protected float moveTimer = 0;
    protected Random rand = new Random();

    public Fish(string spritePath, float startX, float startY)
    {
        sprite = Raylib.LoadTexture(spritePath);
        x = startX; y = startY;
    }

    public virtual void Update(List<Coin> coins)
    {
        // Movement every few seconds
        moveTimer -= Raylib.GetFrameTime();
        if (moveTimer <= 0)
        {
            x += rand.Next(-50, 50);
            y += rand.Next(-30, 30);
            moveTimer = 2f; 
        }

        x = Math.Clamp(x, 0, Raylib.GetScreenWidth() - sprite.Width);
        y = Math.Clamp(y, 0, Raylib.GetScreenHeight() - sprite.Height);
    }

    public virtual void Draw()
    {
        Raylib.DrawTexture(sprite, (int)x, (int)y, Color.White);
        // HP Bar
        Raylib.DrawRectangle((int)x, (int)y - 10, 50, 5, Color.DarkGray);
        Raylib.DrawRectangle((int)x, (int)y - 10, (int)(50 * (hp / 100)), 5, Color.Green);
    }
}
