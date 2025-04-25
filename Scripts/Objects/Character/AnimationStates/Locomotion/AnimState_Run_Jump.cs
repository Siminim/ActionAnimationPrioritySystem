using Godot;
using System;

public class AnimState_Run_Jump : AnimationState
{
    public AnimState_Run_Jump(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Run_Air");
    }
}