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

		private RaycastHit2D[] _hitAsteroid;

        public override void OnStart()
        {
			bool hitWaypoint = false;

			_hitAsteroid = Physics2D.RaycastAll(LeviathanController.instance._spaceship.Position, LeviathanController.instance._dirA, 30);

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

					if (_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("Asteroid") && !hitWaypoint)
					{
						Asteroid asteroid = _hitAsteroid[i].collider.GetComponentInParent<Asteroid>();
						float actualDistAsteroid = Vector2.Distance(LeviathanController.instance._spaceship.Position, asteroid.Position);
						Debug.Log("Asteroid: " + _hitAsteroid[i].collider.gameObject.transform.parent.name);

						if (actualDistAsteroid < _lastDistAsteroid.Value)
						{
							LeviathanController.instance.tree.SetVariableValue("LastAsteroidDist", actualDistAsteroid);
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


