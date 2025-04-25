using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterAnimator
{
    public Character character;
    
    private AnimationStateManager locoAnimStateManager;
    private AnimationStateManager upperBodyAnimStateManager;
    private AnimationStateManager fullBodyAnimStateManager;

    public AnimationStateManager LocoAnimStateManager => locoAnimStateManager;
    public AnimationStateManager UpperBodyAnimStateManager => upperBodyAnimStateManager;
    public AnimationStateManager FullBodyAnimStateManager => fullBodyAnimStateManager;

    // ----------------------------------------------------------------------------------
    // ----------------------------------- Nodes ----------------------------------------
    // ----------------------------------------------------------------------------------

    public Skeleton3D skeleton;
    public AnimationPlayer animationPlayer;
    public AnimationTree animationTree;
    public AnimationNodeStateMachinePlayback animLocomotionStateMachinePlayback;
    public AnimationNodeStateMachinePlayback animUpperBodyStateMachinePlayback;
    public AnimationNodeStateMachinePlayback animFullBodyStateMachinePlayback;

    // ----------------------------------------------------------------------------------
    // ---------------------------- Animation Variables ---------------------------------
    // ---------------------------------------------------------------------------------- 

    //private double deltaTime => character.GetPhysicsProcessDeltaTime();

    private float faceAngle = 0.0f;
    private float turnSpeed = 5.0f;

    public float FaceAngle => faceAngle;
    public float TurnSpeed => turnSpeed;

    // private Vector2 headTurn = Vector2.Zero;
    // private float headTurnTransitionSpeed = 2.5f;

    private Dictionary<CharacterLocomotionState, AnimationState> locomotionStates;
    private Dictionary<CharacterUpperBodyState, AnimationState> upperBodyStates;
    private Dictionary<CharacterFullBodyState, AnimationState> fullBodyStates;

    public Dictionary<CharacterLocomotionState, AnimationState> LocomotionStates => locomotionStates;
    public Dictionary<CharacterUpperBodyState, AnimationState> UpperBodyStates => upperBodyStates;
    public Dictionary<CharacterFullBodyState, AnimationState> FullBodyStates => fullBodyStates;

    // ----------------------------------------------------------------------------------
    // ------------------------------- Initialization -----------------------------------
    // ---------------------------------------------------------------------------------- 

    public void Initialize(Character character)
    {
        this.character = character;

        animationPlayer = character.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        skeleton = character.GetNode<Skeleton3D>("GeneralSkeleton");

        animLocomotionStateMachinePlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/Locomotion_StateMachine/playback");
        animUpperBodyStateMachinePlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/UpperBody_StateMachine/playback");
        animFullBodyStateMachinePlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/Actions/playback");
    
        locoAnimStateManager = new AnimationStateManager(this);
        upperBodyAnimStateManager = new AnimationStateManager(this);
        fullBodyAnimStateManager = new AnimationStateManager(this);

        InitializeStates();

        character.OnUpdateMoveState += UpdateLocomotionState;
        character.OnUpdateUpperBodyState += UpdateUpperBodyState;
        character.OnUpdateFullBodyState += UpdateFullBodyState;
    }

    public void Update(double delta)
    {
        locoAnimStateManager.UpdateState(delta);
        upperBodyAnimStateManager.UpdateState(delta);
        fullBodyAnimStateManager.UpdateState(delta);
    }

    private void InitializeStates()
    {
        locomotionStates = new Dictionary<CharacterLocomotionState, AnimationState>
        {
            { CharacterLocomotionState.Idle, new AnimState_Idle(locoAnimStateManager) },
            { CharacterLocomotionState.WalkRun, new AnimState_WalkRun(locoAnimStateManager) },
            { CharacterLocomotionState.Sprint, new AnimState_Sprint(locoAnimStateManager) },
            { CharacterLocomotionState.Idle_Jump_Start, new AnimState_Idle_Jump(locoAnimStateManager) },
            { CharacterLocomotionState.Run_Jump_Start, new AnimState_Run_Jump(locoAnimStateManager) },
            { CharacterLocomotionState.Idle_Air, new AnimState_Idle_Air(locoAnimStateManager) },
            { CharacterLocomotionState.Run_Air, new AnimState_Run_Air(locoAnimStateManager) }
        };

        upperBodyStates = new Dictionary<CharacterUpperBodyState, AnimationState>
        {
            { CharacterUpperBodyState.None, new AnimState_Upper_None(upperBodyAnimStateManager) },
            { CharacterUpperBodyState.WeaponsReady, new AnimState_Upper_WeaponsReady(upperBodyAnimStateManager) },
            { CharacterUpperBodyState.Blocking, new AnimState_Upper_Blocking(upperBodyAnimStateManager) },
        };

        fullBodyStates = new Dictionary<CharacterFullBodyState, AnimationState>
        {
            { CharacterFullBodyState.None, new AnimState_Full_None(fullBodyAnimStateManager) },
            { CharacterFullBodyState.Attack, new AnimState_Full_Attack(fullBodyAnimStateManager) },
        };
    }

    // ----------------------------------------------------------------------------------
    // ----------------------------- Determine States -----------------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateLocomotionState(CharacterLocomotionState state)
    {
        if (locomotionStates.ContainsKey(state))
            locoAnimStateManager.ChangeState(locomotionStates[state]);
    }

    private void UpdateUpperBodyState(CharacterUpperBodyState state)
    {
        if (upperBodyStates.ContainsKey(state))
            upperBodyAnimStateManager.ChangeState(upperBodyStates[state]);
    }

    private void UpdateFullBodyState(CharacterFullBodyState state)
    {
        if (fullBodyStates.ContainsKey(state))
            fullBodyAnimStateManager.ChangeState(fullBodyStates[state]);
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------ Other Functions -----------------------------------
    // ---------------------------------------------------------------------------------- 


    public void TurnToDirection(float angle)
    {
        faceAngle = angle;
        skeleton.Rotation = new Vector3(0, angle, 0);
    }

    // public void SetLookingDirection(Basis basisX, Basis basisY)
    // {
    //     Vector3 cameraX = basisX * Vector3.Forward;
    //     Vector3 cameraY = basisY.Inverse() * Vector3.Back;

    //     float x = Vector3.Back.SignedAngleTo(cameraX, Vector3.Up);
    //     float y = Vector3.Back.SignedAngleTo(cameraY, Vector3.Left);

    //     float wrappedX = Mathf.Wrap(faceAngle - x, -Mathf.Pi, Mathf.Pi);

    //     headTurn = headTurn.Lerp(new Vector2(wrappedX, y), (float)deltaTime * headTurnTransitionSpeed);

    //     animationTree.Set("parameters/HeadTurn/blend_position", headTurn);
    // }



}