using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

namespace Leviathan
{
    public class OnNewWaypoint : Action
    {
        public SharedBool newWaypoint;

        public override void OnStart()
        {
            LeviathanController.instance.tree.SetVariableValue("newWaypoint", false);
            //StartCoroutine(ResetAsteroidPos());
            LeviathanController.instance.NewWaypoint();
            Debug.Log("New waypoint");
        }

        //IEnumerator ResetAsteroidPos()
        //{
        //    yield return new WaitForEndOfFrame();
        //    LeviathanController.instance.tree.SetVariableValue("AsteroidPosition", Vector2.zero);
        //    Debug.Log("AAA");
        //}
    }
}
