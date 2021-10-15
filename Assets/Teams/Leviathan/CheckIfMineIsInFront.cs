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
            RaycastHit2D[] hit = Physics2D.RaycastAll(LeviathanController.instance._spaceship.Position, LeviathanController.instance.forward);

            Debug.Log("OH LE TEST LA : " + hit.Length);
            for (int i = 0; i < hit.Length; i++)
            {
                //Debug.Log("hit : " + hit[i].collider.gameObject.transform.parent.name);
                if (hit[i].distance <= maxDistDestroyMine.Value)
                {
                    Debug.Log("PREMIER IF OUI");
                    if (hit[i].collider.gameObject.transform.parent.CompareTag("Mine"))
                    {
                        Debug.Log("DEUXIEME IF OUI");
                        Debug.Log("TEST : " + hit[i].collider.gameObject.transform.parent.GetComponent<Mine>().IsActive);
                        if (hit[i].collider.gameObject.GetComponentInParent<Mine>().IsActive)
                        {
                            LeviathanController.instance.tree.SetVariableValue("mineIsInFront", true);
                            Debug.Log("MINE");
                        }
                        else
                        {
                            LeviathanController.instance.tree.SetVariableValue("mineIsInFront", false);
                        }
                    }
                }

                else
                    LeviathanController.instance.tree.SetVariableValue("mineIsInFront", false);
            }
        }
    }
}

