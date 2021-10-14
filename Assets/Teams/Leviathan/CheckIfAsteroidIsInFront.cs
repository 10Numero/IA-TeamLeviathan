using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leviathan
{
    public class CheckIfAsteroidIsInFront : Action
    {
		public SharedFloat _lastDistAsteroid;
		public SharedVector2 asteroidPos;
		public SharedVector2 dirA;

		private RaycastHit2D[] _hitAsteroid;

		public SharedFloat dot;
		public SharedFloat avoidAsteroidOffset;

		public override void OnStart()
        {
			bool hitWaypoint = false;

			Debug.DrawRay(LeviathanController.instance._spaceship.Position, dirA.Value, Color.red);

			_hitAsteroid = Physics2D.RaycastAll(LeviathanController.instance._spaceship.Position, dirA.Value, 30);

			//Debug.Log("check asteroid");

			for (int i = 0; i < _hitAsteroid.Length; i++)
			{
				if (_hitAsteroid[i].collider != null)
				{
					if (_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
					{
						int wpOwner = _hitAsteroid[i].collider.GetComponentInParent<WayPoint>().Owner;

						if (wpOwner != LeviathanController.instance._spaceship.Owner)
							hitWaypoint = true;
					}

					//Debug.Log("hit something : " + _hitAsteroid[i].collider.transform.parent.name);

					if (_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("Asteroid") && !hitWaypoint)
					{
						//Debug.Log("Hit asteroid");
						Asteroid asteroid = _hitAsteroid[i].collider.GetComponentInParent<Asteroid>();
						float actualDistAsteroid = Vector2.Distance(LeviathanController.instance._spaceship.Position, asteroid.Position);
						Debug.Log("Asteroid: " + _hitAsteroid[i].collider.gameObject.transform.parent.name);

						if (actualDistAsteroid < _lastDistAsteroid.Value && asteroid.Position != asteroidPos.Value)
						{
							Vector2 perpendicular = Vector2.zero;

							//Which Perpendicular
							if (dot.Value > 0)
								perpendicular = Vector2.Perpendicular(dirA.Value);
							else
								perpendicular = -Vector2.Perpendicular(dirA.Value);

							Vector2 origin = asteroidPos.Value;

							Vector2 asteroidAvoidPos = (origin + perpendicular.normalized) * (asteroid.Radius * avoidAsteroidOffset.Value);

							Transform _debug = new GameObject("Debug").transform;
							_debug.position = asteroidAvoidPos;

							Vector2 dir = asteroidAvoidPos - LeviathanController.instance._spaceship.Position;

							float orient = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


							LeviathanController.instance.tree.SetVariableValue("LastAsteroidDist", actualDistAsteroid);
							LeviathanController.instance.tree.SetVariableValue("OrientToAvoidAsteroid", orient);
							LeviathanController.instance.tree.SetVariableValue("AsteroidPosition", asteroid.Position);
							LeviathanController.instance.tree.SetVariableValue("LastAsteroidRadius", asteroid.Radius);
							LeviathanController.instance.tree.SetVariableValue("AsteroidInFront", true);
						}
					}
				}

				LeviathanController.instance.tree.SetVariableValue("AsteroidInFront", false);
			}
		}
    }
}


