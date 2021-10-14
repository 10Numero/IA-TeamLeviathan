using Leviathan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leviathan
{
    public class TestSideCheck : MonoBehaviour
    {
        public float angle;
        public float dot;
        void LateUpdate()
        {
            float deviantRot = LeviathanController.instance._spaceship.Orientation + 90;
            deviantRot *= Mathf.Deg2Rad;
            Vector2 right = new Vector2(Mathf.Cos(deviantRot), Mathf.Sin(deviantRot));

            float rot = LeviathanController.instance._spaceship.Orientation;
            rot *= Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));

            //Debug.DrawRay(LeviathanController.instance._spaceship.Position, forward, Color.green);
            //Debug.DrawRay(LeviathanController.instance._spaceship.Position, LeviathanController.instance._dirA, Color.red);
            //Debug.DrawRay(LeviathanController.instance._spaceship.Position, LeviathanController.instance._targetDir, Color.yellow);

            Vector2 dir = (LeviathanController.instance._nextWaypoint.Position - LeviathanController.instance._spaceship.Position);

            //Angle exact -> + élevée + delta est grand
            angle = Vector2.Angle(forward, dir);

            //dot = Vector2.Dot(forward, dir.normalized);
            dot = (Vector2.Dot(dir.normalized, right));

            if (dot > 0)
            {
                Debug.Log("Looking left");
            }
            else
                Debug.Log("Looking right");


            //Debug.Log("Angle : " + angle);
        }
    }
}


