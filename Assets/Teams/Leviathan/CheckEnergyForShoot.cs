using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

namespace Leviathan
{
    public class CheckEnergyForShoot : Conditional
    {
        public SharedFloat threshold;
        public SharedFloat mineEnergyCost;
        private float spaceshipEnergy;
        private LeviathanController leviathan;
        private BehaviorTree tree;

        public override void OnStart()
        {
            tree = gameObject.GetComponentInParent<BehaviorTree>();
            leviathan = tree.GetComponentInParent<LeviathanController>();
            spaceshipEnergy = leviathan.getSpaceship().Energy;
            checkEnergy(spaceshipEnergy);
        }

        private void checkEnergy(float actualSpaceshipEnergy)
        {
            if (actualSpaceshipEnergy >= mineEnergyCost.Value + threshold.Value)
            {
                tree.SetVariableValue("EnergyToShoot", true);
            }
            else
            {
                tree.SetVariableValue("EnergyToShoot", false);
            }

        }
    }
}
