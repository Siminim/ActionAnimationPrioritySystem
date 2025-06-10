using System.Collections.Generic;

public static class CharacterActionLibrary
{
    public static readonly Dictionary<CharacterAction, ActionRequest> Actions = new Dictionary<CharacterAction, ActionRequest>
    {
        // Locomotion

        { CharacterAction.Idle, new IdleRequest() },
        { CharacterAction.Idle_Crouched, new IdleCrouchedRequest() },
        { CharacterAction.Walk, new WalkRequest() },
        { CharacterAction.Walk_Crouched, new WalkCrouchedRequest() },
        { CharacterAction.Run, new RunRequest() },
        { CharacterAction.Sprint, new SprintRequest() },
        { CharacterAction.Fall, new FallRequest() },
        { CharacterAction.Jump, new JumpRequest() },
        { CharacterAction.Land, new LandRequest() },

        // Upperbody

        { CharacterAction.WeaponsReady, new WeaponsReadyRequest() },
        { CharacterAction.WeaponsUnready, new WeaponsUnReadyRequest() },
        //{ CharacterAction.Blocking, new BlockingRequest() },

        // Fullbody Override

        { CharacterAction.Flinch, new FlinchRequest() },
        { CharacterAction.Attack, new AttackRequest() }
    };
}