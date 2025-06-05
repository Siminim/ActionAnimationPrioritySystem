using Godot;

public class SprintRequest : ActionRequest
{
    public SprintRequest(Character character) : base(character)
    {
        animName = CharacterAnimation.Sprint;
        actionLayer = ActionLayer.Legs;
        priority = 3;
    }
}