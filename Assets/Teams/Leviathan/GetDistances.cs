using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

namespace Leviathan
{
    public class GetDistances : Action
    {
        private float nextWaypointDist;
        public SharedFloat enemyDist;
        public SharedVector2 nextWaypoint;

        public override void OnStart()
        {
            nextWaypointDist = Vector2.Distance(LeviathanController.instance._spaceship.Position, nextWaypoint.Value);

            LeviathanController.instance.tree.SetVariableValue("distNextWaypoint", nextWaypointDist);
        }
    }

}
