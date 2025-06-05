using Godot;

public class WalkRequest : ActionRequest
{
    public WalkRequest(Character character) : base(character)
    { 
        animName = CharacterAnimation.WalkRun;
        actionLayer = ActionLayer.Legs;
        priority = 1;
    }

    public override void UpdateState(double delta)
    {
        GD.Print("Walking");
        float moveSpeed = character.walkSpeed;
        float moveAcceleration = character.walkAcceleration;

        /*if (!character.IsOnFloor())
        {
            moveSpeed = character.airSpeed;
            moveAcceleration = character.airAcceleration;
        }
        else
        {
            if (character.sprintEnabled)
            {
                moveSpeed = character.sprintSpeed;
                moveAcceleration = character.sprintAcceleration;
            }
            else
            {
                if (character.walkEnabled)
                {
                    moveSpeed = character.walkSpeed;
                    moveAcceleration = character.walkAcceleration;
                }
                else
                {
                    moveSpeed = character.runSpeed;
                    moveAcceleration = character.runAcceleration;
                }
            }
        }*/

        Vector3 targetVelocity = (character.globalMoveVector * moveSpeed) - new Vector3(character.Velocity.X, 0, character.Velocity.Z);
        character.Velocity += targetVelocity * moveAcceleration * (float)delta;

        if (character.globalMoveVector == Vector3.Zero)
            EndAction();
    }

    // private void UpdateAnimation(double delta)
    // {
    //     float diff = animator.moveAngle - animator.faceAngle;
    //     Vector2 locomotionTargetAngleVector = new Vector2(-Mathf.Sin(diff), Mathf.Cos(diff));
    //     animator.locomotionAngleVector = animator.locomotionAngleVector.Lerp(locomotionTargetAngleVector, (float)delta * animator.moveScaleTransitionSpeed);

    //     float scale = 1.0f;

    //     if (animator.character.walkEnabled)
    //         scale = 0.3f;

    //     animator.moveScale = Mathf.Lerp(animator.moveScale, scale, (float)delta * animator.moveScaleTransitionSpeed);

    //     animator.animationTree.Set("parameters/Locomotion_StateMachine/WalkRun/blend_position", animator.locomotionAngleVector * animator.moveScale);
    // }
}