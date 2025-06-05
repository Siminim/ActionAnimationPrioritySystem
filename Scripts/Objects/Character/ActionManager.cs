using Godot;
using System.Collections.Generic;

public class ActionManager
{
    //private CharacterAnimator animator;
    private Character character;
    private List<ActionRequest> activeActions = new List<ActionRequest>();
    private LinkedList<ActionRequest> actionsToRemove = new LinkedList<ActionRequest>();

    public ActionManager(Character character)
    {
        this.character = character;
    }

    public ActionRequest RequestAction(ActionRequest request)
    {
        List<ActionRequest> actionsToRemove = new List<ActionRequest>();

        for (int i = 0; i < activeActions.Count; i++)
        {
            if ((activeActions[i].actionLayer & request.actionLayer) != 0)
            {
                if (activeActions[i].priority > request.priority)
                    return null;

                actionsToRemove.Add(activeActions[i]);
            }
        }

        //request.timeRemaining = character.animator.GetAnimationDuration(request.animName);

        foreach (ActionRequest action in actionsToRemove)
        {
            EndAction(action);
        }

        activeActions.Add(request);
        request.EnterState();

        return request;
    }

    public void Update(double delta)
    {
        foreach (ActionRequest action in actionsToRemove)
        {
            action.ExitState();
            activeActions.Remove(action);
        }
        
        actionsToRemove.Clear();

        for (int i = activeActions.Count - 1; i >= 0; i--)
        {
            activeActions[i].UpdateState(delta);

            if (activeActions[i].isOnTimer)
            {
                activeActions[i].timeRemaining -= (float)delta;

                if (activeActions[i].timeRemaining <= 0.0f)
                    EndAction(activeActions[i]);
            }
        }
    }

    public void EndAction(ActionRequest action)
    {
        actionsToRemove.AddLast(action);
    }

    public void EndActionByType<T>() where T : ActionRequest
    {
        for (int i = activeActions.Count - 1; i >= 0; i--)
        {
            if (activeActions[i] is T)
            {
                EndAction(activeActions[i]);
                return;
            }
        }
    }
}