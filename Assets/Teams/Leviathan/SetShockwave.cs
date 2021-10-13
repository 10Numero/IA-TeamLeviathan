using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class SetShockwave : Action
{
    public bool setShockwaveValue;
    public LeviathanController leviathanController;

    public override void OnStart()
    {
        setValue(setShockwaveValue);
    }

    public void setValue(bool shockwaveValue)
    {
        leviathanController.setShockwaveCondition(shockwaveValue);
    }
}
