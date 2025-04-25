using Godot;
using System;

public class AnimationStateManager
{
    private AnimationState currentState;
    public AnimationState CurrentState => currentState;

    public CharacterAnimator animator;
    public Character character => animator.character;

    public AnimationStateManager(CharacterAnimator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(AnimationState newState)
    {
        if (newState == currentState)
            return;

        currentState?.ExitState();
        
        currentState = newState;
        currentState.EnterState();
    }

    public void UpdateState(double delta)
    {
        currentState?.UpdateState(delta);
    }
}
