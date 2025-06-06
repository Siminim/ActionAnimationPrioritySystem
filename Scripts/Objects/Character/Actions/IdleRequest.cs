using Godot;

public class IdleRequest : ActionRequest
{
    public IdleRequest()
    {
        actionName = CharacterAction.Idle;
        actionLayer = ActionLayer.Legs;
        priority = 0;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Standing.ToString());
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.AnimateLocoStanding(delta, 0.0f);
    }
}