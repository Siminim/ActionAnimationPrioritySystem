using Godot;

public class RunRequest : ActionRequest
{
    public RunRequest()
    {
        actionName = CharacterAction.Run;
        actionLayer = CharacterActionLayer.Legs;
        priority = 1;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimStateMachineName.Loco_Standing.ToString());
    }

    public override void UpdateState(double delta, Character character)
    {
        float moveSpeed = character.runSpeed;
        float moveAcceleration = character.runAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.TurnToMoveDirection(delta);
        animator.AnimateLocoStanding(delta, 0.6f);
    }

    public override void CheckRelevance(Character character)
    {
        if (character.walkEnabled || character.globalMoveVector == Vector3.Zero)
            EndAction(character);
    }
}