using Godot;
using System;

public class AnimState_Sprint : AnimationState
{
    public AnimState_Sprint(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("Sprint");
    }

    public override void UpdateState(double delta)
    {
        TurnToMoveDirection(delta);
    }

    public void TurnToMoveDirection(double delta)
    {
        if (animStateManager.character.GlobalMoveVector == Vector3.Zero)
            return;

        float moveAngle = Vector3.Back.SignedAngleTo(animStateManager.character.GlobalMoveVector, Vector3.Up);

        float faceAngle = Godot.Mathf.LerpAngle(animStateManager.animator.FaceAngle, moveAngle, (float)delta * animStateManager.animator.TurnSpeed);
        faceAngle = Godot.Mathf.Wrap(faceAngle, -Mathf.Pi, Mathf.Pi);

        animStateManager.animator.TurnToDirection(faceAngle);
    }
}
