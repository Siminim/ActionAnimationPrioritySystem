using Godot;
using System;

public class WeaponsUnReadyRequest : ActionRequest
{
    public WeaponsUnReadyRequest()
    {
        actionName = CharacterAction.WeaponsReady;
        actionLayer = CharacterActionLayer.UpperBody;
        priority = 0;
    }

    public override void EnterState(Character character)
    {
        character.animator.SetUpperbodyAnimation("Arms_Up");
        character.animator.SetUpperbodyBlendTarget(0.0f);
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
        if (character.weaponsReady)
        {
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.WeaponsReady]);
            EndAction(character);
        }
        else if (character.animator.GetUpperbodyBlend() <= 0.01f)
        {
            EndAction(character);
        }
    }
}