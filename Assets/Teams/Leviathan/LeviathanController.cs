﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;

namespace Leviathan
{
	public class LeviathanController : BaseSpaceShipController
	{
		private float _thrust;
		private float _targetOrientation;
		private float _aTargetOrientation;
		private bool _needShoot;
		private bool _dropMine;
		private bool _fireShockwave;

		public SpaceShipView _otherSpaceship;
		public GameData _data;
		public SpaceShipView _spaceship;

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
		private Vector2 _dirC;
		private float _dst;
		private float _t;

		private float _minDistDrift = 2f;

		private Asteroid _lastAsteroidObstacle;

		private float _lastDistAsteroid;

		private bool _needToAdjustVelo;
        private Vector2 _dirOffset;

		public static LeviathanController instance;
		public BehaviorTree tree;

        private void Awake()
        {
			instance = this;
			tree = GetComponent<BehaviorTree>();
        }

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
			_Spaceship();

			return new InputData(_thrust, _targetOrientation, _needShoot, _dropMine, _fireShockwave);
		}


        void _SpaceShipDirection()
        {
			//Actual WayPoint
			if (directions.Count == 0)
				directions.Add(_GetClosestWaypointPosition());
			else
				directions[0] = _GetClosestWaypointPosition();

			//Next WayPoint
			if (directions.Count == 1)
				directions.Add(_GetNextClosestWaypointPosition(directions[0]));
			else
				directions[1] = _GetNextClosestWaypointPosition(directions[0]);


			if (Vector2.Distance(_spaceship.Position, directions[0]) < .1f)
				directions.Remove(directions[0]);

			_dirA = (directions[0] - _spaceship.Position);
			_dirB = directions[1] - _spaceship.Position;

			_aTargetOrientation = Mathf.Atan2(_dirA.y, _dirA.x) * Mathf.Rad2Deg;

			Debug.Log("_aTargetOrientation : " + _aTargetOrientation);
			Debug.Log("spaceship : " + _spaceship.Orientation);

			//_minDistDrift = _dirB

			if (_dst <= _minDistDrift)
				_t = 1 - (_dst / _minDistDrift);
			else
				_t = 0;

            //Debug.Log("T: " + _t);q
            Vector2 delta = _dirA - _spaceship.Velocity;
			float threshold = Mathf.Atan2(_dirA.y, _dirA.x) * Mathf.Rad2Deg;
			float ratio = 0.1f;
			
	


			_targetDir = Vector2.Lerp(_dirA, _dirB, _t);

			Debug.DrawRay(_spaceship.Position, _targetDir, Color.yellow);

			_targetOrientation = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;

            Debug.Log(". : " + threshold);

			Vector2 perpendicular = Vector2.Perpendicular(_targetDir);
			Vector2 origin = (directions[0] + perpendicular.normalized) * (_nextWaypoint.Radius * 1);


            if ((Mathf.Atan2(_dirA.y, _dirA.x) * Mathf.Rad2Deg) > 0)
            {
                _targetOrientation -= (threshold * ratio);
                Debug.Log("Positif : " + _targetOrientation);
            }

            else
            {
                _targetOrientation += (-threshold * ratio);
                Debug.Log("Negatif : " + _targetOrientation);
            }

        }

		void _OnNewTargetWayPoint()
        {
		}

		//Evaluate
		void DriftForNextWaypoint()
		{
			//if (_dst > 2)
			//	return;

			//float delta = (_spaceship.Orientation - _aTargetOrientation) * Mathf.Deg2Rad;
			//delta = Mathf.Abs(delta) / (Mathf.PI);

			//Debug.Log("delta : " + delta);

			//float ratio = 15;

			//Debug.Log("Target orientation A: " + _targetOrientation);

			//_targetOrientation += delta * ratio;


			//Debug.Log("Target orientation B: " + _targetOrientation);
		}


		// Spaceship à revoir 
		void _Spaceship()
		{

			_SpaceShipDirection();

			if (_dst < 30.0f)
				DriftForNextWaypoint();
			else
				_thrust = 1;

			//Shoot
			_needShoot = AimingHelpers.CanHit(_spaceship, _otherSpaceship.Position, _otherSpaceship.Velocity, 0.15f);
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

			_hitAsteroid = Physics2D.RaycastAll(_spaceship.Position, _dirA, 30);

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
						float actualDistAsteroid = Vector2.Distance(_spaceship.Position, _hitAsteroid[i].collider.GetComponentInParent<Asteroid>().Position);
						Asteroid actualAsteroid = _hitAsteroid[i].collider.GetComponentInParent<Asteroid>();
						Debug.Log("Asteroid: " + _hitAsteroid[i].collider.gameObject.transform.parent.name);

						if (_lastAsteroidObstacle == null)
							_lastAsteroidObstacle = actualAsteroid;

						if (actualDistAsteroid < _lastDistAsteroid)
						{
							_lastAsteroidObstacle = actualAsteroid;
							_lastDistAsteroid = Vector2.Distance(_spaceship.Position, _hitAsteroid[i].collider.GetComponentInParent<Asteroid>().Position);
							Debug.Break();
							_actualAsteroidObstacle = _hitAsteroid[i].collider.GetComponentInParent<Asteroid>();
							return true;
						}
					}
				}
			}

			return false;
		}

		//Owner
		Vector2 _GetClosestWaypointPosition()
		{

			Vector2 closestWaypointPosition = Vector2.zero;

			if (_nextWaypoint != null)
				closestWaypointPosition = _nextWaypoint.Position;

			float shortestDist = Mathf.Infinity;

			for (int i = 0; i < _data.WayPoints.Count; i++)
			{
				//No owner = -1
				if (_data.WayPoints[i].Owner == _otherSpaceship.Owner || _data.WayPoints[i].Owner == -1)
				{
					float dst = Vector2.Distance(_spaceship.Position, _data.WayPoints[i].Position);

					if (dst < shortestDist)
					{
						shortestDist = dst;
						_nextWaypoint = _data.WayPoints[i];
					}
				}
			}

			_dst = Vector2.Distance(_spaceship.Position, _nextWaypoint.Position);

			if (_nextWaypoint.Position != closestWaypointPosition)
				_OnNewTargetWayPoint();

			closestWaypointPosition = _nextWaypoint.Position;

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
				if (_data.WayPoints[i].Owner == _otherSpaceship.Owner || _data.WayPoints[i].Owner == -1 && _data.WayPoints[i].Position != _nextWaypoint.Position)
				{
					float dst = Vector2.Distance(actualClosestWaypoint, _data.WayPoints[i].Position);


					if (dst < shortestDist && _dst != 0)
					{
						shortestDist = dst;
						_waypointAfter = _data.WayPoints[i];
						nextClosestWaypoint = _data.WayPoints[i].Position;
					}
				}
			}

			return nextClosestWaypoint;
		}


		float EaseInCubic(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value + start;
		}

		public void setShootCondition(bool shoot)
		{
			_needShoot = shoot;
		}

		public void setMineCondition(bool mine)
		{
			_dropMine = mine;
		}

		public void setShockwaveCondition(bool shockwave)
		{
			_fireShockwave = shockwave;
		}

		public SpaceShipView getSpaceship()
		{
			return _spaceship;
		}

        public void SetOrientation(float orientation)
        {
			_targetOrientation = orientation;
        }

	}

}
