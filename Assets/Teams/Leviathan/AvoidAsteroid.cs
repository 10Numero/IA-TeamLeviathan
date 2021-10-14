using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leviathan
{
    public class AvoidAsteroid : Action
    {
		public SharedFloat avoidAsteroidOffset;
		public SharedVector2 targetDir;
        public SharedVector2 asteroidPosition;
        public SharedFloat asteroidRadius;

		public override void OnStart()
        {
			Vector2 perpendicular = Vector2.zero;

            //Which Perpendicular
            if (LeviathanController.instance.dot > 0)
                perpendicular = Vector2.Perpendicular(targetDir.Value);
            else
                perpendicular = -Vector2.Perpendicular(targetDir.Value);

            Vector2 origin = asteroidPosition.Value;

            Vector2 asteroidDir = (origin + perpendicular.normalized) * (asteroidRadius.Value * avoidAsteroidOffset.Value);
        }
    }
}


