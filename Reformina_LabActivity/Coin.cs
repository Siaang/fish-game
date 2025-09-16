using System;
using System.Numerics;
using Raylib_cs;

class Coin
{
    private Texture2D sprite;
    private float x, y;
    public int Value { get; private set; }
    private float spriteW = 32f, spriteH = 32f;

    public Coin(float startX, float startY, int value)
    {
        sprite = Raylib.LoadTexture("Fishking/coinSilver.png"); 
        x = startX;
        y = startY;
        Value = value;

        GetTextureSizeSafe();
    }

    private void GetTextureSizeSafe()
    {
        var t = sprite.GetType();

        var fw = t.GetField("width");
        var fh = t.GetField("height");
        if (fw != null && fh != null)
        {
            spriteW = Convert.ToSingle(fw.GetValue(sprite));
            spriteH = Convert.ToSingle(fh.GetValue(sprite));
            return;
        }

        var pw = t.GetProperty("Width");
        var ph = t.GetProperty("Height");
        if (pw != null && ph != null)
        {
            spriteW = Convert.ToSingle(pw.GetValue(sprite));
            spriteH = Convert.ToSingle(ph.GetValue(sprite));
            return;
        }

        spriteW = 32f;
        spriteH = 32f;
    }

    public void Update()
    {
        y += 1f; 
    }

    public void Draw()
    {
        Raylib.DrawTexture(sprite, (int)x, (int)y, Color.White);
    }

    public bool IsClicked(Vector2 mouse)
    {
        return Raylib.CheckCollisionPointRec(
            mouse,
            new Raylib_cs.Rectangle(x, y, spriteW, spriteH)
        );
    }
}
