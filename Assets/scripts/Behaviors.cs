using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors {

    IDictionary<int, Action> ActionIds;

    // Returns Action object from given id
    public Action getAction(int id)
    {
        return ActionIds[id];
    }

    protected void setIds(IDictionary<int, Action> ActionIds)
    {
        this.ActionIds = ActionIds;
    }

}

