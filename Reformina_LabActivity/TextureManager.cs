using Raylib_cs;

public class UITextureHandler
{
    public Texture2D Bg { get; private set; }

    // Fish
    public Texture2D SmallFishIcon { get; private set; }
    public Texture2D MediumFishIcon { get; private set; }
    public Texture2D CarnivoreFishIcon { get; private set; }
    public Texture2D JanitorFishIcon { get; private set; }
    public Texture2D AlphaFishIcon { get; private set; }

    // Pellets
    public Texture2D greenPelletIcon { get; private set; }
    public Texture2D redPelletIcon { get; private set; }

    public UITextureHandler()
    {
        Bg = Raylib.LoadTexture("assets/background.png");

        SmallFishIcon = Raylib.LoadTexture("assets/smallFish_buy.png");
        MediumFishIcon = Raylib.LoadTexture("assets/mediumFish_buy.png");
        CarnivoreFishIcon = Raylib.LoadTexture("assets/largeFish_buy.png");
        JanitorFishIcon = Raylib.LoadTexture("assets/janitorFish_buy.png");
        AlphaFishIcon = Raylib.LoadTexture("assets/massiveFish_buy.png");

        greenPelletIcon = Raylib.LoadTexture("assets/greenPelletIcon.png");
        redPelletIcon = Raylib.LoadTexture("assets/redPelletIcon.png");
    }

    public void Dispose()
    {
        Raylib.UnloadTexture(Bg);
        Raylib.UnloadTexture(SmallFishIcon);
        Raylib.UnloadTexture(MediumFishIcon);
        Raylib.UnloadTexture(CarnivoreFishIcon);
        Raylib.UnloadTexture(JanitorFishIcon);
        Raylib.UnloadTexture(AlphaFishIcon);

        Raylib.UnloadTexture(greenPelletIcon);
        Raylib.UnloadTexture(redPelletIcon);
    }
}

public class FishTextureHandler
{
    // store textures using arrays
    private Dictionary<string, Texture2D[]> fishTextures = new Dictionary<string, Texture2D[]>();

    public FishTextureHandler()
    {
        // SmallFish sprites
        fishTextures["SmallFish"] = new Texture2D[] {
            Raylib.LoadTexture("assets/small_fish1.png"),
            Raylib.LoadTexture("assets/small_fish2.png")
        };

        // MediumFish sprites
        fishTextures["MediumFish"] = new Texture2D[] {
            Raylib.LoadTexture("assets/medium_fish1.png"),
            Raylib.LoadTexture("assets/medium_fish2.png")
        };

        // LargeFish sprite 
        fishTextures["CarnivoreFish"] = new Texture2D[] {
            Raylib.LoadTexture("assets/large_fish.png")
        };

        //JanitorFish sprite
        fishTextures["JanitorFish"] = new Texture2D[] {
            Raylib.LoadTexture("assets/janitor_fish.png")
        };

        //AlphaFish Sprite
        fishTextures["AlphaFish"] = new Texture2D[] {
            Raylib.LoadTexture("assets/massive_fish.png")
        };
    }

    // Get all textures for a fish type
    public Texture2D[] GetTextures(string fishType)
    {
        return fishTextures[fishType];
    }

    // Get a random texture for a fish type
    public Texture2D GetRandomTexture(string fishType)
    {
        var textures = fishTextures[fishType];
        int index = Raylib.GetRandomValue(0, textures.Length - 1);
        return textures[index];
    }

    public void Dispose()
    {
        foreach (var kvp in fishTextures)
        {
            foreach (var tex in kvp.Value)
            {
                Raylib.UnloadTexture(tex);
            }
        }
    }
}
