using Godot;
using System;

public class ActionState_Attack : ActionState
{
    // After an attack ends, how long to wait for an input before going back to idle and starting over
    private float time = 1.0f;
    private float attackBuffer = 1.0f;

    public ActionState_Attack(Character character) : base(character) {}

    public override void EnterState()
    {
        character.Hurtbox.Monitoring = true;
        character.Hurtbox.ProcessMode = Node.ProcessModeEnum.Inherit;
        character.Hurtbox.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
        
        // Play current weapon attack animation 
    }

    public override void ExitState()
    {
        character.Hurtbox.Monitoring = false;
        character.Hurtbox.ProcessMode = Node.ProcessModeEnum.Disabled;
        character.Hurtbox.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
        
        // if attacked again, play next attack animation
    }

    

}