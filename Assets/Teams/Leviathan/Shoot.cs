using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Leviathan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Action
{
    private LeviathanController leviathan;
    private BehaviorTree tree;
    public SharedBool canShoot;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        setValue(canShoot.Value);
    }

    public void setValue(bool mineValue)
    {
        leviathan.setShootCondition(mineValue);
        tree.SetVariableValue("Shoot", mineValue);
    }

}
