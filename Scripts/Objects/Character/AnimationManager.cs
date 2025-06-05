using Godot;
using System.Collections.Generic;

public class AnimationManager
{
    private CharacterAnimator animator;
    private List<AnimationRequest> activeAnimations = new List<AnimationRequest>();

    public AnimationManager(CharacterAnimator animator)
    {
        this.animator = animator;
    }

    public void RequestAnimation(AnimationRequest request)
    {

    }

    public void UpdateAnimations(double delta)
    {

    }

    public void EndAnimation(AnimationRequest request)
    {

    }

}