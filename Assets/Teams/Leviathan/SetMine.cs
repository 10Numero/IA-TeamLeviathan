using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class SetMine : Action
{
    public SharedBool placeMine;
    public LeviathanController leviathan;

    public override void OnStart()
    {
        useMine(placeMine.Value);
    }

    public void useMine(bool placeMine)
    {
        leviathan.setMineCondition(placeMine);
    }
}
