using Godot;
using System;

public partial class Character : CharacterBody3D
{
    public Player player; 
    public CharacterAnimator animator { get; private set; }
    public ActionManager actionManager { get; private set; }

    // ----------------------------------------------------------------------------------
    // ------------------------------ Combat Variables ----------------------------------
    // ---------------------------------------------------------------------------------- 

    #region Combat variables

    public bool weaponsReady = false;
    public bool blockEnabled = false;

    #endregion

    #region Locomotion and Speed variables

    public Vector3 globalMoveVector { get; private set; } = Vector3.Zero;
    public bool moving = false;

    public bool crouchEnabled = false;
    public bool walkEnabled = false;
    public bool sprintEnabled = false;

    #endregion

    #region Jumping and Gravity variables

    public bool queuedJump = false;
    private bool onGround = false;

    public float timeSinceQueuedJump = 0.0f;
    public float coyoteTimer = 0.0f;

    #endregion

    #region Combat variables

    public ItemType heldItem = ItemType.None;
    public bool queuedItemUse = false;
    public short attackChain = 0;

    #endregion

    #region Locomotion and Speed properties

    public float airSpeed { get; private set; } = 5.0f;
    public float airAcceleration { get; private set; } = 0.5f;

    public float walkSpeed { get; private set; } = 2.0f;
    public float walkAcceleration { get; private set; } = 3.0f;

    public float runSpeed { get; private set; } = 6.0f;
    public float runAcceleration { get; private set; } = 3.0f;

    public float sprintSpeed { get; private set; } = 10.0f;
    public float sprintAcceleration { get; private set; } = 5.0f;

    public float maxFallSpeed { get; private set; } = -50.0f;

    public float groundFrictionStrength { get; private set; } = 0.85f;
    public float airFrictionStrength { get; private set; } = 0.2f;

    #endregion

    #region Jumping and Gravity properties

    public float jumpForce = 8.5f;
    public float jumpBuffer = 0.125f;
    public float jumpDecay = 2.0f;

    public float coyoteTimeLimit = 0.3f;

    #endregion

    #region Combat properties

    public float itemUseQueueTimeCounter = 0.0f;
    public float itemUseQueueTime = 0.5f;

    #endregion

    public override void _EnterTree()
    {
        animator = new CharacterAnimator(this);
        actionManager = new ActionManager(this);

        player = GetNode<Player>("PlayerController");
    }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        player.PhysicsUpdate(delta);

        Jump(delta);

        if (weaponsReady)
            actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.WeaponsReady]);

        //GD.Print("Use: " + queuedItemUse);
        UpdateItemUseTimer(delta);

        actionManager.Update(delta);
        animator.Update(delta);

        ApplyGravity(delta);
        ApplyFriction(delta);

        MoveAndSlide();
    }

    // ----------------------------------------------------------------------------------
    // -------------------------- Always Active Functions -------------------------------
    // ---------------------------------------------------------------------------------- 

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
        if (moving || !IsOnFloor())
            return;
        
        Vector3 normalFriction = new Vector3(Velocity.X, 0, Velocity.Z).Normalized();

        float frictionStrength = IsOnFloor() ? groundFrictionStrength : airFrictionStrength;
        Vector3 totalFriction = normalFriction * frictionStrength;

        Velocity -= totalFriction;

        if (Velocity.LengthSquared() < 0.5f)
            Velocity = Vector3.Zero;
    }

    // ----------------------------------------------------------------------------------
    // ------------------------- Use in Inherited Functions -----------------------------
    // ---------------------------------------------------------------------------------- 

    public void SetMoveVector(Vector3 moveVector)
    {
        globalMoveVector = moveVector;
    }

    public void QueueJump()
    {
        timeSinceQueuedJump = 0.0f;
        queuedJump = true;
    }

    public void Jump(double delta)
    {
        if (queuedJump && (IsOnFloor() || coyoteTimer < coyoteTimeLimit) && Velocity.Y <= 0.0f)
            actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Jump]);

        timeSinceQueuedJump += (float)delta;

        if (timeSinceQueuedJump >= jumpBuffer)
            queuedJump = false;
    }

    public void EndJumpEarly()
    {
        actionManager.CancelAction(CharacterActionLibrary.Actions[CharacterAction.Jump]);
    }


    // ----------------------------------------------------------------------------------
    // ------------------------------- Item Use Timer -----------------------------------
    // ----------------------------------------------------------------------------------

    public void StartItemUseTimer()
    {
        itemUseQueueTimeCounter = itemUseQueueTime;
        queuedItemUse = true;
    }

    private void UpdateItemUseTimer(double delta)
    {
        if (!queuedItemUse)
            return;

        actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Attack]);

        itemUseQueueTimeCounter -= (float)delta;

        if (itemUseQueueTimeCounter <= 0.0f)
            EndItemUseTimer();
    }

    public void EndItemUseTimer()
    {
        queuedItemUse = false;
    }

}