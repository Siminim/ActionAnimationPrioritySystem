using Godot;

public class WalkRequest : ActionRequest
{
    public WalkRequest()
    {
        actionName = CharacterAction.Walk;
        //animName = CharacterAnimation.Loco_Standing;
        actionLayer = ActionLayer.Legs;
        priority = 1;
    }

    public override void UpdateState(double delta, Character character)
    {
        float moveSpeed = character.walkSpeed;
        float moveAcceleration = character.walkAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;
    }
    
    public override void CheckRelevance(Character character)
    {
        if (character.globalMoveVector == Vector3.Zero)
            EndAction(character);
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.TurnToMoveDirection(delta);

        if (animator.character.crouchEnabled)
            animator.AnimateLocoCrouched(delta, 0.3f);
        else
            animator.AnimateLocoStanding(delta, 0.3f);
    }
}