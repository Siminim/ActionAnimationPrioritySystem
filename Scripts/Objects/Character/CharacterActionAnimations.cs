using System.Collections.Generic;

public static class CharacterActionLibrary
{
    public static Dictionary<CharacterAction, ActionRequest> Actions = new Dictionary<CharacterAction, ActionRequest>
    {
        { CharacterAction.Idle, new IdleRequest() },
        { CharacterAction.Walk, new WalkRequest() },
        { CharacterAction.Run, new RunRequest() },
        { CharacterAction.Sprint, new SprintRequest() }
    };
}

public enum CharacterAnimation
{
    Loco_Standing,
    Loco_Crouched,
    Loco_Air
}

public enum CharacterAction
{
    Idle,
    Walk,
    Run,
    Sprint
}