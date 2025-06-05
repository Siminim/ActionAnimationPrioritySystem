using Godot;

public class RunRequest : ActionRequest
{
    public RunRequest(Character character) : base(character)
    {
        animName = CharacterAnimation.WalkRun;
        actionLayer = ActionLayer.Legs;
        priority = 2;
    }

    public override void UpdateState(double delta)
    {
        float moveSpeed = character.runSpeed;
        float moveAcceleration = character.runAcceleration;

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;

        if (character.globalMoveVector == Vector3.Zero)
            EndAction();
    }
}