using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace Leviathan {

	public class LeviathanController : BaseSpaceShipController
	{

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			Vector3 posToLookAt = (data.WayPoints[15].Position - spaceship.Position).normalized;
			float targetOrient = posToLookAt.z +90.0f;  //Mathf.Atan2(data.WayPoints[8].Position.x - spaceship.Position.x,data.WayPoints[8].Position.y - spaceship.Position.y) * Mathf.Rad2Deg /*+ 90.0f*/;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);
		}
	}

}
