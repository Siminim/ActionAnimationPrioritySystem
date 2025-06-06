using Godot;
using System;

public partial class Character : CharacterBody3D
{
    public CharacterAnimator animator { get; private set; }
    public ActionManager actionManager { get; private set; }

    // ----------------------------------------------------------------------------------
    // ------------------------------ Combat Variables ----------------------------------
    // ---------------------------------------------------------------------------------- 

    //protected bool weaponsReady = false;
    //protected bool blockEnabled = false;

    // ----------------------------------------------------------------------------------
    // ----------------------------- Movement Variables ---------------------------------
    // ---------------------------------------------------------------------------------- 

    public bool crouchEnabled { get; private set; } = false;
    public bool walkEnabled { get; protected set; } = false;
    public bool sprintEnabled { get; protected set; } = false;

    public Vector3 globalMoveVector { get; private set; } = Vector3.Zero;

    public float airSpeed { get; private set; } = 5.0f;
    public float airAcceleration { get; private set; } = 0.5f;

    public float walkSpeed { get; private set; } = 2.0f;
    public float walkAcceleration { get; private set; } = 3.0f;

    public float runSpeed { get; private set; } = 6.0f;
    public float runAcceleration { get; private set; } = 3.0f;

    public float sprintSpeed { get; private set; } = 10.0f;
    public float sprintAcceleration { get; private set; } = 5.0f;

    public float maxFallSpeed { get; private set; } = -50.0f;

    public float frictionStrength { get; private set; } = 0.85f;

    // ----------------------------------------------------------------------------------
    // ------------------------------ Jump Variables ------------------------------------
    // ---------------------------------------------------------------------------------- 

    public bool queuedJump = false;
    private bool onGround = false;

    public float jumpForce = 8.5f;

    public float timeSinceQueuedJump = 0.0f;

    public float jumpBuffer = 0.125f;

    public float jumpDecay = 2.0f;

    public float coyoteTimer = 0.0f;
    public float coyoteTimeLimit = 0.3f;

    // ----------------------------------------------------------------------------------
    // -------------------------- Default Godot Functions -------------------------------
    // ---------------------------------------------------------------------------------- 

    public override void _EnterTree()
    {
        animator = new CharacterAnimator(this);
        actionManager = new ActionManager(this);
    }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        actionManager.Process(delta);
        actionManager.Animate(delta);

        ApplyGravity(delta);
        ApplyFriction(delta);


        MoveAndSlide();
    }

    // ----------------------------------------------------------------------------------
    // -------------------------- Always Active Functions -------------------------------
    // ---------------------------------------------------------------------------------- 

    // private void Jump(double delta)
    // {
    //     if (queuedJump && (IsOnFloor() || coyoteTimer < coyoteTimeLimit) && Velocity.Y <= 0.0f)
    //     {
    //         Velocity -= GetGravity().Normalized() * jumpForce;

    //         queuedJump = false;
    //         coyoteTimer = coyoteTimeLimit;
    //     }

    //     timeSinceQueuedJump += (float)delta;

    //     if (timeSinceQueuedJump >= jumpBuffer)
    //         queuedJump = false;
    // }

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

    protected void SetCrouchedState(bool state)
    {
        crouchEnabled = state;

        if (!IsOnFloor())
            animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Air.ToString());
        else if (crouchEnabled)
            animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Crouched.ToString());
        else
            animator.animLocomotionStateMachine.Travel(CharacterAnimation.Loco_Standing.ToString());
    }

    // protected void InvertWeaponReadyState()
    // {
    //     weaponsReady = !weaponsReady;
    // }

    // protected void SetBlockState(bool state)
    // {
    //     blockEnabled = state;
    // }
}
