using Godot;

public class FlinchRequest : ActionRequest
{
    public FlinchRequest()
    {
        actionName = CharacterAction.Flinch;
        actionLayer = CharacterActionLayer.FullbodyOverride;
        priority = 100;
    }

    public override void EnterState(Character character)
    {
        GD.Print("Flinch");
    }
}