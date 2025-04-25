using Godot;
using System;

public class ActionStateManager
{
    private ActionState currentState;
    public ActionState CurrentState => currentState;

    public void ChangeState(ActionState newState)
    {
        if (newState == currentState)
            return;

        currentState?.ExitState();

        currentState = newState;
        currentState?.EnterState();
    }

    public void UpdateState(double delta)
    {
        currentState?.UpdateState(delta);
    }
}
