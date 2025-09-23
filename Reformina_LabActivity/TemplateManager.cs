using System;
using System.Numerics;
using Raylib_cs;


// the base template for the coins
public class Coin
{
    private Texture2D spriteSheet;
    private float x, y;

    // Frames 
    protected int frameWidth, frameHeight;
    protected int currentFrame = 0;
    protected float timer = 0f;
    protected float frameTime = 0.1f;
    protected int maxFrames = 8;

    public int Value { get; private set; }

    public Coin(string spritePath, float startX, float startY, int value)
    {
        spriteSheet = Raylib.LoadTexture(spritePath);
        x = startX; y = startY;
        Value = value;

        frameWidth = spriteSheet.Width / maxFrames;
        frameHeight = spriteSheet.Height;
    }

    public virtual void Update()
    {
        y += 1f;

        timer += Raylib.GetFrameTime();
        if (timer >= frameTime)
        {
            currentFrame++;
            if (currentFrame >= maxFrames) currentFrame = 0;
            timer = 0f;
        }
    }

    public virtual void Draw()
    {
        Rectangle source = new Rectangle(
            frameWidth * currentFrame,
            0,
            frameWidth,
            frameHeight
        );

        Rectangle dest = new Rectangle(
            x,
            y,
            frameWidth * 2,
            frameHeight * 2
        );

        Vector2 origin = new Vector2(0, 0);
        Raylib.DrawTexturePro(spriteSheet, source, dest, origin, 0f, Color.White);
    }

    public bool IsClicked(Vector2 mouse)
    {
        return Raylib.CheckCollisionPointRec(
            mouse,
            new Rectangle(x, y, frameWidth, frameHeight)
        );
    }
}

// the base template for the fishyyy
public class Fish
{
    protected Texture2D sprite;
    protected float x, y;
    protected float hp = 100;
    protected float speed = 2f;
    public int direction = 1;
    protected float directionTimer = 0;
    protected Random rand = new Random();

    public Fish(Texture2D texture, float startX, float startY)
    {
    sprite = texture;
    x = startX;
    y = startY;
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
