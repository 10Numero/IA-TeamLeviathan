using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

public class OnNewWaypoint : Action
{
    public SharedBool newWaypoint;

    public override void OnStart()
    {
        LeviathanController.instance.tree.SetVariableValue("newWaypoint", false);
    }
}
