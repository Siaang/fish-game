using System;
using System.Numerics;
using Raylib_cs;

abstract class Coin
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