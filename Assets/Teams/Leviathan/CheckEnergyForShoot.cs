using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class CheckEnergyForShockwave : Conditional
{

    public SharedFloat minEnergyValue;
    private float spaceshipEnergy;
    private LeviathanController leviathan;
    private BehaviorTree tree;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        spaceshipEnergy = leviathan.getSpaceship().Energy;
        checkEnergy(minEnergyValue.Value, spaceshipEnergy);
    }

    private void checkEnergy(float requiredEnergy, float actualSpaceshipEnergy)
    {

        if (actualSpaceshipEnergy >= requiredEnergy)
        {
            tree.SetVariableValue("EnergyToShoot", true);
        }
        else {
            tree.SetVariableValue("EnergyToShoot", false);
        }

    }
}
