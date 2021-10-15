using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;


namespace Leviathan
{
    public class CheckIfEnemyInFront : Action
{


    private LeviathanController leviathan;
    private BehaviorTree tree;



    public override void OnStart()
        {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        bool detectTarget = AimingHelpers.CanHit(leviathan._spaceship, leviathan._otherSpaceship.Position, leviathan._otherSpaceship.Velocity, 0.15f);
            
                if (detectTarget)
                {
                    LeviathanController.instance.tree.SetVariableValue("enemyIsInFront", true);
                    Debug.Log("ENEMY");
                }

                else
                    LeviathanController.instance.tree.SetVariableValue("enemyIsInFront", false);
        
        }
    }
}
