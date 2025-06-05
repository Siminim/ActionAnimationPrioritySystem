using Godot;
using System;

public partial class Player : Character
{
    protected Vector3 localMoveVector = Vector3.Zero;
    private PlayerCamera playerCamera;

    private ActionRequest currentMoveAction;

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
        //SprintInput();
        //JumpInput();
        //ReadyWeaponInput();
        //BlockInput();

        WalkInput();
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
    }

    public void MovementInput()
    {
        Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

        localMoveVector = new Vector3(inputDir.X, 0, inputDir.Y);
        SetMoveVector((playerCamera.Basis * localMoveVector).Normalized());

        if (inputDir != Vector2.Zero)
        {
            if (sprintEnabled) // if sprinting
            {
                if (currentMoveAction is SprintRequest)
                    return;

                crouchEnabled = false;
                currentMoveAction = actionManager.RequestAction(new SprintRequest(this)); 
            }
            else if (!walkEnabled) // if running
            {
                if (currentMoveAction is RunRequest)
                    return;

                currentMoveAction = actionManager.RequestAction(new RunRequest(this));
            }
            else if (walkEnabled) // if walking
            {
                if (currentMoveAction is WalkRequest)
                    return;

               currentMoveAction = actionManager.RequestAction(new WalkRequest(this));
            }
        }

        else if (currentMoveAction is not IdleRequest) // if not moving
        {
            currentMoveAction = actionManager.RequestAction(new IdleRequest(this));
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
            InvertWeaponReadyState();
    }

    public void BlockInput()
    {
        if (Input.IsActionJustPressed("Block"))
            SetBlockState(true);

        if (Input.IsActionJustReleased("Block"))
            SetBlockState(false);
    }
}
