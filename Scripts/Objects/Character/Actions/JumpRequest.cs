using Godot;

public class JumpRequest : ActionRequest
{
    public JumpRequest(Character character) : base(character) 
    { 
        animName = CharacterAnimation.Jump;
        actionLayer = ActionLayer.Legs;
        priority = 5;
    }

    public override void UpdateState(double delta)
    {
        if (character.queuedJump && (character.IsOnFloor() || character.coyoteTimer < character.coyoteTimeLimit) && character.Velocity.Y <= 0.0f)
        {
            character.Velocity -= character.GetGravity().Normalized() * character.jumpForce;

            character.queuedJump = false;
            character.coyoteTimer = character.coyoteTimeLimit;
        }

        character.timeSinceQueuedJump += (float)delta;

        if (character.timeSinceQueuedJump >= character.jumpBuffer)
            character.queuedJump = false;
    }
}