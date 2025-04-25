using Godot;
using System;

public class AnimState_Run_Air : AnimationState
{
    public AnimState_Run_Air(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Run_Air");
    }
}