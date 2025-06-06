using Godot;

public class RunRequest : ActionRequest
{
    public RunRequest()
    {
        actionName = CharacterAction.Run;
        actionLayer = ActionLayer.Legs;
        priority = 2;
    }

    public override void UpdateState(double delta, Character character)
    {
        float moveSpeed = character.runSpeed;
        float moveAcceleration = character.runAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;
    }

    public override void CheckRelevance(Character character)
    {
        if (character.walkEnabled || character.globalMoveVector == Vector3.Zero)
            EndAction(character);
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.TurnToMoveDirection(delta);

        if (animator.character.crouchEnabled)
            animator.AnimateLocoCrouched(delta, 0.6f);
        else
            animator.AnimateLocoStanding(delta, 0.6f);
    }
}