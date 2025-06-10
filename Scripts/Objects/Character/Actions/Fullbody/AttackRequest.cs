using Godot;

public class AttackRequest : ActionRequest
{
    public AttackRequest()
    {
        actionName = CharacterAction.Attack;
        actionLayer = CharacterActionLayer.FullbodyOverride;
        priority = 10;
    }

    public override void EnterState(Character character)
    {
        if (character.heldItem == ItemType.None)
        {
            if (character.attackChain >= ItemLibraries.heldItemLibrary[character.heldItem].useAnimations.Length)
            {
                character.queuedItemUse = false;
                EndAction(character);
            }
            else
            {
                string anim = ItemLibraries.heldItemLibrary[character.heldItem].useAnimations[character.attackChain];
                character.animator.SetFullbodyOverrideAnimation("Attack_" + anim);
                character.animator.SetFullbodyOverrideBlendTarget(1.0f);
            }
        }

        character.EndItemUseTimer();
        character.attackChain++;
    }

    public override void ExitState(Character character)
    {
        if (!character.queuedItemUse)
            character.attackChain = 0;
    }

    public override void CheckRelevance(Character character)
    {
        
    }
}