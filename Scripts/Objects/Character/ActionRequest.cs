abstract public class ActionRequest
{
    public CharacterAction actionName;
    public CharacterAnimation animName;
    public ActionLayer actionLayer = ActionLayer.None;
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

public enum ActionLayer
{
    None = 0,
    Legs = 1 << 0,
    UpperBody = 1 << 1,
    LeftArm = 1 << 2,
    Head = 1 << 3,
    
    FullbodyOverride = 1 << 4 | Legs | UpperBody | LeftArm | Head,

}