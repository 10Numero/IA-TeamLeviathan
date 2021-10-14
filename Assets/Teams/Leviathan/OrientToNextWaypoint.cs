using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

namespace Leviathan
{
	public class OrientToNextWaypoint : Action
	{
		public SharedVector2 nextClosestWaypoint;
		public SharedVector2 nextNextClosestWaypoint;
		public SharedFloat dst;
		public SharedFloat minDistDrift;
		public SharedVector2 asteroidPos;
		public SharedFloat asteroidRadius;

		public SharedVector2 targetDir;
		public SharedVector2 dirA;

		public SharedFloat avoidAsteroidOffset;

		public SharedFloat distConsideringAsteroidIsReach;

		public SharedFloat orientToAvoid;

		public override void OnStart()
		{
			if(asteroidPos.Value == Vector2.zero)
            {
				float _t = 0;

				if (dst.Value <= minDistDrift.Value)
					_t = 1 - (dst.Value / minDistDrift.Value);
				else
					_t = 0;

				LeviathanController.instance.tree.SetVariableValue("T", _t);

				Debug.DrawRay(LeviathanController.instance._spaceship.Position, targetDir.Value, Color.white);

				float _targetOrientation = Mathf.Atan2(targetDir.Value.y, targetDir.Value.x) * Mathf.Rad2Deg;

				float threshold = Mathf.Atan2(dirA.Value.y, dirA.Value.x) * Mathf.Rad2Deg;
				float ratio = 0.1f;


				if (threshold > 0)
					_targetOrientation -= (threshold * ratio);
				else
					_targetOrientation += (-threshold * ratio);

				LeviathanController.instance.SetOrientation(_targetOrientation);
            }
            else
            {
				Debug.Log("Avoiding ..");

				LeviathanController.instance.SetOrientation(orientToAvoid.Value);

				float dist = Vector2.Distance(asteroidPos.Value, LeviathanController.instance._spaceship.Position);

				Debug.Log("dist : " + dist);

				if (dist < distConsideringAsteroidIsReach.Value)
				{
					Debug.Log("Reset dist");
					asteroidPos.Value = Vector2.zero;
					LeviathanController.instance.tree.SetVariableValue("AsteroidPosition", Vector2.zero);
				}
			}

		}
	}
}

