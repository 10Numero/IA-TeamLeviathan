using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShockwave : Action
{
    public bool setShockwaveValue;
    public BehaviorTreeReference LeviathanBehavior;

    public override void OnStart()
    {
        setValue(setShockwaveValue);
    }

    public void setValue(bool shockwaveValue)
    {
        LeviathanBehavior.variables.SetValue(setShockwaveValue, 4);
    }
}
