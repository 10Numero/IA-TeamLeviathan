using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Leviathan;
using UnityEngine;

public class GetClosestNextWaypoint : Action
{
	public SharedVector2 nextWaypoint;
	private BehaviorTree _tree;
	public SharedFloat distNextWaypoint;

    public override void OnStart()
    {

		_tree = LeviathanController.instance.tree;

		Vector2 closestWaypointPosition = Vector2.zero;

		closestWaypointPosition = nextWaypoint.Value;


		float shortestDist = Mathf.Infinity;

		for (int i = 0; i < LeviathanController.instance._data.WayPoints.Count; i++)
		{
			//No owner = -1
			if (LeviathanController.instance._data.WayPoints[i].Owner == LeviathanController.instance._otherSpaceship.Owner || LeviathanController.instance._data.WayPoints[i].Owner == -1)
			{
				float dst = Vector2.Distance(LeviathanController.instance._spaceship.Position, LeviathanController.instance._data.WayPoints[i].Position);

				if (dst < shortestDist)
				{
					shortestDist = dst;
					nextWaypoint.Value = LeviathanController.instance._data.WayPoints[i].Position;
				}
			}
		}

		//_dst = Vector2.Distance(_spaceship.Position, _nextWaypoint.Position);

		if (nextWaypoint.Value != closestWaypointPosition)
			_tree.SetVariableValue("newWaypoint", true);

		Debug.Log("Closertwp: " + nextWaypoint.Value);

		_tree.SetVariableValue("closestWaypoint", nextWaypoint.Value);
		//closestWaypointPosition = nextWaypoint.Value;





		Vector2 nextClosestWaypoint = nextWaypoint.Value;
		float shortestDistB = Mathf.Infinity;

		for (int i = 0; i < LeviathanController.instance._data.WayPoints.Count; i++)
		{

			//No owner = -1
			if (LeviathanController.instance._data.WayPoints[i].Owner == LeviathanController.instance._otherSpaceship.Owner || LeviathanController.instance._data.WayPoints[i].Owner == -1 && LeviathanController.instance._data.WayPoints[i].Position != nextWaypoint.Value)
			{
				float dst = Vector2.Distance(nextWaypoint.Value, LeviathanController.instance._data.WayPoints[i].Position);


				if (dst < shortestDist && distNextWaypoint.Value != 0)
				{
					shortestDistB = dst;
					nextClosestWaypoint = LeviathanController.instance._data.WayPoints[i].Position;
				}
			}
		}

		LeviathanController.instance.tree.SetVariableValue("nextClosestWaypoint", nextClosestWaypoint);
	}
}
