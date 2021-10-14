using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

public class OrientToNextWaypoint : Action
{
	public SharedVector2 nextClosestWaypoint;
	public SharedVector2 nextNextClosestWaypoint;
	public SharedFloat dst;
	public SharedFloat minDistDrift;

    public override void OnStart()
    {
		float _t = 0;

		Vector2 _dirA = (nextClosestWaypoint.Value - LeviathanController.instance._spaceship.Position);
		Vector2 _dirB = nextNextClosestWaypoint.Value - LeviathanController.instance._spaceship.Position;


		if (dst.Value <= minDistDrift.Value)
			_t = 1 - (dst.Value / minDistDrift.Value);
		else
			_t = 0;

		Vector2 _targetDir = Vector2.Lerp(_dirA, _dirB, _t);

		Debug.DrawRay(LeviathanController.instance._spaceship.Position, _targetDir, Color.white);

		float _targetOrientation = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;

		float threshold = Mathf.Atan2(_dirA.y, _dirA.x) * Mathf.Rad2Deg;
		float ratio = 0.1f;


		if (threshold > 0)
		{
			_targetOrientation -= (threshold * ratio);
			//Debug.Log("Positif : " + _targetOrientation);
		}

		else
		{
			_targetOrientation += (-threshold * ratio);
			//Debug.Log("Negatif : " + _targetOrientation);
		}

		LeviathanController.instance.SetOrientation(_targetOrientation);
	}
}
