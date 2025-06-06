using Godot;

public class IdleRequest : ActionRequest
{
    public IdleRequest()
    {
        actionName = CharacterAction.Idle;
        actionLayer = ActionLayer.Legs;
        priority = 0;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        if (animator.character.crouchEnabled)
            animator.AnimateLocoCrouched(delta, 0.0f);
        else
            animator.AnimateLocoStanding(delta, 0.0f);
    }
}