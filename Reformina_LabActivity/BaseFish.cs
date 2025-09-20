using System;
using System.Numerics;
using Raylib_cs;

abstract class Fish
{
    protected Texture2D sprite;
    protected float x, y;
    protected float hp = 100;
    protected float speed = 2f;
    public int direction = 1;
    protected float directionTimer = 0;
    protected Random rand = new Random();

    public Fish(string spritePath, float startX, float startY)
    {
        sprite = Raylib.LoadTexture(spritePath);
        x = startX; y = startY;
    }

    public virtual void Update(List<Coin> coins)
    {
        // ---------- MOVE RANDOMLY -----------
        float swimSpeed = 50f * Raylib.GetFrameTime();
        x += swimSpeed * direction;

        directionTimer -= Raylib.GetFrameTime();
        if (directionTimer <= 0)
        {
            direction *= -1;
            directionTimer = 5f;
        }

        if (x > 890 && x < 580)
        {
            direction *= -1;
            directionTimer = 5f;
        }

        if (x > Raylib.GetScreenWidth())
        {
            x = 0f;
            y = rand.Next(100, 401);
        }
        else if (x < 0)
        {
            x = Raylib.GetScreenWidth();
            y = rand.Next(100, 401);
        }          
    }

    public virtual void Draw()
    {
        Rectangle src = new Rectangle(
            0,
            0,
            direction == 1 ? sprite.Width : -sprite.Width,
            sprite.Height);
        Rectangle dest = new Rectangle(
            x, 
            y, 
            sprite.Width * 2, 
            sprite.Height * 2
        );

        Raylib.DrawTexturePro(sprite, src, dest, new Vector2(0, 0), 0f, Color.White);

        Raylib.DrawRectangle((int)x, (int)y - 10, (int)(sprite.Width * 2), 5, Color.DarkGray);
        Raylib.DrawRectangle((int)x, (int)y - 10, 
            (int)((sprite.Width * 2) * (hp / 100)), 5, Color.Green);
    }

}
