using Godot;

public class WalkRequest : ActionRequest
{
    public WalkRequest()
    {
        actionName = CharacterAction.Walk;
        actionLayer = CharacterActionLayer.Legs;
        priority = 1;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimStateMachineName.Loco_Standing.ToString());
        character.moving = true;
    }

    public override void UpdateState(double delta, Character character)
    {
        float moveSpeed = character.walkSpeed;
        float moveAcceleration = character.walkAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.TurnToMoveDirection(delta);
        animator.AnimateLocoStanding(delta, 0.3f);
    }
    
    public override void ExitState(Character character)
    {
        character.moving = false;
    }

    public override void CheckRelevance(Character character)
    {
        if (character.globalMoveVector == Vector3.Zero || !character.walkEnabled)
            EndAction(character);
    }
}