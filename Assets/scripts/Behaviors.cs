using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors {

    IDictionary<int, Attack> attackIds;
    IDictionary<int, Advanced> advancedIds;

    // Returns attack object from given id
    public Attack getAttack(int id)
    {
        return attackIds[id];
    }

    public Advanced getAdvanced(int id)
    {
        return advancedIds[id];
    }

    protected void setIds(IDictionary<int, Attack> attackIds, IDictionary<int, Advanced> advancedIds)
    {
        this.attackIds = attackIds;
        this.advancedIds = advancedIds;
    }

}

