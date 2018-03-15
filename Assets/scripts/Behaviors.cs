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
       
    }

    public void populateHurtboxes()
    {

    }

}

