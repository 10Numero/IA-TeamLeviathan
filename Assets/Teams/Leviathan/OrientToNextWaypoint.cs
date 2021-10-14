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
		public SharedFloat dot;

		public SharedFloat distConsideringAsteroidIsReach;

		public override void OnStart()
		{
			if(asteroidPos.Value != Vector2.zero)
            {
				if (Vector2.Distance(asteroidPos.Value, LeviathanController.instance._spaceship.Position) < distConsideringAsteroidIsReach.Value)
				{
					asteroidPos.Value = Vector2.zero;
					LeviathanController.instance.tree.SetVariableValue("AsteroidPosition", Vector2.zero);
				}
			}


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
				Vector2 perpendicular = Vector2.zero;

				//Which Perpendicular
				if (dot.Value > 0)
					perpendicular = Vector2.Perpendicular(targetDir.Value);
				else
					perpendicular = -Vector2.Perpendicular(targetDir.Value);

				Vector2 origin = asteroidPos.Value;

				Vector2 asteroidAvoidPos = (origin + perpendicular.normalized) * (asteroidRadius.Value * avoidAsteroidOffset.Value);

				new GameObject("Debug").transform.position = asteroidAvoidPos;

				Vector2 dir = asteroidAvoidPos - LeviathanController.instance._spaceship.Position;
				
				float orient = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

				LeviathanController.instance.SetOrientation(orient);
            }

		}
	}
}

