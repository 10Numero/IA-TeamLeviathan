using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Leviathan
{
    public class CheckEnemyInDangerousZone : Action
    {
        public SharedFloat dangerousRange;

        public override void OnStart()
        {
            float enemyDist = Vector2.Distance(LeviathanController.instance._spaceship.Position, LeviathanController.instance._otherSpaceship.Position);

            if (enemyDist <= dangerousRange.Value)
                LeviathanController.instance.tree.SetVariableValue("EnemyIsInDangerousRange", true);
            else
                LeviathanController.instance.tree.SetVariableValue("EnemyIsInDangerousRange", false);
        }
    }
}

