using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Action
{
    public bool mustShoot;
    public BehaviorTreeReference LeviathanBehavior;

    public override void OnStart()
    {
        setValue(mustShoot);
    }

    public void setValue(bool mineValue)
    {
        LeviathanBehavior.variables.SetValue(mustShoot, 3);
    }

}
