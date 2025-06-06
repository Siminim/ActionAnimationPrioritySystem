using Godot;

public class JumpRequest : ActionRequest
{
    public JumpRequest()
    {
        actionName = CharacterAction.Jump;
        actionLayer = ActionLayer.Legs;
        priority = 5;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Air.ToString());
        character.animator.landingScale = -1.0f;

        character.Velocity -= character.GetGravity().Normalized() * character.jumpForce;

        character.queuedJump = false;
        character.coyoteTimer = character.coyoteTimeLimit;
    }

    public override void UpdateState(double delta, Character character)
    {

    }

    public override void CancelState(Character character)
    {
        float velocityMod = character.Velocity.Y * 0.25f;
        character.Velocity += character.GetGravity().Normalized() * velocityMod * character.jumpDecay;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        if (animator.landingScale < 0.0f)
            animator.landingScale += (float)delta * 5.0f;

        Character character = animator.character;

        float speed = new Vector3(character.Velocity.X, 0, character.Velocity.Z).Length();
        animator.airSpeed = 1 - ((character.airSpeed - speed) / character.airSpeed);

        animator.AnimateLocoAir(delta);
    }

    public override void CheckRelevance(Character character)
    {
        if (character.IsOnFloor() && character.Velocity.Y <= 0.0f)
        {
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Land]);
            EndAction(character);
        }
        else if (character.Velocity.Y < 0.0f)
        {
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Fall]);
            EndAction(character);
        }

    }
}