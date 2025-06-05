public class AnimationRequest
{
    public CharacterAnimation animName      { get; protected set; }
    public ActionLayer animationLayer       { get; protected set; }
    public int priority                     { get; protected set; }
    public bool isOnTimer                   { get; protected set; }
    public float timeRemaining              { get; set; }

    protected CharacterAnimator animator;


    public virtual void EnterState(CharacterAnimator animator)
    {
        this.animator = animator;
    }

    public virtual void UpdateState(double delta) { }
    public virtual void ExitState() { }
}