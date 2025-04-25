using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody3D
{
    // ----------------------------------------------------------------------------------
    // ------------------------------------ Nodes ---------------------------------------
    // ---------------------------------------------------------------------------------- 

    private Area3D hurtbox;
    public Area3D Hurtbox => hurtbox;

    // ----------------------------------------------------------------------------------
    // ---------------------------------- Delegates -------------------------------------
    // ---------------------------------------------------------------------------------- 

    public Delegates.CharacterMoveStateParameterDelegate OnUpdateMoveState;
    public Delegates.CharacterUpperBodyStateParameterDelegate OnUpdateUpperBodyState;
    public Delegates.CharacterFullBodyStateParameterDelegate OnUpdateFullBodyState;

    // ----------------------------------------------------------------------------------
    // ----------------------------------- Visuals --------------------------------------
    // ---------------------------------------------------------------------------------- 

    private CharacterAnimator animator;
    //public void AnimateHead(Basis basisX, Basis basisY) => animator.SetLookingDirection(basisX, basisY);

    // ----------------------------------------------------------------------------------
    // ------------------------------ State Conditions ----------------------------------
    // ---------------------------------------------------------------------------------- 

    public CharacterLocomotionState moveState = CharacterLocomotionState.Idle;
    public CharacterUpperBodyState upperBodyState = CharacterUpperBodyState.None;
    public CharacterFullBodyState fullBodyState = CharacterFullBodyState.None;

    // ----------------------------------------------------------------------------------
    // ------------------------------ Combat Variables ----------------------------------
    // ---------------------------------------------------------------------------------- 

    protected bool weaponsReady = false;
    protected bool blockEnabled = false;

    protected bool queueAttack = false;
    public bool QueueAttack => queueAttack;

    public WeaponType currentWeapon = WeaponType.Fists;

    // ----------------------------------------------------------------------------------
    // ----------------------------- Movement Variables ---------------------------------
    // ---------------------------------------------------------------------------------- 

    protected bool walkEnabled = false;
    protected bool sprintEnabled = false;
    protected bool crouchEnabled = false;
    public bool WalkEnabled => walkEnabled;
    public bool SprintEnabled => sprintEnabled;
    public bool CrouchEnabled => crouchEnabled;

    private Vector3 globalMoveVector = Vector3.Zero;
    public Vector3 GlobalMoveVector => globalMoveVector;

    private float airSpeed = 5.0f;
    private float airAcceleration = 0.5f;

    private float walkSpeed = 2.0f;
    private float walkAcceleration = 3.0f;

    private float runSpeed = 6.0f;
    private float runAcceleration = 3.0f;

    private float sprintSpeed = 10.0f;
    private float sprintAcceleration = 5.0f;

    private float maxFallSpeed = -50.0f;

    private float frictionStrength = 0.85f;

    // ----------------------------------------------------------------------------------
    // ------------------------------ Jump Variables ------------------------------------
    // ---------------------------------------------------------------------------------- 

    public bool queuedJump = false;
    private bool onGround = false;

    private float jumpForce = 8.5f;

    private float timeSinceQueuedJump = 0.0f;

    private float jumpBuffer = 0.125f;

    private float jumpDecay = 2.0f;

    private float coyoteTimer = 0.0f;
    private float coyoteTimeLimit = 0.3f;

    // ----------------------------------------------------------------------------------
    // -------------------------- Default Godot Functions -------------------------------
    // ---------------------------------------------------------------------------------- 

    public override void _EnterTree()
    {
        hurtbox = GetNode<Area3D>("Hurtbox");

        animator = new CharacterAnimator(); 
        animator.Initialize(this);
    }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {

        Move(delta);
        Jump(delta);

        ApplyGravity(delta);
        ApplyFriction(delta);

        MoveAndSlide();

        UpdateMoveState();
        UpdateUpperBodyState();
        UpdateFullBodyState();

        animator.Update(delta);

    }

    // ----------------------------------------------------------------------------------
    // -------------------------- Always Active Functions -------------------------------
    // ---------------------------------------------------------------------------------- 

    private void Move(double delta)
    {
        if (fullBodyState != CharacterFullBodyState.None)
            return;

        if (globalMoveVector == Vector3.Zero)
            return;

        float moveSpeed = 0.0f;
        float moveAcceleration = 0.0f;

        if (!IsOnFloor())
        {
            moveSpeed = airSpeed;
            moveAcceleration = airAcceleration;
        }
        else
        {
            if (sprintEnabled)
            {
                moveSpeed = sprintSpeed;
                moveAcceleration = sprintAcceleration;
            }
            else
            {
                if (walkEnabled)
                {
                    moveSpeed = walkSpeed;
                    moveAcceleration = walkAcceleration;
                }
                else
                {
                    moveSpeed = runSpeed;
                    moveAcceleration = runAcceleration;
                }
            }
        }

        Vector3 targetVelocity = (globalMoveVector * moveSpeed) - new Vector3(Velocity.X, 0, Velocity.Z);
        Velocity += targetVelocity * moveAcceleration * (float)delta;
    }

    private void Jump(double delta)
    {
        if (queuedJump && (IsOnFloor() || coyoteTimer < coyoteTimeLimit) && Velocity.Y <= 0.0f)
        {
            Velocity -= GetGravity().Normalized() * jumpForce;

            queuedJump = false;
            coyoteTimer = coyoteTimeLimit;
        }

        timeSinceQueuedJump += (float)delta;

        if (timeSinceQueuedJump >= jumpBuffer)
            queuedJump = false;
    }

    private void ApplyGravity(double delta)
    {
        Velocity += GetGravity() * (float)delta;

        if (IsOnFloor() && Velocity.Y <= 0.0f)
            Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
        else if (Velocity.Y < maxFallSpeed)
            Velocity = new Vector3(Velocity.X, maxFallSpeed, Velocity.Z);
    }

    private void ApplyFriction(double delta)
    {
        if (globalMoveVector != Vector3.Zero || (Velocity.X == 0 && Velocity.Z == 0))
            return;

        Vector3 normalFriction = new Vector3(Velocity.X, 0, Velocity.Z).Normalized();
        Vector3 totalFriction = normalFriction * frictionStrength;
        Velocity -= totalFriction;

        if (Velocity.LengthSquared() < 0.5f)
            Velocity = Vector3.Zero;
    }

    // ----------------------------------------------------------------------------------
    // ------------------------- Use in Inherited Functions -----------------------------
    // ---------------------------------------------------------------------------------- 

    protected void SetMoveVector(Vector3 moveVector)
    {
        globalMoveVector = moveVector;
    }

    protected void QueueJump()
    {
        timeSinceQueuedJump = 0.0f;
        queuedJump = true;
    }

    protected void EndJumpEarly()
    {
        if (Velocity.Y <= 0.0f)
            return;

        float velocityMod = Velocity.Y * 0.25f;
        Velocity += GetGravity().Normalized() * velocityMod * jumpDecay;
    }

    protected void InvertWeaponReadyState()
    {
        weaponsReady = !weaponsReady;
    }

    protected void SetBlockState(bool state)
    {
        blockEnabled = state;
    }

    protected void SetAttackState(bool state)
    {
        weaponsReady = true;
        queueAttack = state;
    }

    // ----------------------------------------------------------------------------------
    // -------------------------- Move State and Conditions -----------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateMoveState()
    {
        CharacterLocomotionState newState = CharacterLocomotionState.None;

        newState = AirStateUpdate();

        if (IsOnFloor())
            newState = GroundStateUpdate();

        if (newState != moveState)
        {
            OnUpdateMoveState?.Invoke(newState);
            moveState = newState;
        }
    }

    private CharacterLocomotionState AirStateUpdate()
    {
        bool moving = Velocity.X != 0 || Velocity.Z != 0;

        if (!IsOnFloor() && onGround)
        {
            onGround = false;

            if (moving && Velocity.Y > 0.0f)
                return CharacterLocomotionState.Run_Jump_Start;
            else if (moving)
                return CharacterLocomotionState.Run_Air;
            else if (Velocity.Y > 0.0f)
                return CharacterLocomotionState.Idle_Jump_Start;
            else
                return CharacterLocomotionState.Idle_Air;
        }
        else if (IsOnFloor() && !onGround)
        {
            // Landed
            onGround = true;
        }

        return CharacterLocomotionState.None;
    }

    private CharacterLocomotionState GroundStateUpdate()
    {
        bool moving = Velocity.X != 0 || Velocity.Z != 0;

        if (IsOnFloor())
        {
            if (!moving)
                return GroundIdleStateUpdate();
            else
                return GroundMovingStateUpdate();
        }

        return CharacterLocomotionState.None;
    }

    private CharacterLocomotionState GroundIdleStateUpdate()
    {
        if (crouchEnabled)
            return CharacterLocomotionState.Idle_Crouch;
        else
            return CharacterLocomotionState.Idle;
    }

    private CharacterLocomotionState GroundMovingStateUpdate()
    {
        if (!sprintEnabled && !crouchEnabled)
            return CharacterLocomotionState.WalkRun;
        else if (sprintEnabled)
            return CharacterLocomotionState.Sprint;
        else if (crouchEnabled)
            return CharacterLocomotionState.Walk_Crouch;

        return CharacterLocomotionState.None;
    }

    // ----------------------------------------------------------------------------------
    // ----------------------- Upper Body State and Conditions --------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateUpperBodyState()
    {
        CharacterUpperBodyState newState = CharacterUpperBodyState.None;

        if (!weaponsReady)
            newState = CharacterUpperBodyState.None;
        else if (!IsOnFloor() || sprintEnabled)
            newState = CharacterUpperBodyState.WeaponsReady;
        else
            newState = UpperBodyStateUpdate();

        if (newState != upperBodyState)
        {
            OnUpdateUpperBodyState?.Invoke(newState);
            upperBodyState = newState;
        }
    }

    private CharacterUpperBodyState UpperBodyStateUpdate()
    {
        if (blockEnabled)
            return CharacterUpperBodyState.Blocking;

        return CharacterUpperBodyState.WeaponsReady;
    }

    // ----------------------------------------------------------------------------------
    // ------------------------ Full Body State and Conditions --------------------------
    // ---------------------------------------------------------------------------------- 

    private void UpdateFullBodyState()
    {
        if (fullBodyState != CharacterFullBodyState.None)
            return;

        CharacterFullBodyState newState = CharacterFullBodyState.None;

        if (!IsOnFloor() || blockEnabled || sprintEnabled)
            newState = CharacterFullBodyState.None;
        else if (queueAttack)
        {
            queueAttack = false;
            newState = CharacterFullBodyState.Attack;
        }

        if (newState != fullBodyState)
        {
            OnUpdateFullBodyState?.Invoke(newState);
            fullBodyState = newState;
        }
    }

    // ----------------------------------------------------------------------------------
    // ------------------------ Full Body State and Conditions --------------------------
    // ---------------------------------------------------------------------------------- 



}

public enum CharacterLocomotionState
{
    None,

    Idle,
    WalkRun,
    Sprint,

    Idle_Jump_Start,
    Run_Jump_Start,

    Idle_Air,
    Run_Air,

    Idle_Crouch,
    Walk_Crouch
}

public enum CharacterUpperBodyState
{
    None,

    WeaponsReady,

    Blocking,
    Casting
}

public enum CharacterFullBodyState
{
    None,

    Attack,
    Casting,
    Action
}

public enum WeaponType
{
    None,
    Fists
}

public enum ActionType
{
    None,

    Fists_Attack_1,
    Fists_Attack_2
}