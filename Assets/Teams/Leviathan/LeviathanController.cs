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

		private RaycastHit2D[] _hitAsteroid;
		private RaycastHit2D _hitWaypoint;
		private RaycastHit2D _lastHit;

        private int _asteroidMask;
        private int _wayPointMask;

        public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			int maskAsteroid = (1 << LayerMask.NameToLayer("Asteroid"));
			int maskWaypoint = (1 << LayerMask.NameToLayer("WayPoint"));
			_asteroidMask = maskAsteroid;
			_wayPointMask = maskWaypoint;
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

			Debug.DrawRay(_spaceship.Position, (directions[0] - _spaceship.Position), Color.red);
			Debug.DrawRay(_spaceship.Position, _dir * 30, Color.yellow);

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

			if (directions.Count == 1)
				directions.Add((origin + perpendicular.normalized) * (_actualAsteroidObstacle.view.Radius * radiusRatio));
			else
				directions[1] = (origin + perpendicular.normalized) * (_actualAsteroidObstacle.view.Radius * radiusRatio);


			_debug.transform.position = (origin + perpendicular.normalized) * (_actualAsteroidObstacle.view.Radius * radiusRatio);
		}

		bool _DetectedAsteroid()
        {
			bool hitWaypoint = false;

			_hitAsteroid = Physics2D.RaycastAll(_spaceship.Position, _dir, 30);

			for(int i = 0; i < _hitAsteroid.Length; i++)
            {
				if (_hitAsteroid[i].collider != null)
				{
					if(_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
                    {
						int wpOwner = _hitAsteroid[i].collider.GetComponentInParent<WayPoint>().Owner;

						if (wpOwner != _spaceship.Owner)
							hitWaypoint = true;
					}

					if (_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("Asteroid") && !hitWaypoint)
					{
						_actualAsteroidObstacle = _hitAsteroid[i].collider.GetComponentInParent<Asteroid>();
						return true;
					}
				}
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
