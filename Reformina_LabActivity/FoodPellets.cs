using Raylib_cs;

public class FoodPellets
{
    internal float x;
    internal float y;
    public int nutrition;
    internal bool isActive = true;
    public Texture2D sprite;
    protected float fallSpeed = 30f; 

    public FoodPellets(float startX, float startY, int nutritionValue)
    {
        x = startX;
        y = startY;
        nutrition = nutritionValue;
    }
    public virtual void Update()
    {
        if (!isActive) return;

        y += fallSpeed * Raylib.GetFrameTime();

        if (y >= Raylib.GetScreenHeight())
        {
            Destroy();
        }
    }
    public virtual void Draw()
    {
        if (!isActive) return;

        Raylib.DrawTexture(sprite, (int)x, (int)y, Color.White);
    }
    protected virtual void Destroy()
    {
        isActive = false;
    }

}
public class BigFoodPellet : FoodPellets
{
    public BigFoodPellet(float startX, float startY) : base(startX, startY, 10)
    {
        sprite = Raylib.LoadTexture("assets/greenPellet.png");
    }
}
public class SmallFoodPellet : FoodPellets
{
    public SmallFoodPellet(float startX, float startY) : base(startX, startY, 5)
    {
        sprite = Raylib.LoadTexture("assets/redPellet.png");
    }
}