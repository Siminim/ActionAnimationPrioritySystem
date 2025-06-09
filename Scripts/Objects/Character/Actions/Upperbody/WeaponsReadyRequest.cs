using Godot;
using System;

public class WeaponsReadyRequest : ActionRequest
{
    public WeaponsReadyRequest()
    {
        actionName = CharacterAction.WeaponsReady;
        actionLayer = CharacterActionLayer.UpperBody;
        priority = 0;
    }

    public override void EnterState(Character character)
    {
        //TODO: Determine which upperbody weapons ready to use based on held weapon

        character.animator.SetUpperbodyAnimation("Arms_Up");
        character.animator.SetUpperbodyBlendTarget(0.85f);
    }

    public override void UpdateState(double delta, Character character)
    {
        
    }

    public override void Animate(double delta, CharacterAnimator animator)
    {
        animator.UpdateUpperbodyBlend(delta);
    }

    public override void ExitState(Character character)
    {

    }

    public override void CheckRelevance(Character character)
    {
        if (!character.weaponsReady)
        {
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.WeaponsUnready]);
            EndAction(character);
        }
    }
}