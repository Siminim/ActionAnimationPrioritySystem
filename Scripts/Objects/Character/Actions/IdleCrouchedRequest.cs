using Godot;

public class IdleCrouchedRequest : ActionRequest
{
    public IdleCrouchedRequest()
    {
        actionName = CharacterAction.Idle_Crouched;
        actionLayer = ActionLayer.Legs;
        priority = 0;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Crouched.ToString());
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.AnimateLocoCrouched(delta, 0.0f);
    }

    public override void CheckRelevance(Character character)
    {
        if (!character.crouchEnabled)
            EndAction(character);
    }
}