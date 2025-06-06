using Godot;
using System;

public partial class PlayerCamera : Node3D
{
    private Node3D springArmPivot;
    public Basis SpringArmBasis => springArmPivot.Basis;

    private Node3D cameraTarget;
    private Camera3D camera;

    [Export] public float sensitivity = 0.00175f;

    private float lerpSpeed = 15.0f;

    public override void _EnterTree()
    {
        springArmPivot = GetNode<Node3D>("SpringArmPivot");
        cameraTarget = GetNode<Node3D>("SpringArmPivot/SpringArm3D/CameraTarget");
        camera = GetNode<Camera3D>("SpringArmPivot/Camera3D");
    }

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        camera.Position = camera.Position.Lerp(cameraTarget.Position, (float)delta * lerpSpeed);
    }

    public void RotateArm(float x, float y)
    {
        RotateY(x * sensitivity);
        springArmPivot.RotateX(y * sensitivity);

        float clampX = Mathf.Clamp(springArmPivot.Rotation.X, -Mathf.DegToRad(89.5f), Mathf.DegToRad(55.0f));
        springArmPivot.Rotation = new Vector3(clampX, springArmPivot.Rotation.Y, springArmPivot.Rotation.Z);
    }

    public void RotateCamera(float x, float y)
    {
        camera.RotateY(x * sensitivity);
        camera.RotateX(y * sensitivity);

        float clampX = Mathf.Clamp(camera.Rotation.X, -Mathf.DegToRad(89.5f), Mathf.DegToRad(89.5f));
        camera.Rotation = new Vector3(clampX, camera.Rotation.Y, camera.Rotation.Z);
    }

    public Vector2 GetLookDirection()
    {
        return new Vector2(springArmPivot.Rotation.X, Rotation.Y);
    }

}
