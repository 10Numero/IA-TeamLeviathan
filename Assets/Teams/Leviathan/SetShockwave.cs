using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class SetShockwave : Action
{
    public bool setShockwaveValue;
    private LeviathanController leviathan;
    private BehaviorTree tree;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        setValue(setShockwaveValue);
    }

    public void setValue(bool shockwaveValue)
    {
        leviathan.setShockwaveCondition(shockwaveValue);
        tree.SetVariableValue("Shockwave", shockwaveValue);
    }
}
