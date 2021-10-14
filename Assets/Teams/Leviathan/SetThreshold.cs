using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leviathan
{
    public class SetThreshold : Action
    {
        public float thresholdValue;
        private BehaviorTree tree;
        public override void OnStart()
        {
            tree = gameObject.GetComponentInParent<BehaviorTree>();
            setTreshold(thresholdValue);
        }

        public void setTreshold(float value)
        {
            tree.SetVariableValue("Threshold", value);
        }
    }
}