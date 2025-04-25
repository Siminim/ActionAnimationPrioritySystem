using Godot;
using System;

public partial class Player : Character
{
    protected Vector3 localMoveVector = Vector3.Zero;
    private PlayerCamera playerCamera;

    private bool LockRotationToForward = false;

    private bool holdToSprint = true;
    private bool holdToWalk = false;

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
        SprintInput();
        WalkInput();
        MovementInput();
        JumpInput();
        ReadyWeaponInput();
        BlockInput();
        AttackInput();


        //AnimateHead(playerCamera.Basis, playerCamera.SpringArmBasis);

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
    }

    public void MovementInput()
    {
        Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

        localMoveVector = new Vector3(inputDir.X, 0, inputDir.Y);
        SetMoveVector((playerCamera.Basis * localMoveVector).Normalized());

        if (inputDir == Vector2.Zero)
            return;

        // if (LockRotationToForward)
        //     SetTargetFacingAngle(Vector3.Forward.SignedAngleTo(playerCamera.Basis * Vector3.Back, Vector3.Up));
        // else
        //     SetTargetFacingAngle(MoveAngle);
    }

    public void WalkInput()
    {
        if (Input.IsActionJustPressed("SwapWalkMode"))
            walkEnabled = !walkEnabled;
    }
    public void SprintInput()
    {
        if (Input.IsActionPressed("Sprint") && localMoveVector != Vector3.Zero)
            sprintEnabled = true;
        else
            sprintEnabled = false;
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
            InvertWeaponReadyState();
    }

    public void BlockInput()
    {
        if (Input.IsActionJustPressed("Block"))
            SetBlockState(true);

        if (Input.IsActionJustReleased("Block"))
            SetBlockState(false);
    }

    public void AttackInput()
    {
        if (Input.IsActionJustPressed("Attack"))
            SetAttackState(true);
    }
}
