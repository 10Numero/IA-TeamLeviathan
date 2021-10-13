using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Action
{
    public bool mustShoot;
    public LeviathanController leviathanController;

    public override void OnStart()
    {
        setValue(mustShoot);
    }

    public void setValue(bool mineValue)
    {
        leviathanController.setShootCondition(mineValue);
    }

}
