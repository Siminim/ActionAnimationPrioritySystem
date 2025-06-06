using Godot;

public class JumpRequest : ActionRequest
{
    public JumpRequest()
    { 
        //actionName = CharacterAction.Jump;
        actionLayer = ActionLayer.Legs;
        priority = 5;
    }

    public override void UpdateState(double delta, Character character)
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