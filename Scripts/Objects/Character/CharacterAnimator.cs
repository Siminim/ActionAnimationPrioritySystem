using Godot;
using System;

public partial class CharacterAnimator
{
    public Character character { get; private set; }

    // ----------------------------------------------------------------------------------
    // ----------------------------------- Nodes ----------------------------------------
    // ----------------------------------------------------------------------------------

    public Skeleton3D skeleton { get; private set; }
    public AnimationPlayer animationPlayer { get; private set; }
    public AnimationTree animationTree { get; private set; }

    public AnimationNodeStateMachinePlayback animLocomotionStateMachine;

    public Vector2 lookingVector = Vector2.Zero;

    #region locomotion variables

    public Vector2 locomotionBlendspace2DVector = Vector2.Zero;
    public float moveAngle = 0.0f;

    #endregion

    #region air variables

    public float airSpeed = 0.0f;
    public float landingScale = 0.0f;

    #endregion

    #region Properties

    public readonly float locomotionBlendspace2DTransitionSpeed = 4.0f;
    public readonly float turnSpeed = 3.0f;

    #endregion

    //public float upperBodyBlendValue = 0.0f;
    //public float upperBodyBlendTransitionSpeed = 5.0f;

    //public Vector2 headTurn = Vector2.Zero;
    //public float headTurnTransitionSpeed = 2.5f;


    public CharacterAnimator(Character character)
    {
        this.character = character;

        animationPlayer = character.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        skeleton = character.GetNode<Skeleton3D>("GeneralSkeleton");

        animLocomotionStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/Locomotion/playback");
    }

    public void UpdateLookingAngle(Vector2 direction)
    {
        lookingVector = direction;
    }

    public void TurnToLookDirection(double delta)
    {
        float lookAngle = lookingVector.Y - Mathf.Pi * Mathf.Sign(lookingVector.Y);
        skeleton.RotateY(Mathf.AngleDifference(skeleton.Rotation.Y, lookAngle) * turnSpeed * (float)delta);
    }

    public void TurnToMoveDirection(double delta)
    {
        skeleton.RotateY(Mathf.AngleDifference(skeleton.Rotation.Y, moveAngle) * turnSpeed * (float)delta);
    }

    public void UpdateVariableMoveDirections(double delta, float scale)
    {
        Vector2 moveVector = new Vector2(character.globalMoveVector.X, character.globalMoveVector.Z);
        moveAngle = Mathf.Atan2(moveVector.X, moveVector.Y);

        float diff = moveAngle - skeleton.Rotation.Y;

        Vector2 targetLocomotionBlendspace2DVector = new Vector2(-Mathf.Sin(diff), Mathf.Cos(diff)) * scale;

        locomotionBlendspace2DVector = locomotionBlendspace2DVector.Lerp(targetLocomotionBlendspace2DVector, (float)delta * locomotionBlendspace2DTransitionSpeed);
    }

    public void AnimateLocoStanding(double delta, float scale)
    {
        UpdateVariableMoveDirections(delta, scale);
        animationTree.Set("parameters/Locomotion/Loco_Standing/blend_position", locomotionBlendspace2DVector);
    }

    public void AnimateLocoCrouched(double delta, float scale)
    {
        UpdateVariableMoveDirections(delta, scale);
        animationTree.Set("parameters/Locomotion/Loco_Crouched/blend_position", locomotionBlendspace2DVector);
    }

    public Vector2 UpdateVariablesAirDirections(double delta)
    {
        return new Vector2(airSpeed, landingScale);
    }

    public void AnimateLocoAir(double delta)
    {
        Vector2 vec = UpdateVariablesAirDirections(delta);
        animationTree.Set("parameters/Locomotion/Loco_Air/blend_position", vec);
    }

    // public float GetAnimationDuration(CharacterAnimation animName)
    // {
    //     string name = CharacterActionAnimations.AnimName[animName];
    //     return animationPlayer.GetAnimation(name)?.Length ?? 0.0f;
    // }

    // public void Play(CharacterAnimation anim)
    // {
    //     string name = CharacterActionAnimations.AnimName[anim];
    //     animationPlayer.Play(name);
    // }

    // public void Play(CharacterAnimation anim)
    // {
    //     string name = CharacterActionAnimations.AnimName[anim];

    //     // switch (layer)
    //     // {
    //     //     case ActionLayer.Legs:
    //     //         animLocomotionStateMachine.Travel(name);
    //     //         break;

    //     //     case ActionLayer.LeftArm:
    //     //         animLeftArmStateMachine.Travel(name);
    //     //         break;

    //     //     case ActionLayer.RightArm:
    //     //         animRightArmStateMachine.Travel(name);
    //     //         break;

    //     //     case ActionLayer.Torso:
    //     //         animTorsoStateMachine.Travel(name);
    //     //         break;

    //     //     case ActionLayer.Head:
    //     //         animHeadStateMachine.Travel(name);
    //     //         break;

    //     //     case ActionLayer.FullBody:
    //     //         animFullBodyStateMachine.Travel(name);
    //     //         break;

    //     // }
    // }

}
