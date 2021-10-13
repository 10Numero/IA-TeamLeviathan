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
		private WayPointView _waypointAfter;
		private Asteroid _actualAsteroidObstacle;

		private GameObject _debug;

		private Vector2 _targetDir;

		private List<Vector2> directions = new List<Vector2>();

		private RaycastHit2D _hit;
		private RaycastHit2D _lastHit;
		private RaycastHit2D[] _hitAsteroid;
		private RaycastHit2D _hitWaypoint;


		private Vector2 _lastConsideredWaypoint;
		private int _asteroidMask;
		private int _wayPointMask;


		private Vector2 _dirA;
		private Vector2 _dirB;
		private float _t;

		private float _minDistDrift = 1;


		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			int maskAsteroid = (1 << LayerMask.NameToLayer("Asteroid"));
			int maskWaypoint = (1 << LayerMask.NameToLayer("WayPoint"));
			_asteroidMask = maskAsteroid;
			_wayPointMask = maskWaypoint;
			_thrust = 1;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_StoreDatas(spaceship, data);
			_Enemy();
			_Spaceship();

			return new InputData(_thrust, _targetOrientation, _needShoot, _dropMine, _fireShockwave);
		}


		// Spaceship à revoir 
		void _Spaceship()
		{
			//_thrust = 1f;

			if (directions.Count == 0)
			{
				directions.Add(_GetClosestWaypointPosition());
			}
			else
			{
				directions[0] = _GetClosestWaypointPosition();


			}

			if (directions.Count == 1)
			{
				directions.Add(_GetNextClosestWaypointPosition(directions[0]));
			}
			else
			{
				directions[1] = _GetNextClosestWaypointPosition(directions[0]);
			}


			float dst = Vector2.Distance(_spaceship.Position, directions[0]);

			if (Vector2.Distance(_spaceship.Position, directions[0]) < .1f)
			{
				directions.Remove(directions[0]);
			}



			_dirA = directions[0] - _spaceship.Position;
			_dirB = directions[1] - _spaceship.Position;
			if ((dst - _minDistDrift) <= 1)
				_t = 1 - (dst / _minDistDrift);
			else
				_t = 0;

			Debug.DrawRay(_spaceship.Position, _targetDir, Color.yellow);

			_targetDir = Vector2.Lerp(_dirA, _dirB, _t);

			_targetOrientation = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;

			//Shoot
			_needShoot = AimingHelpers.CanHit(_spaceship, _otherSpaceship.Position, _otherSpaceship.Velocity, 0.15f);


			if (dst < 30.0f)
				DriftForNextWaypoint();
			else
				_thrust = 1;

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

			Vector2 perpendicular = Vector2.Perpendicular(_targetDir);
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

			_hitAsteroid = Physics2D.RaycastAll(_spaceship.Position, _targetDir, 30);

			for (int i = 0; i < _hitAsteroid.Length; i++)
			{
				if (_hitAsteroid[i].collider != null)
				{
					if (_hitAsteroid[i].collider.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
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

			Debug.DrawRay(_spaceship.Position, _targetDir * 30, Color.yellow);

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
				if (_data.WayPoints[i].Owner == _otherSpaceship.Owner || _data.WayPoints[i].Owner == -1)
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


		// A voir avec _GetClosestWaypointPosition pour mix les deux fonctions
		Vector2 _GetNextClosestWaypointPosition(Vector2 actualClosestWaypoint)
		{
			Vector2 nextClosestWaypoint = actualClosestWaypoint;
			float shortestDist = Mathf.Infinity;

			for (int i = 0; i < _data.WayPoints.Count; i++)
			{

				//No owner = -1
				if (_data.WayPoints[i].Owner == _otherSpaceship.Owner || _data.WayPoints[i].Owner == -1)
				{
					float dist = Vector2.Distance(actualClosestWaypoint, _data.WayPoints[i].Position);


					if (dist < shortestDist && dist != 0)
					{
						shortestDist = dist;
						_waypointAfter = _data.WayPoints[i];
						nextClosestWaypoint = _data.WayPoints[i].Position;
					}
				}
			}

			return nextClosestWaypoint;
		}

		//Evaluate
		void DriftForNextWaypoint()
		{
			if (directions.Count > 1)
			{

				float delta = (_spaceship.Orientation - _targetOrientation) * Mathf.Deg2Rad;



				if (delta == 0)
					delta = 1;
				else
					delta = 1 - (delta / Mathf.PI);


				Debug.Log("delta : " + delta);

				_thrust = Mathf.Abs(delta);
			}
		}
	}

}
