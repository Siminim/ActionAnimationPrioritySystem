using Godot;
using System;

public partial class CharacterAnimator
{
    private Character character;

    // ----------------------------------------------------------------------------------
    // ----------------------------------- Nodes ----------------------------------------
    // ----------------------------------------------------------------------------------

    private Skeleton3D skeleton;
    private AnimationPlayer animationPlayer;
    private AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback animLocomotionStateMachine;
    private AnimationNodeStateMachinePlayback animUpperBodyStateMachine;
    private AnimationNodeBlend2 animBlend;

    // ----------------------------------------------------------------------------------
    // ---------------------------- Animation Variables ---------------------------------
    // ---------------------------------------------------------------------------------- 

    private double deltaTime => character.GetPhysicsProcessDeltaTime();

    private float moveAngle = 0.0f;
    private float moveScale = 1.0f;
    private float moveScaleTransitionSpeed = 5.0f;

    private float faceAngle = 0.0f;
    private float turnSpeed = 5.0f;

    private Vector2 locomotionAngleVector = Vector2.Zero;

    private float upperBodyBlendValue = 0.0f;
    private float upperBodyBlendTransitionSpeed = 5.0f;

    private Vector2 headTurn = Vector2.Zero;
    private float headTurnTransitionSpeed = 2.5f;

    // ----------------------------------------------------------------------------------
    // ------------------------------- Initialization -----------------------------------
    // ---------------------------------------------------------------------------------- 

    public void Initialize(Character character)
    {
        this.character = character;

        animationPlayer = character.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        skeleton = character.GetNode<Skeleton3D>("GeneralSkeleton");

        animLocomotionStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/Locomotion_StateMachine/playback");
        animUpperBodyStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/UpperBody_StateMachine/playback");
 

        character.OnUpdateMoveState += UpdateMoveState;
        character.OnUpdateUpperBodyState += UpdateUpperBodyState;

    }

    // ----------------------------------------------------------------------------------
    // ----------------------------- Determine States -----------------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateMoveState(CharacterLocomotionState state)
    {
        switch (state)
        {
            case CharacterLocomotionState.Idle:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Idle");
                break;

            case CharacterLocomotionState.Idle_Crouch:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Idle_Crouch");
                break;

            case CharacterLocomotionState.Idle_Jump_Start:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Idle_Jump");
                break;

            case CharacterLocomotionState.Run_Jump_Start:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Run_Jump");
                break;

            case CharacterLocomotionState.Idle_Air:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Idle_Air");
                break;

            case CharacterLocomotionState.Run_Air:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Run_Air");
                break;

            case CharacterLocomotionState.WalkRun:
                UpdateWalkRunAnim(deltaTime);
                TurnToMoveDirection(deltaTime);
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("WalkRun");
                break;

            case CharacterLocomotionState.Sprint:
                TurnToMoveDirection(deltaTime);
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Sprint");
                break;

            case CharacterLocomotionState.Walk_Crouch:
                if (state != character.moveState)
                    animLocomotionStateMachine.Travel("Walk_Crouch");
                break;

            default:
                break;
        }
        character.moveState = state;
    }

    private void UpdateUpperBodyState(CharacterUpperBodyState state)
    {
        if (state == CharacterUpperBodyState.None)
            SetUpperBodyBlendValue(0.0f);
        else
            SetUpperBodyBlendValue(1.0f);

        switch (state)
        {
            case CharacterUpperBodyState.None:
                break;
            
            case CharacterUpperBodyState.WeaponsReady:
                if (state != character.upperBodyState)
                    WeaponReadyState();
                break;

            case CharacterUpperBodyState.Blocking:
                if (state != character.upperBodyState)
                    BlockingState();
                break;
        }

        character.upperBodyState = state;
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------ Other Functions -----------------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateWalkRunAnim(double delta)
    {
        float diff = moveAngle - faceAngle;
        Vector2 locomotionTargetAngleVector = new Vector2(-Mathf.Sin(diff), Mathf.Cos(diff));
        locomotionAngleVector = locomotionAngleVector.Lerp(locomotionTargetAngleVector, (float)delta * moveScaleTransitionSpeed);

        float scale = 1.0f;

        if (character.WalkEnabled)
            scale = 0.3f;

        moveScale = Mathf.Lerp(moveScale, scale, (float)delta * moveScaleTransitionSpeed);

        animationTree.Set("parameters/Locomotion_StateMachine/WalkRun/blend_position", locomotionAngleVector * moveScale);
    }

    private void TurnToMoveDirection(double delta)
    {
        if (character.GlobalMoveVector == Vector3.Zero)
            return;

        moveAngle = Vector3.Back.SignedAngleTo(character.GlobalMoveVector, Vector3.Up);

        faceAngle = Godot.Mathf.LerpAngle(faceAngle, moveAngle, (float)delta * turnSpeed);
        faceAngle = Godot.Mathf.Wrap(faceAngle, -Mathf.Pi, Mathf.Pi);

        skeleton.Rotation = new Vector3(0, faceAngle, 0);
    }

    private void SetUpperBodyBlendValue(float value)
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);

        upperBodyBlendValue = Mathf.Lerp(upperBodyBlendValue, value, (float)deltaTime * upperBodyBlendTransitionSpeed);

        // DEBUG: Not using the lerped value
        animationTree.Set("parameters/UpperLower_Blend/blend_amount", value);
    }

    public void SetLookingDirection(Basis basisX, Basis basisY)
    {
        Vector3 cameraX = basisX * Vector3.Forward;
        Vector3 cameraY = basisY.Inverse() * Vector3.Back;

        float x = Vector3.Back.SignedAngleTo(cameraX, Vector3.Up);
        float y = Vector3.Back.SignedAngleTo(cameraY, Vector3.Left);

        float wrappedX = Mathf.Wrap(faceAngle - x, -Mathf.Pi, Mathf.Pi);

        headTurn = headTurn.Lerp(new Vector2(wrappedX, y), (float)deltaTime * headTurnTransitionSpeed);

        animationTree.Set("parameters/HeadTurn/blend_position", headTurn);
    }

    public void WeaponReadyState()
    {
        // TODO: Should determine which weapon ready animation based on the weapon
        
        // if (character.weapon == fist)
        animUpperBodyStateMachine.Travel("Fists_Ready");
    }

    public void BlockingState()
    {
        // TODO: Should determine which blocking animation based on the weapon
        
        // if (character.weapon == fist)
        animUpperBodyStateMachine.Travel("Fists_Blocking");
    }
}
