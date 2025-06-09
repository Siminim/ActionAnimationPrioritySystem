abstract public class ActionRequest
{
    public CharacterAction actionName;
    public CharacterAnimStateMachineName animName;
    public CharacterActionLayer actionLayer = CharacterActionLayer.None;
    public int priority = 0;
    public bool isOnTimer = false;
    public float timeRemaining = 0.0f;


    public virtual void EnterState(Character character) { }
    public virtual void UpdateState(double delta, Character character) { }
    public virtual void ExitState(Character character) { }
    public virtual void CancelState(Character character) {}

    public virtual void CheckRelevance(Character character) { }
    public virtual void Animate(double delta, CharacterAnimator animator) { }

    public void EndAction(Character character) => character.actionManager.EndAction(this);
}

