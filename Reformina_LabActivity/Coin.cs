using System;
using System.Numerics;
using Raylib_cs;

class Coin
{
    private Texture2D spriteSheet;
    private float x, y;
    public int Value { get; private set; }

    // Frame info for anim
    private int frameWidth = 32;
    private int frameHeight = 32;
    private int maxFrames = 8;        
    private float frameTime = 0.1f;   
    private int currentFrame = 0;
    private float timer = 0f;

    public Coin(float startX, float startY, int value)
    {
        spriteSheet = Raylib.LoadTexture("assets/coin_gold.png"); 
        x = startX;
        y = startY;
        Value = value;

        frameWidth = spriteSheet.Width / maxFrames; 
        frameHeight = spriteSheet.Height;
    }

    public void Update()
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

    public void Draw()
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
