using Godot;
using System;

public class AnimState_WalkRun : AnimationState
{
    private float moveAngle = 0.0f;
    private float moveScale = 1.0f;
    private float moveScaleTransitionSpeed = 5.0f;

    private Vector2 locomotionAngleVector = Vector2.Zero;

    public AnimState_WalkRun(AnimationStateManager animator) : base(animator) { }

    public override void EnterState()
    {
        animStateManager.animator.animLocomotionStateMachinePlayback.Travel("WalkRun");
    }

    public override void UpdateState(double delta)
    {
        UpdateWalkRunAnim(delta);
        if (animStateManager.character.upperBodyState == CharacterUpperBodyState.None)
            TurnToMoveDirection(delta);
    }

    private void UpdateWalkRunAnim(double delta)
    {
        float diff = moveAngle - animStateManager.animator.FaceAngle;
        Vector2 locomotionTargetAngleVector = new Vector2(-Mathf.Sin(diff), Mathf.Cos(diff));        
        locomotionAngleVector = locomotionAngleVector.Lerp(locomotionTargetAngleVector, (float)delta * moveScaleTransitionSpeed);

        float scale = 1.0f;

        if (animStateManager.character.WalkEnabled)
            scale = 0.3f;

        moveScale = Mathf.Lerp(moveScale, scale, (float)delta * moveScaleTransitionSpeed);

        animStateManager.animator.animationTree.Set("parameters/Locomotion_StateMachine/WalkRun/blend_position", locomotionAngleVector * moveScale);
    }

    public void TurnToMoveDirection(double delta)
    {
        if (animStateManager.character.GlobalMoveVector == Vector3.Zero)
            return;

        moveAngle = Vector3.Back.SignedAngleTo(animStateManager.character.GlobalMoveVector, Vector3.Up);

        float faceAngle = Godot.Mathf.LerpAngle(animStateManager.animator.FaceAngle, moveAngle, (float)delta * animStateManager.animator.TurnSpeed);
        faceAngle = Godot.Mathf.Wrap(faceAngle, -Mathf.Pi, Mathf.Pi);

        animStateManager.animator.TurnToDirection(faceAngle);
    }

}
