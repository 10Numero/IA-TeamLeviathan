using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class CheckEnemyInShockwaveRange : Action
{
    public SharedFloat ShockwaveRange;
    private LeviathanController leviathan;
    private BehaviorTree tree;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(leviathan.getSpaceship().Position, ShockwaveRange.Value, 10);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetComponent<LeviathanController>()._spaceship.Owner != leviathan._spaceship.Owner)
                {
                    tree.SetVariableValue("EnemyIsInShockwaveRange", true);
                    i = colliders.Length;
                }
            }
        }

    }


}
