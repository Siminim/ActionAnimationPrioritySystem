using System.Collections.Generic;

public static class CharacterAnimations
{
    public static Dictionary<CharacterAnimation, string> animations = new Dictionary<CharacterAnimation, string>
    {
        { CharacterAnimation.Idle, "Idle" },
        { CharacterAnimation.WalkRun, "WalkRun" },
        { CharacterAnimation.Sprint, "Sprint" }
    };
}

public enum CharacterAnimation
{
    Standing,
    Crouching,
    
    Idle,
    WalkRun,
    Sprint,
    Jump,
    Fall
}