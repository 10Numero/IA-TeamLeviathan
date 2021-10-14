using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leviathan
{
    public class CalculEssentialValues : Action
    {
        public SharedVector2 nextWaypointPosition;
        public SharedVector2 nextNextWaypointPosition;
        public float distBeforeStartDrift;


        public override void OnStart()
        {
            //Dot
            if (nextWaypointPosition.Value != Vector2.zero)
            {
                float deviantRot = LeviathanController.instance._spaceship.Orientation + 90;
                deviantRot *= Mathf.Deg2Rad;
                Vector2 right = new Vector2(Mathf.Cos(deviantRot), Mathf.Sin(deviantRot));

                Vector2 dir = (nextWaypointPosition.Value - LeviathanController.instance._spaceship.Position);

                float dot = (Vector2.Dot(dir.normalized, right));

                LeviathanController.instance.tree.SetVariableValue("Dot", dot);
            }

            //Dist to next WP
            float _dist = Vector2.Distance(LeviathanController.instance._spaceship.Position, nextWaypointPosition.Value);
            LeviathanController.instance.tree.SetVariableValue("distNextWaypoint", _dist);


            //Spaceship Dir
            float rot = LeviathanController.instance._spaceship.Orientation;
            rot *= Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));
            LeviathanController.instance.tree.SetVariableValue("Forward", forward);

            //Dir A & B
            Vector2 dirA = nextWaypointPosition.Value - LeviathanController.instance._spaceship.Position;
            Vector2 dirB = nextNextWaypointPosition.Value - LeviathanController.instance._spaceship.Position;
            LeviathanController.instance.tree.SetVariableValue("DirA", dirA);
            LeviathanController.instance.tree.SetVariableValue("DirB", dirB);

            //T
            float t = 0;

            if (_dist <= distBeforeStartDrift)
                t = 1 - (_dist / distBeforeStartDrift);
            else
                t = 0;
            LeviathanController.instance.tree.SetVariableValue("T", t);


            Vector2 _targetDir = Vector2.Lerp(dirA, dirB, t);
            LeviathanController.instance.tree.SetVariableValue("TargetDir", _targetDir);
        }
    }
}


