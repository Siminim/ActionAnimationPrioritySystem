using Godot;

public class LandRequest : ActionRequest
{
    public LandRequest()
    {
        actionName = CharacterAction.Land;
        actionLayer = CharacterActionLayer.Legs;
        priority = 6;
    }

    public override void EnterState(Character character)
    {
        character.animator.landingScale = 0.0f;
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimStateMachineName.Loco_Air.ToString());
        character.moving = true;
    }

    public override void UpdateState(double delta, Character character)
    {
        Vector3 targetVelocity = (character.globalMoveVector * character.airSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * character.airAcceleration * (float)delta;
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        Character character = animator.character;

        float speed = new Vector3(character.Velocity.X, 0, character.Velocity.Z).Length();
        animator.airSpeed = 1 - ((character.airSpeed - speed) / character.airSpeed);

        animator.landingScale += (float)delta * 7.5f;

        animator.AnimateLocoAir(delta);
    }

    public override void ExitState(Character character)
    {
        character.moving = false;
    }

    public override void CheckRelevance(Character character)
    {
        if (character.animator.landingScale >= 1.0f)
            EndAction(character);
    }
}