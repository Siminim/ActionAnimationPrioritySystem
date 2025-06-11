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
        if (character.heldItem == ItemType.None)
            character.animator.SetUpperbodyAnimation("Arms_Up");

        character.animator.SetUpperbodyBlendTarget(0.85f);
    }

    public override void CheckRelevance(Character character)
    {
        if (!character.weaponsReady)
        {
            EndAction(character);
            character.actionManager.RequestAction(CharacterActionLibrary.Actions[CharacterAction.WeaponsUnready]);
        }
    }
}