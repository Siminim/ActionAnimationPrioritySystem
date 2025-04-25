using Godot;
using System;

public class AnimState_Idle_Crouch : AnimationState
{
    public AnimState_Idle_Crouch(AnimationStateManager animator) : base(animator) {}

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Idle_Crouch");
    }
}