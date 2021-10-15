using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Leviathan
{
    public class CheckIfMineIsInFront : Action
    {
        public SharedFloat maxDistDestroyMine;

        public override void OnStart()
        {
            RaycastHit2D hit = Physics2D.Raycast(LeviathanController.instance._spaceship.Position, LeviathanController.instance.forward);

            if(hit.distance <= maxDistDestroyMine.Value && hit.collider.gameObject.transform.parent.CompareTag("Mine"))
            {
                if (hit.collider.GetComponentInParent<Mine>().IsActive)
                {
                    LeviathanController.instance.tree.SetVariableValue("mineIsInFront", true);
                    Debug.Log("MINE");
                }
                else
                {
                    LeviathanController.instance.tree.SetVariableValue("mineIsInFront", false);
                }
            }

            else
                LeviathanController.instance.tree.SetVariableValue("mineIsInFront", false);
        }
    }
}

