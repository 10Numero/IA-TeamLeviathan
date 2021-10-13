using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMine : Action
{
    public bool setMineValue;
    public BehaviorTreeReference LeviathanBehavior;

    public override void OnStart()
    {
        setValue(setMineValue);
    }

    public void setValue(bool mineValue)
    {
        LeviathanBehavior.variables.SetValue(setMineValue, 2);
    }
}
