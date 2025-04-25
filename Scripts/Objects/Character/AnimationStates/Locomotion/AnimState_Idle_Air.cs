using Godot;
using System;

public class AnimState_Idle_Air : AnimationState
{
    public AnimState_Idle_Air(AnimationStateManager animator) : base(animator) {}

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Idle_Air");
    }
}