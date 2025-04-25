using Godot;
using System;

public class AnimState_Idle : AnimationState
{
    public AnimState_Idle(AnimationStateManager animator) : base(animator) { }
    
    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Idle");
    }
}
