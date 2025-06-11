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

        character.attacking = true;
        character.EndItemUseTimer();
        character.attackChain++;
    }

    public override void ExitState(Character character)
    {
        character.animator.SetFullbodyOverrideBlendTarget(0.0f);

        if (!character.queuedItemUse)
            character.attackChain = 0;
    }

    public override void CheckRelevance(Character character)
    {
        if (!character.attacking)
            EndAction(character);
    }
}