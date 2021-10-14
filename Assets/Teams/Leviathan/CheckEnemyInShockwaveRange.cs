using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Leviathan;

public class CheckEnemyInShockwaveRange : Action
{
    public SharedFloat ShockwaveRange;

    public override void OnStart()
    {
        float enemyDist = Vector2.Distance(LeviathanController.instance._spaceship.Position, LeviathanController.instance._otherSpaceship.Position);

        if(enemyDist <= ShockwaveRange.Value)
            LeviathanController.instance.tree.SetVariableValue("EnemyIsInShockwaveRange", true);
    }
}
