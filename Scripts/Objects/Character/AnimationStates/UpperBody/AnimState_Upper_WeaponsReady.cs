using Godot;
using System;

public class AnimState_Upper_WeaponsReady : AnimationState
{
    public AnimState_Upper_WeaponsReady(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        SetUpperBodyBlendValue(1.0f);
        animStateManager.animator.animUpperBodyStateMachinePlayback.Travel("Fists_Ready");
    }

    public void SetUpperBodyBlendValue(float value)
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);

        //upperBodyBlendValue = Mathf.Lerp(upperBodyBlendValue, value, (float)deltaTime * upperBodyBlendTransitionSpeed);

        // DEBUG: Not using the lerped value
        animStateManager.animator.animationTree.Set("parameters/UpperLower_Blend/blend_amount", value);
    }
}