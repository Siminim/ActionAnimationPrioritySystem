using Godot;
using System;

public class AnimState_Idle_Jump : AnimationState
{
    public AnimState_Idle_Jump(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Idle_Jump");
    }
}