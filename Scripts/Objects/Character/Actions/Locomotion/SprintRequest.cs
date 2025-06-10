using Godot;

public class SprintRequest : ActionRequest
{
    public SprintRequest()
    {
        actionName = CharacterAction.Sprint;
        actionLayer = CharacterActionLayer.Legs;
        priority = 3;
    }

    public override void EnterState(Character character)
    {
        character.crouchEnabled = false;
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimStateMachineName.Loco_Standing.ToString());
        character.moving = true;
    }

    public override void UpdateState(double delta, Character character)
    {
        float moveSpeed = character.sprintSpeed;
        float moveAcceleration = character.sprintAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;
    }

    public override void CheckRelevance(Character character)
    {
        if (!character.sprintEnabled || character.globalMoveVector == Vector3.Zero)
            EndAction(character);
    }

    public override void ExitState(Character character)
    {
        character.moving = false;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.TurnToMoveDirection(delta);
        animator.AnimateLocoStanding(delta, 1.0f);
    }
}