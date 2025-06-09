using Godot;
using System;

public partial class Player : Character
{
    private PlayerCamera playerCamera;

    protected Vector3 localMoveVector = Vector3.Zero;

    public override void _EnterTree()
    {
        base._EnterTree();
        playerCamera = GetNode<PlayerCamera>("PlayerCamera");
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        //BlockInput();

        ReadyWeaponInput();
        JumpInput();
        SprintInput();
        WalkInput();
        CrouchInput();

        MovementInput();

        base._PhysicsProcess(delta);
    }

    public override void _Input(InputEvent @event)
    {
        LookMouse(@event);
    }

    public void LookMouse(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion mouseMotion || Input.MouseMode != Input.MouseModeEnum.Captured)
            return;

        playerCamera.RotateArm(-mouseMotion.Relative.X, -mouseMotion.Relative.Y);
        animator.UpdateLookingAngle(playerCamera.GetLookDirection());
    }

    public void MovementInput()
    {
        Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

        localMoveVector = new Vector3(inputDir.X, 0, inputDir.Y);
        SetMoveVector((playerCamera.Basis * localMoveVector).Normalized());

        if (!IsOnFloor())
        {
            actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Fall]);
        }
        else if (inputDir != Vector2.Zero)
        {
            if (sprintEnabled) // if sprinting
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Sprint]);

            if (crouchEnabled)
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Walk_Crouched]);

            if (!walkEnabled) // if running
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Run]);
            else
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Walk]);

        }
        else // if not moving
        {
            if (crouchEnabled)
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Idle_Crouched]);
            else
                actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Idle]);
        }
    }

    public void WalkInput()
    {
        if (Input.IsActionJustPressed("SwapWalkMode"))
            walkEnabled = !walkEnabled;
    }
    public void SprintInput()
    {
        sprintEnabled = Input.IsActionPressed("Sprint");
    }

    public void CrouchInput()
    {
        if (Input.IsActionJustPressed("Crouch"))
            crouchEnabled = !crouchEnabled;
    }

    public void JumpInput()
    {
        if (Input.IsActionJustPressed("Jump"))
            QueueJump();

        if (Input.IsActionJustReleased("Jump"))
            EndJumpEarly();
    }

    public void ReadyWeaponInput()
    {
        if (Input.IsActionJustPressed("ReadyWeapon"))
            weaponsReady = !weaponsReady;
    }

    public void UseHeldItem()
    {
        if (Input.IsActionJustPressed("UseHeldItem"))
        {
            
        }
    }

    // public void BlockInput()
    // {
    //     if (Input.IsActionJustPressed("Block"))
    //         SetBlockState(true);

    //     if (Input.IsActionJustReleased("Block"))
    //         SetBlockState(false);
    // }
}
