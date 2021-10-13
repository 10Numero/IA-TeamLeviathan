using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetThreshold : Action
{

    public float thresholdValue;
    public BehaviorTreeReference LeviathanBehavior;
    public override void OnStart()
    {
        setTreshold(thresholdValue);
    }

    public void setTreshold (float value)
    {
        LeviathanBehavior.variables.SetValue(value, 1);
    }
}
