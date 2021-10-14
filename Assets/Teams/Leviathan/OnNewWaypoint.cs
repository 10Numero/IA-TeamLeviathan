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
            LeviathanController.instance.tree.SetVariableValue("AsteroidPosition", Vector2.zero);
            Debug.Log("New waypoint");
        }
    }
}
