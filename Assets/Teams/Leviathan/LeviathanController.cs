using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace Leviathan 
{
	public class LeviathanController : BaseSpaceShipController
	{
		private float _thrust;
		private float _targetOrientation;
		private bool _needShoot;
		private bool _dropMine;
		private bool _fireShockwave;

		private SpaceShipView _otherSpaceship;
		private GameData _data;
		private SpaceShipView _spaceship;

		private WayPointView _nextWaypoint;
		private Asteroid _actualAsteroidObstacle;

		private GameObject _debug;

		private Vector2 _dir;

		private List<Vector2> directions = new List<Vector2>();

		private RaycastHit2D _hit;
		private RaycastHit2D _lastHit;
		

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_StoreDatas(spaceship, data);
			_Enemy();
			_Spaceship();

			return new InputData(_thrust, _targetOrientation, _needShoot, _dropMine, _fireShockwave);
		}

		void _Spaceship()
		{
			//Move
			_thrust = 1f;

			if (directions.Count == 0)
				directions.Add(_GetClosestWaypointPosition());
			else
				directions[0] = _GetClosestWaypointPosition();

			if (Vector2.Distance(_spaceship.Position, directions[directions.Count - 1]) < .1f)
				directions.Remove(directions[directions.Count - 1]);

			_dir = directions[directions.Count - 1] - _spaceship.Position;
			_targetOrientation = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

			Debug.DrawRay(_spaceship.Position, _dir, Color.yellow);

            //Shoot
            _needShoot = AimingHelpers.CanHit(_spaceship, _otherSpaceship.Position, _otherSpaceship.Velocity, 0.15f);

			if (_DetectedAsteroid())
				_AvoidAsteroidPoint();
		}


		void _Enemy()
        {
			//Enemy stuff
		}


		void _StoreDatas(SpaceShipView spaceship, GameData data)
        {
			_data = data;
			_otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			_spaceship = spaceship;
        }

		void _AvoidAsteroidPoint()
        {
			if (_debug == null)
				_debug = new GameObject("Debug");

			float radiusRatio = 1.1f;

			Vector2 perpendicular = Vector2.Perpendicular(_dir);
			Vector2 origin = _actualAsteroidObstacle.view.Position;

			directions.Add((origin + perpendicular.normalized) * radiusRatio);

			_debug.transform.position = (origin + perpendicular.normalized) * radiusRatio;
		}

		bool _DetectedAsteroid()
        {
			int mask = (1 << LayerMask.NameToLayer("Asteroid"));

			_hit = Physics2D.Raycast(_spaceship.Position, _dir, 30, mask);

			if (_hit.collider != null && _hit != _lastHit)
            {
				_actualAsteroidObstacle = _hit.collider.GetComponentInParent<Asteroid>();
				Debug.Log("Avoid asteroid : " + _actualAsteroidObstacle.gameObject.name + " radius : " + _actualAsteroidObstacle.view.Radius);
				_lastHit = _hit;
				return true;
			}

			return false;
        }

		//Owner
		Vector2 _GetClosestWaypointPosition()
        {
			Vector2 closestWaypointPosition = Vector2.zero;
			float shortestDist = Mathf.Infinity;

            for (int i = 0; i < _data.WayPoints.Count; i++)
            {
				//No owner = -1
				if(_data.WayPoints[i].Owner == _otherSpaceship.Owner || _data.WayPoints[i].Owner == -1)
                {
					float dist = Vector2.Distance(_spaceship.Position, _data.WayPoints[i].Position);

					if (dist < shortestDist)
                    {
						shortestDist = dist;
						_nextWaypoint = _data.WayPoints[i];
						closestWaypointPosition = _data.WayPoints[i].Position;
					}
				}
            }

            return closestWaypointPosition;
        }
	}

}
