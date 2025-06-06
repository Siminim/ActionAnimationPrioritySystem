using Godot;

public class FallRequest : ActionRequest
{
    public FallRequest()
    {
        actionName = CharacterAction.Fall;
        actionLayer = ActionLayer.Legs;
        priority = 4;
    }

    public override void EnterState(Character character)
    {
        character.animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Air.ToString());
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
        float percentSpeed = 1 - ((character.airSpeed - speed) / character.airSpeed);

        GD.Print("---------------------------");
        GD.Print($"Max Speed: {character.airSpeed.ToString("0.0000")}");
        GD.Print($"Cur Speed: {speed.ToString("0.0000")}");
        GD.Print($"Dif Speed: {percentSpeed.ToString("0.0000")}");

        animator.AnimateLocoAir(delta, percentSpeed);
    }

    public override void CheckRelevance(Character character)
    {
        if (character.IsOnFloor())
            EndAction(character);
    }
}