using Godot;
using System;

public class ActionState
{
    protected Character character;

    public ActionState(Character character)
    {
        this.character = character;
    }

    public virtual void EnterState()
    {

    }

    public virtual void UpdateState(double delta)
    {

    }

    public virtual void ExitState()
    {
        
    }
}
