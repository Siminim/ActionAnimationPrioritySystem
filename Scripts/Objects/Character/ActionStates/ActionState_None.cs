public class ActionState_None : ActionState
{
    // After an attack ends, how long to wait for an input before going back to idle and starting over
    private float time = 1.0f;
    private float attackBuffer = 1.0f;

    public ActionState_None(Character character) : base(character) { }

    
}