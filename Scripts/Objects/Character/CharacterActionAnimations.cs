using System.Collections.Generic;

public static class CharacterActionLibrary
{
    public static Dictionary<CharacterAction, ActionRequest> Actions = new Dictionary<CharacterAction, ActionRequest>
    {
        { CharacterAction.Idle, new IdleRequest() },
        { CharacterAction.Idle_Crouched, new IdleCrouchedRequest() },
        { CharacterAction.Walk, new WalkRequest() },
        { CharacterAction.Walk_Crouched, new WalkCrouchedRequest() },
        { CharacterAction.Run, new RunRequest() },
        { CharacterAction.Sprint, new SprintRequest() },
        { CharacterAction.Fall, new FallRequest() }
    };
}

public enum CharacterAnimation : byte
{
    Loco_Standing,
    Loco_Crouched,
    Loco_Air
}

public enum CharacterAction : byte
{
    // --------------- Locomotion ----------------
    Idle,
    Idle_Crouched,
    Walk,
    Walk_Crouched,
    Run,
    Sprint,
    Fall


}