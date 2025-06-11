using Godot;
using System;

public partial class Player : Node
{
    private Character character;
    private PlayerCamera playerCamera;

    protected Vector3 localMoveVector = Vector3.Zero;

    public override void _EnterTree()
    {
        character = GetParent<Character>();
        playerCamera = character.GetNode<PlayerCamera>("PlayerCamera");
    }

    public void PhysicsUpdate(double delta)
    {
        //BlockInput();

        UseHeldItem();

        ReadyWeaponInput();
        JumpInput();
        SprintInput();
        WalkInput();
        CrouchInput();

        MovementInput();

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
        character.animator.UpdateLookingAngle(playerCamera.GetLookDirection());
    }

    public void MovementInput()
    {
        Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

        localMoveVector = new Vector3(inputDir.X, 0, inputDir.Y);
        character.SetMoveVector((playerCamera.Basis * localMoveVector).Normalized());

        if (!character.IsOnFloor())
        {
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Fall]);
        }
        else if (inputDir != Vector2.Zero)
        {
            if (character.sprintEnabled) // if sprinting
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Sprint]);

            if (character.crouchEnabled)
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Walk_Crouched]);

            if (!character.walkEnabled) // if running
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Run]);
            else
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Walk]);

        }
        else // if not moving
        {
            if (character.crouchEnabled)
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Idle_Crouched]);
            else
                character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.Idle]);
        }
    }

    public void WalkInput()
    {
        if (Input.IsActionJustPressed("SwapWalkMode"))
            character.walkEnabled = !character.walkEnabled;
    }
    public void SprintInput()
    {
        character.sprintEnabled = Input.IsActionPressed("Sprint");
    }

    public void CrouchInput()
    {
        if (Input.IsActionJustPressed("Crouch"))
            character.crouchEnabled = !character.crouchEnabled;
    }

    public void JumpInput()
    {
        if (Input.IsActionJustPressed("Jump"))
            character.QueueJump();

        if (Input.IsActionJustReleased("Jump"))
            character.EndJumpEarly();
    }

    public void ReadyWeaponInput()
    {
        if (Input.IsActionJustPressed("ReadyWeapon"))
            character.weaponsReady = !character.weaponsReady;
    }

    public void UseHeldItem()
    {
        if (Input.IsActionJustPressed("UseHeldItem"))
        {
            if (!character.weaponsReady)
                character.weaponsReady = true;
            else
                character.StartItemUseTimer();
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
