using Godot;

public class IdleRequest : ActionRequest
{
    public IdleRequest(Character character) : base(character)
    {
        animName = CharacterAnimation.Idle;
        actionLayer = ActionLayer.Legs;
        priority = 0;
    }

    public override void UpdateState(double delta)
    {
        
    }
}