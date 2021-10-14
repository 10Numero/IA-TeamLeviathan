using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class SetMine : Action
{
    public override void OnStart() 
    {
        LeviathanController.instance.setMineCondition(true);
        StartCoroutine(LeviathanController.instance.ResetMineCondition());
    } 
}
