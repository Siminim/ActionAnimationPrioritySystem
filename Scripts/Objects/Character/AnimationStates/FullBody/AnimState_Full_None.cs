using Godot;
using System;

public class AnimState_Full_None : AnimationState
{
    public AnimState_Full_None(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animationTree.Set("parameters/FullBodyAction/request", (int)AnimationNodeOneShot.OneShotRequest.FadeOut);
    }
}










