using Godot;
using System;

public class WeaponsUnReadyRequest : ActionRequest
{
    public WeaponsUnReadyRequest()
    {
        actionName = CharacterAction.WeaponsUnready;
        actionLayer = CharacterActionLayer.UpperBody;
        priority = 1;
    }

    public override void EnterState(Character character)
    {
        character.animator.SetUpperbodyBlendTarget(0.0f);
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