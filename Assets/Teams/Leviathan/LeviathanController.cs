using System.Collections;
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
		private bool _needShoot;
		private bool _dropMine;
		private bool _fireShockwave;

		public SpaceShipView _otherSpaceship;
		public GameData _data;
		public SpaceShipView _spaceship;

		public WayPointView _nextWaypoint;
		private WayPointView _waypointAfter;
		private Asteroid _actualAsteroidObstacle;

		private GameObject _debug;

		public Vector2 _targetDir;

		private List<Vector2> directions = new List<Vector2>();

		private RaycastHit2D _hit;
		private RaycastHit2D _lastHit;
		private RaycastHit2D[] _hitAsteroid;
		private RaycastHit2D _hitWaypoint;


		private Vector2 _lastConsideredWaypoint;
		private int _asteroidMask;
		private int _wayPointMask;

		public Vector2 _dirA;
		private Vector2 _dirB;
		private Vector2 _dirC;
		private float _dst;
		public float _t;

		private float _minDistDrift = 2f;

		private Asteroid _lastAsteroidObstacle;

		private float _lastDistAsteroid;

		private bool _needToAdjustVelo;
        private Vector2 _dirOffset;

		public static LeviathanController instance;
		public BehaviorTree tree;

		public float orientationA;
		public float orientationB;
		public float lerpOrient;

		public float dot;

		private bool readaptingRot;
		public float angle;
		public float angleB;
		public bool newDirection;

		public Vector2 asteroid;
		private Vector2 asteroidDestination;

		public Vector2 forward;
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
            //_Spaceship();
            _thrust = 1;
			//_targetOrientation = spaceship.Orientation;

			StartCoroutine(ResetDatas());

            return new InputData(_thrust, _targetOrientation, _needShoot, _dropMine, _fireShockwave);
		}

		IEnumerator ResetDatas()
        {
			yield return new WaitForEndOfFrame();
			_dropMine = false;
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


			//Some calcul
			float deviantRot = LeviathanController.instance._spaceship.Orientation + 90;
			deviantRot *= Mathf.Deg2Rad;
			Vector2 right = new Vector2(Mathf.Cos(deviantRot), Mathf.Sin(deviantRot));

			Vector2 dir = (_nextWaypoint.Position - _spaceship.Position);
			Vector2 dirB = (directions[1] - _spaceship.Position);

			dot = (Vector2.Dot(dir.normalized, right));

			float _t2 = 0;

			//Calcul delta - Besoin ??
			float rot = LeviathanController.instance._spaceship.Orientation;
			rot *= Mathf.Deg2Rad;
			forward = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));

			//Angle exact -> + élevée + delta est grand
			angle = Vector2.Angle(forward, dir);

			//Angle exact -> + élevée + delta est grand
			angleB = Vector2.Angle(forward, dirB);

			_OnNewTargetWayPoint();

			//Direction
			_dirA = directions[0] - _spaceship.Position;
			_dirB = directions[1] - _spaceship.Position;


			float ratio = 1f;

			//Drift
			if (_dst <= _minDistDrift)
				_t = 1 - (_dst / _minDistDrift);
			else
				_t = 0;

			//if (dot > 0)
   //         {
			//	//Gros angle
			//	if(dot > 0.4f)
   //             {
			//		_targetOrientation = (dot * ratio);
			//		_targetDir = _dirA;
			//		Debug.Log("Left Chaud");

			//	}
   //             else
   //             {
			//		_targetDir = Vector2.Lerp(_dirA, _dirB, _t);
			//	}
   //         }
   //         else
   //         {
			//	if(dot < -0.4f)
   //             {
			//		_targetOrientation = -(dot * ratio);
			//		_targetDir = _dirA;
			//		Debug.Log("Right Chaud");
			//	}
   //             else
   //             {

			//	}
            //}
			_targetDir = Vector2.Lerp(_dirA, _dirB, _t);

			//optimization drift orientation
			//if (_dst > 0.1f && _dst <= 0.2f)
			//	_t2 = 1 - (_dst / 0.1f);
			//else
			//	_t2 = 0;


			//if (dot > 0)
			//{
			//	//Faut aller vers la droite
			//	Debug.Log("Looking left");
			//	//orientationA += (dot * _t2 * ratio);
			//}
			//else
			//{
			//	Debug.Log("Looking right");
			//	//orientationA -= (dot * _t2 * ratio);
			//}


			//Orientation
			_targetOrientation = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;

			//float deltaAngle = Vector2.SignedAngle(_spaceship.Velocity, _nextWaypoint.Position - _nextWaypoint.Position);
			//deltaAngle *= 1.5f;
			//deltaAngle = Mathf.Clamp(deltaAngle, -170, 170);
			//float velocityOrientation = Vector2.SignedAngle(Vector2.right, _spaceship.Velocity);
			//_targetOrientation = velocityOrientation + deltaAngle;

			if (_DetectedAsteroid())
				_AvoidAsteroidPoint();

			//Debug
			Debug.DrawRay(_spaceship.Position, forward, Color.red);
            Debug.DrawRay(_spaceship.Position, _dirB, Color.green);
            Debug.DrawRay(_spaceship.Position, _dirA, Color.cyan);
            Debug.DrawRay(_spaceship.Position, _targetDir, Color.yellow);
		}

		void _OnNewTargetWayPoint()
        {
			if (!newDirection)
				return;

			newDirection = false;
			if (angleB > 20)
				Debug.Log("PROCHAIN POINT CHAUD");
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

			Vector2 perpendicular = Vector2.zero;

			//Which Perpendicular
			if(dot > 0)
				perpendicular = Vector2.Perpendicular(_targetDir);
			else
				perpendicular = -Vector2.Perpendicular(_targetDir);

			Vector2 origin = _actualAsteroidObstacle.view.Position;

			asteroidDestination = (origin + perpendicular.normalized) * (_actualAsteroidObstacle.view.Radius * radiusRatio);

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
							asteroid = _actualAsteroidObstacle.Position;
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
				newDirection = true;

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

			StartCoroutine(ResetMineCondition());
		}

		private IEnumerator ResetMineCondition()
		{
			 yield return new WaitForSeconds(.1f);
			_dropMine = false;
		}

		public void setShockwaveCondition(bool shockwave)
		{
			_fireShockwave = shockwave;

			StartCoroutine(ResetShockwave());
		}

		IEnumerator ResetShockwave()
        {
			yield return new WaitForSeconds(1);
			_fireShockwave = false;
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
