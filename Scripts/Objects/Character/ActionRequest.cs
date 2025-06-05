public class ActionRequest
{
    public CharacterAnimation animName { get; protected set; }
    public ActionLayer actionLayer { get; protected set; } = ActionLayer.None;
    public int priority { get; protected set; } = 0;
    public bool isOnTimer { get; protected set; } = false;
    public float timeRemaining { get; set; } = 0.0f;

    protected Character character;

    protected ActionRequest(Character character)
    {
        this.character = character;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState(double delta) { }
    public virtual void ExitState() { }

    public void EndAction() => character.actionManager.EndAction(this);
}

public enum ActionLayer
{
    None = 0,
    Legs = 1 << 0,
    LeftArm = 1 << 1,
    RightArm = 1 << 2,
    Torso = 1 << 3,
    Head = 1 << 4,
    UpperBody = LeftArm | RightArm | Torso,
    FullBody = Legs | LeftArm | RightArm | Torso | Head,

    Additive = 1 << 8
}