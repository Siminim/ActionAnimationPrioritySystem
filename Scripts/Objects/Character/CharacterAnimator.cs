using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterAnimator : GodotObject
{
    public Character character { get; private set; }

    public Skeleton3D skeleton { get; private set; }
    public AnimationPlayer animationPlayer { get; private set; }
    public AnimationTree animationTree { get; private set; }

    public AnimationNodeStateMachinePlayback animLocomotionStateMachine;

    public Vector2 lookingVector;

    public Vector2 locomotionBlendspace2DVector;
    public float moveAngle = 0.0f;

    public readonly float locomotionBlendspace2DTransitionSpeed = 4.0f;
    public readonly float turnSpeed = 3.0f;

    public float airSpeed = 0.0f;
    public float landingScale = 0.0f;

    private float locoBodyBlend = 0.0f;
    private float locoBodyBlendTarget = 0.0f;
    private readonly float locoBodyBlendTransitionSpeed = 4.0f;

    private float fullBodyBlend = 0.0f;
    private float fullBodyBlendTarget = 0.0f;
    private readonly float fullBodyBlendTransitionSpeed = 4.0f;

    private static readonly Dictionary<AnimationParameterPath, StringName> animationPaths = new Dictionary<AnimationParameterPath, StringName>
    {
        { AnimationParameterPath.Locomotion_Playback,                   "parameters/Locomotion/playback"                                        },
        { AnimationParameterPath.Loco_Standing_BlendPosition,           "parameters/Locomotion/Loco_Standing/blend_position"                    },
        { AnimationParameterPath.Loco_Crouched_BlendPosition,           "parameters/Locomotion/Loco_Crouched/blend_position"                    },
        { AnimationParameterPath.Loco_Air_BlendPosition,                "parameters/Locomotion/Loco_Air/blend_position"                         },
        { AnimationParameterPath.Loco_Body_Blend,                       "parameters/LocomotionBodyBlend/blend_amount"                           },
        { AnimationParameterPath.Upperbody_Transition_Request,          "parameters/UpperbodyAnimations/Transition/transition_request"          },
        { AnimationParameterPath.FullbodyOverride_Transition_Request,   "parameters/FullbodyAnimation/FullbodyTransitions/transition_request"   },
        { AnimationParameterPath.FullbodyOverride_Transition_Current,   "parameters/FullbodyAnimation/FullbodyTransitions/current_state" },
        { AnimationParameterPath.FullbodyOverride_Blend,                "parameters/FullbodyOverrideBlend/blend_amount"         }
    };


    public CharacterAnimator(Character character)
    {
        this.character = character;

        animationPlayer = character.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        skeleton = character.GetNode<Skeleton3D>("GeneralSkeleton");

        animLocomotionStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get(animationPaths[AnimationParameterPath.Locomotion_Playback]);
    }

    public void Update(double delta)
    {
        UpdateFullbodyOverrideBlend(delta);
    }



    public void UpdateLookingAngle(Vector2 direction)
    {
        lookingVector = direction;
    }

    // ----------------------------------------------------------------------------------
    // ---------------------------- Skeleton Manipulation -------------------------------
    // ----------------------------------------------------------------------------------

    public void TurnToLookDirection(double delta)
    {
        float lookAngle = lookingVector.Y - Mathf.Pi * Mathf.Sign(lookingVector.Y);
        skeleton.RotateY(Mathf.AngleDifference(skeleton.Rotation.Y, lookAngle) * turnSpeed * (float)delta);
    }

    public void TurnToMoveDirection(double delta)
    {
        skeleton.RotateY(Mathf.AngleDifference(skeleton.Rotation.Y, moveAngle) * turnSpeed * (float)delta);
    }

    // ----------------------------------------------------------------------------------
    // ---------------------------- Locomotion Manipulation -----------------------------
    // ----------------------------------------------------------------------------------

    public void UpdateVariableMoveDirections(double delta, float scale)
    {
        Vector2 moveVector = new Vector2(character.globalMoveVector.X, character.globalMoveVector.Z);
        moveAngle = Mathf.Atan2(moveVector.X, moveVector.Y);

        float diff = moveAngle - skeleton.Rotation.Y;

        Vector2 targetLocomotionBlendspace2DVector = new Vector2(-Mathf.Sin(diff), Mathf.Cos(diff)) * scale;

        locomotionBlendspace2DVector = locomotionBlendspace2DVector.Lerp(targetLocomotionBlendspace2DVector, (float)delta * locomotionBlendspace2DTransitionSpeed);
    }

    public Vector2 UpdateVariablesAirDirections(double delta)
    {
        return new Vector2(airSpeed, landingScale);
    }

    public void AnimateLocoStanding(double delta, float scale)
    {
        UpdateVariableMoveDirections(delta, scale);
        animationTree.Set(animationPaths[AnimationParameterPath.Loco_Standing_BlendPosition], locomotionBlendspace2DVector);
    }

    public void AnimateLocoCrouched(double delta, float scale)
    {
        UpdateVariableMoveDirections(delta, scale);
        animationTree.Set(animationPaths[AnimationParameterPath.Loco_Crouched_BlendPosition], locomotionBlendspace2DVector);
    }

    public void AnimateLocoAir(double delta)
    {
        Vector2 airMovementBlendspace2DVector = UpdateVariablesAirDirections(delta);
        animationTree.Set(animationPaths[AnimationParameterPath.Loco_Air_BlendPosition], airMovementBlendspace2DVector);
    }

    // ----------------------------------------------------------------------------------
    // ----------------------------- Upperbody Manipulation -----------------------------
    // ----------------------------------------------------------------------------------

    public void SetUpperbodyAnimation(string anim)
    {
        animationTree.Set(animationPaths[AnimationParameterPath.Upperbody_Transition_Request], anim);
    }

    public void SetUpperbodyBlendTarget(float blend)
    {
        locoBodyBlendTarget = blend;
    }

    public void UpdateUpperbodyBlend(double delta)
    {
        locoBodyBlend = Mathf.Lerp(locoBodyBlend, locoBodyBlendTarget, (float)delta * locoBodyBlendTransitionSpeed);
        animationTree.Set(animationPaths[AnimationParameterPath.Loco_Body_Blend], locoBodyBlend);
    }

    public float GetUpperbodyBlend()
    {
        return locoBodyBlend;
    }

    // ----------------------------------------------------------------------------------
    // ----------------------------- Fullbody Manipulation ------------------------------
    // ----------------------------------------------------------------------------------

    public void SetFullbodyOverrideAnimation(string anim)
    {
        animationTree.Set(animationPaths[AnimationParameterPath.FullbodyOverride_Transition_Request], anim);
    }

    public void SetFullbodyOverrideBlendTarget(float blend)
    {
        fullBodyBlendTarget = blend;
    }

    private void UpdateFullbodyOverrideBlend(double delta)
    {
        fullBodyBlend = Mathf.Lerp(fullBodyBlend, fullBodyBlendTarget, (float)delta * fullBodyBlendTransitionSpeed);
        animationTree.Set(animationPaths[AnimationParameterPath.FullbodyOverride_Blend], fullBodyBlend);
    }
}

public enum AnimationParameterPath : byte
{
    Locomotion_Playback,
    Loco_Standing_BlendPosition,
    Loco_Crouched_BlendPosition,
    Loco_Air_BlendPosition,

    Loco_Body_Blend,

    Upperbody_Transition_Request,

    FullbodyOverride_Transition_Request,
    FullbodyOverride_Transition_Current,
    FullbodyOverride_Blend


}