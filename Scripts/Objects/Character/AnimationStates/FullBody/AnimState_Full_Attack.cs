using Godot;
using System;

public class AnimState_Full_Attack : AnimationState
{
    public AnimState_Full_Attack(AnimationStateManager animator) : base(animator) {}

    private int attackCombo = 1;
    private float counter = 0.0f;
    private float attackBuffer = 1.0f;

    public override void EnterState()
    {
        GetAnimationPlaybackNode().Travel("Attack_" + attackCombo);
        animStateManager.animator.animationTree.Set("parameters/FullBodyAction/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }

    public override void UpdateState(double delta)
    {
        if (!IsAnimationPlaying() && IsAttackQueued())
        {
            counter = 0.0f;
            attackCombo++;
            
            GetAnimationPlaybackNode().Travel("Attack_" + attackCombo);
            animStateManager.animator.animationTree.Set("parameters/FullBodyAction/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }
        else if (!IsAnimationPlaying() && !IsAttackQueued())
        {
            counter += (float)delta;

            if (counter >= attackBuffer)
                animStateManager.ChangeState(animStateManager.animator.FullBodyStates[CharacterFullBodyState.None]);
        }

    }

    public override void ExitState()
    {
        attackCombo = 1;
    }

    private AnimationNodeStateMachinePlayback GetAnimationPlaybackNode()
    {
        return (AnimationNodeStateMachinePlayback)animStateManager.animator.animationTree.Get(GetWeaponAnimationPath());
    }

    private string GetWeaponAnimationPath()
    {
        return $"parameters/Actions/{animStateManager.character.currentWeapon}/playback";
    }

    private bool IsAnimationPlaying()
    {
        return (bool)animStateManager.animator.animationTree.Get("parameters/FullBodyAction/active");
    }

    private bool IsAttackQueued()
    {
        return animStateManager.character.QueueAttack;
    }
}
