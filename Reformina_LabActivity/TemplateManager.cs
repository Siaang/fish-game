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
    public float x, y;
    public float hp;
    public float hpTimer = 2f;

    public float age = 0f;
    public virtual float lifespan { get; set; }

    public bool isAdult = false;
    public bool isDead = false;
    public bool isActive = true;

    public float maxHp = 100;
    public float hungerTimer = 1f;
    public float coinTimer = 5f;
    public float poopTimer = 10f;
    public float scale = 0.5f;
    protected float speed = 2f;
    public int direction = 1;
    public bool triggered = false;
    public FishState currentState = FishState.Swim;
    protected float directionTimer = 0;
    protected Random rand = new Random();

    public Fish(Texture2D texture, float startX, float startY)
    {
        sprite = texture;
        x = startX;
        y = startY;
        maxHp = 100; 
        hp = maxHp;

    }

    public virtual void Update(List<Coin> coins, List<FoodPellets> pellets, string type)
    {
        y = Math.Clamp(y, 0, Raylib.GetScreenHeight() - (sprite.Height * scale));

        if (directionTimer <= 0)
        {
            directionTimer = rand.Next(5, 11);
        }
    }

    public virtual void Move(List<FoodPellets> pellets)
    {
        switch (currentState)
        {
            // ---------- MOVE RANDOMLY --------
            case FishState.Swim:
                float swimSpeed = 40f * Raylib.GetFrameTime();
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
                break;

            // ---------- HUNGRY -----------
            case FishState.Hungry:
                float hungrySpeed = 60f * Raylib.GetFrameTime();

                FoodPellets target = FindNearestPellet(pellets);
                if (target != null)
                {
                    Vector2 dir = Vector2.Normalize(new Vector2(target.x - x, target.y - y));
                    x += dir.X * hungrySpeed;
                    y += dir.Y * hungrySpeed;
                    direction = dir.X >= 0 ? 1 : -1;
                }
                else
                {
                    x += hungrySpeed * direction;

                    directionTimer -= Raylib.GetFrameTime();
                    if (directionTimer <= 0)
                    {
                        if (rand.NextDouble() < 0.8)
                        {
                            direction *= -1;
                        }
                        directionTimer = rand.Next(5, 11);
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
                break;
        }
    }

    public void Dead()
        {
            hp = 0;
            direction = 1; 
            y -= 20 * Raylib.GetFrameTime();
            if (!triggered)
            {
                //PlaySingle.PlaySound("FishDeath");
                triggered = true;
            }
            if (y <= 30)
            {
                isActive = false;
            }
        }

    public void MoveTowards(float targetX, float targetY, float speed)
    {
        Vector2 currentPos = new Vector2(x, y);
        Vector2 targetPos = new Vector2(targetX, targetY);

        Vector2 direction = targetPos - currentPos;

        if (direction.Length() > 0.01f)
        {
            direction = Vector2.Normalize(direction); // get direction unit vector
            float moveSpeed = speed * Raylib.GetFrameTime();
            x += direction.X * moveSpeed;
            y += direction.Y * moveSpeed;

            // update facing direction (flip sprite)
            this.direction = direction.X >= 0 ? 1 : -1;
        }
    }

    protected virtual FoodPellets FindNearestPellet(List<FoodPellets> pellets)
    {
        FoodPellets closest = null;
        float minDist = float.MaxValue;

        foreach (var pellet in pellets)
        {
            float dist = Vector2.Distance(new Vector2(x, y), new Vector2(pellet.x, pellet.y));
            if (dist < minDist)
            {
                minDist = dist;
                closest = pellet;
            }
        }

        return closest;
    }

    public static Fish FindNearestPrey(CarnivoreFish predator, List<Fish> fishes)
    {
        Fish closest = null;
        float minDist = float.MaxValue;

        foreach (var fish in fishes)
        {
            if (fish is SmallFish basic && !basic.isAdult)
            {
                float dist = Vector2.Distance(
                    new Vector2(predator.x, predator.y),
                    new Vector2(basic.x, basic.y)
                );
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = basic;
                }
            }
        }

        return closest;
    }

    public virtual bool IsCollidingWith(Fish otherFish)
    {
        Rectangle fishRect = new Rectangle(x, y, sprite.Width * scale, sprite.Height * scale);
        Rectangle otherRect = new Rectangle(otherFish.x, otherFish.y, otherFish.sprite.Width * otherFish.scale, otherFish.sprite.Height * otherFish.scale);
        return Raylib.CheckCollisionRecs(fishRect, otherRect);
    }
    
    public virtual bool IsCollidingWith(FoodPellets pellet)
    {
        Rectangle fishRect = new Rectangle(x, y, sprite.Width * scale, sprite.Height * scale);
        Rectangle pelletRect = new Rectangle(pellet.x, pellet.y, 8, 8);
        return Raylib.CheckCollisionRecs(fishRect, pelletRect);
    }

    public virtual void Draw()
    {
        Color tintColor = isDead ? new Color(150, 150, 150, 255) : Color.White;

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

        if (isDead)
        {
            src.Height = -Math.Abs(src.Height);
        }

        Raylib.DrawTexturePro(sprite, src, dest, new Vector2(0, 0), 0f, Color.White);
        Raylib.DrawTexturePro(sprite, src, dest, new Vector2(0, 0), 0f, tintColor);

        // Lifespan
        float lifePercent = 1f;
        if (lifespan > 0)
        {
            lifePercent = Math.Clamp((lifespan - age) / lifespan, 0f, 1f);
        }
        Raylib.DrawRectangle(
            (int)x,
            (int)y + 46,
            (int)(sprite.Width * 2 * lifePercent),
            5,
            Color.Yellow
        );

        // HP 
        float hpPercent = Math.Clamp(hp / maxHp, 0f, 1f);
        Raylib.DrawRectangle(
            (int)x,
            (int)y + 40,
            (int)(sprite.Width * 2 * hpPercent),
            5,
            Color.Green
        );
    }
}

