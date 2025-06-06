using Godot;
using System.Collections.Generic;

public class ActionManager
{
    //private CharacterAnimator animator;
    private Character character;
    private List<ActionRequest> activeActions = new List<ActionRequest>();
    private List<ActionRequest> actionsToRemove = new List<ActionRequest>();
    private List<ActionRequest> cancelledActions = new List<ActionRequest>();

    public ActionManager(Character character)
    {
        this.character = character;
    }

    public void RequestAction(ActionRequest request)
    {
        List<ActionRequest> actionsToRemove = new List<ActionRequest>();

        for (int i = 0; i < activeActions.Count; i++)
        {
            if ((activeActions[i].actionLayer & request.actionLayer) != 0)
            {
                if (activeActions[i].priority > request.priority)
                    return;

                actionsToRemove.Add(activeActions[i]);
            }
        }

        //request.timeRemaining = character.animator.GetAnimationDuration(request.animName);

        foreach (ActionRequest action in actionsToRemove)
        {
            EndAction(action);
        }

        activeActions.Add(request);
        request.EnterState(character);
    }

    public void Process(double delta)
    {
        CancelActions();
        RemoveActions();
        UpdateActions(delta);
        CheckRelevance();
    }

    private void CancelActions()
    {
        foreach (ActionRequest action in cancelledActions)
        {
            action.CancelState(character);
            EndAction(action);
        }

        cancelledActions.Clear();
    }

    private void RemoveActions()
    {
        foreach (ActionRequest action in actionsToRemove)
        {
            action.ExitState(character);
            activeActions.Remove(action);
        }

        actionsToRemove.Clear();
    }

    public void UpdateActions(double delta)
    {
        for (int i = 0; i < activeActions.Count; i++)
        {
            activeActions[i].UpdateState(delta, character);

            if (activeActions[i].isOnTimer)
            {
                activeActions[i].timeRemaining -= (float)delta;

                if (activeActions[i].timeRemaining <= 0.0f)
                    EndAction(activeActions[i]);
            }
        }
    }

    public void CheckRelevance()
    {
        for (int i = 0; i < activeActions.Count; i++)
        {
            activeActions[i].CheckRelevance(character);
        }
    }

    public void Animate(double delta)
    {
        for (int i = 0; i < activeActions.Count; i++)
        {
            activeActions[i].Animate(delta, character.animator);
        }
    }

    public void CancelAction(ActionRequest action)
    {
        if (!activeActions.Contains(action) || cancelledActions.Contains(action)) return;
            cancelledActions.Add(action);
    }

    public void EndAction(ActionRequest action)
    {
        if (!activeActions.Contains(action) || actionsToRemove.Contains(action)) return;
            actionsToRemove.Add(action);
    }

    public void CancelActionByType<T>() where T : ActionRequest
    {
        for (int i = 0; i < activeActions.Count; i++)
        {
            if (activeActions[i] is T)
            {
                CancelAction(activeActions[i]);
                return;
            }
        }
    }

    public void EndActionByType<T>() where T : ActionRequest
    {
        for (int i = 0; i < activeActions.Count; i++)
        {
            if (activeActions[i] is T)
            {
                EndAction(activeActions[i]);
                return;
            }
        }
    }
}