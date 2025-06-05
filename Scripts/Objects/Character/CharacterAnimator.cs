using Godot;
using System;

public partial class CharacterAnimator
{
    public Character character              { get; private set; }

    // ----------------------------------------------------------------------------------
    // ----------------------------------- Nodes ----------------------------------------
    // ----------------------------------------------------------------------------------

    public Skeleton3D skeleton              { get; private set; }
    public AnimationPlayer animationPlayer  { get; private set; }
    public AnimationTree animationTree      { get; private set; }

    private AnimationNodeStateMachinePlayback animLocomotionStateMachine;
    private AnimationNodeStateMachinePlayback animLeftArmStateMachine;
    private AnimationNodeStateMachinePlayback animRightArmStateMachine;
    private AnimationNodeStateMachinePlayback animTorsoStateMachine;
    private AnimationNodeStateMachinePlayback animHeadStateMachine;
    private AnimationNodeStateMachinePlayback animFullBodyStateMachine;

    // ----------------------------------------------------------------------------------
    // ---------------------------- Animation Variables ---------------------------------
    // ---------------------------------------------------------------------------------- 

    public float moveAngle = 0.0f;
    public float moveScale = 1.0f;
    public float moveScaleTransitionSpeed = 5.0f;

    public float faceAngle = 0.0f;
    public float turnSpeed = 5.0f;

    public Vector2 locomotionAngleVector = Vector2.Zero;

    public float upperBodyBlendValue = 0.0f;
    public float upperBodyBlendTransitionSpeed = 5.0f;

    public Vector2 headTurn = Vector2.Zero;
    public float headTurnTransitionSpeed = 2.5f;

    // ----------------------------------------------------------------------------------
    // ------------------------------- Initialization -----------------------------------
    // ---------------------------------------------------------------------------------- 

    public void Initialize(Character character)
    {
        this.character = character;

        animationPlayer = character.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = character.GetNode<AnimationTree>("AnimationTree");
        skeleton = character.GetNode<Skeleton3D>("GeneralSkeleton");

        animLocomotionStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/Locomotion_StateMachine/playback");
        // animUpperBodyStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/UpperBody_StateMachine/playback");
    }

    public float GetAnimationDuration(CharacterAnimation animName)
    {
        string name = CharacterAnimations.animations[animName];
        return animationPlayer.GetAnimation(name)?.Length ?? 0.0f;
    }

    public void Play(CharacterAnimation anim, ActionLayer layer)
    {
        string name = CharacterAnimations.animations[anim];
        
        switch (layer)
        {
            case ActionLayer.Legs:
                animLocomotionStateMachine.Travel(name);
                break;
            
            case ActionLayer.LeftArm:
                animLeftArmStateMachine.Travel(name);
                break;
            
            case ActionLayer.RightArm:
                animRightArmStateMachine.Travel(name);
                break;
            
            case ActionLayer.Torso:
                animTorsoStateMachine.Travel(name);
                break;

            case ActionLayer.Head:
                animHeadStateMachine.Travel(name);
                break;
            
            case ActionLayer.FullBody:
                animFullBodyStateMachine.Travel(name);
                break;

        }
    }
  
}
