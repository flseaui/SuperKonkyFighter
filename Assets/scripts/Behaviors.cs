using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors {

    IDictionary<int, Action> ActionIds;
    IDictionary<Action, int> AnimAction;

    // Returns Action object from given id
    public Action getAction(int id)
    {
        return ActionIds[id];
    }

    protected void setIds(IDictionary<int, Action> ActionIds, IDictionary<Action, int> AnimAction)
    {
        this.ActionIds = ActionIds;
        this.AnimAction = AnimAction;
    }

    public int getAnimAction(Action id)
    {
        return AnimAction[id];
    }

    public void populateStunboxes(Action.rect filler)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            for (int j = 0; j < actions[i].hitboxFrames; j++)
            {
                for (int k = 0; k < actions[i].hitboxData.GetLength(1); k++)
                {
                    Action.rect hitboxData = actions[i].hitboxData[j, k];
                    if (hitboxData.Equals(null))
                        hitboxData = filler;
                }
            }
        }
    }

    public void populateHurtboxes()
    {

    }

}

