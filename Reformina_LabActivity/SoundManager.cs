using Raylib_cs;
using System.Collections.Generic;

public class SoundManager
{
    private Music bgMusic;
    private Dictionary<string, Sound> soundEffects = new();

    public void LoadAudio()
    {
        bgMusic = Raylib.LoadMusicStream("assets/music/Flask77-BG.mp3");
        Raylib.PlayMusicStream(bgMusic);

        soundEffects["click"] = Raylib.LoadSound("assets/music/click.wav");
        soundEffects["buyFish"] = Raylib.LoadSound("assets/music/buyFish.wav");
        soundEffects["coin1"] = Raylib.LoadSound("assets/music/coin1.wav");
        soundEffects["poop"] = Raylib.LoadSound("assets/music/poop.wav");
        soundEffects["drop"] = Raylib.LoadSound("assets/music/drop.wav");
        soundEffects["die"] = Raylib.LoadSound("assets/music/fishDeath.wav");
        soundEffects["eat"] = Raylib.LoadSound("assets/music/fishEat.wav");
    }

    public void Update()
    {
        Raylib.UpdateMusicStream(bgMusic);
    }

    public void Dispose()
    {
        Raylib.StopMusicStream(bgMusic);
        Raylib.UnloadMusicStream(bgMusic);

        foreach (var sound in soundEffects.Values)
        {
            Raylib.UnloadSound(sound);
        }

        soundEffects.Clear();
    }
    
    public void PlaySound(string name)
















































    {
        if (soundEffects.ContainsKey(name))
        {
            Raylib.PlaySound(soundEffects[name]);
        }
        else
        {
            System.Console.WriteLine($"Sound '{name}' not found.");
        }
    }

    public void UnloadAudio()
    {
        Raylib.StopMusicStream(bgMusic);
        Raylib.UnloadMusicStream(bgMusic);

        foreach (var sound in soundEffects.Values)
        {
            Raylib.UnloadSound(sound);
        }

        Raylib.CloseAudioDevice();
    }
}
public static class PlaySingle
{
    public static void PlaySound( string soundName)
    {
        Raylib.PlaySound( Raylib.LoadSound("res/" + soundName + ".wav"));
    }
}