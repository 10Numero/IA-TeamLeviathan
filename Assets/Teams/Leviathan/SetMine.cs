using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class SetMine : Action
{
    public SharedBool mine;
    private LeviathanController leviathan;
    private BehaviorTree tree;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        useMine(mine.Value);
        
    }

    public void useMine(bool placeMine)
    {
        
        leviathan.setMineCondition(placeMine);
        tree.SetVariableValue("dropMine", placeMine);
    }
}
