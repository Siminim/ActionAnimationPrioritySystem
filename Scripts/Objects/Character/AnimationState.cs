using Godot;
using System;

public class AnimationState
{
    protected AnimationStateManager animStateManager;

    public AnimationState(AnimationStateManager animator)
    {
        animStateManager = animator;
    }

    public virtual void EnterState()
    {

    }

    public virtual void UpdateState(double delta)
    {

    }

    public virtual void ExitState()
    {

    }
}
